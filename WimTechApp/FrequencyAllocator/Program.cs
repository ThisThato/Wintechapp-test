using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FrequencyAllocator
{
    class Program
    {
        static void Main(string[] args)
        {
            //Create list and graph, along with frequency range
            List<Antenna> antennas = new List<Antenna>()
            {
                new Antenna('A',51.53657, -0.03098),
                new Antenna('B',51.53833, -0.02554),
                new Antenna('C',51.53721, -0.02448),
                new Antenna('D',51.5445, -0.02415),
                new Antenna('E',51.54439, -0.02277),
                new Antenna('F',51.54735, -0.02204),
                new Antenna('G',51.54739, -0.02201),
                new Antenna('H',51.54525, -0.02185),
                new Antenna('I',51.53328, -0.02234),
                new Antenna('J',51.53948, -0.02206),
                new Antenna('K',51.54653, -0.02052),
                new Antenna('L',51.54472, -0.02025),
                new Antenna('M',51.54262, -0.01921),
                new Antenna('N',51.53934, -0.01725),
                new Antenna('O',51.53862, -0.01561),
                new Antenna('P',51.54337, -0.01273),
                new Antenna('Q',51.54202, -0.01272),
                new Antenna('R',51.5407, -0.01216),
                new Antenna('S',51.54023, -0.01078),
            };
            LocationGraph location = new LocationGraph(antennas);
            int[] frequencyRange = new int[] { 110, 111, 112, 113, 114, 115 };

            //The next section of code deals with creating edges for the Vertices(antennas). 
            //Edges are created according to closest vertices by distance
            List<double>[] preEdges = new List<double>[antennas.Count];     //preEdges caters for all distances between all vertices
            List<double>[] edgessorted = new List<double>[antennas.Count];
           
            for (int i = 0; i < antennas.Count; i++)
            {
                preEdges[i] = new List<double>();
                for (int j = 0; j < antennas.Count; j++)
                {
                   double distance=  location.CalcDistance(antennas[i].Latitude, antennas[i].Longitude,
                        antennas[j].Latitude, antennas[j].Longitude);
                    preEdges[i].Add(distance); 
                }
                edgessorted[i] = preEdges[i].OrderBy(x => x).ToList(); //edgeSorted is used just to sort out preEdges
              
                double mindistance = edgessorted[i][1];   //sort and get the closest distance, make it adjecent
                int indexclosest = preEdges[i].IndexOf(mindistance);
                location.AddEdge(antennas[i].CellID, antennas[indexclosest].CellID);
            }
            
            //alocates frequency according to added adjecent vertices
            location.FrequencyAllocator(frequencyRange);

            Console.WriteLine("Press any key");
           
            Console.ReadKey();
        }
       
        
    }
}
