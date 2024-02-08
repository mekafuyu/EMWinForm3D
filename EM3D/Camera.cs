
using System;
using System.Numerics;
using static EM3D.EMUtils.EMGeometry;

namespace EM3D;
public class Camera
{
  public Vector3 VCamera =
      new()
      {
        X = 0,
        Y = 0,
        Z = 0
      };
  public Vector3 VLookDirection =
    new()
    {
      X = 0,
      Y = 0,
      Z = 1
    };
  public Vector3 VLookDirectionSnaped =
    new()
    {
      X = 0,
      Y = 0,
      Z = 1
    };
  public Vector3 VTarget =
    new()
    {
      X = 0,
      Y = 0,
      Z = 1
    };
  public Vector3 VUp =
    new()
    {
      X = 0,
      Y = 1,
      Z = 0
    };
  public Vector3 VRight =
    new()
    {
      X = 1,
      Y = 0,
      Z = 0
    };
  public Vector3 VFoward =
    new()
    {
      X = 0,
      Y = 0,
      Z = 0
    };
  public Matrix4x4 CameraMatrix;
  public Matrix4x4 ViewMatrix;
  public Matrix4x4 ProjectionMatrix;
  public float Yaw;
  public Vector4 YawMove;
  public float Pitch;
  public Vector4 PitchMove;

  public void RefreshVTarget()
  {
    // this.VTarget = VCamera + VLookDirection;
    Vector4 front = new(0,0,1,1);
    Matrix4x4 cameraRotMatYaw = MatrixMath.GetRotateInYMatrix(Yaw);
    Matrix4x4 cameraRotMatPitch = MatrixMath.GetRotateInXMatrix(Pitch);

    YawMove = Vector4.Transform(front, cameraRotMatYaw);
    PitchMove = Vector4.Transform(front, cameraRotMatPitch);

    VLookDirection.X = YawMove.X;
    VLookDirection.Z = YawMove.Z;
    VLookDirection.Y = PitchMove.Y;

    VTarget = VCamera + VLookDirection;
  }

  public void RefreshCameraMatrix()
  {
    this.CameraMatrix = MatrixMath.GetPointAtMatrix(VCamera, VTarget, VUp);
    this.ViewMatrix = MatrixMath.QuickReverse(CameraMatrix);
  }

  public void RefreshView()
  {
    this.RefreshVTarget();
    this.RefreshCameraMatrix();
  }

  public void MoveLeft(float step)
  {
    VCamera += Vector3.Cross(VUp, VLookDirection) * step;
  }
  public void MoveRight(float step)
  {
    VCamera -= Vector3.Cross(VUp, VLookDirection) * step;
  }
  public void MoveUp(float step)
  {
    this.VCamera.Y += step;
  }
  public void MoveDown(float step)
  {
    this.VCamera.Y -= step;
  }
  public void MoveFront(float step)
  {
    var foward = VLookDirection * step;
    this.VCamera += foward;
  }
  public void MoveBack(float step)
  {
    var foward = VLookDirection * step;
    this.VCamera -= foward;
  }

  private float reverseAcos = 1;
  private float reverseAsin = 1;
  public void RotateAroundPoint(float step, float centerX, float centerZ)
  {
    Vector3 center = new(centerX, 0, centerZ);
    Vector3 amelia2D = new(VCamera.X, 0, VCamera.Z);
    float radius = Vector3.Distance(center, amelia2D);
    step *= MathF.PI;

    // Bug when re-setting

    float acosStep = MathF.Acos((amelia2D.X - center.X) / radius) + step * reverseAcos;
    if (acosStep > MathF.PI || acosStep < 0)
      reverseAcos *= -1;

    float asinStep = MathF.Asin((amelia2D.Z - center.Z) / radius) + step * reverseAsin;
    if (asinStep > MathF.PI / 2 || asinStep < -MathF.PI / 2)
      reverseAsin *= -1;

    float x = center.X + radius * MathF.Cos(acosStep);
    float y = center.Z + radius * MathF.Sin(asinStep);
    VCamera.X = x;
    VCamera.Z = y;

    var vDirection = VCamera - center;
    Yaw = MathF.Atan2(-vDirection.Z, -vDirection.X) - MathF.PI / 2f;
  }
}
