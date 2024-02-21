using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;

namespace AntColony
{
    internal class Graph
    {
        private List<Vertex> vertices;
        private List<Edge> edges;
        private double minPheromone;
        public List<Vertex> Vertices
        { 
            get { return vertices; }
            set { vertices = value; }
        }
        public List<Edge> Edges
        {
            get { return edges; }
        }
        public Graph()
        {
            vertices = new List<Vertex>();
            edges = new List<Edge>();
            minPheromone = 0;
        }
        public void CreateEdges()
        {
            for (int i = 0; i < vertices.Count; i++)
            {
                for (int j = 0; j < vertices.Count; j++)
                {
                    if (i != j)
                    {                        
                        edges.Add(new Edge(vertices[i], vertices[j]));
                    }
                }
            }
        }
        public void ResetPheromones(double _defaultPheromone, double _minPheramone)
        {
            minPheromone = _minPheramone;
            foreach (var edge in Edges)
            {
                edge.Pheromone = _defaultPheromone;
            }
        }
        public void EvaporatePheromones(double _evaporateCoeff)
        {
            foreach (var edge in Edges)
            {
                edge.Pheromone = Math.Max(minPheromone, edge.Pheromone * (1 -_evaporateCoeff));
            }
        }
        public void AddToPheromones(double additionalPheromone, List<Edge> path)
        {
            foreach (var edge in Edges)
            {
                foreach (var pathEdge in path)
                {
                    if (edge == pathEdge) edge.Pheromone += additionalPheromone;
                    if (edge.End == pathEdge.Start && edge.Start == pathEdge.End) edge.Pheromone += additionalPheromone;
                }
            }
        }
        public void ClearEdges()
        {
            edges.Clear();
        }
        public void ClearGraph()
        {
            if (vertices.Count > 0) vertices[0].ResetCount();
            vertices.Clear();
            edges.Clear();
        }
    }
}
