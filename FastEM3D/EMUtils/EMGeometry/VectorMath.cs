using System.Numerics;

namespace FastEM3D.EMUtils;

public static partial class EMGeometry
{
  public static class VectorMath
  {
    public static Vector3 FindNormal(Triangle t)
    {
      Vertex l1 = VertexMath.VertexSub(t.P.l2, t.P.l1);
      Vertex l2 = VertexMath.VertexSub(t.P.l3, t.P.l1);
      return FindNormal(l1, l2);
    }
    public static Vector3 FindNormal(Vertex l1, Vertex l2)
    {
      return new()
      {
        X = l1.Y * l2.Z - l1.Z * l2.Y,
        Y = l1.Z * l2.X - l1.X * l2.Z,
        Z = l1.X * l2.Y - l1.Y * l2.X
      };
    }
    // public static Vector3 Normalize(Vector3 v)
    // {
    //   var s = MathF.Sqrt(
    //     (v.X * v.X) +
    //     (v.Y * v.Y) +
    //     (v.Z * v.Z)
    //   );

    //   if(s != 0)
    //     return v / 3;
    //   return v;
    // }
    public static Vector3 Normalize(Vertex v)
    {
      return Vector3.Normalize(v.V3);
    }
  }
}