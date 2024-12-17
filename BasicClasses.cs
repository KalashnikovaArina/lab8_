using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/// <summary>
/// Вершина
/// </summary>
internal class XYZPoint
{
    /// <summary>
    /// Координата по X
    /// </summary>
    public double x;
    /// <summary>
    /// Координата по Y
    /// </summary>
    public double y;
    /// <summary>
    /// Координата по Z
    /// </summary>
    public double z;
    public XYZPoint(double x, double y, double z) { this.x = x; this.y = y; this.z = z; }
    public XYZPoint(XYZPoint other) { this.x = other.x; this.y = other.y; this.z = other.z; }

    /// <summary>
    /// Проверка на равенство двух вершин
    /// </summary>
    /// <param name="Point1">Вершина 1</param>
    /// <param name="Point2">Вершина 1</param>
    /// <returns>Равны ли координаты двух вершин</returns>
    static public bool operator ==(XYZPoint Point1, XYZPoint Point2) => Point1.x == Point2.x && Point1.y == Point2.y && Point1.z == Point2.z;
    /// <summary>
    /// Проверка на неравенство двух вершин
    /// </summary>
    /// <param name="Point1">Вершина 1</param>
    /// <param name="Point2">Вершина 1</param>
    /// <returns>Не равны ли координаты двух вершин</returns>
    static public bool operator !=(XYZPoint Point1, XYZPoint Point2) => !(Point1 == Point2);
    /// <summary>
    /// Сложение двух вершин
    /// </summary>
    /// <param name="Point1">Вершина 1</param>
    /// <param name="Point2">Вершина 1</param>
    /// <returns>Вершина из суммы координат двух вершин</returns>
    static public XYZPoint operator +(XYZPoint Point1, XYZPoint Point2) => new XYZPoint(Point1.x + Point2.x, Point1.y + Point2.y, Point1.z + Point2.z);
    /// <summary>
    /// Разность двух вершин
    /// </summary>
    /// <param name="Point1">Вершина 1</param>
    /// <param name="Point2">Вершина 1</param>
    /// <returns>Вершина из разности координат двух вершин</returns>
    static public XYZPoint operator -(XYZPoint Point1, XYZPoint Point2) => new XYZPoint(Point1.x - Point2.x, Point1.y - Point2.y, Point1.z - Point2.z);
    /// <summary>
    /// Строковое представление вершины
    /// </summary>
    /// <returns>Координаты вершины</returns>
    public override string ToString() => $"X: {Math.Round(x, 2)}, Y: {Math.Round(y, 2)}, Z: {Math.Round(z, 2)}";
}

/// <summary>
/// Полигон
/// </summary>
internal class Polygon
{
    /// <summary>
    /// Список вершин
    /// </summary>
    public List<XYZPoint> points = new List<XYZPoint>();
    public Polygon(List<XYZPoint> vertices) => this.points = vertices;
}

/// <summary>
/// Многогранник
/// </summary>
internal class Polyhedron
{
    /// <summary>
    /// Список вершин
    /// </summary>
    public List<XYZPoint> points = new List<XYZPoint>();
    //матрица смежности -
    // кол-во элементов листа = колву вершин, каждой вершине сорответствует лист в котором перечислены номера вершин с которыми она смежна 
    /// <summary>
    /// Список рёбер
    /// </summary>
    public List<List<int>> edges = new List<List<int>>();

    /// <summary>
    /// Список граней
    /// </summary>
    public List<List<int>> verges = new List<List<int>>();

    private string name;
    /// <summary>
    /// Задать имя многогранника
    /// </summary>
    public void SetName(string name) => this.name = name;
    
    /// <summary>
    /// Конструктор по умолчанию
    /// </summary>
    public Polyhedron()
    {
        //vertices.Add(new Vertex(0,0,0));
        //edges.Add(new List<int>() { });
    }

    /// <summary>
    /// Конструктор
    /// </summary>
    /// <param name="points">Список вершин</param>
    /// <param name="edges">Список рёбер</param>
    /// <param name="verges">Список граней</param>
    public Polyhedron(List<XYZPoint> points, List<List<int>> edges, List<List<int>> verges = null)
    {
        this.points = points;
        this.edges = edges;
        this.verges = verges;
    }

    public Polyhedron(Polyhedron polyhedron)
    {
        this.points = polyhedron.points;
        this.edges = polyhedron.edges;
        this.verges = polyhedron.verges;
    }
    /// <summary>
    /// Возвращает точку центра многогранника
    /// </summary>
    /// <returns></returns>
    public XYZPoint Center() {
        double x = points.Average(v => v.x);
        double y = points.Average(v => v.y);
        double z = points.Average(v => v.z);
        return new XYZPoint(x, y, z);
    }

    public override string ToString()
    {
        if (name == null)
            return base.ToString();
        else
            return name;
    }
}