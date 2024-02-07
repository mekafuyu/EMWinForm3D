using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;

public class ColissionManager
{
  public List<Entity> entities = new List<Entity>();
  // private static ColissionManager crr = new ColissionManager();
  // public static ColissionManager Current => crr;

  // private ColissionManager() { }
  // public void Reset()
  //   => crr = new ColissionManager();

  public List<Entity> IsColliding(Entity entity)
  {
    List<Entity> collidingEntities = new List<Entity>();

    foreach (var e in entities)
    { 
      if (entity == e)
        continue;

      var h1 = entity.Hitbox;
      var h2 = e.Hitbox;

      if(h1.Width < 0)
        h1 = new(h1.X + h1.Width, h1.Y, -h1.Width, h1.Height);
      if(h1.Height < 0)
        h1 = new(h1.X, h1.Y + h1.Height, h1.Width, -h1.Height);

      if(h2.Width < 0)
        h2 = new(h2.X + h2.Width, h2.Y, -h2.Width, h2.Height);
      if(h2.Height < 0)
        h2 = new(h2.X, h2.Y + h2.Height, h2.Width, -h2.Height);

      if (h1.IntersectsWith(h2))
        collidingEntities.Add(e);
    }
    return collidingEntities;
  }

  public void AddEntity(Entity entity)
  {
    entities.Add(entity);
  }
  public void AddEntity(params Entity[] entity)
  {
    foreach (var e in entity)
    {
      this.entities.Add(e);
    }
  }
}
