using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace LAB7 {
    internal class SaveLoad {
        public static Polyhedron Load(string filePath) {
            var vertices = new List<XYZPoint>();
            var edges = new List<List<int>>();
            var faces = new List<List<int>>();

            foreach (var line in File.ReadLines(filePath)) {
                string[] parts = line.Split(' ');

                if (parts[0] == "v") {
                    double x = double.Parse(parts[1].Replace('.', ','));
                    double y = double.Parse(parts[2].Replace('.', ','));
                    double z = double.Parse(parts[3].Replace('.', ','));
                    vertices.Add(new XYZPoint(x, y, z));
                    edges.Add(new List<int>());
                }
                else if (parts[0] == "f") {
                    var faceVertices = parts.Skip(1)
                        .Select(p => int.Parse(p.Split('/')[0]) - 1)
                        .Where(index => index >= 0 && index < vertices.Count)
                        .ToList();

                    for (int i = 0; i < faceVertices.Count; ++i) {
                        int start = faceVertices[i];
                        int end = faceVertices[(i + 1) % faceVertices.Count];
                        if (!edges[start].Contains(end)) edges[start].Add(end);
                        if (!edges[end].Contains(start)) edges[end].Add(start);
                    }
                    faces.Add(faceVertices);
                }
            }
            Polyhedron polyhedron = new Polyhedron(vertices, edges, faces);
            polyhedron.SetName(Path.GetFileNameWithoutExtension(filePath));
            return polyhedron;
        }
        public static void Save(Polyhedron polyhedron, string filePath) {
            using (StreamWriter writer = new StreamWriter(filePath)) {
                foreach (var vertex in polyhedron.points)
                    writer.WriteLine($"v {vertex.x} {vertex.y} {vertex.z}");

                var addedFaces = new HashSet<string>();

                foreach (var face in polyhedron.verges) {
                    var faceLine = "f";
                    foreach (var vertexIndex in face)
                        if (vertexIndex < polyhedron.points.Count)
                            faceLine += $" {vertexIndex + 1}";
                    if (!addedFaces.Contains(faceLine)) {
                        writer.WriteLine(faceLine);
                        addedFaces.Add(faceLine);
                    }
                }
            }

            return;
        }
    }
}