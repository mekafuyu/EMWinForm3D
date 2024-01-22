using System;

namespace EM3D;

public partial class EMEngine
{
  public int fpsCalculator()
  {
    double secondsElapsed = (DateTime.Now - lastCheckTime).TotalSeconds;
    lastCheckTime = DateTime.Now;
    return (int)(1 / secondsElapsed);
  }
}