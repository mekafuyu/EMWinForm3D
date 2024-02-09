using System;
using System.Collections.Generic;
using System.Drawing;
using System.Numerics;
using System.Windows.Forms;
using EM3D;

public class Amelia : Entity
{
  public Triangle Tr { get; set; }
  public float SpeedX { get; set; } = 0f;
  public float SpeedZ { get; set; } = 0f;
  public SpriteManager manager;

   public GameSound gameSound = new GameSound();

  public Amelia(float x, float y, float z, float width, float height, float length)
  {
    this.Speed = 0.1f;
    this.Height = height;
    this.Anchor3D = new(x, y, z);
    this.Length = length;
    this.Width = width;
    SetHitbox();

    manager = new SpriteManager("./assets/imgs/Amelia bonita de todos.png", 9, 16, 38, 90)
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
  public void StartLeft(Vector3 dir)
  {
    if(MathF.Abs(dir.X) > MathF.Abs(dir.Z))
    {
      dir.Z = MathF.Sign(dir.X);
      dir.X = 0;
    }
    else
    {
      dir.X = -MathF.Sign(dir.Z);
      dir.Z = 0;
    }
   
    SpeedZ = dir.Z * Speed;
    SpeedX = dir.X * Speed;
    
    manager.StartIndex = 12;
    manager.QuantSprite = 4;
  }
  public void StartRight(Vector3 dir)
  { 
    if(MathF.Abs(dir.X) > MathF.Abs(dir.Z))
    {
      dir.Z = -MathF.Sign(dir.X);
      dir.X = 0;
    }
    else
    {
      dir.X = MathF.Sign(dir.Z);
      dir.Z = 0;
    }

    SpeedZ = dir.Z * Speed;
    SpeedX = dir.X * Speed;

    manager.StartIndex = 8;
    manager.QuantSprite = 4;
  }
  public void StartFront(Vector3 dir)
  {
    dir *= -1;
    if(MathF.Abs(dir.X) > MathF.Abs(dir.Z))
    {
      dir.X = MathF.Sign(dir.X);
      dir.Z = 0;
    }
    else
    {
      dir.X = 0;
      dir.Z = MathF.Sign(dir.Z);
    }
    SpeedZ = dir.Z * Speed;
    SpeedX = dir.X * Speed;

    manager.StartIndex = 4;
    manager.QuantSprite = 4;
  }
  public void StartBack(Vector3 dir)
  {
    if(MathF.Abs(dir.X) > MathF.Abs(dir.Z))
    {
      dir.X = MathF.Sign(dir.X);
      dir.Z = 0;
    }
    else
    {
      dir.X = 0;
      dir.Z = MathF.Sign(dir.Z);
    }
    SpeedZ = dir.Z * Speed;
    SpeedX = dir.X * Speed;
    manager.StartIndex = 0;
    manager.QuantSprite = 4;
  }

  public void Move(ColissionManager colissionManager, Vector3 cameraPos, Game game)
  {
    this.Anchor3D = new(Anchor3D.X + SpeedX, Anchor3D.Y, Anchor3D.Z + SpeedZ);
    SetHitbox();
    var list = colissionManager.IsColliding(this);
    bool onFloor = false;
    if (list.Count > 0)
    {
      foreach (var obj in list)
      {
        if(obj is Book book )
        {
          if(MathF.Abs(book.Anchor3D.Y - this.Anchor3D.Y) > 3)
            continue;
          game.ColectedPages += 1;
          game.RemoveBook(book);
        }
        if (obj is Floor)
        {
          // MessageBox.Show("coringa");
          onFloor = true;
        }
        if (obj is Wall)
        {
          this.Anchor3D = new(Anchor3D.X - SpeedX, Anchor3D.Y, Anchor3D.Z - SpeedZ);
        }

        if (obj is Door door && !door.IsOpen)
        {
          this.Anchor3D = new(Anchor3D.X - SpeedX, Anchor3D.Y, Anchor3D.Z - SpeedZ);
        }

        if (obj is PerspectiveObstacle perspectiveObstacle && !perspectiveObstacle.IsOpen(cameraPos))
        {
          this.Anchor3D = new(Anchor3D.X - SpeedX, Anchor3D.Y, Anchor3D.Z - SpeedZ);
        }
        if (obj is PerspectivePortal perspectivePortal)
        {
          if (this.Anchor3D.Y != perspectivePortal.Anchor3D.Y)
            continue;
          if (perspectivePortal.IsOpen(cameraPos))
          {
            perspectivePortal.TryTeleportEntity(this);
            if(this.Anchor3D.X != perspectivePortal.destiny.Anchor3D.X ||  this.Anchor3D.Z != perspectivePortal.destiny.Anchor3D.Z)
              perspectivePortal.destiny.PortalUsed = false;
            continue;
          }
          // this.Anchor3D = new(Anchor3D.X - SpeedX, Anchor3D.Y, Anchor3D.Z - SpeedZ);
        }
        
        if (obj is Portal portal)
        {
          if(portal.IsOpen)
          {
            portal.TryTeleportEntity(this);
            if(this.Anchor3D.X != portal.destiny.Anchor3D.X ||  this.Anchor3D.Z != portal.destiny.Anchor3D.Z)
              portal.destiny.PortalUsed = false;
            continue;
          }
          this.Anchor3D = new(Anchor3D.X - SpeedX, Anchor3D.Y, Anchor3D.Z - SpeedZ);
        }
      }
    }
    if(!onFloor)
    {
      // MessageBox.Show("a");
      this.Anchor3D = new(Anchor3D.X - SpeedX, Anchor3D.Y, Anchor3D.Z - SpeedZ);
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
      gameSound.StopMusic();
      SpeedX = 0;
      SpeedZ = 0;
      manager.StartIndex = 0;
      manager.QuantSprite = 0;
    }
  }
}