using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AntColony
{
    internal class AntColony
    {
        private Graph graph;
        private int numberOfAnts;
        private List<Ant> ants;
        private double evaporateCoeff;
        private int Q;
        private float alpha;
        private float beta;
        public AntColony(Graph _graph, int _numberOfAnts, double _evaporateCoeff, int _Q, float _alpha, float _beta)
        {
            graph = _graph;
            numberOfAnts = _numberOfAnts;
            evaporateCoeff = _evaporateCoeff;
            Q = _Q;
            ants = new List<Ant>();
            alpha = _alpha;
            beta = _beta;
            for (int i = 0; i < numberOfAnts; i++)
            {
                ants.Add(new Ant(graph, i, alpha, beta));
            }
        }
        public string Search()
        {
            double additionalPheromone;
            double minPath = double.MaxValue;
            for (int i = 0; i < numberOfAnts; i++)
            {
                for (int j = 0; j < graph.Vertices.Count; j++)
                {
                    ants[i].NextVertex();
                }
            }
            graph.EvaporatePheromones(evaporateCoeff);
            for (int i = 0; i < numberOfAnts; i++)
            {
                additionalPheromone = (double)Q / ants[i].Distance;
                graph.AddToPheromones(additionalPheromone, ants[i].Path);
            }
            string pathStr = "";
            for (int i = 0; i < numberOfAnts; i++)
            {
                if (minPath > ants[i].Distance) 
                {
                    pathStr = "";
                    foreach (var pathEdge in ants[i].Path)
                    {
                        pathStr += pathEdge.Start.Id.ToString() + " -> ";
                    }
                    pathStr += ants[i].Path[0].Start.Id.ToString();
                    minPath = ants[i].Distance; 
                }
                ants[i].ResetAnt(graph);
            }            
            string str = " интерация\r\nМинимальное найденное расстояние: " + minPath.ToString() + "\r\nКратчайший гамильтонов цикл: " + pathStr + "\r\n";
            return str;
        }
    }
}
