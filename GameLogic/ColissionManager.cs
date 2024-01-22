using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Windows.Forms;

public class ColissionManager
{
    private List<Entity> entities = new List<Entity>();
    private static ColissionManager crr = new ColissionManager();
    public static ColissionManager Current => crr;

    private ColissionManager() { }
    public void Reset()
        => crr = new ColissionManager();

    private void asdasd()
    {

    }

    public bool IsColliding(Entity entity)
    {
        foreach (var anotherEntity in entities)
        {
            if(entity.rec.Contains(anotherEntity.rec))
                return true;
            if(anotherEntity.rec.Contains(entity.rec))
                return true;
        }
        return false;
    }

}
