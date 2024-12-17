using MathNet.Numerics.Distributions;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace LAB7 {
    internal class BackfaceCulling {
        public static Bitmap Cull(double[,] matrix, ListBox.ObjectCollection polyhedrons, int width, int height) {
            var bmp = new Bitmap(width, height);
            var graphics = Graphics.FromImage(bmp);
            graphics.Clear(Color.White);
            var viewDirection = new XYZPoint(0, 0, 1);

            // Вектор обзора, направленный из экрана к пользователю
            if (matrix[2,3] == 0)
            {
                viewDirection = new XYZPoint(-1, 1, 1);
            }


            double length = Math.Sqrt(viewDirection.x * viewDirection.x + viewDirection.y * viewDirection.y + viewDirection.z * viewDirection.z);
            viewDirection.x /= length;
            viewDirection.y /= length;
            viewDirection.z /= length;


            var random = new Random(42);

            for (var i = 0; i < polyhedrons.Count; ++i) {
                var polyhedron = polyhedrons[i] as Polyhedron;
                var transformedVertices = new List<XYZPoint>();

                // Преобразование вершин с использованием матрицы трансформации
                foreach (var v in polyhedron.points) {
                    var result = AffineTransformations.Multiply(new double[,] { { v.x, v.y, v.z, 1 } }, matrix);
                    transformedVertices.Add(new XYZPoint(result[0, 0], result[0, 1], result[0, 2]));
                }
                var visibleFaces = new List<List<int>>();

                // Определение видимых граней по нормалям
                foreach (var face in polyhedron.verges) {
                    var normal = EnsureNormalDirectionOutward(polyhedron.Center(), GetNormalVector(face, polyhedron.points), face, polyhedron.points);
                    double lengthh = Math.Sqrt(normal.x * normal.x + normal.y * normal.y + normal.z * normal.z);
                    normal.x /= lengthh;
                    normal.y /= lengthh;
                    normal.z /= lengthh;
                    var scalar = normal.x * viewDirection.x + normal.y * viewDirection.y + normal.z * viewDirection.z;
                    if (scalar < 0) visibleFaces.Add(face); 
                }

                // Отрисовка видимых граней
                foreach (var face in visibleFaces) {
                    var points = new List<Point>();
                    for (var j = 0; j < face.Count; ++j) {
                        var projectedPoint = ProjectTo2D(transformedVertices[face[j]]);
                        points.Add(new Point((int)projectedPoint.x, (int)projectedPoint.y));
                    }

                    // Генерация случайного цвета для каждой грани
                    var color = Color.FromArgb(random.Next(100, 256), random.Next(100, 256), random.Next(100, 256));

                    // Закрашивание грани случайным цветом
                    graphics.FillPolygon(new SolidBrush(color), points.ToArray());
                    graphics.DrawPolygon(Pens.Black, points.ToArray());
                }
            }

            graphics.Dispose();
            return bmp;
        }

        private static XYZPoint GetNormalVector(List<int> face, List<XYZPoint> vertices) => CrossProduct(vertices[face[1]] - vertices[face[0]], vertices[face[2]] - vertices[face[0]]);

        public static XYZPoint EnsureNormalDirectionOutward(XYZPoint objectCenter, XYZPoint Normal, List<int> face, List<XYZPoint> points)
        {

            var toCenter = new XYZPoint(
                objectCenter.x - points[face[0]].x,
                objectCenter.y - points[face[0]].y,
                objectCenter.z - points[face[0]].z
            );

            double dotProduct = Normal.x * toCenter.x + Normal.y * toCenter.y + Normal.z * toCenter.z;

            if (dotProduct > 0) // Нормаль направлена внутрь, инвертируем её
            {
                Normal.x = -Normal.x;
                Normal.y = -Normal.y;
                Normal.z = -Normal.z;
            }
            return Normal;
        }
        private static XYZPoint CrossProduct(XYZPoint vec1, XYZPoint vec2) => new XYZPoint(
                vec1.y * vec2.z - vec1.z * vec2.y,
                vec1.z * vec2.x - vec1.x * vec2.z,
                vec1.x * vec2.y - vec1.y * vec2.x
            );

        private static XYZPoint ProjectTo2D(XYZPoint v) => new XYZPoint(v.x, v.y, 0);
    }
}
