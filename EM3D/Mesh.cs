namespace EM3D;

public class Mesh
{
  public Triangle[] t;
  public Mesh() { }

  public Mesh(Triangle[] tri)
  {
    this.t = tri;
  }
}
