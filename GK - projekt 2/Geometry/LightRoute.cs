using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GK___projekt_2.Geometry
{

    public class LightRoute
    {
        public double R { get; set; }
        public double X { get; set; }
        public double Y { get; set; }

        public LightRoute(double r)
        {
            R = r;
            X = r;
            Y = 0;
        }

        public void CalculateNewCoordinates(double alfa)
        {
            X = R * Math.Cos(alfa);
            Y = R * Math.Sin(alfa);
        }

    }
}
