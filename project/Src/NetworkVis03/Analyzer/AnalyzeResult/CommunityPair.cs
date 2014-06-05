using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Analyzer
{
    public class CommunityPair : Object
    {

        public CommunityPair()
        {
            m_oCommunity1 = null;
            m_oCommunity2 = null;
            m_fDeltaQ = Community.DeltaQNotSet;

            
        }

        

        public Community
        Community1
        {
            get
            {
                
                return (m_oCommunity1);
            }

            set
            {
                m_oCommunity1 = value;

            }
        }


        public Community
        Community2
        {
            get
            {
                
                return (m_oCommunity2);
            }

            set
            {
                m_oCommunity2 = value;
            }
        }

        

        public Single
        DeltaQ
        {
            get
            {
                
                return (m_fDeltaQ);
            }

            set
            {
                m_fDeltaQ = value;

            }
        }

        

        public override String
        ToString()
        {

            if (m_oCommunity1 == null || m_oCommunity2 == null)
            {
                return (base.ToString());
            }

            return (String.Format(

                "{0}, {1}, DeltaQ = {2}"
                ,
                m_oCommunity1.ToString(),
                m_oCommunity2.ToString(),

                (m_fDeltaQ == Community.DeltaQNotSet) ? "DeltaQNotSet" :
                    m_fDeltaQ.ToString("N3")
                ));
        }



        //*************************************************************************
        //  Protected fields
        //*************************************************************************

        /// The first community in the pair.

        protected Community m_oCommunity1;

        /// The second community in the pair.

        protected Community m_oCommunity2;

        /// Maximum delta Q value among all community pairs within
        /// m_oCommunityPairs.

        protected Single m_fDeltaQ;
    }
}
