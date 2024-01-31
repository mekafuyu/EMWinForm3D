using System.Drawing;

public partial class Game
{
  private float pitchMove = 0;
  private float yawMove = 0;
  private float sense = 0.001f;
  private Point cursorReset;
  public void mouseHandle()
  {
    Pb.MouseMove += (o, e) =>
    {
      pitchMove = (e.Location.Y - cursorReset.Y) * sense + 23 * sense;
      yawMove = (e.Location.X - cursorReset.X) * sense;
      Engine.VirtualCamera.Yaw += yawMove;
      Engine.VirtualCamera.Pitch -= pitchMove;
      CursorPos = e.Location;
    };
    Pb.MouseDown += (o, e) =>
      IsDown = true;
    Pb.MouseUp += (o, e) =>
      IsDown = false;
  }
}