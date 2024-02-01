using System;
using System.Windows.Forms;
using EM3D;

public class KeyboardHandle
{
  float speed = 0.01f;
  float tspeed = 0.5f;
  int xSpeed = 0;
  int zSpeed = 0;
  public void KeyDown(KeyEventArgs e, EMEngine eng, Amelia amelia)
  {
    switch (e.KeyCode)
    {
      case Keys.W:
        amelia.StartUp();
        if (zSpeed > 0)
          amelia.StartRight();
        if (zSpeed < 0)
          amelia.StartLeft();

        xSpeed = 20;
        break;

      case Keys.A:
        amelia.StartLeft();

        if (xSpeed > 0)
          amelia.StartUp();
        if (xSpeed < 0)
          amelia.StartDown();

        zSpeed = -20;
        break;

      case Keys.S:
        amelia.StartDown();

        if (zSpeed > 0)
          amelia.StartRight();
        if (zSpeed < 0)
          amelia.StartLeft();

        xSpeed = -20;
        break;

      case Keys.D:
        amelia.StartRight();

        if (xSpeed > 0)
          amelia.StartUp();
        if (xSpeed < 0)
          amelia.StartDown();

        zSpeed = 20;
        break;

      case Keys.J:
        eng.VirtualCamera.MoveLeft(tspeed);
        break;
      case Keys.L:
        eng.VirtualCamera.MoveRight(tspeed);
        break;
      case Keys.U:
        eng.VirtualCamera.Pitch -= speed;
        break;
      case Keys.O:
        eng.VirtualCamera.Pitch += speed;
        break;
      case Keys.I:
        eng.VirtualCamera.MoveFront(tspeed);
        break;
      case Keys.K:
        eng.VirtualCamera.MoveBack(tspeed);
        break;


      case Keys.Right:
        eng.VirtualCamera.MoveRight(tspeed);
        break;
      case Keys.Left:
        eng.VirtualCamera.MoveLeft(tspeed);
        break;
      case Keys.Space:
        eng.VirtualCamera.MoveUp(tspeed);
        break;
      case Keys.ShiftKey:
        eng.VirtualCamera.MoveDown(tspeed);
        break;


      // case Keys.Escape:
      //   GameForm.Close();
      //   break;
      case Keys.Enter:
        eng.ShowMesh = !eng.ShowMesh;
        break;

      case Keys.NumPad8:
        eng.VirtualCamera.VCamera = new(0, 0, 0);
        eng.VirtualCamera.VLookDirection = new(0, 0, 1);
        eng.VirtualCamera.Yaw = 0;
        eng.VirtualCamera.Pitch = 0;
        break;
      case Keys.NumPad2:
        eng.VirtualCamera.VCamera = new(0, 0, 0);
        eng.VirtualCamera.VLookDirection = new(0, 0, -1);
        eng.VirtualCamera.Yaw = MathF.PI;
        eng.VirtualCamera.Pitch = 0;
        break;
      case Keys.NumPad4:
        eng.VirtualCamera.VCamera = new(0, 0, 0);
        eng.VirtualCamera.VLookDirection = new(1, 0, 0);
        eng.VirtualCamera.Yaw = MathF.PI / 2;
        eng.VirtualCamera.Pitch = 0;
        break;
      case Keys.NumPad6:
        eng.VirtualCamera.VCamera = new(0, 0, 0);
        eng.VirtualCamera.VLookDirection = new(-1, 0, 0);
        eng.VirtualCamera.Yaw = 3 * MathF.PI / 2;
        eng.VirtualCamera.Pitch = 0;
        break;

      case Keys.Z:
        MessageBox.Show(eng.VirtualCamera.VCamera.ToString());
        break;
      case Keys.X:
        eng.VirtualCamera.VCamera = new(0, 0, 75);
        eng.VirtualCamera.VLookDirection = new(0, 0, -1);
        eng.VirtualCamera.Yaw = 9.5f;
        eng.VirtualCamera.Pitch = 0f;
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