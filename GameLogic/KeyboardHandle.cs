using System;
using System.Numerics;
using System.Windows.Forms;
using EM3D;

public class KeyboardHandle
{
  float speed = 0.01f;
  float tspeed = 0.5f;
  int xSpeed = 0;
  int zSpeed = 0;
  public void KeyDown(KeyEventArgs e, EMEngine eng, Level lvl, Amelia amelia)
  {
    Vector3 dir = Vector3.Normalize(lvl.VirtualCamera.VCamera - amelia.Anchor3D.V3);

    switch (e.KeyCode)
    {
      case Keys.W:
        amelia.StartFront(dir);
        if (zSpeed > 0)
          amelia.StartRight(dir);
        if (zSpeed < 0)
          amelia.StartLeft(dir);

        xSpeed = 20;
        break;

      case Keys.A:
        amelia.StartLeft(dir);

        if (xSpeed > 0)
          amelia.StartFront(dir);
        if (xSpeed < 0)
          amelia.StartBack(dir);

        zSpeed = -20;
        break;

      case Keys.S:
        amelia.StartBack(dir);

        if (zSpeed > 0)
          amelia.StartRight(dir);
        if (zSpeed < 0)
          amelia.StartLeft(dir);

        xSpeed = -20;
        break;

      case Keys.D:
        amelia.StartRight(dir);

        if (xSpeed > 0)
          amelia.StartFront(dir);
        if (xSpeed < 0)
          amelia.StartBack(dir);

        zSpeed = 20;
        break;

      case Keys.J:
        lvl.VirtualCamera.MoveLeft(tspeed);
        break;
      case Keys.L:
        lvl.VirtualCamera.MoveRight(tspeed);
        break;
      case Keys.U:
        lvl.VirtualCamera.Pitch -= speed;
        break;
      case Keys.O:
        lvl.VirtualCamera.Pitch += speed;
        break;
      case Keys.I:
        lvl.VirtualCamera.MoveFront(tspeed);
        break;
      case Keys.K:
        lvl.VirtualCamera.MoveBack(tspeed);
        break;


      case Keys.Right:
        lvl.VirtualCamera.MoveRight(tspeed);
        break;
      case Keys.Left:
        lvl.VirtualCamera.MoveLeft(tspeed);
        break;
      case Keys.Space:
        lvl.VirtualCamera.MoveUp(tspeed);
        break;
      case Keys.ShiftKey:
        lvl.VirtualCamera.MoveDown(tspeed);
        break;


      // case Keys.Escape:
      //   GameForm.Close();
      //   break;
      case Keys.Enter:
        eng.ShowMesh = !eng.ShowMesh;
        break;

      case Keys.NumPad8:
        lvl.VirtualCamera.VCamera = new(0, 0, 0);
        lvl.VirtualCamera.VLookDirection = new(0, 0, 1);
        lvl.VirtualCamera.Yaw = 0;
        lvl.VirtualCamera.Pitch = 0;
        break;
      case Keys.NumPad2:
        lvl.VirtualCamera.VCamera = new(0, 0, 0);
        lvl.VirtualCamera.VLookDirection = new(0, 0, -1);
        lvl.VirtualCamera.Yaw = MathF.PI;
        lvl.VirtualCamera.Pitch = 0;
        break;
      case Keys.NumPad4:
        lvl.VirtualCamera.VCamera = new(0, 0, 0);
        lvl.VirtualCamera.VLookDirection = new(1, 0, 0);
        lvl.VirtualCamera.Yaw = MathF.PI / 2;
        lvl.VirtualCamera.Pitch = 0;
        break;
      case Keys.NumPad6:
        lvl.VirtualCamera.VCamera = new(0, 0, 0);
        lvl.VirtualCamera.VLookDirection = new(-1, 0, 0);
        lvl.VirtualCamera.Yaw = 3 * MathF.PI / 2;
        lvl.VirtualCamera.Pitch = 0;
        break;
      case Keys.NumPad5:
        // eng.VirtualCamera.VCamera = new(-60.8391f, 40.6396f, -93.1818f);
        lvl.VirtualCamera.VCamera = new(-79.70122f, 52.03208f, -111.0604f);
        lvl.VirtualCamera.VLookDirection = new(-1, 0, 0);
        lvl.VirtualCamera.Yaw = -0.6224661f;
        lvl.VirtualCamera.Pitch = -0.24f;
        break;
      case Keys.NumPad0:
        lvl.VirtualCamera.Yaw = MathF.PI / 2;
        break;
      case Keys.Z:
        lvl.VirtualCamera.RotateAroundPoint(0.001f, 0, 0);
        break;
      case Keys.C:
        lvl.VirtualCamera.RotateAroundPoint(-0.001f, 0, 0);
        break;
      case Keys.X:
        lvl.VirtualCamera.VCamera = new(0, 0, 75);
        lvl.VirtualCamera.VLookDirection = new(0, 0, -1);
        lvl.VirtualCamera.Yaw = 9.5f;
        lvl.VirtualCamera.Pitch = 0f;
        break;
      default:
        break;
    }
  }

  public void KeyUp(KeyEventArgs e)
  {
    switch (e.KeyCode)
    {
      case Keys.W:
        xSpeed = 0;
        break;
      case Keys.A:
        zSpeed = 0;
        break;

      case Keys.S:
        xSpeed = 0;
        break;

      case Keys.D:
        zSpeed = 0;
        break;

      default:
        break;
    }
  }
}