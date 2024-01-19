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
    public static Triangle ScaledTriangleTransformation(Triangle tr, Matrix4x4 tMatrix)
    {
      var newTr = (Triangle) tr.Clone();
      newTr.P.l1.V4 = Vector4.Transform(newTr.P.l1.V4, tMatrix);
      newTr.P.l2.V4 = Vector4.Transform(newTr.P.l2.V4, tMatrix);
      newTr.P.l3.V4 = Vector4.Transform(newTr.P.l3.V4, tMatrix);

      newTr.P.l1.V3 /= newTr.P.l1.W;
      newTr.P.l2.V3 /= newTr.P.l2.W;
      newTr.P.l3.V3 /= newTr.P.l3.W;

      return newTr;
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

    private static float distancePointPlane(Vector3 point, Vector3 planePoint, Vector3 planeNormal)
    {
      var n = Vector3.Normalize(point);
      return Vector3.Dot(planeNormal, point) - Vector3.Dot(planeNormal, planePoint);
    }

    public static (int, Triangle[]) ClipAgainstPlane(Vector3 planePoint, Vector3 planeNormal, Triangle inputTr)
    {
      planeNormal = Vector3.Normalize(planeNormal);
      
      Vertex[] insidePoints = new Vertex[3];
      int countIP = 0;
      Vertex[] outsidePoints = new Vertex[3];
      int countOP = 0;

      float d0 = distancePointPlane(inputTr.P.l1.V3, planePoint, planeNormal);
      float d1 = distancePointPlane(inputTr.P.l2.V3, planePoint, planeNormal);
      float d2 = distancePointPlane(inputTr.P.l3.V3, planePoint, planeNormal);


      // ! TALVEZ BUGADO
      if (d0 >= 0) { insidePoints[countIP++] = inputTr.P.l1; }
      else { outsidePoints[countOP++] = inputTr.P.l1; }
      if (d1 >= 0) { insidePoints[countIP++] = inputTr.P.l2; }
      else { outsidePoints[countOP++] = inputTr.P.l2; }
      if (d2 >= 0) { insidePoints[countIP++] = inputTr.P.l3; }
      else { outsidePoints[countOP++] = inputTr.P.l3; }

      if(countIP == 3)
        return (1, new Triangle[]{inputTr});
      if(countIP == 1 && countOP == 2)
      {
        Triangle newTr = new((
          insidePoints[0],
          VectorMath.IntersectPlane(planePoint, planeNormal, insidePoints[0], outsidePoints[0]),
          VectorMath.IntersectPlane(planePoint, planeNormal, insidePoints[0], outsidePoints[1])
        ))
        {
          lightIntensity = inputTr.lightIntensity
        };
        
        return (1, new Triangle[]{newTr});
      }
      if(countIP == 2 && countOP == 1)
      {
        Triangle newTr1 = new((
          insidePoints[0],
          insidePoints[1],
          VectorMath.IntersectPlane(planePoint, planeNormal, insidePoints[0], outsidePoints[0])
        ))
        {
          lightIntensity = inputTr.lightIntensity
        };

        // ! TALVEZ BUGADO
        Triangle newTr2 = new((
          insidePoints[1],
          newTr1.P.l3,
          VectorMath.IntersectPlane(planePoint, planeNormal, insidePoints[1], outsidePoints[0])
        ))
        {
          lightIntensity = inputTr.lightIntensity
        };
        
        return (2, new Triangle[]{newTr1, newTr2});
      }
      return (0, null);
    }
  }
}