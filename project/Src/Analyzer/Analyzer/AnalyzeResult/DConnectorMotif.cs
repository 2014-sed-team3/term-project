using Smrf.NodeXL.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Analyzer
{
    public class DConnectorMotif : Motif, IGroup
    {

        public DConnectorMotif(List<IVertex> anchorVertices)
        {
            m_oAnchorVertices = anchorVertices;
            m_oSpanVertices = new List<IVertex>();
            m_dSpanScale = 1.0;

           
        }

        public IVertex[] getVertexWithin
        {
            get { return (m_oSpanVertices.ToArray()); }
        }

        //*************************************************************************
        //  Property: AnchorVertices
        //
        /// <summary>
        /// Gets the motif's D anchor vertices.
        /// </summary>
        ///
        /// <value>
        /// The motif's anchor vertices, as a List of D <see cref="IVertex" />
        /// objects.
        /// </value>
        ///
        /// <remarks>
        /// Note that the anchor vertices are not actually a part of the motif.
        /// </remarks>
        //*************************************************************************

        public List<IVertex> AnchorVertices
        {
            get
            {
               
                return (m_oAnchorVertices);
            }
        }

        //*************************************************************************
        //  Property: SpanVertices
        //
        /// <summary>
        /// Gets the motif's span vertices.
        /// </summary>
        ///
        /// <value>
        /// The fan's span vertices, as a List of <see cref="IVertex" /> objects.
        /// </value>
        ///
        /// <remarks>
        /// The List starts out empty.  It's up to the caller to populate the List
        /// with at least two span vertices.  The motif is not valid until this is
        /// done.
        /// </remarks>
        //*************************************************************************

        public List<IVertex>
        SpanVertices
        {
            get
            {
               
                return (m_oSpanVertices);
            }
        }

        //*************************************************************************
        //  Property: SpanScale
        //
        /// <summary>
        /// Gets or sets the scale factor to use when determining the size of the
        /// vertex that represents the collapsed motif.
        /// </summary>
        ///
        /// <value>
        /// A span scale factor between 0 and 1.0.  The code that draws the
        /// collapsed motif should use a minimum size when the scale factor is 0,
        /// and a maximum size when the scale factor is 1.0.  The default is 1.0.
        /// </value>
        //*************************************************************************

        public Double
        SpanScale
        {
            get
            {
                
                return (m_dSpanScale);
            }

            set
            {
                m_dSpanScale = value;

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

                // Only the span vertices are part of the motif.

                return (m_oSpanVertices.ToArray());
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
                    CollapsedGroupAttributeValues.DConnectorMotifType);

                Int32 iAnchorVertices = m_oAnchorVertices.Count;

                oCollapsedGroupAttributes.Add(
                    CollapsedGroupAttributeKeys.AnchorVertices, iAnchorVertices);

                for (Int32 i = 0; i < iAnchorVertices; i++)
                {
                    oCollapsedGroupAttributes.Add(
                        CollapsedGroupAttributeKeys.GetAnchorVertexNameKey(i),
                        m_oAnchorVertices[i].Name);
                }

                oCollapsedGroupAttributes.Add(
                    CollapsedGroupAttributeKeys.SpanVertices,
                    m_oSpanVertices.Count);

                oCollapsedGroupAttributes.Add(
                    CollapsedGroupAttributeKeys.SpanScale,
                    m_dSpanScale);

                return (oCollapsedGroupAttributes.ToString());
            }
        }



        //*************************************************************************
        //  Protected fields
        //*************************************************************************

        /// The motif's D anchor vertices.

        protected List<IVertex> m_oAnchorVertices;

        /// The motif's zero or more span vertices.

        protected List<IVertex> m_oSpanVertices;

        /// A span scale factor between 0 and 1.0.

        protected Double m_dSpanScale;

        

        public string getDescription
        {
            get { throw new NotImplementedException(); }
        }
    }
}
