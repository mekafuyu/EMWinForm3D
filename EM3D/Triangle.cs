using System;
using System.Drawing;
using System.Numerics;

namespace EM3D;

public class Triangle : ICloneable
{
  public (Vertex v1, Vertex v2, Vertex v3) P;
  public float lightIntensity = 1f;
  public Vector3 N = new();
  public (Vector2 v1, Vector2 v2, Vector2 v3) T;
  public byte[] Color;

  public float ZPos
    =>  1 / (P.v1.Z + P.v2.Z + P.v3.Z) / 3f;

  public Triangle() { }
  
  public Triangle((Vertex, Vertex, Vertex) p)
    => this.P = p;
  
  public Triangle(Vertex p1, Vertex p2, Vertex p3)
    => this.P = (p1, p2, p3);
  
  public Triangle(
    (Vertex, Vertex, Vertex) p,
    (Vector2, Vector2, Vector2) t
    )
  {
    this.P = p;
    this.T = t;
  }
  
  public object Clone()
    => new Triangle(this.P);
}
