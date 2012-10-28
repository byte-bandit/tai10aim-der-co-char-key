/*******************************************************************************
 * Game Server for Co-Char-Key!
 * 
 * Author: Christian Lohr
 * 
 * ******************************************************************************
 */





using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

using Lidgren.Network;

using Classes;
using Classes.Net;

using Microsoft.Xna.Framework;




namespace GameServer
{


    class Program
    {


		/**********************************************************************************************
		 * Global Variable Definitions
		 * ********************************************************************************************
		 */
        static NetServer Server;				//The Server Object itself
        static NetPeerConfiguration Config;		//Used to store the server config
		static ServerStatus serverStatus;		//Simpleton to check whether we're in game or lobby.
		static List<Gamer> connectedGamers = new List<Gamer>();	//Stores all our players


		


		static void Main(string[] args)
        {
			//Delete the old log file
			if (System.IO.File.Exists("GameServer.log"))
				System.IO.File.Delete("GameServer.log");
			//Initiate our logging
			Trace.Listeners.Add(new TextWriterTraceListener("GameServer.log"));
			Trace.AutoFlush = true;

			//Register the closing Event
			AppDomain.CurrentDomain.ProcessExit += new EventHandler(DisposeServer);






			//****************************************************************************
			//First, we start the Server itself, socket it to the specific port and give
			//it a certain ID to identify itself with the game.
			//****************************************************************************

			if (!startServer())
			{
				print("Unable to start server! Quitting");
				return;
			}




			//****************************************************************************
			//More Internal Variables
			//****************************************************************************

			DateTime time = DateTime.Now;							//Used for time measurement
			TimeSpan interval = new TimeSpan(0, 0, 0, 0, 30);		//This is the interval for our network communication, currently 30ms
            NetIncomingMessage inc;									//This holds incoming messages





			print("Binding to Port " + Server.Configuration.Port.ToString());
			print("Server is up and running, we are ready to roll...");




			//****************************************************************************
			//Sending the creation signal
			//****************************************************************************

			NetOutgoingMessage ret = Server.CreateMessage();
			ret.Write(true);
			Server.SendDiscoveryResponse(ret, new System.Net.IPEndPoint(0000,Server.Configuration.Port));












			/****************************************************************************
			 * 
			 * M A I N   L O G I C   L O O P
			 * 
			 ****************************************************************************/
            while (true)
             {
				//Sleep a little to give CPU Power to the rest of the System
				System.Threading.Thread.Sleep(50);
				


				//Check to see whether we have new messages in our mailbox
                if ((inc = Server.ReadMessage()) != null)
                {

					print(inc.MessageType.ToString());
					

                    switch (inc.MessageType)
                    {

                        case NetIncomingMessageType.ConnectionApproval:		//Login
							loginPlayer(inc);
                            break;


						case NetIncomingMessageType.DiscoveryRequest :		//Ping
							handleDiscoveryRequest(inc);
							break;

						case NetIncomingMessageType.ErrorMessage:			//Error Msg
							print("Error Message, Length :" + inc.LengthBytes.ToString() + " Bytes");
							print(inc.ReadString());
							print("------------------------------------------------------------------");
							break;


						case NetIncomingMessageType.DebugMessage:			//Debug
							print("Debug Message, Length :" + inc.LengthBytes.ToString() + " Bytes");
							print(inc.ReadString());
							print("------------------------------------------------------------------");
							break;


						case NetIncomingMessageType.WarningMessage:			//Warning
							print("Warning Message, Length :" + inc.LengthBytes.ToString() + " Bytes");
							print(inc.ReadString());
							print("------------------------------------------------------------------");
							break;

						case NetIncomingMessageType.UnconnectedData:		//Logout | Connection Lost
							inc.SenderConnection.Disconnect("Bye");
							print(inc.SenderConnection.ToString() + " disconnected.");
							break;

                        case NetIncomingMessageType.StatusChanged:			//Status Changed
							handleStatusChange(inc);
                            break;








						/*##################################################
						 * DATA Packages
						 * 
						 * These packages are categorized in several types of
						 * communication protocols and processes.
						 * They usually begin with a (byte)ServerStatus Type
						 *##################################################
						 */
						case NetIncomingMessageType.Data:

							byte packageID = inc.ReadByte();

							switch ((PacketTypes)packageID)
							{
								case PacketTypes.LOBBY:				//Lobby Input - Updates during the Lobby (ready, etc...)
									handleData_Lobby(inc);
									break;

								case PacketTypes.PLAYER_INFO:		//Player Info - Updates for all players
									break;

								case PacketTypes.GAME_STATE_CHANGED:	//Game State - Switch between Lobby and Game Mode
									handleData_GameStateChanged(inc);
									break;
							}
							break;

                        default:
							print("Unknown Message Type: " + inc.MessageType.ToString() + inc.LengthBytes.ToString() );
                            break;
					}



					_BROADCAST(time, interval);
				}
			}
        }



