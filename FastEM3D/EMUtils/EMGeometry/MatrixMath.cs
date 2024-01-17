using System;
using System.Numerics;

namespace FastEM3D.EMUtils;

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
        MathF.Cos(angle), 0, -MathF.Sin(angle), 0,
        0, 1, 0, 0,
        MathF.Sin(angle), 0, MathF.Cos(angle), 0,
        0, 0, 0, 1
      );
    }
    public static Matrix4x4 GetRotateInZMatrix(float angle)
    {
      return new(
        MathF.Cos(angle), MathF.Sin(angle), 0, 0,
        -MathF.Sin(angle), MathF.Cos(angle), 0, 0,
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
  }
}