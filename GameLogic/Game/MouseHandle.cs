using System;
using System.Drawing;

public partial class Game
{
  private float pitchMove = 0;
  private float yawMove = 0;
  private float sense = 0.001f;
  private Point cursorReset;
  private void initMouseHandle()
  {
    Pb.MouseMove += (o, e) =>
    {
      if(!GameForm.Focused)
        return;

      pitchMove = (e.Location.Y - cursorReset.Y) * sense + 23 * sense;
      yawMove = (e.Location.X - cursorReset.X) * sense;
      if(MathF.Abs(yawMove) > sense * 5)
        currLevel.VirtualCamera.Yaw += yawMove;
      if(MathF.Abs(pitchMove) > sense * 5)
      currLevel.VirtualCamera.Pitch -= pitchMove;
      CursorPos = e.Location;
    };
    Pb.MouseDown += (o, e) =>
      IsDown = true;
    Pb.MouseUp += (o, e) =>
      IsDown = false;
  }
}