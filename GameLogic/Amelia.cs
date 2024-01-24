using System.Drawing;
using System.Windows.Forms;
using EM3D;

public class Amelia : Entity
{
  public Triangle Tr { get; set; }
  public float RealMoveX { get; set; } = 0f;
  public float FalseMoveX { get; set; } = 0f;
  public float RealMoveZ { get; set; } = 0f;
  public float FalseMoveZ { get; set; } = 0f;
  public SpriteManager manager;

  public Amelia(float x, float y, float z, float width, float height, float length)
  {
    this.Speed = 0.05f;
    this.Height = height;
    this.Anchor3D = new(x, y, z);
    this.Length = length;
    this.Width = width;
    SetHitbox();

    manager = new SpriteManager("Amelia bonita de todos.png", 8, 16)
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
    FalseMoveX = -Speed;
    manager.StartIndex = 12;
    manager.QuantSprite = 4;
  }
  public void StartRight()
  {
    FalseMoveX = Speed;
    manager.StartIndex = 8;
    manager.QuantSprite = 4;
  }
  public void StartUp()
  {
 
    FalseMoveZ = -Speed;
    manager.StartIndex = 4;
    manager.QuantSprite = 4;
  }
  public void StartDown()
  {
    FalseMoveZ = Speed;
    manager.StartIndex = 0;
    manager.QuantSprite = 4;
  }
  public void Move(int xmin, int xmax, int zmin, int zmax)
  {
    if (FalseMoveX != 0)
    {
      this.Anchor3D = new(Anchor3D.X + FalseMoveX, Anchor3D.Y, Anchor3D.Z);
      FalseMoveX *= 0.9f;
      SetHitbox();
    }
    if (FalseMoveZ != 0)
    {
      this.Anchor3D = new(Anchor3D.X, Anchor3D.Y, Anchor3D.Z + FalseMoveZ);
      FalseMoveZ *= 0.9f;
      SetHitbox();
    }

    if (FalseMoveX != 0 && ColissionManager.Current.IsColliding(this) == false)
      RealMoveX = FalseMoveX;

    if (FalseMoveZ != 0 && ColissionManager.Current.IsColliding(this) == false)
      RealMoveZ = FalseMoveZ;

    if (
      FalseMoveX < (0.1f * Speed) &&
      FalseMoveX > -(0.1f * Speed) &&
      FalseMoveZ < (0.1f * Speed) &&
      FalseMoveZ > -(0.1f * Speed)
      )
    {
      FalseMoveX = 0;
      FalseMoveZ = 0;
      manager.StartIndex = 0;
      manager.QuantSprite = 0;
    }


  }
}