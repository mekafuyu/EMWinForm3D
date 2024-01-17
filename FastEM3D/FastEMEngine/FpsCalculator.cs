using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Numerics;
using System.Windows.Forms;

using FastEM3D.EMUtils;
using static FastEM3D.EMUtils.EMGeometry;

namespace FastEM3D;

public partial class FastEMEngine
{
  public int fpsCalculator()
  {
    double secondsElapsed = (DateTime.Now - lastCheckTime).TotalSeconds;
    lastCheckTime = DateTime.Now;
    return (int)(1 / secondsElapsed);
  }
}