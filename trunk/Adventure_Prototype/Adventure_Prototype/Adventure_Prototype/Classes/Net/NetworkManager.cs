using System;
using System.Diagnostics;
using System.ComponentModel;
using System.Collections.Generic;
using System.Linq;
using System.Text;


using Microsoft.Xna.Framework.Net;

using Classes.Pipeline;

using Lidgren.Network;




enum PacketTypes
{
	LOGIN,
	MOVE,
	WORLDSTATE
}



namespace Classes.Net
{
	class NetworkManager
	{
		private static NetClient client;
		private static System.Timers.Timer timer;
		private static NetPeerConfiguration config;
		private static List<String> searchResults;
		private static bool isHost;
		private static bool isEveryoneReady;
		private static int connectedPlayers;
		private static Process hostProcess;

		public static void Initialize()
		{
			config = new NetPeerConfiguration("CoCharKey");
			client = new NetClient(config);
			connectedPlayers = 0;
			client.Start();
		}



		public static int ConnectedPlayers
		{
			get { return connectedPlayers; }
		}




		public static bool IsHost
		{
			get { return isHost; }
		}



		public static bool IsEveryoneReady
		{
			get { return isEveryoneReady ; }
		}



		public static List<String> availibleServers
		{
			get { return searchResults; }
		}



		public static void lookForServers()
		{
			config.EnableMessageType(NetIncomingMessageType.DiscoveryResponse);
			client.DiscoverLocalPeers(14242);
			searchResults = new List<String>();
		}


		public static void createSession()
		{
			//ProcessStartInfo start = new ProcessStartInfo();
			//start.FileName = "GameServer.exe";
			//start.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
			//hostProcess = Process.Start(start);

			//config.EnableMessageType(NetIncomingMessageType.DiscoveryResponse);
			//NetIncomingMessage inc;

			//while (true)
			//{
			//    client.DiscoverKnownPeer("localhost", 14242);
			//    if ((inc = client.ReadMessage()) == null)
			//    {
			//        continue;
			//    }
			//    if (inc.MessageType == NetIncomingMessageType.DiscoveryResponse)
			//    {
			//        break;
			//    }
			//}

			connect("localhost");
		}



		public static void quitSession()
		{
			leaveSession();
			try
			{
				hostProcess.Kill();
			}
			catch (Exception ex)
			{
				System.Diagnostics.Debug.Print(ex.ToString());
			}
		}


		public static void connect(String ip)
		{

			NetOutgoingMessage outmsg = client.CreateMessage();
			outmsg.Write((byte)PacketTypes.LOGIN);
			client.Connect(ip, 14242, outmsg);
		}



		public static void leaveSession()
		{
			client.Disconnect("QUIT");
		}



		public static void Update()
		{
			NetIncomingMessage inc;
			while ((inc = client.ReadMessage()) != null)
			{
				switch (inc.MessageType)
				{
					//Ping Response
					case NetIncomingMessageType.DiscoveryResponse:
						try
						{
							searchResults.Add(inc.SenderEndpoint.Address.ToString());
						}
						catch (Exception ex)
						{
							System.Diagnostics.Debug.Print(ex.ToString());
						}
						
						break;
				}
			}
		}
	}
}
