using System.Numerics;

namespace FastEM3D.Utils;

public static class TriangleMovement
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
}