using System.Collections.Generic;
using System.Drawing;
using System.Numerics;

using static EM3D.EMUtils.EMGeometry;

namespace EM3D;

public partial class EMEngine
{
  public float Darkest = 0f;
  public float Brightest = 1f;

  private int renderTriangle(
    Triangle tr,
    (float width, float height) size,
    List<Triangle> trianglesToRasterBuffer
  )
  {
    // Move farther
    Triangle trTranslated = tr;
    trTranslated = TriangleMath.TranslateTriangle3D(trTranslated, (0f, 0f, 8f));

    // Find normal vector and normalize
    Vector3 normal = VectorMath.FindNormal(trTranslated);
    normal = Vector3.Normalize(normal);

    // Test if needs to project triangle by similarity
    if (Vector3.Dot(normal, trTranslated.P.v1.V3 - this.VirtualCamera.VCamera) > 0)
      return 0;

    // Convert World Space to view Space
    Triangle trViewed = new((
      Vector4.Transform(trTranslated.P.v1.V4, this.VirtualCamera.ViewMatrix),
      Vector4.Transform(trTranslated.P.v3.V4, this.VirtualCamera.ViewMatrix),
      Vector4.Transform(trTranslated.P.v2.V4, this.VirtualCamera.ViewMatrix)
    ));

    // The point to test is zNear, the plane is the Z axis, 
    var (clippedTrCount, trs) = TriangleMath.ClipAgainstPlane(new(0f, 0f, this.fNear), new(0f, 0f, 1f), trViewed);
    
    // Calculate light intensity
    float dp = Vector3.Dot(normal, this.LightDirection);

    if(dp > Brightest)
      Brightest = dp;
    if(dp < Darkest)
      Darkest = dp;

    if (dp < 0.2 || dp is float.NaN)
      dp = 0.2f;
    if (dp > 1f)
      dp = 1f;
      
    for (int i = 0; i < clippedTrCount; i++)
    {
      // Project Triangle
      Triangle trProjected = TriangleMath
        .ScaledTriangleTransformation(trs[i], this.VirtualCamera.ProjectionMatrix);

      // Scale triangle
      TriangleMath.ScaleTriangle(trProjected, size.width, size.height);

      trProjected.lightIntensity = dp;
      trProjected.N = normal;
      trProjected.T = trs[i].T;
      trProjected.Color = tr.Color;
      trianglesToRasterBuffer.Add(trProjected);
    }
    return clippedTrCount; 
  }
}
