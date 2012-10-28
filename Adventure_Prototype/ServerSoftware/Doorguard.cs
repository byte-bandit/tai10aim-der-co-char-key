using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Lidgren.Network;

using Classes.Net;

namespace ServerSoftware
{
	class Doorguard
	{




		public static bool CheckIn(NetIncomingMessage inc, Form1 parent)
		{
			try
			{
				parent.print(inc.SenderEndpoint.Address.ToString() + " trying to connect...");

				inc.SenderConnection.Approve();

				////Initialize Player here
				Peer tmp = new Peer(inc.ReadString(), inc.SenderConnection);
				tmp.Ready = false;
				parent.connectedPeers.Add(tmp);

				NetOutgoingMessage outmsg = parent.server.CreateMessage();
				
				outmsg.Write((byte)PacketTypes.LOGIN);
				outmsg.Write(parent.server.Connections.Length);
				outmsg.Write(tmp.Token);
				outmsg.Write(tmp.Name);

				parent.server.SendMessage(outmsg, inc.SenderConnection, NetDeliveryMethod.ReliableOrdered, 0);

				parent.lb_cp.Items.Add(tmp.Name);
				parent.print(tmp.Name + " [" + inc.SenderEndpoint.Address.ToString() + "] connected to the Server!");

				return true;

			}
			catch (Exception ex)
			{
				parent.print(ex.ToString());
				return false;
			}
		}




		public static void CheckState(NetIncomingMessage inc, Form1 parent)
		{
			parent.print(inc.SenderEndpoint.Address.ToString() + " has reported a status change!");


			byte new_status_byte = inc.ReadByte();
			NetConnectionStatus new_status = (NetConnectionStatus) new_status_byte;

			parent.print(inc.SenderEndpoint.Address.ToString() + "'s new status: " + new_status.ToString());

			switch(new_status)
			{
				case NetConnectionStatus.Disconnecting:
					inc.SenderConnection.Disconnect("Bye");
					break;
			}

			
		}





		public static void CheckTheDoor(NetIncomingMessage inc, Form1 parent)
		{
			parent.print(inc.SenderEndpoint.Address.ToString() + " pinged me!");

			NetOutgoingMessage outmsg = parent.server.CreateMessage();
			outmsg.Write("Speak friend and enter");

			parent.server.SendDiscoveryResponse(outmsg, inc.SenderEndpoint);
		}
	}
}
