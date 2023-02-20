using GK___projekt_2.Containers;
using GK___projekt_2.Geometry;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GK___projekt_2
{
    public static class Logic
    {
        public static float Sign(Point p1, Point p2, Point p3)
        {
            return (p1.X - p3.X) * (p2.Y - p3.Y) - (p2.X - p3.X) * (p1.Y - p3.Y);
        }

        public static bool PointInTriangle(Point pt, Point v1, Point v2, Point v3)
        {
            float d1, d2, d3;
            bool has_neg, has_pos;

            d1 = Sign(pt, v1, v2);
            d2 = Sign(pt, v2, v3);
            d3 = Sign(pt, v3, v1);

            has_neg = (d1 < 0) || (d2 < 0) || (d3 < 0);
            has_pos = (d1 > 0) || (d2 > 0) || (d3 > 0);

            return !(has_neg && has_pos);
        }

        public static double MyPow(double num, int exp)
        {
            double result = 1.0;
            while (exp > 0)
            {
                if (exp % 2 == 1)
                    result *= num;
                exp >>= 1;
                num *= num;
            }

            return result;
        }

        public static void CalculateCosinusesUsingNormalMap(Context context, int k, int y, ref double cosAngleNL, ref double cosAngleVR)
        {
            Color c = context.imageBitmap.GetPixel(k, y);
            (double n1, double n2, double n3) N_texture = (
                                                           Logic.Map(c.R, 0, 255, -1, 1),
                                                           Logic.Map(c.G, 0, 255, -1, 1),
                                                           Logic.Map(c.B, 0, 255, 0, 1)
                                                           );

            (double n1, double n2, double n3) B = context.surfaceNormalVector == (0, 0, 1) ? (0, 1, 0) : (context.surfaceNormalVector.n2, -context.surfaceNormalVector.n1, 0);
            (double n1, double n2, double n3) T = (B.n2 * context.surfaceNormalVector.n3 - B.n3 * context.surfaceNormalVector.n2,
                                                   B.n3 * context.surfaceNormalVector.n1 - B.n1 * context.surfaceNormalVector.n3,
                                                   B.n1 * context.surfaceNormalVector.n2 - B.n2 * context.surfaceNormalVector.n1);

            double[,] M = { { T.n1, B.n1, context.surfaceNormalVector.n1 },
                                    { T.n2, B.n2, context.surfaceNormalVector.n2 },
                                    { T.n3, B.n3, context.surfaceNormalVector.n3 }};

            context.normalMapNormalVector = (M[0, 0] * N_texture.n1 + M[0, 1] * N_texture.n2 + M[0, 2] * N_texture.n3
                                   , M[1, 0] * N_texture.n1 + M[1, 1] * N_texture.n2 + M[1, 2] * N_texture.n3
                                   , M[2, 0] * N_texture.n1 + M[2, 1] * N_texture.n2 + M[2, 2] * N_texture.n3);

            cosAngleNL = context.normalMapNormalVector.n1 * context.lightVector.n1 + context.normalMapNormalVector.n2 * context.lightVector.n2 + context.normalMapNormalVector.n3 * context.lightVector.n3;

            (double n1, double n2, double n3) RVector = (
                2 * (cosAngleNL) * context.normalMapNormalVector.n1 - context.lightVector.n1,
                2 * (cosAngleNL) * context.normalMapNormalVector.n2 - context.lightVector.n2,
                2 * (cosAngleNL) * context.normalMapNormalVector.n3 - context.lightVector.n3
                );

            cosAngleVR = context.eyeVector.n1 * RVector.n1 + context.eyeVector.n2 * RVector.n2 + context.eyeVector.n3 * RVector.n3;

            cosAngleNL = Math.Max(0, cosAngleNL);
            cosAngleVR = Math.Max(0, cosAngleVR);
        }

        public static double NormalVectorScalarProduct((double n1, double n2, double n3) normalVector)
        {
            return normalVector.n1 * normalVector.n1 +
                normalVector.n2 * normalVector.n2 +
                normalVector.n3 * normalVector.n3;
        }

        public static void CalculateCosinusesUsingByPointInterpolation(Context context, int k, int y, ref double cosAngleNL, ref double cosAngleVR)
        {
            cosAngleNL = context.surfaceNormalVector.n1 * context.lightVector.n1 + context.surfaceNormalVector.n2 * context.lightVector.n2 + context.surfaceNormalVector.n3 * context.lightVector.n3;

            (double n1, double n2, double n3) RVector = (
                2 * (cosAngleNL) * context.surfaceNormalVector.n1 - context.lightVector.n1,
                2 * (cosAngleNL) * context.surfaceNormalVector.n2 - context.lightVector.n2,
                2 * (cosAngleNL) * context.surfaceNormalVector.n3 - context.lightVector.n3
                );

            cosAngleVR = context.eyeVector.n1 * RVector.n1 + context.eyeVector.n2 * RVector.n2 + context.eyeVector.n3 * RVector.n3;

            cosAngleNL = Math.Max(0, cosAngleNL);
            cosAngleVR = Math.Max(0, cosAngleVR);
        }

        public static void CalculateCosinusesUsingByColorInterpolation(Context context, Triangle t, int i, ref double cosAngleNL, ref double cosAngleVR)
        {
            cosAngleNL = t.NormalVectors[i].n1 * context.lightVector.n1 + t.NormalVectors[i].n2 * context.lightVector.n2 + t.NormalVectors[i].n3 * context.lightVector.n3;

            (double n1, double n2, double n3) RVector = (
                2 * (cosAngleNL) * t.NormalVectors[i].n1 - context.lightVector.n1,
                2 * (cosAngleNL) * t.NormalVectors[i].n2 - context.lightVector.n2,
                2 * (cosAngleNL) * t.NormalVectors[i].n3 - context.lightVector.n3
                );

            cosAngleVR = context.eyeVector.n1 * RVector.n1 + context.eyeVector.n2 * RVector.n2 + context.eyeVector.n3 * RVector.n3;

            cosAngleNL = Math.Max(0, cosAngleNL);
            cosAngleVR = Math.Max(0, cosAngleVR);
        }

        public static void CalculateBarycentricCoordinates(ref Context context, int k, int y, Triangle t)
        {
            context.dist0 = Math.Sqrt(Math.Pow(k - t.Vertices[0].X, 2) + MyPow(y - t.Vertices[0].Y, 2));
            context.dist1 = Math.Sqrt(Math.Pow(k - t.Vertices[1].X, 2) + MyPow(y - t.Vertices[1].Y, 2));
            context.dist2 = Math.Sqrt(Math.Pow(k - t.Vertices[2].X, 2) + MyPow(y - t.Vertices[2].Y, 2));

            context.side0 = Math.Sqrt(Math.Pow(t.Vertices[0].X - t.Vertices[1].X, 2) + MyPow(t.Vertices[0].Y - t.Vertices[1].Y, 2));
            context.side1 = Math.Sqrt(Math.Pow(t.Vertices[1].X - t.Vertices[2].X, 2) + MyPow(t.Vertices[1].Y - t.Vertices[2].Y, 2));
            context.side2 = Math.Sqrt(Math.Pow(t.Vertices[2].X - t.Vertices[0].X, 2) + MyPow(t.Vertices[2].Y - t.Vertices[0].Y, 2));

            context.pw = (context.side0 + context.side1 + context.side2) / 2;
            context.p0 = (context.side0 + context.dist0 + context.dist1) / 2;
            context.p1 = (context.side1 + context.dist1 + context.dist2) / 2;
            context.p2 = (context.side2 + context.dist2 + context.dist0) / 2;

            context.wholeTriangleArea = Math.Sqrt(context.pw * (context.pw - context.side0) * (context.pw - context.side1) * (context.pw - context.side2));
            context.triangle0Area = Math.Sqrt(Math.Abs(context.p0 * (context.p0 - context.side0) * (context.p0 - context.dist0) * (context.p0 - context.dist1)));
            context.triangle1Area = Math.Sqrt(Math.Abs(context.p1 * (context.p1 - context.side1) * (context.p1 - context.dist1) * (context.p1 - context.dist2)));
            context.triangle2Area = Math.Sqrt(Math.Abs(context.p2 * (context.p2 - context.side2) * (context.p2 - context.dist2) * (context.p2 - context.dist0)));

            context.alfa0 = context.triangle0Area / context.wholeTriangleArea;
            context.alfa1 = context.triangle1Area / context.wholeTriangleArea;
            context.alfa2 = context.triangle2Area / context.wholeTriangleArea;
        }

        public static double Map(double value, double fromSource, double toSource, double fromTarget, double toTarget)
        {
            return (value - fromSource) / (toSource - fromSource) * (toTarget - fromTarget) + fromTarget;
        }

        public static Image ResizeImage(Image imgToResize, Size size)
        {
            return (Image)(new Bitmap(imgToResize, size));
        }

        public static void SortVertices(Context context, int y, ref int idx, ref List<Point> verticesOnCurrentScanline)
        {
            while (context.vertices[context.ind[idx]].Y < y - 1)
                idx++;
            while (context.vertices[context.ind[idx]].Y == y - 1)
                verticesOnCurrentScanline.Add(context.vertices[context.ind[idx++]]);
        }

        public static void SortTriangles(Context context, int y, ref int idxT, ref List<Triangle> trianglesOnCurrentScanline)
        {
            if (idxT < context.indT.Count && context.triangles[context.indT[idxT]].ymax >= y && context.triangles[context.indT[idxT]].ymin <= y)
            {
                for (int i = 0; i < trianglesOnCurrentScanline.Count; i++)
                {
                    if (trianglesOnCurrentScanline[i].ymax <= y) trianglesOnCurrentScanline.RemoveAt(i);
                }
                do
                {
                    trianglesOnCurrentScanline.Add(context.triangles[context.indT[idxT++]]);
                } while (idxT < context.indT.Count && context.triangles[context.indT[idxT]].ymax >= y && context.triangles[context.indT[idxT]].ymin <= y);
            }
        }

        public static void ManageAET(Context context, int y, ref List<Point> verticesOnCurrentScanline)
        {
            List<int> trianglesIndexes;
            Triangle triangle;

            foreach (var vertex in verticesOnCurrentScanline)
            {
                trianglesIndexes = context.verticesTrianglesIndexes[context.vertices.IndexOf(vertex)];

                for (int i = 0; i < trianglesIndexes.Count; i++)
                {
                    triangle = context.triangles[trianglesIndexes[i]];

                    foreach (var edge in triangle.Edges)
                    {
                        if (edge.Contains(vertex) && edge.Contains(triangle.Neighbours(vertex).v2))
                        {
                            if (triangle.Neighbours(vertex).v2.Y > vertex.Y)
                            {
                                if (!context.AET.Contains(edge)) context.AET.Add(edge);
                            }
                            else
                            {
                                context.AET.Remove(edge);
                            }
                        }
                        else if (edge.Contains(vertex) && edge.Contains(triangle.Neighbours(vertex).v1))
                        {
                            if (triangle.Neighbours(vertex).v1.Y > vertex.Y)
                            {
                                if (!context.AET.Contains(edge)) context.AET.Add(edge);
                            }
                            else
                            {
                                context.AET.Remove(edge);
                            }
                        }
                    }
                }
            }
            context.AET = context.AET.OrderBy(e => e.ScanLineX(y)).ToList();
        }

        public static void InitializeEngine(Context context)
        {
            ReadOBJ("..\\..\\..\\proj2_sfera.obj", context);

            foreach (Triangle t in context.triangles)
            {
                t.ConfigureEdges(context.ET);
            }
            context.ind.Clear();
            context.ind.Add(0);

            for (int k = 1; k < context.vertices.Count; k++)
                context.ind.Add(k);

            for (int i = 1; i < context.vertices.Count; i++)
                for (int j = 1; j < context.vertices.Count; j++)
                {
                    if (context.vertices[context.ind[i]].Y < context.vertices[context.ind[j]].Y)
                    {
                        int temp = context.ind[i];
                        context.ind[i] = context.ind[j];
                        context.ind[j] = temp;
                    }
                }

            context.indT.Clear();

            for (int k = 0; k < context.triangles.Count; k++)
                context.indT.Add(k);

            for (int i = 0; i < context.triangles.Count; i++)
                for (int j = 0; j < context.triangles.Count; j++)
                {
                    if (context.triangles[context.indT[i]].ymin < context.triangles[context.indT[j]].ymin)
                    {
                        int temp = context.indT[i];
                        context.indT[i] = context.indT[j];
                        context.indT[j] = temp;
                    }
                }
        }

        private static void ReadOBJ(string path, Context context)
        {
            using (StreamReader sr = new StreamReader(path))
            {
                while (sr.Peek() >= 0)
                {
                    var line = sr.ReadLine();
                    if (line != null && line[0] == 'v' && line[1] != 'n')
                    {
                        var coordinates = line.Split();

                        context.vertices.Add(new Point((int)(double.Parse(coordinates[1], CultureInfo.InvariantCulture) * 328 + context.canvas.Size.Width / 2),
                            (int)(double.Parse(coordinates[2], CultureInfo.InvariantCulture) * 328 + context.canvas.Size.Height / 2)));
                        context.verticesTrianglesIndexes.Add(new List<int>());
                    }
                    else
                    if (line != null && line[0] == 'v' && line[1] == 'n')
                    {
                        var values = line.Split();

                        var n1 = Double.Parse(values[1], CultureInfo.InvariantCulture);
                        var n2 = Double.Parse(values[2], CultureInfo.InvariantCulture);
                        var n3 = Double.Parse(values[3], CultureInfo.InvariantCulture);
                        
                        context.normalVectors.Add((n1, n2, n3));
                    }
                    if (line != null && line[0] == 'f')
                    {
                        var indices = line.Split();

                        var v1 = context.vertices[int.Parse(indices[1].ReadUntilCharacter('/'))];
                        var v2 = context.vertices[int.Parse(indices[2].ReadUntilCharacter('/'))];
                        var v3 = context.vertices[int.Parse(indices[3].ReadUntilCharacter('/'))];

                        (double, double, double) nv1 = context.normalVectors[int.Parse(indices[1].ReadUntilCharacter('/'))];
                        (double, double, double) nv2 = context.normalVectors[int.Parse(indices[2].ReadUntilCharacter('/'))];
                        (double, double, double) nv3 = context.normalVectors[int.Parse(indices[3].ReadUntilCharacter('/'))];

                        context.triangles.Add(new Triangle(new List<Point> { v1, v2, v3 }, new List<(double, double, double)> { nv1, nv2, nv3 }));

                        context.verticesTrianglesIndexes[context.vertices.IndexOf(v1)].Add(context.triangles.Count - 1);
                        context.verticesTrianglesIndexes[context.vertices.IndexOf(v2)].Add(context.triangles.Count - 1);
                        context.verticesTrianglesIndexes[context.vertices.IndexOf(v3)].Add(context.triangles.Count - 1);

                        // Prevent from adding the same edge twice
                        bool a = true, b = true, c = true;
                        foreach (var e in context.ET)
                        {
                            if (e.Contains(v1) && e.Contains(v2)) a = false;
                            if (e.Contains(v2) && e.Contains(v3)) b = false;
                            if (e.Contains(v3) && e.Contains(v1)) c = false;
                        }
                        if (a) context.ET.Add(new Edge(v1, v2));
                        if (b) context.ET.Add(new Edge(v2, v3));
                        if (c) context.ET.Add(new Edge(v3, v1));
                    }
                }
            }
        }
    }
}
