using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GraphStorageManagement
{
    public class statusEventHandler : EventArgs
    {
        private int state;
        private String text;
        public statusEventHandler(int status, String _text)
        {
            state = status;
            text = _text;
        }
        public int getStatus(){
            return state;
        }
        public String getText()
        {
            return text;
        }
    }
}
