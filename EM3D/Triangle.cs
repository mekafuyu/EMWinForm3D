using System;

namespace EM3D;

public class Triangle : ICloneable
{
  public (Vertex l1, Vertex l2, Vertex l3) P;
  public float lightIntensity = 1f;
  public float ZPos
    =>  1 / (P.l1.Z + P.l2.Z + P.l3.Z) / 3f;

  public Triangle() { }
  public Triangle((Vertex, Vertex, Vertex) p)
  {
    this.P = p;
  }
  public Triangle(Vertex p1, Vertex p2, Vertex p3)
  {
    this.P = (p1, p2, p3);
  }

  public object Clone()
  {
    return new Triangle(this.P);
  }
}
