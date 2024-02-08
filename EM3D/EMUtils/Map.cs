using System.Drawing;
using System.Globalization;
using System.IO;
using System.Numerics;
using System.Text;
using System.Windows.Forms.VisualStyles;

namespace EM3D.EMUtils;

public static class Mapper
{
  private static SizeF ballSize = new(10, 10);
  private static SizeF mapSize = new(300, 300);
  private static PointF mapPos = new(0, 0);
  public static void Map(Graphics g, Camera cam)
  {
    g.DrawRectangle(Pens.Red, new RectangleF(mapPos, mapSize));
    float ratio = 1;
    float offsetX = mapSize.Width / 2;
    float offsetY = mapSize.Height / 2;

    PointF ballPoint = new(
      offsetX + (cam.VCamera.X * ratio) - ballSize.Width / 2,
      offsetY + (cam.VCamera.Z * ratio) - ballSize.Height / 2);
    RectangleF ball = new RectangleF(ballPoint, ballSize);

    var lkdir = cam.VLookDirection;
    lkdir *= 1.5f;
    g.FillEllipse(Brushes.Red, ball);
    g.DrawLine(Pens.Yellow,
    ballPoint.X + ball.Size.Width / 2,
    ballPoint.Y + ball.Size.Height / 2,
    ballPoint.X + ball.Size.Width / 2 + lkdir.X * 10,
    ballPoint.Y + ball.Size.Height / 2 + lkdir.Z * 10);
  }
}