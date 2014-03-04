using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Smrf.NodeXL.GraphDataProviders.Facebook
{
    public class VertexCollection : HashSet<Vertex>
    {
        public VertexCollection()
            : base()
        {
            
        }

        public VertexCollection(IEnumerable<Vertex> oVertices)
            : base(oVertices)
        {

        }

        public Vertex this[string ID]
        {
            get { return this.FirstOrDefault(x => x.ID.Equals(ID)); }
            set { this.RemoveWhere(x => x.ID.Equals(ID)); this.Add(value); }           
        }

        public Vertex this[int index]
        {            
            get { return this.ElementAt(index); }
            set { this.Remove(this.ElementAt(index)); this.Add(value); }
        }        
    }
}
