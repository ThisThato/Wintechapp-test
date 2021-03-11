using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FrequencyAllocator
{
    class Antenna
    {
        public char CellID { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public int Frequency { get; set; } = 1;
        public List<Antenna> AdjAntenna { get; set; }
        public Antenna(char id, double latitude, double longitude)
        {
            this.CellID = id;
            this.Latitude = latitude;
            this.Longitude = longitude;
            AdjAntenna = new List<Antenna>();
        }
        
    }
}
