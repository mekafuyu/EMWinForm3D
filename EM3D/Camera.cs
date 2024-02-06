
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
    // X = Xc + R * Cos(t)
    // (X - Xc) / R = Cos(t)
    // ACos((X - Xc) / R) = t

    // float radius = Vector2.Distance(new(VCamera.X, VCamera.Z), new(centerX, centerZ));
    // float currPosC = MathF.Acos((VCamera.X - centerX) / radius);
    // float currPosS = MathF.Asin((VCamera.Z - centerZ) / radius);

    // VCamera = new Vector3(
    //   centerX + radius * MathF.Cos(currPosC + step),
    //   VCamera.Y,
    //   centerZ - radius * MathF.Sin(currPosS + step) 
    // );
    // VCamera = new Vector3(
    //   VCamera.X * MathF.Cos(currPos + step),
    //   VCamera.Y,
    //   VCamera.Z * MathF.Sin(currPos + step) 
    // );
    
    Vector3 center = new Vector3(centerX, 0, centerZ);

    var move = Vector3.Cross(VUp, VCamera - center) * step;
    VCamera.X += move.X;
    VCamera.Z += move.Z;

    Yaw = -(MathF.Acos(Vector3.Dot(Vector3.Normalize(VRight), VCamera - center) / (VRight.Length() * (VCamera - center).Length())) - MathF.PI / 2); 
  }
}