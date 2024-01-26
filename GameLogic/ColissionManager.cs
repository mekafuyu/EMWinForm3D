using System.Collections.Generic;

public class ColissionManager
{
  private List<Entity> entities = new List<Entity>();
  private static ColissionManager crr = new ColissionManager();
  public static ColissionManager Current => crr;

  private ColissionManager() { }
  public void Reset()
    => crr = new ColissionManager();

  public List<Entity> IsColliding(Entity entity)
  {
    List<Entity> collidingEntities = new List<Entity>();

    foreach (var anotherEntity in entities)
    { 
      if (entity == anotherEntity)
        continue;
      if (entity.Hitbox.IntersectsWith(anotherEntity.Hitbox))
        collidingEntities.Add(anotherEntity);
    }
    return collidingEntities;
  }

  public void AddEntity(Entity entity)
  {
    entities.Add(entity);
  }
}
