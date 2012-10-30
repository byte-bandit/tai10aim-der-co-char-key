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
        private List<Action> actions;
        private List<Event> dependencies;
        private Boolean executed;
        private string alternative;

        #region Properties

        public string ID
        {
            get { return this.id; }
            set { this.id = value; }
        }

        public List<Action> Actions
        {
            get {return this.actions; }
            set {this.actions = value;}
        }

        public List<Event> Dependencies
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
            this.alternative = "Geht nicht.";
        }

        public Event(string id)
        {
            this.id = id;
            this.executed = false;
            this.alternative = "Geht nicht.";
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns>returns true if all dependencies were executed</returns>
        public Boolean checkDependencies()
        {
            Boolean check = true ; 
            foreach(Event e in this.dependencies)
            {
                check = check && e.executed;
            }
                return check;
        }
    }
}
