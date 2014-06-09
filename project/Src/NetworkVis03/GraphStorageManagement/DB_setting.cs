using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphStorageManagement
{
    public class DB_setting
    {
        public int vertexIdx = 1;
        public int EdgeDropdown = 1;
        public int EdgeIdx = -1;
        public void changeEdgeDropdown(int change){
            EdgeDropdown = change;
            EdgeIdx = -1;
        }
        
    }
}
