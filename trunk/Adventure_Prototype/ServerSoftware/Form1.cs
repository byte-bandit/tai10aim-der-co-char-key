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


		public Form1()
		{
			InitializeComponent();
		}






		private void Form1_Load(object sender, EventArgs e)
		{
			gb_cc.Text = "Server offline";
			gb_cc.ForeColor = Color.Red;

			btnClearLog.ForeColor = Color.Black;
			btnClearLog.Text = "Clear Log";

			btnStartServer.ForeColor = Color.Black;
			btnStartServer.Text = "Start Server";
		}





		private void btnStartServer_Click(object sender, EventArgs e)
		{
			if (server == null)
			{
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

					gb_cc.ForeColor = Color.Green;
					gb_cc.Text = "Server running...";

					btnStartServer.ForeColor = Color.Black;
					btnStartServer.Text = "Stop Server";

					btnClearLog.ForeColor = Color.Black;
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
					server.Shutdown("Bye");
					server = null;

					gb_cc.ForeColor = Color.Red;
					gb_cc.Text = "Server offline";

					btnStartServer.ForeColor = Color.Black;
					btnStartServer.Text = "Start Server";

					btnClearLog.ForeColor = Color.Black;
					print("Server is offline.");
				}
				catch (Exception ex)
				{
					print(ex.ToString());
					return;
				}
			}
		}




		private void print(String text)
		{
			rtb_log.AppendText("\n" + DateTime.Now.ToString() + ": " + text);
		}


	}
}
