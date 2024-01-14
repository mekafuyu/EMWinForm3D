using System;
using System.Numerics;

namespace EM3D.EMUtils;

public static class Geometry
{
  public static Vector3 GetVectorFromPoints(Vector3 p1, Vector3 p2)
  {
    return new()
    {
      X = p1.X - p2.X,
      Y = p1.Y - p2.Y,
      Z = p1.Z - p2.Z
    };
  }
  public static Vector3 FindNormal(Vector3 l1, Vector3 l2)
  {
    Vector3 res = new()
    {
      X = l1.Y * l2.Z - l1.Z * l2.Y,
      Y = l1.Z * l2.X - l1.X * l2.Z,
      Z = l1.X * l2.Y - l1.Y * l2.X
    };
    if (res.X == float.NaN)
      return res;

    return res;
  }
  public static Vector3 MultiplyVectorMatrix(Vector3 i, Matrix4x4 m)
  {
    Vector3 res =
    // new()
    // {
    //   X = i.X * m[0, 0] + i.Y * m[1, 0] + i.Z * m[2, 0] + m[3, 0],
    //   Y = i.X * m[0, 1] + i.Y * m[1, 1] + i.Z * m[2, 1] + m[3, 1],
    //   Z = i.X * m[0, 2] + i.Y * m[1, 2] + i.Z * m[2, 2] + m[3, 2]
    // };
    // float w = i.X * m[0, 3] + i.Y * m[1, 3] + i.Z * m[2, 3] + m[3, 3];
    new()
    {
      X = i.X * m[0, 0] + i.Y * m[0, 1] + i.Z * m[0, 2] + m[0, 3],
      Y = i.X * m[1, 0] + i.Y * m[1, 1] + i.Z * m[1, 2] + m[1, 3],
      Z = i.X * m[2, 0] + i.Y * m[2, 1] + i.Z * m[2, 2] + m[2, 3]
    };
    float w = i.X * m[0, 3] + i.Y * m[1, 3] + i.Z * m[2, 3] + m[3, 3];

    if (w != 0.0f)
    {
      res.X /= w;
      res.Y /= w;
      res.Z /= w;
    }

    return res;
  }
  public static Triangle RotateTriangle3D(Triangle tr, Matrix4x4 rotationMatrix)
  {
    // Triangle rotatedTri = new();
    tr.P[0] = MultiplyVectorMatrix(tr.P[0], rotationMatrix);
    tr.P[1] = MultiplyVectorMatrix(tr.P[1], rotationMatrix);
    tr.P[2] = MultiplyVectorMatrix(tr.P[2], rotationMatrix);

    return tr;
  }
  public static Matrix4x4 GetRotateInXMatrix(float angle)
  {
    return new(
      1, 0, 0, 0,
      0, MathF.Cos(angle), MathF.Sin(angle), 0,
      0, -MathF.Sin(angle), MathF.Cos(angle), 0,
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
  public static Triangle TranslateTriangle3D(Triangle tr, float x, float y, float z)
  {
    if (x != 0)
    {
      tr.P[0].X = tr.P[0].X + x;
      tr.P[1].X = tr.P[1].X + x;
      tr.P[2].X = tr.P[2].X + x;
    }
    if (y != 0)
    {
      tr.P[0].Y = tr.P[0].Y + y;
      tr.P[1].Y = tr.P[1].Y + y;
      tr.P[2].Y = tr.P[2].Y + y;
    }
    if (z != 0)
    {
      tr.P[0].Z = tr.P[0].Z + z;
      tr.P[1].Z = tr.P[1].Z + z;
      tr.P[2].Z = tr.P[2].Z + z;
    }
    return tr;
  }
  public static Triangle ProjectTriangle(
    Triangle tr,
    Vector3 light,
    Vector3 camera,
    Matrix4x4 m,
    (float width, float height) size
  )
  {
    bool nan = false;

    Triangle trTranslated = (Triangle)tr.Clone();
    trTranslated.P[0].Z = tr.P[0].Z + 10f;
    trTranslated.P[1].Z = tr.P[1].Z + 10f;
    trTranslated.P[2].Z = tr.P[2].Z + 10f;

    Vector3 l1 = GetVectorFromPoints(trTranslated.P[1], trTranslated.P[0]);
    Vector3 l2 = GetVectorFromPoints(trTranslated.P[2], trTranslated.P[0]);
    Vector3 normal = FindNormal(l1, l2);
    float length = MathF.Sqrt(normal.X * normal.X + normal.Y * normal.Y + normal.Z * normal.Z);
    if (length != 0)
    {
      normal.X /= length;
      normal.Y /= length;
      normal.Z /= length;
    }

    if (
      normal.X * (trTranslated.P[0].X - camera.X) +
      normal.Y * (trTranslated.P[0].Y - camera.Y) +
      normal.Z * (trTranslated.P[0].Z - camera.Z)
      > 0
    )
      return null;

    float dp = normal.X * light.X + normal.Y * light.Y + normal.Z * light.Z;
    if (dp < 0)
      dp = 0;

    Triangle trProjected = new();

    trProjected.P[0] = MultiplyVectorMatrix(trTranslated.P[0], m);
    trProjected.P[1] = MultiplyVectorMatrix(trTranslated.P[1], m);
    trProjected.P[2] = MultiplyVectorMatrix(trTranslated.P[2], m);

    trProjected.P[0].X += 1f;
    trProjected.P[0].Y += 1f;
    trProjected.P[1].X += 1f;
    trProjected.P[1].Y += 1f;
    trProjected.P[2].X += 1f;
    trProjected.P[2].Y += 1f;

    trProjected.P[0].X *= 0.5f * size.width;
    trProjected.P[0].Y *= 0.5f * size.height;
    trProjected.P[1].X *= 0.5f * size.width;
    trProjected.P[1].Y *= 0.5f * size.height;
    trProjected.P[2].X *= 0.5f * size.width;
    trProjected.P[2].Y *= 0.5f * size.height;
    
    trProjected.lightIntensity = dp;
    return trProjected;
  }
}
