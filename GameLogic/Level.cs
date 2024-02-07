
using System.Collections.Generic;
using System.Drawing;
using System.Numerics;
using System.Windows.Forms;
using EM3D;

public class Level
{
  public Amelia Amelia;
  public List<Entity> Entities;
  public List<Mesh> Meshes;
  public Bitmap Background;
  public (float x, float y, float z) GlobalRotation;
  public (float x, float y, float z) GlobalTranslation;
  public ColissionManager CM;
  public KeyboardHandle KeyboardMap = new();
  public Camera VirtualCamera = new();

  public void Initialize(List<Entity> e, List<Mesh> m, Bitmap bg)
  {
    Amelia = new Amelia(0, 0, -5, 1, 10, 1);
    Background = bg;
    e.Add(Amelia);
    Entities = e;
    Meshes = m;
    CM = new()
    {
      entities = Entities
    };
  }
  public void Initialize(Vector3 pos, float yaw, float pitch, List<Entity> e, List<Mesh> m, Bitmap bg)
  {
    Initialize(e, m, bg);
    VirtualCamera.VCamera = pos;
    VirtualCamera.Yaw = yaw;
    VirtualCamera.Pitch = pitch;

  }

  public void Refresh(Graphics g, PictureBox pb, EMEngine eng)
  {
    g.DrawImage(Background, 0, 0, pb.Width, pb.Height);

    Amelia.Move(CM, eng.VirtualCamera.VCamera);
    eng.GetFrame(
      (pb.Width, pb.Height),
      g,
      Meshes,
      GlobalRotation,
      GlobalRotation,
      Entities
    );
  }
}