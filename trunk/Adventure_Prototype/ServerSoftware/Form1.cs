using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using Lidgren.Network;

namespace ServerSoftware
{
	public partial class Form1 : Form
	{

		public NetServer server;
		public BackgroundWorker worker;
		public List<Peer> connectedPeers = new List<Peer>();
		public ServerStatus serverMode = ServerStatus.LOBBY;

		private bool autostart = false;



		public Form1(string[] args)
		{
			InitializeComponent();

			foreach (string s in args)
			{
				if (s == "-autostart")
				{
					autostart = true;
				}
			}

		}





		private void Form1_Load(object sender, EventArgs e)
		{
			gb_cc.Text = "Server offline";
			gb_cc.ForeColor = Color.Red;

			btnStartServer.ForeColor = Color.Black;
			btnStartServer.Text = "Start Server";

			onlineIndicator.BackColor = Color.Red;

			btnStartServer_Click(null, null);
		}





		private void btnStartServer_Click(object sender, EventArgs e)
		{
			if (server == null)
			{
				rtb_log.Clear();
				NetPeerConfiguration config = new NetPeerConfiguration("CoCharKey");
				config.Port = 14242;
				config.MaximumConnections = 2;
				config.EnableMessageType(NetIncomingMessageType.ConnectionApproval);
				config.EnableMessageType(NetIncomingMessageType.DiscoveryRequest);

				try
				{
					server = new NetServer(config);
					server.Start();
					print("Server started!");
					print("Listening on Port " + config.Port.ToString());

					worker = new BackgroundWorker();
					worker.WorkerSupportsCancellation = true;
					worker.WorkerReportsProgress = true;
					worker.DoWork += new DoWorkEventHandler(worker_DoWork);
					worker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(worker_RunWorkerCompleted);
					worker.RunWorkerAsync();


					gb_cc.ForeColor = Color.Green;
					gb_cc.Text = "Server running...";

					onlineIndicator.BackColor = Color.Green;

					btnStartServer.ForeColor = Color.Black;
					btnStartServer.Text = "Stop Server";
				}
				catch (Exception ex)
				{
					print(ex.ToString());
					return;
				}
			}
			else
			{
				try
				{
					print("Shutting down...");

					worker.CancelAsync();
					worker.Dispose();
					worker = null;


					server.Shutdown("Bye");
					server = null;

					gb_cc.ForeColor = Color.Red;
					gb_cc.Text = "Server offline";

					lb_cp.Items.Clear();

					onlineIndicator.BackColor = Color.Red;

					btnStartServer.ForeColor = Color.Black;
					btnStartServer.Text = "Start Server";

					print("Server is offline.");
				}
				catch (Exception ex)
				{
					print(ex.ToString());
					return;
				}
			}
		}





		void worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
		{
			try
			{
				//Is the worker still existant?
				if (worker == null)
				{
					return;
				}

				//Do we have new packages?
				NetIncomingMessage inc;
				if ((inc = server.ReadMessage()) != null)
				{
					Postman.checkMails(inc, this);
				}

				//Broadcast to players
				if (server.ConnectionsCount > 0)
				{
					Radio.update(this);
				}

				//Update Form
				gbCon.Text = "Connected Players: " + server.ConnectionsCount.ToString();
				lb_cp.Items.Clear();
				List<Peer> Peers_to_be_removed = new List<Peer>();

				foreach(Peer n in connectedPeers)
				{
					if (!server.Connections.Contains<NetConnection>(n.Connection))
					{
						print("Server can't find peer " + n.Name + ". Removing from peer list.");
						Peers_to_be_removed.Add(n);
					}
					if (serverMode == ServerStatus.LOBBY)
					{
						lb_cp.Items.Add(n.Name);
					}
					else
					{
						lb_cp.Items.Add(n.Name + "(X:" + n.X.ToString() + ", Y:" + n.Y.ToString() + ")");
					}
					
				}

				foreach (Peer n in Peers_to_be_removed)
				{
					n.Connection.Disconnect("Bye");
					connectedPeers.Remove(n);
				}

				//Now go to sleep
				worker.RunWorkerAsync();
			}
			catch (Exception ex)
			{
				print(ex.ToString());
			}
		}




		public void print(String text)
		{
			rtb_log.AppendText("\n" + DateTime.Now.ToString() + ": " + text);
		}



		private void worker_DoWork(object sender, DoWorkEventArgs e)
		{
			//Let thread sleep to give OS some time
			System.Threading.Thread.Sleep(50);
		}




	}
}
