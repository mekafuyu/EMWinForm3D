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
      if (entity.Hitbox.Contains(anotherEntity.Hitbox))
        return true;
      if (anotherEntity.Hitbox.Contains(entity.Hitbox))
        return true;
    }
    return false;
  }
}
