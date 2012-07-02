using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Lidgren.Network;

namespace ServerSoftware
{
	class Doorguard
	{
		public static void CheckIn(NetIncomingMessage inc)
		{
			try
			{
				//Form1.print(inc.SenderEndpoint.Address.ToString() + " trying to connect...");

				//inc.SenderConnection.Approve();

				////Initialize Player here
				//Gamer tmp = new Gamer(inc.ReadString(), inc.SenderConnection);
				//tmp.Ready = false;
				//connectedGamers.Add(tmp);

				//NetOutgoingMessage outmsg = Server.CreateMessage();

				//outmsg.Write((byte)PacketTypes.LOGIN);
				//outmsg.Write(Server.Connections.Length);
				//outmsg.WriteAllProperties(tmp);

				//Server.SendMessage(outmsg, inc.SenderConnection, NetDeliveryMethod.ReliableOrdered, 0);

				//return true;

			}
			catch (Exception ex)
			{
				//print("Login failed: " + ex.ToString());
				//return false;
			}
		}
	}
}
