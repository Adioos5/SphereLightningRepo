using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GK___projekt_2.Engine;
using GK___projekt_2.Geometry;

namespace GK___projekt_2.Containers
{
    public class Context
    {
        public List<(double, double, double)> normalVectors = new List<(double, double, double)>();
        public List<int> ind = new List<int>();
        public List<int> indT = new List<int>();
        public List<Triangle> triangles = new List<Triangle>();
        public List<Edge> ET = new List<Edge>();
        public List<Edge> AET = new List<Edge>();
        public List<Point> vertices = new List<Point>();
        public List<List<int>> verticesTrianglesIndexes = new List<List<int>>();
        public List<List<Triangle>> trianglesBasedOnDistanceFromCenter = new List<List<Triangle>>();

        public Point pointInTriangle = new Point(0, 0);

        public (double n1, double n2, double n3) surfaceNormalVector, objectColor, lightColor, lightVector, eyeVector, normalMapNormalVector;

        public double dist0, dist1, dist2;
        public double side0, side1, side2;
        public double pw, p0, p1, p2;
        public double wholeTriangleArea, triangle0Area, triangle1Area, triangle2Area;
        public double alfa0, alfa1, alfa2;
        
        public double kd, ks, ka;
        public int mirroring;

        public bool drawFromImage = false;

        public LightRoute lightRoute;

        public DirectBitmap dbm;
        public Bitmap imageBitmap;
        public Pen pen = new Pen(Brushes.Black, Constants.THICKNESS);
        public Brush brush = Brushes.White;

        public PictureBox canvas;
        public RadioButton byPointRadioButton, byColorRadioButton;
        public CheckBox loadedCheckBox;
        public CheckBox showCheckBox;

        public Context(PictureBox _canvas, RadioButton _byPointRadioButton, RadioButton _byColorRadioButton, CheckBox _loadedCheckBox, CheckBox _showCheckBox)
        {
            canvas = _canvas;
            byPointRadioButton = _byPointRadioButton;
            byColorRadioButton = _byColorRadioButton;
            loadedCheckBox = _loadedCheckBox;
            showCheckBox = _showCheckBox;

            for (int l = 0; l < 5; l++)
                trianglesBasedOnDistanceFromCenter.Add(new List<Triangle>());

            PictureBox PictureBox1 = new PictureBox();

            PictureBox1.Image = Logic.ResizeImage(new Bitmap("..\\..\\..\\Images\\ball.jpg"), new Size(canvas.Size.Width, canvas.Size.Height));

            imageBitmap = (Bitmap)PictureBox1.Image;

            kd = 1;
            ks = 0;
            ka = 0;
            mirroring = 1;

            objectColor = (1, 1, 1);
            lightColor = (1,1,1);
            eyeVector = (0, 0, 1);

            vertices.Add(new Point(0, 0));
            verticesTrianglesIndexes.Add(new List<int>());

            normalVectors.Add((0, 0, 0));

            dbm = new DirectBitmap(canvas.Size.Width, canvas.Size.Height);
            canvas.Image = dbm.Bitmap;

            lightRoute = new LightRoute(0.7);
            lightRoute.CalculateNewCoordinates(0);
        }
    }
}
