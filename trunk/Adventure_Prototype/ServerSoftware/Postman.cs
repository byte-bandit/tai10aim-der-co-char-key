using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Lidgren.Network;

namespace ServerSoftware
{
	class Postman
	{
		public static void checkMails(NetIncomingMessage inc)
		{
			switch (inc.MessageType)
			{
				case NetIncomingMessageType.ConnectionApproval :
					Doorguard.CheckIn(inc);
					break;
			}

		}
	}
}
