using GK___projekt_2.Geometry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GK___projekt_2
{
    public class CloudTriangle
    {
        public List<Point> Vertices { get; set; }
        public List<Edge> Edges { get; set; }
        public CloudTriangle(List<Point> vertices)
        {
            Vertices = vertices;
            Edges = new List<Edge>();
        }

        public (Point v1, Point v2) Neighbours(Point v)
        {
            return (Vertices[(Vertices.IndexOf(v) + 1) % 3], Vertices[(Vertices.IndexOf(v) + 2) % 3]);
        }
    }
}
