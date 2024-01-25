using System.Collections.Generic;

public class ColissionManager
{
  private List<Entity> entities = new List<Entity>();
  private static ColissionManager crr = new ColissionManager();
  public static ColissionManager Current => crr;

  private ColissionManager() { }
  public void Reset()
    => crr = new ColissionManager();

  public bool IsColliding(Entity entity)
  {
    foreach (var anotherEntity in entities)
    {
      if (entity == anotherEntity)
        continue;
      if (entity.Hitbox.IntersectsWith(anotherEntity.Hitbox))
        return true;
    }
    return false;
  }

  public void AddEntity(Entity entity)
  {
    entities.Add(entity);
  }
}
