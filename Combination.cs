using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Skocko
{
    class Combination
    {
        public int[] order;

        public Combination(int a, int b, int c, int d)
        {
            order = new int[4];
            order[0] = a;
            order[1] = b;
            order[2] = c;
            order[3] = d;
        }

    }
}
