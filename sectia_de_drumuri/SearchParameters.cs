using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sectia_de_drumuri
{
    /// <summary>
    /// Defines the parameters which will be used to find a path across a section of the map
    /// </summary>
    public class SearchParameters
    {
        public Point StartLocation { get; set; }

        public Point EndLocation { get; set; }
        
        public bool[,] Map { get; set; }

        public Cautare cautare = Cautare.Dijkastra;

        public SearchParameters(Point startLocation, Point endLocation, bool[,] map,Cautare cautare)
        {
            this.StartLocation = startLocation;
            this.EndLocation = endLocation;
            this.Map = map;
            this.cautare = cautare;
        }
    }
}
