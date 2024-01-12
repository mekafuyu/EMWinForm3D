using System;
using System.Numerics;
using static EM3D.EMUtils.Geometry;
using static EM3D.EMUtils.Drawing;

namespace EM3D;

public class EMEngine
{
  public float fNear = 0.1f;
  public float fFar = 1000.0f;
  public float fFov = 90.0f;
  public float fAspectRatio;
  public float fFovRad;
  public Matrix4x4 matProj;
  public Vector3 VCamera =
      new()
      {
        X = 0,
        Y = 0,
        Z = 0
      };
  public Vector3 LightDirection =
      new()
      {
        X = 0,
        Y = 0,
        Z = -1f
      };
  public Vector3 NLightDirection;

  public EMEngine()
  {
    this.fAspectRatio = 1.333333f;
    setFovAndMatrix();
  }

  public EMEngine(float height, float width)
  {
    this.fAspectRatio = height / width;
    setFovAndMatrix();
    normalizeLight();
  }

  private void setFovAndMatrix()
  {
    fFovRad = 1f / MathF.Tan(this.fFov * 0.5f / 180f * MathF.PI);
    this.matProj = new();
    matProj[0, 0] = this.fAspectRatio * this.fFovRad;
    matProj[1, 1] = this.fFovRad;
    matProj[2, 2] = this.fFar / (this.fFar - this.fNear);
    matProj[3, 2] = (-this.fFar * this.fNear) / (this.fFar - this.fNear);
    matProj[2, 3] = 1f;
    matProj[3, 3] = 0f;
  }

  private void normalizeLight()
  {
    float length = MathF.Sqrt(
      LightDirection.X * LightDirection.X +
      LightDirection.Y * LightDirection.Y +
      LightDirection.Z * LightDirection.Z
    );
    LightDirection.X /= length;
    LightDirection.Y /= length;
    LightDirection.Z /= length;
  }
}
