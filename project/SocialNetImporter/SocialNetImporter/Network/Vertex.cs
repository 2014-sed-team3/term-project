using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Smrf.AppLib;
using Facebook;

namespace Smrf.NodeXL.GraphDataProviders.Facebook
{
    public class Vertex
    {
        public string ID { get; set; }
        public string Name {get; set; }
        public string Type { get; set; }
        public string ToolTip { get; set; }
        public AttributesDictionary<JSONObject> Attributes { get; set; }

        public Vertex(string ID, string Name, string Type)
        {
            this.ID = ID;
            this.Name = Name;
            this.Type = Type;
            Attributes = new AttributesDictionary<JSONObject>();            
        }

        public Vertex(string ID, string Name, string Type, AttributesDictionary<JSONObject> Attributes)
        {
            this.ID = ID;
            this.Name = Name;            
            this.Type = Type;
            this.Attributes = Attributes;
        }

        public override int GetHashCode()
        {
            return (ID.GetHashCode());
        }
        public override bool Equals(object obj)
        {
            return Equals(obj as Vertex);
        }
        private bool Equals(Vertex obj)
        {
            return (obj != null &&                    
                    obj.ID.Equals(this.ID));
        }
    }
}
