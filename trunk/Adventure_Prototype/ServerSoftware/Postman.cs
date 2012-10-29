using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Lidgren.Network;

namespace ServerSoftware
{
	class Postman
	{
		public static void checkMails(NetIncomingMessage inc, Form1 parent)
		{
			//parent.print("Incomming " + inc.MessageType.ToString());
			switch (inc.MessageType)
			{
				case NetIncomingMessageType.ConnectionApproval :
					System.Diagnostics.Debug.Print("Postman: Got a " + inc.MessageType.ToString()+", ->Doorguard.CheckIn");
					Doorguard.CheckIn(inc, parent);
					break;

				case NetIncomingMessageType.StatusChanged :
					System.Diagnostics.Debug.Print("Postman: Got a " + inc.MessageType.ToString()+", ->Doorguard.CheckState");
					Doorguard.CheckState(inc, parent);
					break;

				case NetIncomingMessageType.DiscoveryRequest:
					System.Diagnostics.Debug.Print("Postman: Got a " + inc.MessageType.ToString() + ", ->Doorguard.CheckTheDoor");
					Doorguard.CheckTheDoor(inc, parent);
					break;
				case NetIncomingMessageType.WarningMessage:
					System.Diagnostics.Debug.Print("Postman: Got a " + inc.MessageType.ToString() + ", I'm gonna open it:");
					System.Diagnostics.Debug.Print("Postman: " + inc.ReadString());
					break;
				case NetIncomingMessageType.Data:
					System.Diagnostics.Debug.Print("Postman: Got a " + inc.MessageType.ToString() + ", I'll process that shit!");
					processDatShit(inc, parent);
					break;
			}

		}


		//Used to process all incomming Data Packets
		private static void processDatShit(NetIncomingMessage inc, Form1 parent)
		{
			byte packet_id = inc.ReadByte();
			PacketTypes packet_type = (PacketTypes)packet_id;

			switch (packet_type)
			{

				//Gets send peers info
				case PacketTypes.LOBBY:
					String n_token = inc.ReadString();

					foreach (Peer p in parent.connectedPeers)
					{
						if (p.Token == n_token)
						{
							p.Name = inc.ReadString();
							p.Ready = inc.ReadBoolean();
							break;
						}
					}
				break;



				case PacketTypes.ENV_INFO:

					String n_token_w = inc.ReadString();
					float w_X = inc.ReadFloat();
					float w_Y = inc.ReadFloat();

					foreach (Peer p in parent.connectedPeers)
					{
						if (p.Token != n_token_w)
						{
							NetOutgoingMessage outm = parent.server.CreateMessage();
							outm.Write((byte)PacketTypes.ENV_INFO);
							outm.Write(n_token_w);
							outm.Write(w_X);
							outm.Write(w_Y);

							p.Connection.SendMessage(outm, NetDeliveryMethod.ReliableOrdered);
						}
					}

				break;



				case PacketTypes.BROADCAST:

					String n_token_2 = inc.ReadString();
					foreach (Peer p in parent.connectedPeers)
					{
						if (p.Token == n_token_2)
						{
							p.X = inc.ReadFloat();
							p.Y = inc.ReadFloat();
							p.Animation_Cycle = inc.ReadByte();
							break;
						}
					}
				break;







				case PacketTypes.PLAYER_SAY:

					String n_token_s = inc.ReadString();
					String n_text = inc.ReadString();
					float n_X = inc.ReadFloat();
					float n_Y = inc.ReadFloat();
					uint n_c = inc.ReadUInt32();

					foreach (Peer p in parent.connectedPeers)
					{
						if (p.Token != n_token_s)
						{
							NetOutgoingMessage msg = parent.server.CreateMessage();
							msg.Write((byte)PacketTypes.PLAYER_SAY);
							msg.Write(n_token_s);
							msg.Write(n_text);
							msg.Write(n_X);
							msg.Write(n_Y);
							msg.Write((uint)n_c);
							p.Connection.SendMessage(msg, NetDeliveryMethod.ReliableOrdered);
						}
					}

					break;



				case PacketTypes.GAME_STATE_CHANGED:

					ServerStatus n_state = (ServerStatus)inc.ReadByte();
					parent.serverMode = n_state;
					parent.print("Changed Game Mode to: " + n_state.ToString());

					foreach (Peer p in parent.connectedPeers)
					{
						NetOutgoingMessage msg = parent.server.CreateMessage();
						msg.Write((byte)PacketTypes.GAME_STATE_CHANGED);
						msg.Write((byte)n_state);
						p.Connection.SendMessage(msg, NetDeliveryMethod.ReliableOrdered);
					}
					break;
			}
		}
	}
}
