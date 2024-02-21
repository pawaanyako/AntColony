using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace AntColony
{
    internal class Vertex
    {
        private static int count = 0;
        private int x;
        private int y;
        private int id;
        public int X 
        {
            get { return x; }
        }
        public int Y
        {
            get { return y; }
        }
        public int Id
        {
            get { return id; }
        }
        public Vertex(int _x, int _y)
        {
            x = _x;
            y = _y;
            id = count++;
        }
        public void ResetCount()
        {
            count = 0;
        }
    }
}
