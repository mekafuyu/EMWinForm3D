using System.Drawing;

using EM3D;
public class Door : Entity
{
  Image spritesheet = null;
  public bool IsOpen { get; set; }
  public Door(float x, float y, float z, float length, float width, bool isOpen)
  {
    this.Anchor3D = new(x, y, z);
    this.Hitbox = new(x, z, width, length);
    this.IsOpen = isOpen;
  }

  public void Draw(Graphics g, PointF p)
  {
    g.DrawImage(spritesheet, p);
  }

  public void ToggleDoor()
  {
    IsOpen = !IsOpen;
  }
}