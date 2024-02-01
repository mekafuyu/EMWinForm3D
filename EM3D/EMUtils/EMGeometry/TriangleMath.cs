using System.Numerics;

namespace EM3D.EMUtils;

public static partial class EMGeometry
{
  public static class TriangleMath
  {
    public static Triangle TranslateTriangle3D(Triangle tr, Vector3 displacement)
    {
      tr.P.v1.V3 += displacement;
      tr.P.v2.V3 += displacement;
      tr.P.v3.V3 += displacement;

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
      // var newTr = (Triangle)tr.Clone();
      var newTr = tr;
      newTr.P.v1.V4 = Vector4.Transform(newTr.P.v1.V4, tMatrix);
      newTr.P.v2.V4 = Vector4.Transform(newTr.P.v2.V4, tMatrix);
      newTr.P.v3.V4 = Vector4.Transform(newTr.P.v3.V4, tMatrix);

      newTr.P.v1.V3 /= newTr.P.v1.W;
      newTr.P.v2.V3 /= newTr.P.v2.W;
      newTr.P.v3.V3 /= newTr.P.v3.W;

      return newTr;
    }
    public static void ScaleTriangle(Triangle tr, float width, float height)
    {
      tr.P.v1.X += 1f;
      tr.P.v1.Y += 1f;
      tr.P.v1.X *= 0.5f * width;
      tr.P.v1.Y *= 0.5f * height;

      tr.P.v2.X += 1f;
      tr.P.v2.Y += 1f;
      tr.P.v2.X *= 0.5f * width;
      tr.P.v2.Y *= 0.5f * height;

      tr.P.v3.X += 1f;
      tr.P.v3.Y += 1f;
      tr.P.v3.X *= 0.5f * width;
      tr.P.v3.Y *= 0.5f * height;
    }

    public static float distancePointPlane(Vector3 point, Vector3 planePoint, Vector3 planeNormal)
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
      Vector2[] insidePointsTex = new Vector2[3];
      int countIPTex = 0;
      Vector2[] outsidePointsTex = new Vector2[3];
      int countOPTex = 0;

      float d0 = distancePointPlane(inputTr.P.v1.V3, planePoint, planeNormal);
      float d1 = distancePointPlane(inputTr.P.v2.V3, planePoint, planeNormal);
      float d2 = distancePointPlane(inputTr.P.v3.V3, planePoint, planeNormal);
      
      if (d0 >= 0)
      {
        insidePoints[countIP++] = inputTr.P.v1;
        insidePointsTex[countIPTex++] = inputTr.T.v1;
      }
      else
      {
        outsidePoints[countOP++] = inputTr.P.v1;
        outsidePointsTex[countOPTex++] = inputTr.T.v1;
      }
      if (d1 >= 0)
      {
        insidePoints[countIP++] = inputTr.P.v2;
        insidePointsTex[countIPTex++] = inputTr.T.v2;
      }
      else
      {
        outsidePoints[countOP++] = inputTr.P.v2;
        outsidePointsTex[countOPTex++] = inputTr.T.v2;
      }
      if (d2 >= 0)
      {
        insidePoints[countIP++] = inputTr.P.v3;
        insidePointsTex[countIPTex++] = inputTr.T.v3;
      }
      else
      {
        outsidePoints[countOP++] = inputTr.P.v3;
        outsidePointsTex[countOPTex++] = inputTr.T.v3;
      }

      if (countIP == 3)
        return (1, new Triangle[] { inputTr });
      if (countIP == 1 && countOP == 2)
      {
        var (v2, t2) = VectorMath.IntersectPlane(planePoint, planeNormal, insidePoints[0], outsidePoints[0]);
        var (v3, t3) = VectorMath.IntersectPlane(planePoint, planeNormal, insidePoints[0], outsidePoints[1]);

        Triangle newTr = new(
          (
            insidePoints[0],
            v2,
            v3
          ),
          (
            insidePointsTex[0],
            new Vector2(
              t2 * (outsidePointsTex[0].X - insidePointsTex[0].X) + insidePointsTex[0].X,
              t2 * (outsidePointsTex[0].Y - insidePointsTex[0].Y) + insidePointsTex[0].Y
            ),
            new Vector2(
              t3 * (outsidePointsTex[1].X - insidePointsTex[0].X) + insidePointsTex[0].X,
              t3 * (outsidePointsTex[1].Y - insidePointsTex[0].Y) + insidePointsTex[0].Y
            )
          )
        )

        {
          lightIntensity = inputTr.lightIntensity
        };

        return (1, new Triangle[] { newTr });
      }
      if (countIP == 2 && countOP == 1)
      {
        var (v3, t3) = VectorMath.IntersectPlane(planePoint, planeNormal, insidePoints[0], outsidePoints[0]);
        Triangle newTr1 = new((
          insidePoints[0],
          insidePoints[1],
          v3
        ),
        (
          insidePointsTex[0],
          insidePointsTex[1],
          new(
            t3 * (outsidePointsTex[0].X + insidePointsTex[0].X) + insidePointsTex[0].X,
            t3 * (outsidePointsTex[0].Y + insidePointsTex[0].Y) + insidePointsTex[0].Y
          )
        )
        )
        {
          lightIntensity = inputTr.lightIntensity
        };

        (v3, t3) = VectorMath.IntersectPlane(planePoint, planeNormal, insidePoints[1], outsidePoints[0]);
        Triangle newTr2 = new((
          insidePoints[1],
          newTr1.P.v3,
          v3
        ),
        (
          insidePointsTex[1],
          newTr1.T.v3,
          new(
            t3 * (outsidePointsTex[0].X + insidePointsTex[1].X) + insidePointsTex[1].X,
            t3 * (outsidePointsTex[0].Y + insidePointsTex[1].Y) + insidePointsTex[1].Y
          )
        )
        )
        {
          lightIntensity = inputTr.lightIntensity
        };

        return (2, new Triangle[] { newTr1, newTr2 });
      }
      return (0, null);
    }
  }
}