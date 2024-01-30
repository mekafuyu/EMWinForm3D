using System;

namespace EM3D;

public class Mesh
{
  public Triangle[] t;
  public byte[] Color;
  public Mesh() { }
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
