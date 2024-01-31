using System.Drawing;
using System.Windows.Forms;
using EM3D;

public class FirstLevel : Level
{

  public void Refresh(Graphics g, PictureBox pb, EMEngine eng)
  {
    g.DrawImage(Background, 0, 0, pb.Width, pb.Height);

    

    

    Amelia.Move(0, pb.Width, 0, pb.Height);
    eng.GetFrame(
      (pb.Width, pb.Height),
      g,
      Meshes,
      GlobalRotation,
      GlobalRotation,
      ColissionManager.Current.entities
    );
  }
}