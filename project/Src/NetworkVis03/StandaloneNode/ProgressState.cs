using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StandaloneNode
{
    public class ProgressState
    {
        public int m_Percentage;
        public string m_State;
        public bool m_wbFlag;

        public ProgressState(int p, string msg, bool isWritingBack) {
            m_Percentage = p;
            m_State = msg;
            m_wbFlag = isWritingBack;
        }
    }
}
