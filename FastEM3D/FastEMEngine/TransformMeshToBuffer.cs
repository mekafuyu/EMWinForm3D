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
    private void transformMeshToBuffer(Mesh m, Matrix4x4 rotationMatrix, (float x, float y, float z) translation, (float width, float height) size, List<Triangle> bufferList)
    {
      foreach (var tri in m.t)
      {
        if (tri is null)
          continue;

        var moddedTri = (Triangle)tri.Clone();

        moddedTri = TriangleMath
          .ScaledTriangleTransformation(moddedTri, rotationMatrix);

        moddedTri = TriangleMath
          .TranslateTriangle3D(moddedTri, (translation.x, translation.y, translation.z));

        var triProj = renderTriangle(
          moddedTri,
          this.LightDirection,
          this.VCamera,
          this.matProj,
          size
        );

        if (triProj is null)
          continue;
          
        bufferList.Add(triProj);
      }
    }
}