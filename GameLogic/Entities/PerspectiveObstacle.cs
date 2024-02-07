using System.Drawing;
using System.Numerics;
using EM3D;
public class PerspectiveObstacle : Entity
{
  Image spritesheet = null;
  public Vector3 ClosedPos;
  public PerspectiveObstacle(float x, float y, float z, float length, float width, Vector3 closedPos)
  {
    this.Anchor3D = new(x, y, z);
    this.Hitbox = new(x, z, width, length);
    this.ClosedPos = closedPos;
  }

  public void Draw(Graphics g, PointF p)
  {
    g.DrawImage(spritesheet, p);
  }
  public bool IsOpen(Vector3 camerapos) {
    return Vector3.Distance(ClosedPos, camerapos) > 5;
  }
}