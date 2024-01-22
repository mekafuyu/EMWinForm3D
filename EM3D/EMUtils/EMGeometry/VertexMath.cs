namespace EM3D.EMUtils;

public static partial class EMGeometry
{
  public static class VertexMath
  {
    public static Vertex VertexSub(Vertex p1, Vertex p2)
    {
      return new(p1.V3 - p2.V3);
    }
  }
}