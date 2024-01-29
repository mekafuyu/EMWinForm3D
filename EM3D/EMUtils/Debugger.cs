using System.Drawing;

namespace EM3D.EMUtils;

public static class Debugger
{
  public static void ShowOnScreen(Graphics g, string[] info)
  {
    for (int i = 0; i < info.Length; i++)
    {
      g.DrawString(info[i], SystemFonts.DefaultFont, Brushes.White, 0, 30 + i * 10);
    }
  }
}
