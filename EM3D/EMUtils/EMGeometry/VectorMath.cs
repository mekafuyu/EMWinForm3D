using System.Numerics;

namespace EM3D.EMUtils;

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
    public static Vector3 Normalize(Vertex v)
    {
      return Vector3.Normalize(v.V3);
    }

    public static Vector3 IntersectPlane(Vertex planePoint, Vector3 planeNormal, Vertex lineStartP, Vertex lineEndP)
    {
      // Transform to unit 
      planeNormal = Vector3.Normalize(planeNormal);

      // X * Nx + Y * Ny + Z * Nz - dot(N, P) = 0
      // Ax + By + Cz - D = 0

      // Constant
      float d = -Vector3.Dot(planeNormal, planePoint.V3);

      // Find Gradient
      float ad = Vector3.Dot(lineStartP.V3, planeNormal);
      float bd = Vector3.Dot(lineEndP.V3, planeNormal);
      float t = (-d - ad) / (bd - ad);

      // Lines to test against
      Vector3 lineStartToEnd = lineEndP.V3 - lineStartP.V3;
      Vector3 lineToIntersect = lineStartToEnd * t;

      return lineStartP.V3 + lineToIntersect;
    }
  }
}