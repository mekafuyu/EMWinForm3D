using System;

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