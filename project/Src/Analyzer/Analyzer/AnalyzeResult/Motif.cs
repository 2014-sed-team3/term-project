using Smrf.NodeXL.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Analyzer
{
    public abstract class Motif : Object
    {
        

        public Motif()
        {
            // (Do nothing.)

            // AssertValid();
        }
        

        public abstract IVertex[]
        VerticesInMotif
        {
            get;
        }

        

        public abstract String
        CollapsedAttributes
        {
            get;
        }


        

        
    }
}
