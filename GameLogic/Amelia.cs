using System.Drawing;
using EM3D;

public class Amelia : Entity
{
  public float RealSize { get; set; } = 0;
  public new float X { get; set; }
  public new float Y { get; set; } = 0;
  public new float Speed { get; set; } = 0.05f;
  public SpriteManager manager;
  
  public Amelia(float x, float y, float z, float length, float width)
  {
    this.Size = 100;
    this.Anchor3D = new(x, y, z);
    this.Hitbox = new(x, z, width, length);

    manager = new SpriteManager("Amelia bonita de todos.png", 8, 16)
    {
      QuantSprite = 4
    };
  }

  public void Draw(Graphics g, float distance, float ratio)
  {
    float k = ratio * distance;
    RealSize = k;
    manager.Draw(g, new PointF(X, Y), Size / k, Size / k);
  }
  public float RealMove { get; set; } = 0f;
  public float FalseMove { get; set; } = 0f;
  public float RealMoveY { get; set; } = 0f;
  public float FalseMoveY { get; set; } = 0f;

  public void StartLeft()
  {
    FalseMove = -Speed;
    manager.StartIndex = 12;
    manager.QuantSprite = 4;
  }

  public void StartRight()
  {
    FalseMove = Speed;
    manager.StartIndex = 8;
    manager.QuantSprite = 4;
  }
  public void StartUp()
  {
    if (ColissionManager.Current.IsColliding(this))
    {
      FalseMoveY = -Speed;
      manager.StartIndex = 4;
      manager.QuantSprite = 4;
    }
    // this.Size -= 4;
    FalseMoveY = -Speed;
    manager.StartIndex = 4;
    manager.QuantSprite = 4;
  }
  public void StartDown()
  {
    if (ColissionManager.Current.IsColliding(this))
    {
      FalseMoveY = +Speed;
      manager.StartIndex = 0;
      manager.QuantSprite = 4;
    }
    // this.Size += 4;
    FalseMoveY = +Speed;
    manager.StartIndex = 0;
    manager.QuantSprite = 4;
  }
  public void Move(int xmin, int xmax, int ymin, int ymax)
  {
    if (FalseMove != 0)
    {
      this.Anchor3D = new(Anchor3D.X + FalseMove, Anchor3D.Y, Anchor3D.Z);
      this.Hitbox =  new(Hitbox.X + FalseMove, Hitbox.Y, Hitbox.Width, Hitbox.Height);
    }
    if (X < xmin)
      X = xmin;
    if (X > xmax)
      X = xmax;
    FalseMove *= 0.9f;
    if (FalseMove != 0 && ColissionManager.Current.IsColliding(this) == false)
      RealMove = FalseMove;

    if (FalseMoveY != 0)
    {
      this.Anchor3D = new(Anchor3D.X, Anchor3D.Y, Anchor3D.Z + FalseMoveY);
      this.Hitbox =  new(Hitbox.X, Hitbox.Y + FalseMoveY, Hitbox.Width, Hitbox.Height);
    }
    if (Y < ymin)
      Y = ymin;
    if (Y > ymax - Size / 2)
      Y = ymax - Size / 2;
    FalseMoveY *= 0.9f;
    if (FalseMoveY != 0 && ColissionManager.Current.IsColliding(this) == false)
      RealMoveY = FalseMoveY;

    if (
      RealMove < (0.1f * Speed) &&
      RealMove > -(0.1f * Speed) &&
      RealMoveY < (0.1f * Speed) &&
      RealMoveY > -(0.1f * Speed)
      )
    {
      RealMove = 0;
      RealMoveY = 0;
      manager.StartIndex = 0;
      manager.QuantSprite = 0;
    }
  }
}