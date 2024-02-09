using System.Drawing;

public class Book : Entity
{
    SpriteManager manager;
    public Book(float x, float y, float z, float width, float height, float length)
    {
        this.Height = height;
        this.Anchor3D = new(x, y, z);
        this.Length = length;
        this.Width = width;
        this.Hitbox = new(x, z, width, length);
        manager = new SpriteManager("./assets/imgs/livro.png", 1, 1, 499, 499);
    }
    public void Draw(Graphics g, float distance, float ratio)
    {
        float k = distance;
        RealSize = Height * 17 / k;
        manager.Draw(g, new PointF(X, Y), RealSize, RealSize);
    }
}