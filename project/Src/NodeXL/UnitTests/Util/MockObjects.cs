﻿
using System;
using System.Text;
using System.Drawing;
using System.Collections.Generic;
using System.Diagnostics;
using Smrf.NodeXL.Core;
using Smrf.NodeXL.Layouts;

namespace Smrf.NodeXL.UnitTests
{
//*****************************************************************************
//  Class: MockGraphVertexEdgeBase
//
/// <summary>
/// <see cref="GraphVertexEdgeBase" /> has a protected <see
/// cref="GraphVertexEdgeBase.CopyTo" /> method that needs to be tested.  This
/// mock class makes the method public.
/// </summary>
//*****************************************************************************

internal class MockGraphVertexEdgeBase : GraphVertexEdgeBase
{
    static MockGraphVertexEdgeBase()
    {
        m_iNextID = 0;
    }

    public MockGraphVertexEdgeBase()
    :
    base(m_iNextID++)
    {
        // (Do nothing else.)
    }

    public new void
    CopyTo
    (
        Object oOtherObject,
        Boolean bCopyMetadataValues,
        Boolean bCopyTag
    )
    {
        base.CopyTo(oOtherObject, bCopyMetadataValues, bCopyTag);
    }


    //*************************************************************************
    //  Private fields
    //*************************************************************************

    private static Int32 m_iNextID;
}


//*****************************************************************************
//  Class: MockEdge
//
/// <summary>
/// Implements IEdge for testing.
/// </summary>
//*****************************************************************************

internal class MockEdge : IEdge
{
    protected internal MockEdge
    (
        IVertex oVertex1,
        IVertex oVertex2,
        Boolean bIsDirected,
        Boolean bVerticesReturnsNull,
        Int32 iNumberOfVerticesReturned
    )
    {
        m_oVertex1 = oVertex1;
        m_oVertex2 = oVertex2;
        m_bIsDirected = bIsDirected;
        m_bVerticesReturnsNull = bVerticesReturnsNull;
        m_iNumberOfVerticesReturned = iNumberOfVerticesReturned;
    }

    public String
    Name
    {
        get
        {
            return ("Name");
        }

        set
        {
        }
    }

    public Int32
    ID
    {
        get
        {
            return (1);
        }

        set
        {
        }
    }

    public Object
    Tag
    {
        get
        {
            return (null);
        }

        set
        {
        }
    }

    public Boolean
    ContainsKey
    (
        String key
    )
    {
        return (false);
    }

    public Boolean
    RemoveKey
    (
        String key
    )
    {
        return (false);
    }

    public void
    SetValue
    (
        String key,
        Object value
    )
    {
    }

    public Object
    GetRequiredValue
    (
        String key,
        Type valueType
    )
    {
        return (null);
    }

    public Boolean
    TryGetValue
    (
        String key,
        Type valueType,
        out Object value
    )
    {
        value = null;

        return (false);
    }

    public Boolean
    TryGetValue
    (
        String key,
        out Object value
    )
    {
        value = null;

        return (false);
    }

    public Boolean
    TryGetNonEmptyStringValue
    (
        String key,
        out String value
    )
    {
        value = null;

        return (false);
    }

    public Object
    GetValue
    (
        String key,
        Type valueType
    )
    {
        return (null);
    }

    public Object
    GetValue
    (
        String key
    )
    {
        return (null);
    }

    public IGraph
    ParentGraph
    {
        get
        {
            return (null);
        }
    }

    public Boolean
    IsDirected
    {
        get
        {
            return (m_bIsDirected);
        }
    }

    public IVertex []
    Vertices
    {
        get
        {
            if (m_bVerticesReturnsNull)
            {
                return (null);
            }

            switch (m_iNumberOfVerticesReturned)
            {
                case 0:

                    return ( new IVertex[0] );

                case 1:

                    return ( new IVertex[] {m_oVertex1} );

                case 2:

                    return ( new IVertex[] {m_oVertex1, m_oVertex2} );

                default:

                    Debug.Assert(false);

                    return (null);
            }
        }
    }

    public IVertex
    Vertex1
    {
        get
        {
            return (m_oVertex1);
        }
    }

