using System.Drawing;
using EM3D;

public class Amelia : Entity
{
  public float RealSize { get; set; } = 0;
  public float RealMoveX { get; set; } = 0f;
  public float FalseMoveX { get; set; } = 0f;
  public float RealMoveZ { get; set; } = 0f;
  public float FalseMoveZ { get; set; } = 0f;
  public SpriteManager manager;

  public Amelia(float x, float y, float z, float length, float width)
  {
    this.Speed = 0.05f;
    this.Height = 100;
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
    float k = ratio * distance;
    RealSize = Height / k;
    manager.Draw(g, new PointF(X, Y), Height / k, Height / k);
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
    if (ColissionManager.Current.IsColliding(this))
    {
      FalseMoveZ = -Speed;
      manager.StartIndex = 4;
      manager.QuantSprite = 4;
    }
    FalseMoveZ = -Speed;
    manager.StartIndex = 4;
    manager.QuantSprite = 4;
  }
  public void StartDown()
  {
    if (ColissionManager.Current.IsColliding(this))
    {
      FalseMoveZ = Speed;
      manager.StartIndex = 0;
      manager.QuantSprite = 4;
    }
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



    // if (X < xmin)
    //   X = xmin;
    // if (X > xmax)
    //   X = xmax;
    // if (FalseMove != 0 && ColissionManager.Current.IsColliding(this) == false)
    //   RealMove = FalseMove;


    // if (Y < zmin)
    //   Y = zmin;
    // if (Y > zmax)
    //   Y = zmax;
    // if (FalseMoveY != 0 && ColissionManager.Current.IsColliding(this) == false)
    //   RealMoveY = FalseMoveY;
    // RealMoveX = FalseMoveX;
    // RealMoveZ = FalseMoveZ;

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