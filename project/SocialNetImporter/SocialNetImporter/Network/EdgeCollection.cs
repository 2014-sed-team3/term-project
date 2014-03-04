using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Smrf.NodeXL.GraphDataProviders.Facebook
{
    public class EdgeCollection : HashSet<Edge>
    {
        public EdgeCollection()
            : base()
        {
            
        }

        public Edge this[string Vertex1ID, string Vertex2ID]
        {
            get { return this.FirstOrDefault(x => x.Vertex1.ID.Equals(Vertex1ID) && x.Vertex2.ID.Equals(Vertex2ID)); }
            set { this.RemoveWhere(x => x.Vertex1.ID.Equals(Vertex1ID) && x.Vertex2.ID.Equals(Vertex2ID)); this.Add(value); }           
        }

        public Edge this[Vertex Vertex1, Vertex Vertex2]
        {
            get { return this.FirstOrDefault(x => x.Vertex1.Equals(Vertex1) && x.Vertex2.Equals(Vertex2)); }
            set { this.RemoveWhere(x => x.Vertex1.Equals(Vertex1) && x.Vertex2.Equals(Vertex2)); this.Add(value); }
        }
        
    }
}