    public IVertex
    Vertex2
    {
        get
        {
            return (m_oVertex2);
        }
    }

    public IEdge
    Clone
    (
        Boolean setMetadataValues,
        Boolean setTag
    )
    {
        return (null);
    }

    public IEdge
    Clone
    (
        Boolean copyMetadataValues,
        Boolean copyTag,
        IVertex vertex1,
        IVertex vertex2,
        Boolean isDirected
    )
    {
        return (null);
    }

    public Boolean
    IsSelfLoop
    {
        get
        {
            return (false);
        }
    }

    public Boolean
    IsParallelTo
    (
        IEdge otherEdge
    )
    {
        return (false);
    }

    public Boolean
    IsAntiparallelTo
    (
        IEdge otherEdge
    )
    {
        return (false);
    }

    public IVertex
    GetAdjacentVertex
    (
        IVertex vertex
    )
    {
        return (null);
    }

    public override String
    ToString()
    {
        return ("String");
    }

    public String
    ToString
    (
        String format
    )
    {
        return ("String");
    }

    public String
    ToString
    (
        String format,
        IFormatProvider formatProvider
    )
    {
        return ("String");
    }


    //*************************************************************************
    //  Protected fields
    //*************************************************************************

    protected IVertex m_oVertex1;

    protected IVertex m_oVertex2;

    protected Boolean m_bIsDirected;

    protected Boolean m_bVerticesReturnsNull;

    protected Int32 m_iNumberOfVerticesReturned;
}


//*****************************************************************************
//  Class: MockVertex
//
/// <summary>
/// Implements IVertex for testing.
/// </summary>
//*****************************************************************************

internal class MockVertex : IVertex
{
    public MockVertex()
    {
        m_oParentGraph = null;
    }

    public String
    Name
    {
        get
        {
            return ("Name");
        }

        set
        {
        }
    }

    public Int32
    ID
    {
        get
        {
            return (11111);
        }

        set
        {
        }
    }

    public Object
    Tag
    {
        get
        {
            return (null);
        }

        set
        {
        }
    }

    public Boolean
    ContainsKey
    (
        String key
    )
    {
        return (false);
    }

    public Boolean
    RemoveKey
    (
        String key
    )
    {
        return (false);
    }

    public void
    SetValue
    (
        String key,
        Object value
    )
    {
    }

    public Object
    GetRequiredValue
    (
        String key,
        Type valueType
    )
    {
        return (null);
    }

    public Boolean
    TryGetValue
    (
        String key,
        Type valueType,
        out Object value
    )
    {
        value = null;

        return (false);
    }

    public Boolean
    TryGetValue
    (
        String key,
        out Object value
    )
    {
        value = null;

        return (false);
    }

    public Boolean
    TryGetNonEmptyStringValue
    (
        String key,
        out String value
    )
    {
        value = null;

        return (false);
    }

    public Object
    GetValue
    (
        String key,
        Type valueType
    )
    {
        return (null);
    }

    public Object
    GetValue
    (
        String key
    )
    {
        return (null);
    }

    public IGraph
    ParentGraph
    {
        get
        {
            return (m_oParentGraph);
        }

        set
        {
            m_oParentGraph = value;
        }
    }

    public ICollection<IEdge>
    IncomingEdges
    {
        get
        {
            return (null);
        }
    }

    public ICollection<IEdge>
    OutgoingEdges
    {
        get
        {
            return (null);
        }
    }

    public ICollection<IEdge>
    IncidentEdges
    {
        get
        {
            return (null);
        }
    }

    public Int32
    Degree
    {
        get
        {
            return (0);
        }
    }

    public ICollection<IVertex>
    PredecessorVertices
    {
        get
        {
            return (null);
        }
    }

    public ICollection<IVertex>
    SuccessorVertices
    {
        get
        {
            return (null);
        }
    }

    public ICollection<IVertex>
    AdjacentVertices
    {
        get
        {
            return (null);
        }
    }

    public PointF
    Location
    {
        get
        {
            return (PointF.Empty);
        }

        set
        {
        }
    }

