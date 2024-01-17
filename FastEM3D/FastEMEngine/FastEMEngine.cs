using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Numerics;
using System.Windows.Forms;

using FastEM3D.EMUtils;
using static FastEM3D.EMUtils.EMGeometry;

namespace FastEM3D;

public partial class FastEMEngine
{
  public float fNear = 0.1f;
  public float fFar = 1000.0f;
  public float fFov;
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
  private DateTime lastCheckTime;

  public FastEMEngine()
  {
    this.fAspectRatio = 1.333333f;
    this.SetFov(90);
    this.SetProjectionMatrix();
    NLightDirection = Vector3.Normalize(LightDirection);
  }

  public FastEMEngine(float height, float width)
  {
    this.fAspectRatio = height / width;
    this.SetFov(90);
    this.SetProjectionMatrix();
    NLightDirection = Vector3.Normalize(LightDirection);
  }

  public void SetFov(float angle)
  {
    this.fFov = angle;
    this.fFovRad = 1f / MathF.Tan(this.fFov * 0.5f / 180f * MathF.PI);
  }
  public void SetProjectionMatrix()
  {
    // matProj = new(
    //   this.fAspectRatio * this.fFovRad, 0, 0, 0,
    //   0, this.fFovRad, 0, 0,
    //   0, 0, this.fFar / (this.fFar - this.fNear), 1f,
    //   0, 0, (-this.fFar * this.fNear) / (this.fFar - this.fNear), 0
    // );
    matProj = new();
    matProj[0, 0] = this.fAspectRatio * this.fFovRad;
    matProj[1, 1] = this.fFovRad;
    matProj[2, 2] = this.fFar / (this.fFar - this.fNear);
    matProj[3, 2] = (-this.fFar * this.fNear) / (this.fFar - this.fNear);
    matProj[2, 3] = 1f;
    matProj[3, 3] = 0f;
  }
}
