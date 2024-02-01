using System;
using System.Numerics;

namespace EM3D;

public partial class EMEngine
{
  public float fNear = 0.1f;
  public float fFar = 1000.0f;
  public float fFov;
  public float fAspectRatio;
  public float fFovRad;

  public Camera VirtualCamera = new();

  public Vector3 LightDirection =
    new()
    {
      X = 0,
      Y = 1,
      Z = -1f
    };
  public Vector3 NLightDirection;
  private DateTime lastCheckTime;

  public EMEngine()
  {
    this.fAspectRatio = 1.333333f;
    this.SetFov(120);
    this.SetProjectionMatrix();
    this.VirtualCamera.RefreshView();
    NLightDirection = Vector3.Normalize(LightDirection);
  }

  public EMEngine(float height, float width)
  {
    this.fAspectRatio = height / width;
    this.SetFov(120);
    this.SetProjectionMatrix();
    this.VirtualCamera.RefreshView();
    NLightDirection = Vector3.Normalize(LightDirection);
  }

  public void SetFov(float angle)
  {
    this.fFov = angle;
    this.fFovRad = 1f / MathF.Tan(this.fFov * 0.5f / 180f * MathF.PI);
  }
  
  public void SetProjectionMatrix()
  {
    this.VirtualCamera.ProjectionMatrix = new(
      this.fAspectRatio * this.fFovRad, 0, 0, 0,
      0, this.fFovRad, 0, 0,
      0, 0, this.fFar / (this.fFar - this.fNear), (-this.fFar * this.fNear) / (this.fFar - this.fNear),
      0, 0, 1f, 0
    );
  }

  public void RefreshAspectRatio(float width, float height)
  {
    this.fAspectRatio = height / width;
    VirtualCamera.ProjectionMatrix[0,0] = this.fAspectRatio * this.fFovRad;
  }


}
