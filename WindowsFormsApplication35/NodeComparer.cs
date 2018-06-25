using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApplication35
{
    public class NodeComparer : IComparer
    {
        public NodeComparer()
        {

        }

        public int Compare(object x, object y)
        {
            return ((Node)x).totalCost - ((Node)y).totalCost;
        }
    }
}
