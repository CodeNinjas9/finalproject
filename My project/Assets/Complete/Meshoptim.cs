using System;
using System.Collections.Generic;
using System.Numerics;

public class QEMMeshSimplifier
{
    public struct Triangle
    {
        public int a, b, c;
        public Triangle(int a, int b, int c) { this.a = a; this.b = b; this.c = c; }
    }
    public struct Edge
    {
        public int v0, v1;
        public float cost;
        public Vector3 optimalPos;
        public Edge(int v0, int v1) { this.v0 = v0; this.v1 = v1; cost = 0; optimalPos = Vector3.Zero; }
    }
    public struct SymmetricMatrix
    {
        public float m00, m01, m02, m03;
        public float m11, m12, m13;
        public float m22, m23;
        public float m33;
        public SymmetricMatrix(float a, float b, float c, float d)
        {
            m00 = a; m01 = b; m02 = c; m03 = d;
            m11 = b; m12 = c; m13 = d;
            m22 = c; m23 = d;
            m33 = d;
        }
        public static SymmetricMatrix operator +(SymmetricMatrix a, SymmetricMatrix b)
        {
            return new SymmetricMatrix
            {
                m00 = a.m00 + b.m00,
                m01 = a.m01 + b.m01,
                m02 = a.m02 + b.m02,
                m03 = a.m03 + b.m03,
                m11 = a.m11 + b.m11,
                m12 = a.m12 + b.m12,
                m13 = a.m13 + b.m13,
                m22 = a.m22 + b.m22,
                m23 = a.m23 + b.m23,
                m33 = a.m33 + b.m33
            };
        }
        public float VertexError(Vector3 v)
        {
            float x = v.X, y = v.Y, z = v.Z;
            return m00*x*x + 2*m01*x*y + 2*m02*x*z + 2*m03*x
                 + m11*y*y + 2*m12*y*z + 2*m13*y
                 + m22*z*z + 2*m23*z + m33;
        }
    }
    public Vector3[] vertices;
    public Triangle[] triangles;
    public QEMMeshSimplifier(Vector3[] vertices, Triangle[] triangles)
    {
        this.vertices = vertices;
        this.triangles = triangles;
    }
    private Vector3 SolveOptimalVertex(SymmetricMatrix Q)
    {
        float a00 = Q.m00, a01 = Q.m01, a02 = Q.m02;
        float a10 = Q.m01, a11 = Q.m11, a12 = Q.m12;
        float a20 = Q.m02, a21 = Q.m12, a22 = Q.m22;

        float b0 = -Q.m03;
        float b1 = -Q.m13;
        float b2 = -Q.m23;

        float detA = a00*(a11*a22 - a12*a21)
                   - a01*(a10*a22 - a12*a20)
                   + a02*(a10*a21 - a11*a20);

        if (Math.Abs(detA) < 1e-12f)
            return (vertices[0] + vertices[1]) / 2; 

        float detX = b0*(a11*a22 - a12*a21)
                   - a01*(b1*a22 - a12*b2)
                   + a02*(b1*a21 - a11*b2);

        float detY = a00*(b1*a22 - a12*b2)
                   - b0*(a10*a22 - a12*a20)
                   + a02*(a10*b2 - b1*a20);

        float detZ = a00*(a11*b2 - b1*a21)
                   - a01*(a10*b2 - b1*a20)
                   + b0*(a10*a21 - a11*a20);

        return new Vector3(detX/detA, detY/detA, detZ/detA);
    }

    public void Simplify(int targetTriangleCount)
    {
        int nVerts = vertices.Length;
        int nTris = triangles.Length;
        SymmetricMatrix[] quadrics = new SymmetricMatrix[nVerts];
        for (int i = 0; i < nTris; i++)
        {
            Triangle t = triangles[i];
            Vector3 v0 = vertices[t.a];
            Vector3 v1 = vertices[t.b];
            Vector3 v2 = vertices[t.c];

            Vector3 normal = Vector3.Normalize(Vector3.Cross(v1 - v0, v2 - v0));
            float d = -Vector3.Dot(normal, v0);

            SymmetricMatrix Kp = new SymmetricMatrix(
                normal.X * normal.X,
                normal.X * normal.Y,
                normal.X * normal.Z,
                normal.X * d
            );
            Kp.m11 = normal.Y * normal.Y;
            Kp.m12 = normal.Y * normal.Z;
            Kp.m13 = normal.Y * d;
            Kp.m22 = normal.Z * normal.Z;
            Kp.m23 = normal.Z * d;
            Kp.m33 = d * d;

            quadrics[t.a] = quadrics[t.a] + Kp;
            quadrics[t.b] = quadrics[t.b] + Kp;
            quadrics[t.c] = quadrics[t.c] + Kp;
        }
        List<Edge> edges = new List<Edge>();
        for (int i = 0; i < nTris; i++)
        {
            Triangle t = triangles[i];
            edges.Add(new Edge(Math.Min(t.a, t.b), Math.Max(t.a, t.b)));
            edges.Add(new Edge(Math.Min(t.b, t.c), Math.Max(t.b, t.c)));
            edges.Add(new Edge(Math.Min(t.c, t.a), Math.Max(t.c, t.a)));
        }
        edges.Sort((e1, e2) => e1.v0 != e2.v0 ? e1.v0 - e2.v0 : e1.v1 - e2.v1);
        List<Edge> uniqueEdges = new List<Edge>();
        for (int i = 0; i < edges.Count; i++)
            if (i == 0 || edges[i].v0 != edges[i-1].v0 || edges[i].v1 != edges[i-1].v1)
                uniqueEdges.Add(edges[i]);
        edges = uniqueEdges;
        foreach (var edge in edges)
        {
            SymmetricMatrix Q = quadrics[edge.v0] + quadrics[edge.v1];
            Vector3 vOpt = SolveOptimalVertex(Q);
            float cost = Q.VertexError(vOpt);

            edge.optimalPos = vOpt;
            edge.cost = cost;
        }
        edges.Sort((a, b) => a.cost.CompareTo(b.cost));
        foreach (var edge in edges)
        {
            if (triangles.Length <= targetTriangleCount) break;
            vertices[edge.v0] = edge.optimalPos;
            List<Triangle> newTris = new List<Triangle>();
            foreach (var t in triangles)
            {
                if (t.a == t.b || t.b == t.c || t.c == t.a) continue;
                newTris.Add(t);
            }
            triangles = newTris.ToArray();
        }
    }
}