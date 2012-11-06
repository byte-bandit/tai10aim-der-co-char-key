using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Classes.Events;

namespace Classes.Events
{
	public class Event
	{
        private string id;
        private List<Action> actions = new List<Action>();
        private List<String> dependencies = new List<String>();
        private Boolean executed;
        private combtyp combTyp;
        private string combitem;
        private string alternative;
		private Boolean repeatable;
        public enum combtyp
        {
            WorldObject,
            InventoryItem,
            Unknown
        }

        #region Properties

		public bool Repeatable
		{
			get { return this.repeatable; }
			set { this.repeatable = value; }
		}

        public string ID
        {
            get { return this.id; }
            set { this.id = value; }
        }

        public combtyp CombTyp
        {
            get { return this.combTyp; }
            set { this.combTyp = value; }
        }
        public string CombItem
        {
            get { return this.CombItem; }
            set { this.CombItem = value; }
        }

        public List<Action> Actions
        {
            get {return this.actions; }
            set {this.actions = value;}
        }

        public List<String> Dependencies
        {
            get { return this.dependencies; }
            set { this.dependencies = value; }
        }

        public Boolean Executed
        {
            get { return this.executed; }
            set { this.executed = value; }
        }

        public string Alternative
        {
            get { return this.alternative; }
            set { this.alternative = value; }
        }

        #endregion

        public Event()
        {
            this.executed = false;
			this.repeatable = false;
            this.alternative = "Geht nicht.";
            this.combTyp = combtyp.Unknown;
        }

        public Event(string id)
        {
            this.id = id;
            this.executed = false;
			this.repeatable = false;
            this.alternative = "Geht nicht.";
            this.combTyp = combtyp.Unknown;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns>returns true if all dependencies were executed</returns>
        public Boolean checkDependencies()
        {
            Boolean check = true ; 
            foreach(String s in this.dependencies)
            {
				Event e = Events.EventManager.EventLibrary[s.ToLower()];

				if (e == null)
				{
					continue;
				}
                check = check && e.executed;
            }
                return check;
        }
    }
}
