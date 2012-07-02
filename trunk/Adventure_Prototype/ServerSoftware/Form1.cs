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


		public Form1()
		{
			InitializeComponent();
		}





		private void Form1_Load(object sender, EventArgs e)
		{
			gb_cc.Text = "Server offline";
			gb_cc.ForeColor = Color.Red;

			btnStartServer.ForeColor = Color.Black;
			btnStartServer.Text = "Start Server";

			onlineIndicator.BackColor = Color.Red;
		}





		private void btnStartServer_Click(object sender, EventArgs e)
		{
			if (server == null)
			{
				rtb_log.Clear();
				NetPeerConfiguration config = new NetPeerConfiguration("Co-Char-Key");
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
