using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GK___projekt_2.Containers;
using GK___projekt_2.Geometry;

namespace GK___projekt_2
{
    public static class Painter
    {
        public static void PrepareCanvas(Context context)
        {
            using (Graphics g = Graphics.FromImage(context.canvas.Image))
            {
                g.Clear(Color.DarkCyan);
            }
        }

        private static void DrawSphereNet(Context context)
        {
            using (Graphics g = Graphics.FromImage(context.canvas.Image))
            {
                foreach (Triangle t in context.triangles)
                {
                    foreach (Edge e in t.Edges)
                    {
                        g.DrawLine(context.pen, e.vertices[0], e.vertices[1]);
                    }
                }
            }
        }

        public static void FillSphere(Context context)
        {
            List<Point> verticesOnCurrentScanline = new List<Point>();
            List<Triangle> trianglesOnCurrentScanline = new List<Triangle>();

            Triangle? t;

            int ymin = context.vertices[context.ind[1]].Y;
            int ymax = context.vertices[context.ind[context.ind.Count - 1]].Y;
            int idx = 1;
            int idxT = 0;
            int x, x_next;

            context.AET.Clear();

            for (int y = ymin + 1; y <= ymax; y++)
            {
                verticesOnCurrentScanline.Clear();

                Logic.SortVertices(context, y, ref idx, ref verticesOnCurrentScanline);
                Logic.ManageAET(context, y, ref verticesOnCurrentScanline);
                Logic.SortTriangles(context, y, ref idxT, ref trianglesOnCurrentScanline);

                for (int i = 0; i < context.AET.Count - 1; i++)
                {
                    x = context.AET[i].ScanLineX(y) + 1;
                    x_next = context.AET[i + 1].ScanLineX(y);

                    context.pointInTriangle.X = x;
                    context.pointInTriangle.Y = y;

                    t = null;

                    foreach (var tr in trianglesOnCurrentScanline)
                    {
                        if (Logic.PointInTriangle(context.pointInTriangle, tr.Vertices[0], tr.Vertices[1], tr.Vertices[2]))
                        {
                            t = tr;
                            break;
                        }
                    }

                    for (int k = x; k <= x_next; k++)
                    {
                        context.dbm.SetPixel(k, y, FinalColor(k, y, t, context));
                    }
                }
            }

            if(context.showCheckBox.Checked) DrawSphereNet(context);
        }

