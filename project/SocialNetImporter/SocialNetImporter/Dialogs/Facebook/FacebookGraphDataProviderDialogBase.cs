

//  Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.Net;
using System.Diagnostics;
using Smrf.AppLib;

namespace Smrf.NodeXL.GraphDataProviders.Facebook
{
    //*****************************************************************************
    //  Class: FacebookGraphDataProviderDialogBase
    //
    /// <summary>
    /// Base class for dialogs that get Facebook graph data.
    /// </summary>
    //*****************************************************************************

    public partial class FacebookGraphDataProviderDialogBase :
        GraphDataProviderDialogBase
    {
        //*************************************************************************
        //  Constructor: FacebookGraphDataProviderDialogBase()
        //
        /// <summary>
        /// Initializes a new instance of the <see
        /// cref="FacebookGraphDataProviderDialogBase" /> class.
        /// </summary>
        //*************************************************************************

        public FacebookGraphDataProviderDialogBase
        (
            HttpNetworkAnalyzerBase httpNetworkAnalyzer
        )
            : base(httpNetworkAnalyzer)
        {
            

            AssertValid();
        }

        //*************************************************************************
        //  Constructor: FacebookGraphDataProviderDialogBase()
        //
        /// <summary>
        /// Do not use this constructor.
        /// </summary>
        ///
        /// <remarks>
        /// Do not use this constructor.  It is for the Visual Studio designer
        /// only.
        /// </remarks>
        //*************************************************************************

        public FacebookGraphDataProviderDialogBase()
        {
            // (Do nothing.)
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

            
        }


        //*************************************************************************
        //  Protected fields
        //*************************************************************************

        // This is static so that the dialog's controls will retain their values
        // between dialog invocations.  Most NodeXL dialogs persist control values
        // via ApplicationSettingsBase, but this plugin does not have access to
        // that and so it resorts to static fields.

        
    }
}
