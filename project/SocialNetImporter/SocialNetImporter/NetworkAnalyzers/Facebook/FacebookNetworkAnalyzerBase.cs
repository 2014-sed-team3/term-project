
//  Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.Xml;
using System.Web;
using System.Net;
using System.Collections.Generic;
using System.Diagnostics;
using Smrf.AppLib;
using Smrf.XmlLib;

namespace Smrf.NodeXL.GraphDataProviders.Facebook
{
    //*****************************************************************************
    //  Class: FacebookNetworkAnalyzerBase
    //
    /// <summary>
    /// Base class for classes that analyze a Facebook network.
    /// </summary>
    //*****************************************************************************

    public abstract class FacebookNetworkAnalyzerBase : HttpNetworkAnalyzerBase
    {
        //*************************************************************************
        //  Constructor: FacebookNetworkAnalyzerBase()
        //
        /// <summary>
        /// Initializes a new instance of the <see
        /// cref="FacebookNetworkAnalyzerBase" /> class.
        /// </summary>
        //*************************************************************************

        public FacebookNetworkAnalyzerBase()
        {
            // (Do nothing.)

            AssertValid();
        }

        //*************************************************************************
        //  Method: ExceptionToMessage()
        //
        /// <summary>
        /// Converts an exception to an error message appropriate for a user
        /// interface.
        /// </summary>
        ///
        /// <param name="oException">
        /// The exception that occurred.
        /// </param>
        ///
        /// <returns>
        /// An error message appropriate for a user interface.
        /// </returns>
        //*************************************************************************

        public override String
        ExceptionToMessage
        (
            Exception oException
        )
        {
            Debug.Assert(oException != null);
            AssertValid();

            String sMessage = null;

            const String TimeoutMessage =
                "The Facebook Web service didn't respond.";

            
            if (oException is WebException)
            {
                WebException oWebException = (WebException)oException;

                if (oWebException.Response is HttpWebResponse)
                {
                    HttpWebResponse oHttpWebResponse =
                        (HttpWebResponse)oWebException.Response;

                    switch (oHttpWebResponse.StatusCode)
                    {
                        case HttpStatusCode.RequestTimeout:  // HTTP 408.

                            sMessage = TimeoutMessage;
                            break;

                        default:

                            break;
                    }
                }
                else
                {
                    switch (oWebException.Status)
                    {
                        case WebExceptionStatus.Timeout:

                            sMessage = TimeoutMessage;
                            break;

                        default:

                            break;
                    }
                }
            }

            if (sMessage == null)
            {
                sMessage = ExceptionUtil.GetMessageTrace(oException);
            }

            return (sMessage);
        }

        //*************************************************************************
        //  Method: AppendVertexTooltipXmlNodes()
        //
        /// <summary>
        /// Appends a vertex tooltip XML node for each person in the network.
        /// </summary>
        ///
        /// <param name="oGraphMLXmlDocument">
        /// The GraphMLXmlDocument being populated.
        /// </param>
        ///
        /// <param name="oVertexXmlNode">
        /// The XmlNode representing the vertex.
        /// </param>
        /// 
        /// <param name="sVertex">
        /// The screening name of the vertex. 
        /// </param>
        /// 
        /// <param name="sDisplayString">
        /// The string to be attached after the screening name.
        /// </param>
        //*************************************************************************

        protected void
        AppendVertexTooltipXmlNodes
        (
            GraphMLXmlDocument oGraphMLXmlDocument,
            XmlNode oVertexXmlNode,
            String sVertex,
            String sDisplayString
        )
        {
            // The NodeXL template doesn't wrap long tooltip text.  Break the
            // status into lines so the entire tooltip will show in the graph
            // pane.

            sDisplayString = StringUtil.BreakIntoLines(sDisplayString, 30);

            String sTooltip = String.Format(
                "{0}\n\n{1}"
                ,
                sVertex,
                sDisplayString
                );

            oGraphMLXmlDocument.AppendGraphMLAttributeValue(
                oVertexXmlNode, TooltipID, sTooltip);           

        }              
      

        //*************************************************************************
        //  Method: AssertValid()
        //
        /// <summary>
        /// Asserts if the object is in an invalid state.  Debug-only.
        /// </summary>
        //*************************************************************************

        // [Conditional("DEBUG")]

        public override void
        AssertValid()
        {
            base.AssertValid();

            // (Do nothing else.)
        }


        //*************************************************************************
        //  Protected constants
        //*************************************************************************

        /// HTTP status codes that have special meaning with Facebook.  When they
        /// occur, the requests are not retried.

        protected static readonly HttpStatusCode[]
            HttpStatusCodesToFailImmediately = new HttpStatusCode[] {

        };


        //*************************************************************************
        //  Protected fields
        //*************************************************************************

        ///
        protected const String FacebookURL = "http://www.facebook.com/";
        /// GraphML-attribute IDs.

        protected const String TooltipID = "Tooltip";

        //*************************************************************************
        //  Embedded class: GetNetworkAsyncArgsBase()
        //
        /// <summary>
        /// Base class for classes that contain the arguments needed to
        /// asynchronously get a Facebook network.
        /// </summary>
        //*************************************************************************

        protected class GetNetworkAsyncArgsBase
        {
            ///
            public String AccessToken;
        };
    }

}
