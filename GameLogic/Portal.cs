using System.Drawing;
using System.Windows.Forms;
using EM3D;
public class Portal : Entity
{
    public Portal destiny;
    Image spritesheet = null;
    public bool IsOpen { get; set; }
    public bool PortalUsed { get; set; }

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

    public void TeleportEntity(Entity entity)
    {
        if (PortalUsed && this.Hitbox.IntersectsWith(entity.Hitbox))
            return;
        
        entity.Anchor3D = new(destiny.Anchor3D.X, destiny.Anchor3D.Y, destiny.Anchor3D.Z);
        destiny.PortalUsed = true;

    }

}