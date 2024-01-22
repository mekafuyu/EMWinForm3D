using System.Numerics;

namespace EM3D;

public struct Vertex
{
  public Vector3 V3;
  public float X {
    get => V3.X;
    set => V3.X = value;
  }
  public float Y {
    get => V3.Y;
    set => V3.Y = value;
  }
  public float Z {
    get => V3.Z;
    set => V3.Z = value;
  }
  public float W = 1;
  public Vector4 V4{
    get => new(V3, W);
    set 
    {
      V3.X = value.X;
      V3.Y = value.Y;
      V3.Z = value.Z;
      W = value.W;
    }
  }

  public Vertex() { }
  public Vertex(float x, float y, float z)
  {
    this.V3 = new(x, y, z);
    this.W = 1;
  }
  public Vertex(Vector3 v)
  {
    this.V3 = v;
    this.W = 1;
  }
  public Vertex(Vector4 v)
  {
    this.V3 = new(v.X, v.Y, v.Z);
    this.W = v.W;
  }
  public Vertex(float x, float y, float z, float w)
  {
    this.V3 = new(x, y, z);
    this.W = w;
  }

  // TODO
  public static implicit operator Vertex(Vector3 v)
  {
    return new(v);
  }
  public static implicit operator Vertex(Vector4 v)
  {
    return new(v);
  }
}