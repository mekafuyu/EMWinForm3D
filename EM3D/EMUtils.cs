namespace EM3D;

public static class EMUtils
{
  public static Triangule ArrToTri(params Vec3D[] vecs)
    => vecs;

  public static Triangule ArrToTri(params float[] vecs)
    => ArrToTri(new Vec3D(vecs[0], vecs[1], 0f), new Vec3D(1f, 1f, 0f), new Vec3D(1f, 1f, 1f));
}