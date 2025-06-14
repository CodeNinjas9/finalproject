using Unity;
using UnityEngine;
using System.Collections.Generic;
public class GeneratePolygonMap : MonoBehaviour() 
{ 
    void Start()
    {

    }
    void Update()
    {

    }
    float[] generatePolygonMap(float a, float b)
    {
        float[] map = new float[a + b];
        foreach(float item: map)
        {
            item = Random.Range(a, a + b);
        }
        return map;
    }
    int[] generatePartitionPoints(int points_generate, float a, float b)
    {
        int[] points_map = new int[points_generate];
        foreach(int item: map)
        {
            item = Mathf.Floor(Random.Range(a, a + b));
        }
    }
    float[] centroidPartition(float[] givenPoints)
    {
        int v = 0;
        float summa = 0;
        foreach(float point: givenPoints)
        {
            v++;
            summa += point;
        }
        float centroid = summa / v; 
        return Mathf.Floor(centroid);
    }
    public class Vertice
    {
        double x; 
        double y; 
        public Vertice(double x, double y)
        {
            this.x = x;
            this.y = y;
        }
        public static bool operator == (Vertice lhs, Vertice rhs)
        {
            return (lhs.x == rhs.x) && (rhs.y == lhs.y);
        }
    }
    public class Edge
    {
        public Vertice a
        {
            get {return a;}
            set {a = value;}
        }
        public Vertice b
        {
            get {return b;}
            set {b = value;}
        }
        public Edge(Vertice a, Vertice b)
        {
            this.a = a;
            this.b = b;
        }
        public static bool operator == (Edge f, Edge d)
        {
            return (f.a.equals(d.a)) && (d.b.equals(f.b));
        }
    }
    public class Triangle
    {
        Edge v1;
        Edge v2;
        Edge v3;
        Vertice v0;
        Vertice v0_1;
        Vertice v0_2;
        double dx1;
        double dy1;
        double r1
        public Vertice CircumcenterTriangle()
        {
            Vertice k = new Vertice((v1.a.x + v1.b.x) / 2, (v1.b.y + v1.a.y) / 2);
            Vertice b = new Vertice((v2.a.x + v2.b.x) / 2, (v2.b.y + v2.a.y) / 2);
            double k_slope = -1/(k.y / k.x);
            double b_slope = -1/(b.y / b.x);
            double k_step = k.b + k_slope * (1 - k.a);
            double b_step = b.b + b_slope * (1 - b.a);
            Vertice n = new Vertice(k_slope * (b_step) + k_step, b_slope * (k_step) + b_step);
        }
        public Triangle(Vertice a, Vertice b, Vertice c)
        {
            this.v0 = a;
            this.v0_1 = b;
            this.v0_2 = c;
            this.v1 = new Edge(a, b);
            this.v2 = new Edge(b, c);
            this.v3 = new Edge(a, c);
        }
        public float obtainCircumCircle(Vertice k)
        {
            Vertice z = CircumcenterTriangle();
            double dx = z.x - k.x;
            double dy = z.y - k.y;
            double r = Mathf.Pow(dx * dx + dy * dy, 1/2);
            this.dx1 = dx;
            this.dy1 = dy;
            this.r1 = r;
        }
        
    }
    public Triangle createSuperTriangle(Vertice[] points)
    {
        double min_x = Double.positiveInfinity;
        double min_y = Double.positiveInfinity;
        double max_x = Double.negativeInfinity;
        double max_y = Double.negativeInfinity;
        foreach(Vertice item: points)
        {
            max_x = Mathf.Max(max_x, item.x);
            max_y = Mathf.Max(max_y, item.y);
            min_x = Mathf.Min(min_x, item.x);
            min_y = Mathf.Min(min_y, item.y);
        }
        double dx = (max_x - min_x) * 10;
        double dy = (max_y - min_y) * 10;
        double v_a = new Vertice(min_x - dx, min_y - dy * 3);
        double v_b = new Vertice(min_x - dx, max_y + dy);
        double v_c = new Vertice(max_x + dx, max_y + dy);
        return new Triangle(v_a, v_b, v_c);
    }
    public double[] Triangulate(Vertice[] vertices, int size)
    {
        Triangle superTriangle = createSuperTriangle(vertices);
        Triangle[] triangles = [superTriangle];
        
    }
}