    public IVertex
    Clone
    (
        Boolean setMetadataValues,
        Boolean setTag
    )
    {
        return (null);
    }

    public ICollection<IEdge>
    GetConnectingEdges
    (
        IVertex otherVertex
    )
    {
        return (null);
    }

    public Boolean
    IsIncidentEdge
    (
        IEdge edge
    )
    {
        return (false);
    }

    public Boolean
    IsOutgoingEdge
    (
        IEdge edge
    )
    {
        return (false);
    }

    public Boolean
    IsIncomingEdge
    (
        IEdge edge
    )
    {
        return (false);
    }

    public override String
    ToString()
    {
        return ("String");
    }

    public String
    ToString
    (
        String format
    )
    {
        return ("String");
    }

    public String
    ToString
    (
        String format,
        IFormatProvider formatProvider
    )
    {
        return ("String");
    }


    //*************************************************************************
    //  Protected fields
    //*************************************************************************

    protected IGraph m_oParentGraph;
}


//*****************************************************************************
//  Class: MockMetadataProvider
//
/// <summary>
/// Implements IMetadataProvider and IIdentityProvider for testing the
/// MetadataProvider class.
/// </summary>
//*****************************************************************************

internal class MockMetadataProvider : IMetadataProvider, IIdentityProvider
{
    static MockMetadataProvider()
    {
        m_iNextID = 0;
    }

    public MockMetadataProvider()
    {
        m_iID = m_iNextID;

        m_iNextID++;

        m_sName = null;

        m_oMetadataProvider = new MetadataProvider();
    }

    public String
    Name
    {
        get
        {
            return (m_sName);
        }

        set
        {
            m_sName = value;
        }
    }

    public Int32
    ID
    {
        get
        {
            return (m_iID);
        }

        set
        {
            m_iID = value;
        }
    }

    public Object
    Tag
    {
        get
        {
            return (m_oMetadataProvider.Tag);
        }

        set
        {
            m_oMetadataProvider.Tag = value;
        }
    }

    public Boolean
    ContainsKey
    (
        String key
    )
    {
        return ( m_oMetadataProvider.ContainsKey(key) );
    }

    public Boolean
    RemoveKey
    (
        String key
    )
    {
        return ( m_oMetadataProvider.RemoveKey(key) );
    }

    public void
    SetValue
    (
        String key,
        Object value
    )
    {
        m_oMetadataProvider.SetValue(key, value);
    }

    public Object
    GetRequiredValue
    (
        String key,
        Type valueType
    )
    {
        Object oValue;

        if ( !m_oMetadataProvider.TryGetValue(key, out oValue) )
        {
            throw new ApplicationException("Value not found.");
        }

        return (oValue);
    }

    public Boolean
    TryGetValue
    (
        String key,
        Type valueType,
        out Object value
    )
    {
        return ( m_oMetadataProvider.TryGetValue(key, out value) );
    }

    public Boolean
    TryGetValue
    (
        String key,
        out Object value
    )
    {
        throw new ApplicationException("Don't bother testing this overload.");
    }

    public Boolean
    TryGetNonEmptyStringValue
    (
        String key,
        out String value
    )
    {
        throw new ApplicationException("Don't bother testing this overload.");
    }

    public Object
    GetValue
    (
        String key,
        Type valueType
    )
    {
        throw new ApplicationException("Don't bother testing this overload.");
    }

    public Object
    GetValue
    (
        String key
    )
    {
        throw new ApplicationException("Don't bother testing this overload.");
    }

    public void
    CopyTo
    (
        IMetadataProvider oOtherMetadataProvider,
        Boolean bCopyMetadataValues,
        Boolean bCopyTag
    )
    {
        m_oMetadataProvider.CopyTo(oOtherMetadataProvider, bCopyMetadataValues,
            bCopyTag);
    }

    public void
    ClearMetadata()
    {
        m_oMetadataProvider.ClearMetadata();
    }


    //*************************************************************************
    //  Protected fields
    //*************************************************************************

    protected Int32 m_iID;

    protected String m_sName;

    protected static Int32 m_iNextID;

    protected MetadataProvider m_oMetadataProvider;
}


}
