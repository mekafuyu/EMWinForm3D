using System.Numerics;

namespace FastEM3D.EMUtils;

public static partial class EMGeometry
{
  public static class TriangleMath
  {
    public static Triangle TranslateTriangle3D(Triangle tr, Vector3 displacement)
    {
      tr.P.l1.V3 += displacement;
      tr.P.l2.V3 += displacement;
      tr.P.l3.V3 += displacement;

      return tr;
    }
    public static Triangle TranslateTriangle3D(Triangle tr, (float x, float y, float z) displacement)
    {
      Vector3 d = new Vector3(
        displacement.x,
        displacement.y,
        displacement.z
      );

      return TranslateTriangle3D(tr, d);
    }
    public static Triangle ScaledTriangleTransformation(Triangle tr, Matrix4x4 rotationMatrix)
    {
      tr.P.l1.V4 = Vector4.Transform(tr.P.l1.V4, rotationMatrix);
      tr.P.l2.V4 = Vector4.Transform(tr.P.l2.V4, rotationMatrix);
      tr.P.l3.V4 = Vector4.Transform(tr.P.l3.V4, rotationMatrix);

      tr.P.l1.V3 /= tr.P.l1.W;
      tr.P.l2.V3 /= tr.P.l2.W;
      tr.P.l3.V3 /= tr.P.l3.W;

      return tr;
    }

    public static void ScaleTriangle(Triangle tr, float width, float height)
    {
      tr.P.l1.X += 1f;
      tr.P.l1.Y += 1f;
      tr.P.l1.X *= 0.5f * width;
      tr.P.l1.Y *= 0.5f * height;

      tr.P.l2.X += 1f;
      tr.P.l2.Y += 1f;
      tr.P.l2.X *= 0.5f * width;
      tr.P.l2.Y *= 0.5f * height;

      tr.P.l3.X += 1f;
      tr.P.l3.Y += 1f;
      tr.P.l3.X *= 0.5f * width;
      tr.P.l3.Y *= 0.5f * height;
    }
  }
}