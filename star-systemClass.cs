using System;
using System.Drawing;
using System.Collections.Generic;

namespace StarMap
{

    public class Ellipse
        {
        public Color Color { get; set; }
        public Point Location { get; set; }
        public Size Size { get; set; }
        public bool Filled { get; set; }
        }
    public class sSystemDetails
    {
        public string name { get; set;}
        public string description { get; set; }
        Tuple<int,int> coordinates;
        public string sectorName { get; set; }
        string regionName;
        string[] celestialBodies;
        string capital;

        public sSystemDetails()
        {
        }
        public sSystemDetails(string inName, Tuple<int,int> inCoords, string inSectorName)
        {
            name = inName;
            coordinates = inCoords;
            sectorName = inSectorName;
        }

        public sSystemDetails(string inName, Tuple<int,int> inCoords, string inSectorName, string inRegion)
        {
            name = inName;
            coordinates = inCoords;
            sectorName = inSectorName;
            regionName = inRegion;
        }
    }


}