using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AntColony
{
    internal class Edge
    {
        private Vertex start;
        private Vertex end;
        private double length;
        private double pheromone;
        public Vertex Start 
        {
            get { return start; }
        }
        public Vertex End
        {
            get { return end; }
        }
        public double Length
        {
            get { return length; }
        }
        public double Pheromone
        {
            get { return pheromone; }
            set { pheromone = value; }
        }
        private double Distance(float X1, float X2, float Y1, float Y2)
        {
            return Math.Sqrt(Math.Pow(X1 - X2, 2) + Math.Pow(Y1 - Y2, 2));
        }
        public Edge (Vertex _start, Vertex _end) 
        {
            start = _start;
            end = _end;
            length = Distance(_start.X, _end.X, _start.Y, _end.Y);
            length = Math.Round(length, 3);
        }
    }
}
