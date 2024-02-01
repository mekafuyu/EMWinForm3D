using System;
using System.Drawing;
using System.Windows.Forms;
using System.Xml;
using EM3D;
public class Portal : Entity
{
    public Portal destiny;
    Image spritesheet = null;
    public bool IsOpen { get; set; }
    public bool PortalUsed { get; set; }
    public TimeSpan Cooldown { get; set; } = TimeSpan.Zero;

    public Portal(float x, float y, float z, float length, float width, bool isOpen)
    {
        this.Anchor3D = new(x, y, z);
        this.Hitbox = new(x, z, width, length);
        this.IsOpen = isOpen;
    }

    public void Draw(Graphics g, PointF p)
    {
        g.DrawImage(spritesheet, p);
    }

    public void TogglePortal()
    {
        IsOpen = !IsOpen;
    }

    DateTime? start = null;
    public void TryTeleportEntity(Entity entity)
    {
        if (start is null)
        {
            start = DateTime.Now;
            return;
        }

        var timePassed = DateTime.Now - start.Value;
        if (timePassed < Cooldown)
            return;
        
        Reset();
        destiny.Reset();

        Cooldown = TimeSpan.Zero;
        destiny.Cooldown = TimeSpan.FromSeconds(2);
        entity.Anchor3D = new(destiny.Anchor3D.X, destiny.Anchor3D.Y, destiny.Anchor3D.Z);
    }

    public void Reset()
        => start = null;
}