        public static Color FinalColor(int k, int y, Triangle? t, Context context)
        {
            if (t == null) return Color.White;

            context.lightVector = (-context.lightRoute.X, context.lightRoute.Y, -1);

            double scalarProduct = Logic.NormalVectorScalarProduct(context.lightVector);
            context.lightVector = (context.lightVector.n1 / scalarProduct,
                                   context.lightVector.n2 / scalarProduct,
                                   context.lightVector.n3 / scalarProduct);

            (double r, double g, double b) finalColor = (0, 0, 0);

            if (context.loadedCheckBox.Checked)
            {
                Logic.CalculateBarycentricCoordinates(ref context, k, y, t);

                context.surfaceNormalVector = (context.alfa0 * t.NormalVectors[2].n1 + context.alfa1 * t.NormalVectors[0].n1 + context.alfa2 * t.NormalVectors[1].n1,
                                    context.alfa0 * t.NormalVectors[2].n2 + context.alfa1 * t.NormalVectors[0].n2 + context.alfa2 * t.NormalVectors[1].n2,
                                    context.alfa0 * t.NormalVectors[2].n3 + context.alfa1 * t.NormalVectors[0].n3 + context.alfa2 * t.NormalVectors[1].n3);

                double cosAngleNL = 0.0, cosAngleVR = 0.0;
                Logic.CalculateCosinusesUsingNormalMap(context, k, y, ref cosAngleNL, ref cosAngleVR);

                finalColor = (
                    (Math.Min(Math.Abs(context.kd * context.lightColor.n1 * context.objectColor.n1 * cosAngleNL + context.ks * context.lightColor.n1 * context.objectColor.n1 * Math.Pow(cosAngleVR, context.mirroring)), 255)),
                    (Math.Min(Math.Abs(context.kd * context.lightColor.n2 * context.objectColor.n2 * cosAngleNL + context.ks * context.lightColor.n2 * context.objectColor.n2 * Math.Pow(cosAngleVR, context.mirroring)), 255)),
                    (Math.Min(Math.Abs(context.kd * context.lightColor.n3 * context.objectColor.n3 * cosAngleNL + context.ks * context.lightColor.n3 * context.objectColor.n3 * Math.Pow(cosAngleVR, context.mirroring)),255)));

                return Color.FromArgb(255, Math.Min((int)(255 * finalColor.r), 255), Math.Min((int)(255 * finalColor.g), 255), Math.Min((int)(255 * finalColor.b), 255));
            }

            if (context.drawFromImage)
            {
                context.objectColor = (context.imageBitmap.GetPixel(Math.Min(Math.Abs(k), context.imageBitmap.Width - 1), Math.Min(y, context.imageBitmap.Height - 1)).R,
                               context.imageBitmap.GetPixel(Math.Min(Math.Abs(k), context.imageBitmap.Width - 1), Math.Min(y, context.imageBitmap.Height - 1)).G,
                               context.imageBitmap.GetPixel(Math.Min(Math.Abs(k), context.imageBitmap.Width - 1), Math.Min(y, context.imageBitmap.Height - 1)).B);

                context.objectColor = (Logic.Map((double)context.objectColor.n1, 0.0, 255.0, 0.0, 1.0),
                               Logic.Map((double)context.objectColor.n2, 0.0, 255.0, 0.0, 1.0),
                               Logic.Map((double)context.objectColor.n3, 0.0, 255.0, 0.0, 1.0));
            }

            if (context.byPointRadioButton.Checked)
            {
                Logic.CalculateBarycentricCoordinates(ref context, k, y, t);

                context.surfaceNormalVector = (context.alfa0 * t.NormalVectors[2].n1 + context.alfa1 * t.NormalVectors[0].n1 + context.alfa2 * t.NormalVectors[1].n1,
                                    context.alfa0 * t.NormalVectors[2].n2 + context.alfa1 * t.NormalVectors[0].n2 + context.alfa2 * t.NormalVectors[1].n2,
                                    context.alfa0 * t.NormalVectors[2].n3 + context.alfa1 * t.NormalVectors[0].n3 + context.alfa2 * t.NormalVectors[1].n3);

                double cosAngleNL = 0.0, cosAngleVR = 0.0;
                Logic.CalculateCosinusesUsingByPointInterpolation(context, k, y, ref cosAngleNL, ref cosAngleVR);


                finalColor = (
                    (Math.Min(context.kd * context.lightColor.n1 * context.objectColor.n1 * cosAngleNL + context.ks * context.lightColor.n1 * context.objectColor.n1 * Logic.MyPow(cosAngleVR, context.mirroring),1.0)),
                    (Math.Min(context.kd * context.lightColor.n2 * context.objectColor.n2 * cosAngleNL + context.ks * context.lightColor.n2 * context.objectColor.n2 * Logic.MyPow(cosAngleVR, context.mirroring),1.0)),
                    (Math.Min(context.kd * context.lightColor.n3 * context.objectColor.n3 * cosAngleNL + context.ks * context.lightColor.n3 * context.objectColor.n3 * Logic.MyPow(cosAngleVR, context.mirroring),1.0)));

                return Color.FromArgb(255, Math.Min((int)(255 * finalColor.r), 255), Math.Min((int)(255 * finalColor.g), 255), Math.Min((int)(255 * finalColor.b), 255));
            }
            else
            {
                List<(double r, double g, double b)> finalColors = new List<(double, double, double)>();
                finalColors.Add((0, 0, 0));
                finalColors.Add((0, 0, 0));
                finalColors.Add((0, 0, 0));

                for (int i = 0; i < finalColors.Count; i++)
                {
                    double cosAngleNL = 0.0, cosAngleVR = 0.0;
                    Logic.CalculateCosinusesUsingByColorInterpolation(context, t, i, ref cosAngleNL, ref cosAngleVR);

                    finalColors[i] = (
                        (context.kd * context.lightColor.n1 * context.objectColor.n1 * cosAngleNL + context.ks * context.lightColor.n1 * context.objectColor.n1 * Logic.MyPow(cosAngleVR, context.mirroring)),
                        (context.kd * context.lightColor.n2 * context.objectColor.n2 * cosAngleNL + context.ks * context.lightColor.n2 * context.objectColor.n2 * Logic.MyPow(cosAngleVR, context.mirroring)),
                        (context.kd * context.lightColor.n3 * context.objectColor.n3 * cosAngleNL + context.ks * context.lightColor.n3 * context.objectColor.n3 * Logic.MyPow(cosAngleVR, context.mirroring)));
                }

                Logic.CalculateBarycentricCoordinates(ref context, k, y, t);
                finalColor = (context.alfa1*finalColors[0].r + context.alfa2*finalColors[1].r + context.alfa0 * finalColors[2].r,
                              context.alfa1 * finalColors[0].g + context.alfa2 * finalColors[1].g + context.alfa0 * finalColors[2].g,
                              context.alfa1 * finalColors[0].b + context.alfa2 * finalColors[1].b + context.alfa0 * finalColors[2].b);

                return Color.FromArgb(255, Math.Min((int)(255 * finalColor.r), 255), Math.Min((int)(255 * finalColor.g), 255), Math.Min((int)(255 * finalColor.b), 255));
            }
        }
    }
}
