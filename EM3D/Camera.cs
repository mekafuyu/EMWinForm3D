
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
  public float Pitch;

  public void RefreshVTarget()
  {
    // this.VTarget = VCamera + VLookDirection;
    this.VTarget = Vector3.UnitZ;
    Matrix4x4 cameraRotMat = MatrixMath.GetRotateInYMatrix(Yaw);

    var temp = Vector4.Transform(new Vector4(this.VTarget, 1), cameraRotMat);
    VLookDirection.X = temp.X;
    VLookDirection.Y = temp.Y;
    VLookDirection.Z = temp.Z;

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
    var right = new Vector3(VLookDirection.X * step, VLookDirection.Y * step, 0);
    this.VCamera -= right;
  }
  public void MoveRight(float step)
  {
    var right = new Vector3(VLookDirection.X * step, VLookDirection.Y * step, 0);
    this.VCamera += right;
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


}