using GK___projekt_2.Geometry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GK___projekt_2
{
    public class Triangle
    {
        public List<Point> Vertices { get; set; }
        public List<Edge> Edges { get; set; }
        public (double n1, double n2, double n3) NormalVector { get; set; }
        public List<(double n1, double n2, double n3)> NormalVectors { get; set; }

        public int ymax;
        public int ymin; 

        public Triangle(List<Point> vertices, List<(double n1, double n2, double n3)> normalVectors)
        {
            NormalVectors = normalVectors;

            Vertices = vertices;
            Edges = new List<Edge>();
            
            ymin = vertices[0].Y;
            ymax = vertices[0].Y;

            foreach (var v in vertices)
            {
                ymin = Math.Min(ymin, v.Y);
                ymax = Math.Max(ymax, v.Y);
            }

            NormalVector = ((normalVectors[0].n1 + normalVectors[1].n1 + normalVectors[2].n1) / 3.0,
                            (normalVectors[0].n2 + normalVectors[1].n2 + normalVectors[2].n2) / 3.0,
                            (normalVectors[0].n3 + normalVectors[1].n3 + normalVectors[2].n3) / 3.0);    

            double scalarProduct = Logic.NormalVectorScalarProduct(NormalVector);

            NormalVector = ((double)NormalVector.n1 / (double)scalarProduct, (double)NormalVector.n2 / (double)scalarProduct, (double)NormalVector.n3 / (double)scalarProduct);
           
        }

        public bool Contains(Point p)
        {
            return Vertices.Contains(p);
        }

        public void ConfigureEdges(List<Edge> ET)
        {
            for (int i = 0; i < Vertices.Count; i++)
            {
                foreach (var e in ET)
                {
                    if (e.Contains(Vertices[i]) && e.Contains(Vertices[(i+1)%3]) && !Edges.Contains(e))
                    {
                        Edges.Add(e);
                        break;
                    }
                }
            }
        }

        public (Point v1, Point v2) Neighbours(Point v)
        {
            return (Vertices[(Vertices.IndexOf(v)+1)%3], Vertices[(Vertices.IndexOf(v) + 2) % 3]);
        }

    }
}
