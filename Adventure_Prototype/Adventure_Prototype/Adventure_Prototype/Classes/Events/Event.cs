using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Classes.Events
{
	class Event
	{

		private Object sender;
		private Type myType;


		public enum Type
		{
			CLICK,
			DOUBLECLICK,
			RIGHTCLICK
		}
		
		/// <summary>
		/// Creates a new Event
		/// </summary>
		public Event(Object sender, Type _type)
		{
			this.myType = _type;
			this.sender = sender;
		}



		public bool check()
		{
			switch (this.myType)
			{
				case Type.CLICK:
					
					break;


				case Type.DOUBLECLICK:
					//NO DOUBLE CLICK SUPPORT SO FAR
					break;


				case Type.RIGHTCLICK:

					break;
			}
			return false;
		}

	}
}
