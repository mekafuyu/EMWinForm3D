using System;
using System.Numerics;
using FastEM3D.Utils;
using static FastEM3D.EMUtils.EMGeometry;
using static FastEM3D.Utils.TriangleMovement;

namespace FastEM3D.EMUtils;

public static class FastEMMath
{
  public static Triangle ProjectTriangle(
    Triangle tr,
    Vector3 light,
    Vector3 camera,
    Matrix4x4 m,
    (float width, float height) size
  )
  {
    // Move farther
    Triangle trTranslated = (Triangle)tr.Clone();
    trTranslated = TranslateTriangle3D(trTranslated, (0f, 0f, 10f));

    // Find normal vector and normalize
    Vector3 normal = FindNormal(trTranslated);
    normal = Vector3.Normalize(normal);

    // Test if needs to project triangle by similarity
    Vector3 s = normal * (trTranslated.P.l1.V3 - camera);

    if (s.X + s.Y + s.Z > 0)
      return null;

    float dp = Vector3.Dot(normal, light);
    if (dp < 0)
      dp = 0;

    Triangle trProjected = new();

    trProjected.P.l1.V4 = Vector4.Transform(trTranslated.P.l1.V4, m);
    trProjected.P.l2.V4 = Vector4.Transform(trTranslated.P.l2.V4, m);
    trProjected.P.l3.V4 = Vector4.Transform(trTranslated.P.l3.V4, m);

    trProjected.P.l1.X += 1f;
    trProjected.P.l1.Y += 1f;
    trProjected.P.l2.X += 1f;
    trProjected.P.l2.Y += 1f;
    trProjected.P.l3.X += 1f;
    trProjected.P.l3.Y += 1f;

    trProjected.P.l1.X *= 0.5f * size.width;
    trProjected.P.l1.Y *= 0.5f * size.height;
    trProjected.P.l2.X *= 0.5f * size.width;
    trProjected.P.l2.Y *= 0.5f * size.height;
    trProjected.P.l3.X *= 0.5f * size.width;
    trProjected.P.l3.Y *= 0.5f * size.height;
    
    trProjected.lightIntensity = dp;
    return trProjected;
  }
}
