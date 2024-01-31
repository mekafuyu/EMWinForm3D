
using System;
using System.Windows.Forms;

public partial class Game
{
  float speed = 0.01f;
  float tspeed = 0.5f;
  int xSpeed = 0;
  int zSpeed = 0;
  public void KeyDown(KeyEventArgs e, Amelia amelia)
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
        Engine.VirtualCamera.MoveLeft(tspeed);
        break;
      case Keys.L:
        Engine.VirtualCamera.MoveRight(tspeed);
        break;
      case Keys.U:
        Engine.VirtualCamera.Pitch -= speed;
        break;
      case Keys.O:
        Engine.VirtualCamera.Pitch += speed;
        break;
      case Keys.I:
        Engine.VirtualCamera.MoveFront(tspeed);
        break;
      case Keys.K:
        Engine.VirtualCamera.MoveBack(tspeed);
        break;


      case Keys.Right:
        Engine.VirtualCamera.MoveRight(tspeed);
        break;
      case Keys.Left:
        Engine.VirtualCamera.MoveLeft(tspeed);
        break;
      case Keys.Space:
        Engine.VirtualCamera.MoveUp(tspeed);
        break;
      case Keys.ShiftKey:
        Engine.VirtualCamera.MoveDown(tspeed);
        break;


      case Keys.Escape:
        GameForm.Close();
        break;
      case Keys.Enter:
        Engine.ShowMesh = !Engine.ShowMesh;
        break;

      case Keys.NumPad8:
        Engine.VirtualCamera.VCamera = new(0, 0, 0);
        Engine.VirtualCamera.VLookDirection = new(0, 0, 1);
        Engine.VirtualCamera.Yaw = 0;
        Engine.VirtualCamera.Pitch = 0;
        break;
      case Keys.NumPad2:
        Engine.VirtualCamera.VCamera = new(0, 0, 0);
        Engine.VirtualCamera.VLookDirection = new(0, 0, -1);
        Engine.VirtualCamera.Yaw = MathF.PI;
        Engine.VirtualCamera.Pitch = 0;
        break;
      case Keys.NumPad4:
        Engine.VirtualCamera.VCamera = new(0, 0, 0);
        Engine.VirtualCamera.VLookDirection = new(1, 0, 0);
        Engine.VirtualCamera.Yaw = MathF.PI / 2;
        Engine.VirtualCamera.Pitch = 0;
        break;
      case Keys.NumPad6:
        Engine.VirtualCamera.VCamera = new(0, 0, 0);
        Engine.VirtualCamera.VLookDirection = new(-1, 0, 0);
        Engine.VirtualCamera.Yaw = 3 * MathF.PI / 2;
        Engine.VirtualCamera.Pitch = 0;
        break;

      case Keys.Z:
        MessageBox.Show(Engine.VirtualCamera.VCamera.ToString());
        break;
      case Keys.X:
        Engine.VirtualCamera.VCamera = new(0, 0, 75);
        Engine.VirtualCamera.VLookDirection = new(0, 0, -1);
        Engine.VirtualCamera.Yaw = 9.5f;
        Engine.VirtualCamera.Pitch = 0f;
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