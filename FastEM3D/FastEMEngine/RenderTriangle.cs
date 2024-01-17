using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Numerics;
using System.Windows.Forms;

using FastEM3D.EMUtils;
using static FastEM3D.EMUtils.EMGeometry;

namespace FastEM3D;

public partial class FastEMEngine
{
  private Triangle renderTriangle(
    Triangle tr,
    Vector3 light,
    Vector3 camera,
    Matrix4x4 m,
    (float width, float height) size
  )
  {
    // Move farther
    Triangle trTranslated = (Triangle)tr.Clone();
    trTranslated = TriangleMath.TranslateTriangle3D(trTranslated, (0f, 0f, 10f));

    // Find normal vector and normalize
    Vector3 normal = VectorMath.FindNormal(trTranslated);
    normal = Vector3.Normalize(normal);

    // Test if needs to project triangle by similarity
    if (Vector3.Dot(normal, trTranslated.P.l1.V3 - camera) > 0)
      return null;

    // Project Triangle
    Triangle trProjected = TriangleMath
      .ScaledTriangleTransformation(trTranslated, m);

    // Scale triangle
    TriangleMath.ScaleTriangle(trProjected, size.width, size.height);

    // Calculate light intensity
    float dp = Vector3.Dot(normal, light);
    if (dp < 0)
      dp = 0;
    trProjected.lightIntensity = dp;

    return trProjected;
  }
}
