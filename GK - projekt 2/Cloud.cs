using GK___projekt_2.Geometry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GK___projekt_2
{
    public class Cloud
    {
        public List<CloudTriangle> Triangles { get; set; }
        public List<Point> Vertices { get; set; }

        public List<Edge> Edges { get; set; }
        public Cloud(List<CloudTriangle> cloudTriangles)
        {
            Triangles = cloudTriangles;
            Vertices = new List<Point>();
            Edges = new List<Edge>();
        }

        public void CalculateNewCoordinates(LightRoute lr)
        {
            for(int i = 0;i<Edges.Count;i++)
            {
                Edges[i].vertices[0] = new Point(Edges[i].vertices[0].X + (int)lr.X, Edges[i].vertices[0].Y + (int)lr.Y);
                Edges[i].vertices[1] = new Point(Edges[i].vertices[1].X + (int)lr.X, Edges[i].vertices[1].Y + (int)lr.Y);
            }
        }
    }
}
