using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using Lidgren.Network;

namespace ServerSoftware
{
	public static class Radio
	{

		/*
		 * M A I N   U P D A T E   L O O P
		 */
		public static void update(Form1 parent)
		{
			//Are we in Lobby mode
			if (parent.serverMode == ServerStatus.LOBBY)
			{
				foreach (Peer p in parent.connectedPeers)
				{
					NetOutgoingMessage msg = parent.server.CreateMessage();

					msg.Write((byte)PacketTypes.LOBBY);
					msg.Write(parent.connectedPeers.Count);

					foreach (Peer p2 in parent.connectedPeers)
					{
						msg.Write(p2.Name);
						msg.Write(p2.Token);
						msg.Write(p2.Ready);
					}

					p.Connection.SendMessage(msg, NetDeliveryMethod.ReliableOrdered);
				}
			}
			else
			{
				//We are in game Mode
				foreach (Peer p in parent.connectedPeers)
				{
					NetOutgoingMessage msg = parent.server.CreateMessage();

					msg.Write((byte)PacketTypes.BROADCAST);
					msg.Write(parent.connectedPeers.Count);

					foreach (Peer p2 in parent.connectedPeers)
					{
						msg.Write(p2.Token);
						msg.Write(p2.X);
						msg.Write(p2.Y);
						msg.Write(p2.Animation_Cycle);
					}

					p.Connection.SendMessage(msg, NetDeliveryMethod.ReliableOrdered);
				}
			}
		}

	}
}
