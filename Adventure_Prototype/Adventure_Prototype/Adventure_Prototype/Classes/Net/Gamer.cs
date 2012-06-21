using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Lidgren.Network;

namespace Classes.Net
{
	public class Gamer
	{

		private String name;
		private bool ready;
		private String token;
		private Player puppet;
		private NetConnection connection;







		/// <summary>
		/// Creates a new Gamer.
		/// </summary>
		/// <param name="_name">The Login Name of the Gamer.</param>
		/// <param name="con">The Network Connection of the connecting Gamer.</param>
		public Gamer(string _name, NetConnection con)
		{
			this.name = _name;
			this.connection = con;
			if (this.name != null && this.connection != null)
			{
				this.token = buildTokenString();
			}
		}




		/// <summary>
		/// Builds the individual Token for this Gamer
		/// </summary>
		private String buildTokenString()
		{
			String ret = this.name + this.connection.RemoteEndpoint.Address.ToString() + DateTime.Now.ToString().Replace(".","").Replace(":","").Replace(" ", "");

			try
			{
				System.Security.Cryptography.SHA512 hash = new System.Security.Cryptography.SHA512Managed  ();
				System.Text.ASCIIEncoding enc = new System.Text.ASCIIEncoding();
				Byte[] built = hash.ComputeHash(enc.GetBytes(ret.ToCharArray()));
				String final = enc.GetString(built);
				return final;
			}
			catch (Exception ex)
			{
				System.Diagnostics.Debug.Print(ex.ToString());
			}
			
			return System.Security.Cryptography.SHA256.Create(ret).ToString();
		}





		/// <summary>
		/// Gets or Sets the Name of the Gamer
		/// </summary>
		public String Name
		{
			get { return this.name; }
			set { this.name = value; }
		}





		/// <summary>
		/// Gets the individual Token of the Gamer
		/// </summary>
		public String Token
		{
			get { return this.token; }
			set { this.token = value; }
		}





		/// <summary>
		/// Gets a reference of the Player the Gamer is controlling.
		/// </summary>
		public Player Puppet
		{
			get { return this.puppet; }
			set { this.puppet = value; }
		}






		/// <summary>
		/// Gets or Sets the readyflag of the Gamer
		/// </summary>
		public Boolean Ready
		{
			get { return this.ready; }
			set { this.ready = value; }
		}






		/// <summary>
		/// Gets the Connection Data of the Gamer
		/// </summary>
		public NetConnection Connection
		{
			get { return this.connection; }
			set { this.connection = value; }
		}


	}
}
