using System;
using System.Numerics;

namespace EM3D;

public class Mesh
{
  public static void TranslateMesh(Mesh m, float x, float y, float z)
  {
    Vector3 displace = new(x, y, z);
    foreach (var tr in m.t)
    {
      tr.P.v1.V3 += displace;
      tr.P.v2.V3 += displace;
      tr.P.v3.V3 += displace;
    }
  }

  public static byte[] DefaultColor
  {
    get
     => new byte[] { 128, 128, 255 };
  } 

  public Triangle[] t;
  public byte[] Color = Mesh.DefaultColor;

  public Mesh() {}

  public Mesh(Triangle[] tri)
    => this.t = tri;

  public Mesh(Triangle[] tri, byte[] color)
  {
    this.t = tri;
    if(color.Length != 3)
      throw new Exception("Must have 3 and only 3 itens in color param");
    this.Color = color;
  }
}
