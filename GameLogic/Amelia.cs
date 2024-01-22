using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Drawing;
using EM3D;

public class Amelia : Entity
{
    public float Size { get; set; } = 400;
    public Vertex Pos3D { get; set; }
    public float X { get;  set; }
    public float Y { get; set; } = 0;
    public float Speed { get; set; } = 0.01f;
    private SpriteManager manager;

    private float centerScreen;
    public Amelia(float centerScreen)
    {
        this.centerScreen = centerScreen;
        manager = new SpriteManager("Amelia bonita de todos.png", 32, 16);
        manager.QuantSprite = 0;
        Sprite sprite = new Sprite();
        sprite.Add(0, manager);
    }

    public void Draw(Graphics g)
    {

        manager.Draw(g, new PointF(X, Y), Size / 80, Size / 80);
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
        this.Size -= 4;
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

        this.Size += 4;
        FalseMoveY = +Speed;
        manager.StartIndex = 0;
        manager.QuantSprite = 4;
    }
    public void Move(int xmin, int xmax, int ymin, int ymax)
    {
        if (FalseMove != 0)
            this.X += FalseMove;

        if (X < xmin)
            X = xmin;
        if (X > xmax)
            X = xmax;


        FalseMove *= 0.9f;

        if (FalseMove != 0 && ColissionManager.Current.IsColliding(this) == false)
            RealMove = FalseMove;

        if (FalseMoveY != 0)
            this.Y += FalseMoveY;

        if (Y < ymin)
            Y = ymin;
        if (Y > ymax - Size / 2)
            Y = ymax - Size / 2;

        FalseMoveY *= 0.7f;

        if (FalseMoveY != 0 && ColissionManager.Current.IsColliding(this) == false)
            RealMoveY = FalseMoveY;

        if (RealMove < 0.99f && RealMove > -0.99f && RealMoveY < 0.99f && RealMoveY > -0.99f)
        {
            RealMove = 0;
            RealMoveY = 0;
            manager.StartIndex = 0;
            manager.QuantSprite = 0;
        }
    }
}