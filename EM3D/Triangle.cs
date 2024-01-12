using System;
using System.Numerics;

namespace EM3D;

public class Triangle : ICloneable
{
  public Vector3[] P = new Vector3[3];

  public Triangle() { }
  public Triangle(Vector3[] p)
  {
    this.P = p;
  }

  public object Clone()
  {
    Triangle nt = new Triangle(
      (Vector3[]) this.P.Clone()
      );
    return nt;
  }

  public static implicit operator Triangle(Vector3[] vec)
  {
    if (vec.Length != 3)
      throw new Exception("KKKKKKK sla");

    Triangle output = new()
    {
      P = vec
    };
    return output;
  }
}
