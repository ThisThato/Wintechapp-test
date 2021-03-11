using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FrequencyAllocator
{

    class LocationGraph
    {
        public List<Antenna> Antennas { get; set; }

        #region This part is deprecated
        int[] temp;
        private int[,] adjmatrix;
        #endregion

        public LocationGraph(List<Antenna> antennas)
        {
            Antennas = antennas;

            //adjmatrix = new int[antennas.Count, antennas.Count];

            //for (int i = 0; i < adjmatrix.GetLength(0); i++)
            //{
            //    for (int j = 0; j < adjmatrix.GetLength(1); j++)
            //    {
            //        adjmatrix[i, j] = 0;
            //    }
            //}
            //temp = new int[Antennas.Count];
        }

        #region add edges list ajecent
        private Antenna GetNode(char _id)
        {
            foreach (Antenna node in Antennas)
                if (node.CellID.Equals(_id))
                    return node;
            return null;
        }

        public void AddEdge(char u, char v)
        {
            Antenna U = GetNode(u);
            Antenna V = GetNode(v);
            if (U != null && V != null)   //Ensure that both nodes exist, then add adj nodes to each adj list since graph is undirected
            {
                if (!U.AdjAntenna.Contains(V))
                {
                    U.AdjAntenna.Add(V);
                    V.AdjAntenna.Add(U);
                }
            }
        }
        #endregion


        #region Calculating distance
        public double CalcDistance(double lat1, double long1, double lat2, double long2) // Calculate distance using Haversine Formula 
        {
            double dlon = Radians(long2 - long1);
            double dlat = Radians(lat2 - lat1);

            double a = (Math.Sin(dlat / 2) * Math.Sin(dlat / 2)) + Math.Cos(Radians(lat1)) * Math.Cos(Radians(lat2)) * (Math.Sin(dlon / 2) * Math.Sin(dlon / 2));
            double angle = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
            double km = angle * 6371; //6371 = R of earth
            return km * 1000;
        }

        private double Radians(double x)
        {
            return x * (Math.PI / 180);
        }
        #endregion


        public IList<Antenna> FrequencyAllocator(int[] frequencyRange)
        {
            InitColor(frequencyRange);
            List<Antenna> lstNodes = new List<Antenna>();
            foreach (Antenna node in Antennas)
                GraphColourBFS(lstNodes, frequencyRange, node);
            return lstNodes;
        }
        public void GraphColourBFS(List<Antenna> nodes, int[] frequencyRange, Antenna u)
        {
            if (nodes.Contains(u))  //since all antennas will be travered, makes sure to only work with distinct nodes
            {
                return;
            }
            Queue<Antenna> Q = new Queue<Antenna>();

            int frequencyNum = 0;	//indexer for frequencyRange array
            int MaxFrequency = 1;
            Q.Enqueue(u);
            while (Q.Count > 0)
            {
                u = Q.Dequeue();
                Console.WriteLine(u.CellID + "    " + u.Frequency);
                nodes.Add(u);
                foreach (Antenna v in u.AdjAntenna)
                {

                    if (u.Frequency == v.Frequency)		//if two adjecent nodes have the same frequency, change one according to frequency Range 
                        v.Frequency = frequencyRange[++frequencyNum];

                    MaxFrequency = Math.Max(MaxFrequency, Math.Max(u.Frequency,
                                         v.Frequency));				
                    if (MaxFrequency > frequencyRange[frequencyRange.Length - 1])		//checks if frequency is not out of bounds
                        return;

                    if (!nodes.Contains(v))
                        Q.Enqueue(v);
                }
            }
        }

        private void InitColor(int[] Frequency)
        {
            for (int i = 0; i < Antennas.Count; i++)
            {
                Antennas[i].Frequency = Frequency[0]; //assign first frequency to all antennas
            }
        }



        #region Colouring adjecency matrix (deprecated)
        public void GraphColouring(int k, int[] frequencyRange)
        {
            for (int i = 0; i < frequencyRange.Length; i++)
            {
                if (isSafe(k, frequencyRange[i]))
                {
                    temp[k] = frequencyRange[i];        //assigns node/antenna to a frequency
                    if (k + 1 < Antennas.Count)
                        GraphColouring(k + 1, frequencyRange);
                    else
                    {
                        return;
                    }
                }
            }
        }

        private bool isSafe(int k, int CurrentColor)
        {
            for (int i = 0; i < adjmatrix.GetLength(0); i++)
                if (adjmatrix[k, i] == 1 && CurrentColor == temp[i])
                    return false;
            return true;
        }
        #endregion

        #region Adding edges Matrix (deprecated)
        private int GetNodeIndex(char _id)
        {
            int index = -1;

            for (int i = 0; i < Antennas.Count; i++)
            {
                if (Antennas[i].CellID.Equals(_id))
                {
                    index = i;
                    return index;
                }
            }
            return index;
        }

        public void AddEdge_Matrix(char u, char v)
        {
            int U = GetNodeIndex(u);
            int V = GetNodeIndex(v);
            if (U != -1 && V != -1) //Ensure that both nodes exist, then add adj nodes to each index since graph is undirected
            {
                if (adjmatrix[U, V] == 0 && adjmatrix[V, U] == 0)
                {
                    adjmatrix[U, V] = 1;
                    adjmatrix[V, U] = 1;
                }
            }
        }
        #endregion



    }
}
