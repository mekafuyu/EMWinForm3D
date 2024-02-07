
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

  public void RotateAroundPoint(float step, float centerX, float centerZ)
  {
    Vector3 center = new(centerX, 0, centerZ);
    Vector3 vDirection = new(VCamera.X - centerX, 0, VCamera.Z - centerZ);

    float stepSize = 1e-4f;
    UInt32 iterations = (UInt32)(MathF.Abs(step) / stepSize);

    Vector3 move;

    float sumX = 0f;
    float cX = 0f;

    float sumZ = 0f;
    float cZ = 0f;

    for (UInt32 i = 0; i < iterations; ++i)
    {
      move = Vector3.Cross(VUp, vDirection) * MathF.Sign(step) * stepSize;

      var yX = move.X - cX;
      var tX = sumX + yX;
      cX = (tX - sumX) - yX;
      sumX = tX;

      var yZ = move.Z - cZ;
      var tZ = sumZ + yZ;
      cZ = (tZ - sumZ) - yZ;
      sumZ = tZ;
    }

    VCamera.X += sumX;
    VCamera.Z += sumZ;

    move = Vector3.Cross(VUp, vDirection) * MathF.Sign(step) * (MathF.Abs(step) - iterations * stepSize);
    VCamera.X += move.X;
    VCamera.Z += move.Z;

    vDirection = new((VCamera - center).X, 0, (VCamera - center).Z);
    Yaw = MathF.Atan2(-vDirection.Z, -vDirection.X) - MathF.PI / 2f;
  }
}
