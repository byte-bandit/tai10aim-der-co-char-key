using System;
using System.Diagnostics;
using System.ComponentModel;
using System.Collections.Generic;
using System.Linq;
using System.Text;


using Microsoft.Xna.Framework.Net;

using Classes.Pipeline;

using Lidgren.Network;




enum ServerStatus
{
	LOBBY,
	GAME
}





enum PacketTypes
{
	LOGIN,
	ENV_INFO,
	PLAYER_INFO,
	BROADCAST,
	LOBBY,
	GAME_STATE_CHANGED
}



namespace Classes.Net
{
	class NetworkManager
	{
		private static NetClient client;
		private static NetPeerConfiguration config;
		private static List<String> searchResults;

		private static bool isHost;
		private static bool isEveryoneReady;

		private static int connectedPlayers;
		private static Process hostProcess;


		private static Gamer myProfile = new Gamer("UNKOWN");
		private static List<Gamer> connectedGamers = new List<Gamer>();
		private static NetConnection con;

		private static ServerStatus gameState = ServerStatus.LOBBY;
		private static int UpdateInterval = 60;
		private static int UpdateStep = 0;





		public static void Initialize()
		{
			config = new NetPeerConfiguration("CoCharKey");
			client = new NetClient(config);
			connectedPlayers = 0;
			client.Start();
		}




        public static String GamerName
        {
            get { return myProfile.Name; }
        }




		public static Gamer Profile
		{
			get { return myProfile; }
		}



		public static List<Gamer> ConnectedGamers
		{
			get { return connectedGamers; }
		}




		public static bool setPlayerName()
		{
			String s = Microsoft.VisualBasic.Interaction.InputBox("Wie möchtest du dich nennen?", "Spieler benennen...");

			if (s == "")
			{
				return false;
			}
			else
			{
				myProfile.Name = s;
				return true;
			}
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
            ProcessStartInfo start = new ProcessStartInfo();
            start.FileName = "GameServer.exe";
            start.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden ;
            hostProcess = Process.Start(start);

			while (!connect("localhost"))
			{
				System.Threading.Thread.Sleep(100);
			}

			isHost = true;
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


		public static bool connect(String ip)
		{
			try
			{
				NetOutgoingMessage outmsg = client.CreateMessage();
				outmsg.Write((byte)PacketTypes.LOGIN);
				outmsg.Write(myProfile.Name);
				con = client.Connect(ip, 14242, outmsg);
				return true;
			}
			catch (Exception ex)
			{
				Debug.Print(ex.ToString());
				return false;
			}
			
		}



		public static void leaveSession()
		{
			try
			{
				client.Disconnect("Bye");
			}
			catch (Exception ex)
			{
				System.Diagnostics.Debug.Print(ex.ToString());
			}

			Debug.Print("New Status: " + con.Status.ToString());
			con = null;
		}





		public static void startGame()
		{
			NetOutgoingMessage msg = client.CreateMessage();
			msg.Write((byte)PacketTypes.GAME_STATE_CHANGED);
			msg.Write((byte)ServerStatus.GAME);
			client.SendMessage(msg, NetDeliveryMethod.ReliableOrdered);
		}








		public static void Update()
		{





			//+++
			//Not-net
			//+++
			if (gameState == ServerStatus.LOBBY)
			{
				isEveryoneReady = true;
				foreach (Gamer g in connectedGamers)
				{
					if (!g.ready)
					{
						isEveryoneReady = false;
					}
				}
			}





			//+++
			//GET
			//+++
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



					case NetIncomingMessageType.Error:
						try
						{
							System.Diagnostics.Debug.Print(inc.ReadString());
						}
						catch (Exception ex)
						{
							System.Diagnostics.Debug.Print(ex.ToString());
						}

						break;



					//DATA TYPES
					case NetIncomingMessageType.Data :
						byte packetidentifier = inc.ReadByte();

						if ((PacketTypes)packetidentifier == PacketTypes.LOGIN)
                        {
                            try
                            {
                                connectedPlayers = inc.ReadInt32();
                            }
                            catch (Exception ex)
                            {
                                System.Diagnostics.Debug.Print(ex.ToString());
                            }
							return;
                        }


						if ((PacketTypes)packetidentifier == PacketTypes.LOBBY)
						{
							try
							{
								connectedPlayers = inc.ReadInt32();
								connectedGamers = new List<Gamer>();
								for (int a = 0; a < connectedPlayers; a++)
								{
									Gamer gmp = new Gamer(inc.ReadString());
									gmp.ready = inc.ReadBoolean();
									connectedGamers.Add(gmp);
								}
							}
							catch (Exception ex)
							{
								System.Diagnostics.Debug.Print(ex.ToString());
							}
						}



						if ((PacketTypes)packetidentifier == PacketTypes.GAME_STATE_CHANGED)
						{
							try
							{
								gameState = (ServerStatus)inc.ReadByte();
							}
							catch (Exception ex)
							{
								System.Diagnostics.Debug.Print(ex.ToString());
							}
						}
						break;
				}
			}






			//+++
			//POST
			//+++
			if (UpdateStep < UpdateInterval )
			{
				UpdateStep ++;
			}
			else
			{
				UpdateStep = 0;
				NetOutgoingMessage msg = client.CreateMessage();
				msg.Write((byte)PacketTypes.LOBBY);
				msg.Write(Profile.Name);
				msg.Write(Profile.ready);
				client.SendMessage(msg, NetDeliveryMethod.ReliableOrdered);
			}
		}
	}
}
