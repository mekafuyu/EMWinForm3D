using System;
using System.Drawing;
using System.Numerics;
using EM3D;
public class PerspectivePortal : Entity
{
    Image spritesheet = null;
    public Vector3 ClosedPos;
    public bool reverse;
    public PerspectivePortal destiny;
    public bool IsUsable { get; set; }
    public bool PortalUsed { get; set; }
    public TimeSpan Cooldown { get; set; } = TimeSpan.Zero;
    public PerspectivePortal(float x, float y, float z, float length, float width, Vector3 closedPos, bool isReverse)
    {
        this.Anchor3D = new(x, y, z);
        this.Hitbox = new(x, z, width, length);
        this.ClosedPos = closedPos;
        this.reverse = isReverse;
    }

    public void Draw(Graphics g, PointF p)
    {
        g.DrawImage(spritesheet, p);
    }
    public bool IsOpen(Vector3 camerapos)
    {
        if (reverse)
            return !(Vector3.Distance(ClosedPos, camerapos) > 5);
        return Vector3.Distance(ClosedPos, camerapos) > 5;
    }
    public void TogglePortal()
    {
        IsUsable = !IsUsable;
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