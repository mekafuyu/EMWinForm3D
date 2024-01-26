using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using EM3D;

public class Amelia : Entity
{
  public Triangle Tr { get; set; }
  public float SpeedX { get; set; } = 0f;
  public float SpeedZ { get; set; } = 0f;
  public SpriteManager manager;

  public Amelia(float x, float y, float z, float width, float height, float length)
  {
    this.Speed = 0.09f;
    this.Height = height;
    this.Anchor3D = new(x, y, z);
    this.Length = length;
    this.Width = width;
    SetHitbox();

    manager = new SpriteManager("Amelia bonita de todos.png", 4, 16, 38, 90)
    {
      QuantSprite = 4
    };
  }
  public void SetHitbox()
    => this.Hitbox = new(this.Anchor3D.X - this.Width / 2, this.Anchor3D.Z - this.Length / 2, this.Width, this.Length);

  public void Draw(Graphics g, float distance, float ratio)
  {
    float k = distance;
    RealSize = Height * 17 / k;
    manager.Draw(g, new PointF(X, Y), RealSize, RealSize);
  }
  public void StartLeft()
  {
    SpeedX = -Speed;
    manager.StartIndex = 12;
    manager.QuantSprite = 4;
  }
  public void StartRight()
  {
    SpeedX = Speed;
    manager.StartIndex = 8;
    manager.QuantSprite = 4;
  }
  public void StartUp()
  {
    SpeedZ = -Speed;
    manager.StartIndex = 4;
    manager.QuantSprite = 4;
  }
  public void StartDown()
  {
    SpeedZ = Speed;
    manager.StartIndex = 0;
    manager.QuantSprite = 4;
  }
  public void Move(int xmin, int xmax, int zmin, int zmax)
  {
    this.Anchor3D = new(Anchor3D.X + SpeedX, Anchor3D.Y, Anchor3D.Z + SpeedZ);
    SetHitbox();
    var list = ColissionManager.Current.IsColliding(this);
    if (list.Count > 0)
    {
      foreach (var obj in list)
      {
        if (obj is Wall)
        {
          this.Anchor3D = new(Anchor3D.X - SpeedX, Anchor3D.Y, Anchor3D.Z - SpeedZ);
        }

        if (obj is Door door && door.IsOpen == false)
        {
          this.Anchor3D = new(Anchor3D.X - SpeedX, Anchor3D.Y, Anchor3D.Z - SpeedZ);
        }
        if (obj is Door door2 && door2.IsOpen == true)
        {
          
        }
      }
    }

    SpeedX *= 0.9f;
    SpeedZ *= 0.9f;

    if (
      SpeedX < (0.1f * Speed) &&
      SpeedX > -(0.1f * Speed) &&
      SpeedZ < (0.1f * Speed) &&
      SpeedZ > -(0.1f * Speed)
      )
    {
      SpeedX = 0;
      SpeedZ = 0;
      manager.StartIndex = 0;
      manager.QuantSprite = 0;
    }
  }
}