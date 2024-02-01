using System.Numerics;

namespace EM3D.EMUtils;

public static partial class EMGeometry
{
  public static class VectorMath
  {
    public static Vector3 FindNormal(Triangle t)
    {
      Vertex v1 = VertexMath.VertexSub(t.P.v2, t.P.v1);
      Vertex v2 = VertexMath.VertexSub(t.P.v3, t.P.v1);
      return FindNormal(v1, v2);
    }
    public static Vector3 FindNormal(Vertex v1, Vertex v2)
    {
      return new()
      {
        X = v1.Y * v2.Z - v1.Z * v2.Y,
        Y = v1.Z * v2.X - v1.X * v2.Z,
        Z = v1.X * v2.Y - v1.Y * v2.X
      };
    }
    public static Vector3 Normalize(Vertex v)
    {
      return Vector3.Normalize(v.V3);
    }

    public static (Vector3 intercept, float interceptDistance) IntersectPlane(Vertex planePoint, Vector3 planeNormal, Vertex lineStartP, Vertex lineEndP)
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

      return (lineStartP.V3 + lineToIntersect, t);
    }
  }
}