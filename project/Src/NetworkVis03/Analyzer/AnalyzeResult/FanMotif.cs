using Smrf.NodeXL.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Analyzer
{
    public class FanMotif : Motif, IGroup
    {
        

        public FanMotif(IVertex headVertex,IVertex[] leafVertices)
        {
            m_oHeadVertex = headVertex;
            m_aoLeafVertices = leafVertices;
            m_dArcScale = 1.0;

        }

        public IVertex[] getVertexWithin
        {
            get {
                IVertex[] vs = new IVertex[m_aoLeafVertices.Length+1];
                m_aoLeafVertices.CopyTo(vs, 0);
                vs[m_aoLeafVertices.Length] = HeadVertex;
                return vs;
            }
        }

        public string getDescription
        {
            get { return String.Format("Community with {0} vertices", getVertexWithin.Length); }
        }

        //*************************************************************************
        //  Property: HeadVertex
        //
        /// <summary>
        /// Gets the fan's head vertex.
        /// </summary>
        ///
        /// <value>
        /// The fan's head vertex, as an <see cref="IVertex" />.
        /// </value>
        //*************************************************************************

        public IVertex
        HeadVertex
        {
            get
            {

                return (m_oHeadVertex);
            }
        }

        //*************************************************************************
        //  Property: LeafVertices
        //
        /// <summary>
        /// Gets the fan's leaf vertices.
        /// </summary>
        ///
        /// <value>
        /// The fan's leaf vertices, as an array of at least two <see
        /// cref="IVertex" /> objects.
        /// </value>
        //*************************************************************************

        public IVertex[]
        LeafVertices
        {
            get
            {


                return (m_aoLeafVertices);
            }
        }

        //*************************************************************************
        //  Property: ArcScale
        //
        /// <summary>
        /// Gets or sets the scale factor to use when determining the arc of the
        /// vertex that represents the collapsed motif.
        /// </summary>
        ///
        /// <value>
        /// An arc scale factor between 0 and 1.0.  The code that draws the
        /// collapsed motif should draw a minimum arc when the scale factor is 0,
        /// and a maximum arc when the scale factor is 1.0.  The default is 1.0.
        /// </value>
        //*************************************************************************

        public Double
        ArcScale
        {
            get
            {


                return (m_dArcScale);
            }

            set
            {
                m_dArcScale = value;

            }
        }

        //*************************************************************************
        //  Property: VerticesInMotif
        //
        /// <summary>
        /// Gets the motif's vertices.
        /// </summary>
        ///
        /// <value>
        /// The motif's vertices, as an array of <see cref="IVertex" /> objects.
        /// </value>
        //*************************************************************************

        public override IVertex[]
        VerticesInMotif
        {
            get
            {


                return m_aoLeafVertices;
            }
        }

        //*************************************************************************
        //  Property: CollapsedAttributes
        //
        /// <summary>
        /// Gets a string that describes what the motif should look like when it is
        /// collapsed.
        /// </summary>
        ///
        /// <value>
        /// A delimited set of key/value pairs.  Sample: "Key1=Value1|Key2=Value2".
        /// </value>
        ///
        /// <remarks>
        /// The returned string is created with the <see
        /// cref="CollapsedGroupAttributes" /> class.  The same class can be used
        /// later to parse the string.
        /// </remarks>
        //*************************************************************************

        public override String
        CollapsedAttributes
        {
            get
            {
  

                CollapsedGroupAttributes oCollapsedGroupAttributes =
                    new CollapsedGroupAttributes();

                oCollapsedGroupAttributes.Add(CollapsedGroupAttributeKeys.Type,
                    CollapsedGroupAttributeValues.FanMotifType);

                oCollapsedGroupAttributes.Add(
                    CollapsedGroupAttributeKeys.HeadVertexName,
                    m_oHeadVertex.Name);

                oCollapsedGroupAttributes.Add(
                    CollapsedGroupAttributeKeys.LeafVertices,
                    m_aoLeafVertices.Length);

                oCollapsedGroupAttributes.Add(
                    CollapsedGroupAttributeKeys.ArcScale,
                    m_dArcScale);

                return (oCollapsedGroupAttributes.ToString());
            }
        }


        

        //*************************************************************************
        //  Protected fields
        //*************************************************************************

        /// The fan's head vertex.

        protected IVertex m_oHeadVertex;

        /// An array of two or more leaf vertices.

        protected IVertex[] m_aoLeafVertices;

        /// An arc scale factor between 0 and 1.0.

        protected Double m_dArcScale;

        

        
    }
}
