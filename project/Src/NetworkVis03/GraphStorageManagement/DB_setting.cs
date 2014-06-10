using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphStorageManagement
{
    public class DB_setting
    {
        public String vertexCol = null;
        public int EdgeDropdown = 1;
        public String edgeCol = null;
        public void changeEdgeDropdown(int change){
            EdgeDropdown = change;
        }
        
    }
}
