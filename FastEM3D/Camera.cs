
using System.Numerics;
using static FastEM3D.EMUtils.EMGeometry;

namespace FastEM3D;
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
  public Matrix4x4 CameraMatrix;
  public Matrix4x4 ViewMatrix;
  public Matrix4x4 ProjectionMatrix;

  public void RefreshVTarget()
  {
    this.VTarget = VCamera + VLookDirection;
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
}