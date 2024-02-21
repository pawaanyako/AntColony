using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms.VisualStyles;

namespace AntColony
{
    internal class Ant
    {
        private Graph graph;
        private float alpha;
        private float beta;
        private int startVertexId;
        private List<Vertex> visitedVertexes;
        private List<Vertex> unvisitedVertexes;
        private List<Edge> path;
        private double distance;
        public List<Edge> Path
        {
            get { return path; }
        }
        public double Distance
        {
            get { return distance; }
        }

        public Ant(Graph _graph, int _startVertexId, float _alpha, float _beta)
        {
            graph = _graph;
            alpha = _alpha;
            beta = _beta;
            startVertexId = _startVertexId % _graph.Vertices.Count();
            visitedVertexes = new List<Vertex>();
            unvisitedVertexes = new List<Vertex>();
            for (int i = 0; i < graph.Vertices.Count(); i++)
            {
                if (graph.Vertices[i].Id != startVertexId) unvisitedVertexes.Add(graph.Vertices[i]);
                else visitedVertexes.Add(graph.Vertices[i]);
            }
            path = new List<Edge>();
            distance = 0;            
        }
        private double[] Probability(Vertex currentVertex)
        {
            double[] P = new double[unvisitedVertexes.Count];            
            double SumP = 0;
            int i = 0;
            foreach (var edge in graph.Edges)
            {
                if (edge.Start.Id == currentVertex.Id && !visitedVertexes.Contains(edge.End))
                {
                    P[i] = Math.Pow(edge.Pheromone, alpha) * Math.Pow((200f / edge.Length), beta);
                    i++;
                }
            }
            SumP = P.Sum();
            for (int j = 0; j < P.Length; j++)
            {
                P[j] /= SumP;
            }
            return P;
        }
        private Vertex ChooseNextVertex(double[] P)
        {
            var rnd = new Random();
            double rndDouble = rnd.NextDouble();
            int i;
            for (i = 0; i < P.Length; i++)
            {
                rndDouble -= P[i];
                if (rndDouble <= 0.00001) break;
            }
            return unvisitedVertexes[i];
        }
        public void NextVertex()
        {
            Vertex currentVertex = visitedVertexes[visitedVertexes.Count - 1];
            Vertex nextVertex;
            if (visitedVertexes.Count != graph.Vertices.Count)
            {
                double[] P = Probability(currentVertex);
                nextVertex = ChooseNextVertex(P);
                visitedVertexes.Add(nextVertex);
                unvisitedVertexes.Remove(nextVertex);
            }
            else nextVertex = visitedVertexes[0];
            foreach (var edge in graph.Edges)
            {
                if (edge.Start == currentVertex && edge.End == nextVertex) 
                { 
                    path.Add(edge);
                    distance += edge.Length;
                }
            }
        }
        public void ResetAnt(Graph _graph)
        {
            graph = _graph;
            visitedVertexes.Clear();
            unvisitedVertexes.Clear();
            path.Clear();
            distance = 0;
            for (int i = 0; i < graph.Vertices.Count(); i++)
            {
                if (graph.Vertices[i].Id != startVertexId) unvisitedVertexes.Add(graph.Vertices[i]);
                else visitedVertexes.Add(graph.Vertices[i]);
            }
        }
    }
}
