
using System.Collections.Generic;
using System.Drawing;
using EM3D;

public class Level
{
  public Amelia Amelia;
  public List<Entity> Entities;
  public List<Mesh> Meshes;
  public Bitmap Background;
  public (float x, float y, float z) GlobalRotation;
  public (float x, float y, float z) GlobalTranslation;


}