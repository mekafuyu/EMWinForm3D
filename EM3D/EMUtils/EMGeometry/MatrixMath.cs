using System;
using System.Numerics;

namespace EM3D.EMUtils;

public static partial class EMGeometry
{
  public static class MatrixMath
  {
    public static Matrix4x4 GetRotateInXMatrix(float angle)
    {
      return new(
        1, 0, 0, 0,
        0, MathF.Cos(angle), -MathF.Sin(angle), 0,
        0, MathF.Sin(angle), MathF.Cos(angle), 0,
        0, 0, 0, 1
      );
    }
    public static Matrix4x4 GetRotateInYMatrix(float angle)
    {
      return new(
        MathF.Cos(angle), 0, MathF.Sin(angle), 0,
        0, 1, 0, 0,
        -MathF.Sin(angle), 0, MathF.Cos(angle), 0,
        0, 0, 0, 1
      );
    }
    public static Matrix4x4 GetRotateInZMatrix(float angle)
    {
      return new(
        MathF.Cos(angle), -MathF.Sin(angle), 0, 0,
        MathF.Sin(angle), MathF.Cos(angle), 0, 0,
        0, 0, 1, 0,
        0, 0, 0, 1
      );
    }
    public static Matrix4x4 GetRotationMatrix(float xAngle, float yAngle, float zAngle)
    {
      return
        GetRotateInXMatrix(xAngle) *
        GetRotateInYMatrix(yAngle) *
        GetRotateInZMatrix(zAngle);
    }
    public static Matrix4x4 GetPointAtMatrix(Vertex position, Vertex target, Vertex up)
    {
      // Calculate Foward Direction
      Vertex newFoward = target.V3 - position.V3;
      newFoward.V3 = Vector3.Normalize(newFoward.V3);

      // Calculate Up Direction
      var a = newFoward.V3 * Vector3.Dot(up.V3, newFoward.V3);
      Vertex newUp = up.V3 - a;
      newUp.V3 = Vector3.Normalize(newUp.V3);
      // if(newUp.V3 != Vector3.Zero)

      // Calculate Right Direction
      Vertex newRight = Vector3.Cross(newUp.V3, newFoward.V3);

      Matrix4x4 pointAtMatrix = new(
        newRight.X, newRight.Y, newRight.Z, 0,
        newUp.X, newUp.Y, newUp.Z, 0,
        newFoward.X, newFoward.Y, newFoward.Z, 0,
        position.X, position.Y, position.Z, 1
      );

      return pointAtMatrix;
    }

    public static Matrix4x4 QuickReverse(Matrix4x4 m)
    {
      // Matrix4x4 r = new(
      //   m[0, 0], m[0, 1], m[0, 2], 0,
      //   m[1, 0], m[1, 1], m[1, 2], 0,
      //   m[2, 0], m[2, 1], m[2, 2], 0,
      //   0, 0, 0, 1f
      // );
      Matrix4x4 r = new(
        m[0, 0], m[1, 0], m[2, 0], 0,
        m[0, 1], m[1, 1], m[2, 1], 0,
        m[0, 2], m[1, 2], m[2, 2], 0,
        0, 0, 0, 1f
      );
      // r[0,3] = 2;
      Vector3 lastLine = new(m[3, 0], m[3, 1], m[3, 2]);
      r[3, 0] = -Vector3.Dot(lastLine, new(r[0, 0], r[1, 0], r[2, 0]));
      r[3, 1] = -Vector3.Dot(lastLine, new(r[0, 1], r[1, 1], r[2, 1]));
      r[3, 2] = -Vector3.Dot(lastLine, new(r[0, 2], r[1, 2], r[2, 2]));

      return r;
    }
  }
}