		static void _BROADCAST(DateTime time, TimeSpan interval)
		{

			try
			{

				if ((time + interval) < DateTime.Now)
				{

					if (serverStatus == ServerStatus.LOBBY)
					{
						NetOutgoingMessage msg = Server.CreateMessage();
						Config.EnableMessageType(NetIncomingMessageType.Data);

						//Write byte
						msg.Write((byte)PacketTypes.LOBBY);

						//Number of players
						msg.Write(Server.Connections.Length);

						//Player Names
						foreach (Gamer g in connectedGamers)
						{
							msg.WriteAllProperties(g);
						}

						Server.SendMessage(msg, Server.Connections, NetDeliveryMethod.ReliableOrdered, 0);

						return;
					}




					if (Server.ConnectionsCount != 0)
					{
						if (serverStatus == ServerStatus.GAME)
						{
							NetOutgoingMessage outmsg = Server.CreateMessage();
							outmsg.Write((byte)PacketTypes.BROADCAST);

							foreach (Gamer g in connectedGamers )
							{
								outmsg.Write(g.Token);
								outmsg.Write(g.Name);
								outmsg.WriteAllProperties(g.Puppet);
							}

							Server.SendMessage(outmsg, Server.Connections, NetDeliveryMethod.ReliableOrdered, 0);
						}
					}
					// Update current time
					time = DateTime.Now;
				}
			}
			catch (Exception ex)
			{
				print("Broadcast failed: " + ex.ToString());
			}
		}







		static void handleData_GameStateChanged(NetIncomingMessage inc)
		{
			try
			{
				serverStatus = (ServerStatus)inc.ReadByte();

				NetOutgoingMessage msg = Server.CreateMessage();
				msg.Write((byte)PacketTypes.GAME_STATE_CHANGED);
				msg.Write((byte)serverStatus);

				Server.SendMessage(msg, Server.Connections, NetDeliveryMethod.ReliableOrdered, 0);
			}
			catch(Exception ex)
			{
				print("GameStateChanged failed: " + ex.ToString());
			}
		}






		static void handleData_Lobby(NetIncomingMessage inc)
		{
			String name = inc.ReadString();
			foreach (Gamer  g in connectedGamers )
			{
				if (g.Connection == inc.SenderConnection && g.Name == name)
				{
					g.Name = name;
					bool old = g.Ready;
					g.Ready = inc.ReadBoolean();
					if (g.Ready != old)
					{
						print(g.Name + " is ready: " + g.Ready.ToString());
					}
					break;
				}
			}
		}









		static void handleStatusChange(NetIncomingMessage inc)
		{
			print(inc.SenderEndpoint.Address.ToString() + "'s Status changed to: " + inc.SenderConnection.Status.ToString());

			if (inc.SenderConnection.Status == NetConnectionStatus.Disconnected || inc.SenderConnection.Status == NetConnectionStatus.Disconnecting)
			{
				foreach (Gamer g in connectedGamers )
				{
					if (g.Connection == inc.SenderConnection)
					{
						connectedGamers.Remove(g);
						break;
					}
				}
			}
		}









		static void handleDiscoveryRequest(NetIncomingMessage inc)
		{
			print(inc.SenderEndpoint.Address.ToString() + " pinged me!");
			NetOutgoingMessage outmsg2 = Server.CreateMessage();
			outmsg2.Write("Speak friend and enter");
			Server.SendDiscoveryResponse(outmsg2, inc.SenderEndpoint);
		}










		static bool loginPlayer(NetIncomingMessage inc)
		{
			try
			{
				if (inc.ReadByte() == (byte)PacketTypes.LOGIN)
				{
					print(inc.SenderEndpoint.Address.ToString() + " trying to connect...");

					inc.SenderConnection.Approve();

					//Initialize Player here
					Gamer tmp = new Gamer(inc.ReadString(), inc.SenderConnection);
					tmp.Ready = false;
					connectedGamers.Add(tmp);

					NetOutgoingMessage outmsg = Server.CreateMessage();

					outmsg.Write((byte)PacketTypes.LOGIN);
					outmsg.Write(Server.Connections.Length);
					outmsg.WriteAllProperties(tmp);

					Server.SendMessage(outmsg, inc.SenderConnection, NetDeliveryMethod.ReliableOrdered, 0);

					return true;
				}
				else
				{
					return false;
				}
			}
			catch (Exception ex)
			{
				print("Login failed: " + ex.ToString());
				return false;
			}
		}









		static void DisposeServer(Object sender, EventArgs e)
		{
			//Don't forget to flush the log!
			Trace.Flush();
		}










		static bool startServer()
		{
			//Boot in Lobby mode
			serverStatus = ServerStatus.LOBBY;

			//Setup Configuration
			Config = new NetPeerConfiguration("CoCharKey");	//Make sure the Application ID is the same on Server & Game!
			Config.Port = 14242;
			Config.MaximumConnections = 2;
			Config.EnableMessageType(NetIncomingMessageType.ConnectionApproval);
			Config.EnableMessageType(NetIncomingMessageType.DiscoveryRequest);


			// Create new Server, load in config
			try
			{
				Server = new NetServer(Config);
				Server.Start();
			}
			catch (Exception ex)
			{
				System.Diagnostics.Debug.Print(ex.ToString());
				return false;
			}


			print("Server started!");
			return true;
		}









		static void print(String text)
		{
			Console.WriteLine(DateTime.Now.ToString() + ": " + text);
			Trace.WriteLine(DateTime.Now.ToString() + ": " + text);
		}



    }
}
