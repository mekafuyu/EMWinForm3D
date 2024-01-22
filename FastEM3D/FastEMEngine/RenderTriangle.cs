using System.Collections.Generic;
using System.Numerics;

using static FastEM3D.EMUtils.EMGeometry;

namespace FastEM3D;

public partial class FastEMEngine
{
  private int renderTriangle(
    Triangle tr,
    (float width, float height) size,
    List<Triangle> trianglesToRasterBuffer
  )
  {
    // Move farther
    Triangle trTranslated = (Triangle)tr.Clone();
    trTranslated = TriangleMath.TranslateTriangle3D(trTranslated, (0f, 0f, 8f));

    // Find normal vector and normalize
    Vector3 normal = VectorMath.FindNormal(trTranslated);
    normal = Vector3.Normalize(normal);

    // Test if needs to project triangle by similarity
    if (Vector3.Dot(normal, trTranslated.P.l1.V3 - this.VirtualCamera.VCamera) > 0)
      return 0;

    // Convert World Space to view Space
    Triangle trViewed = new((
      Vector4.Transform(trTranslated.P.l1.V4, this.VirtualCamera.ViewMatrix),
      Vector4.Transform(trTranslated.P.l3.V4, this.VirtualCamera.ViewMatrix),
      Vector4.Transform(trTranslated.P.l2.V4, this.VirtualCamera.ViewMatrix)
    ));

    // The point to test is zNear, the plane is the Z axis, 
    var (clippedTrCount, trs) = TriangleMath.ClipAgainstPlane(new(0f, 0f, this.fNear), new(0f, 0f, 1f), trViewed);
    
    // Calculate light intensity
    float dp = Vector3.Dot(normal, this.LightDirection);
    if (dp < 0.3)
      dp = 0.3f;
      
    for (int i = 0; i < clippedTrCount; i++)
    {
      // Project Triangle
      Triangle trProjected = TriangleMath
        .ScaledTriangleTransformation(trs[i], this.VirtualCamera.ProjectionMatrix);

      // Scale triangle
      TriangleMath.ScaleTriangle(trProjected, size.width, size.height);

      trProjected.lightIntensity = dp;
      trianglesToRasterBuffer.Add(trProjected);      
    }
    return clippedTrCount; 
  }
}
