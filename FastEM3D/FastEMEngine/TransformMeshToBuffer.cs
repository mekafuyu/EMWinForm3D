using System.Collections.Generic;
using System.Numerics;

using static FastEM3D.EMUtils.EMGeometry;

namespace FastEM3D;

public partial class FastEMEngine
{
  private int transformMeshToBuffer(Mesh m, Matrix4x4 rotationMatrix, (float x, float y, float z) translation, (float width, float height) size, List<Triangle> bufferList)
  {
    int countTr = 0;
    foreach (var tri in m.t)
    {
      if (tri is null)
        continue;

      var moddedTri = (Triangle)tri.Clone();

      moddedTri = TriangleMath
        .ScaledTriangleTransformation(moddedTri, rotationMatrix);

      moddedTri = TriangleMath
        .TranslateTriangle3D(moddedTri, (translation.x, translation.y, translation.z));

      countTr += renderTriangle(
        moddedTri,
        size,
        bufferList
      );

      // if (triProj is null)
      //   continue;

      // bufferList.Add(triProj);
    }
    return countTr;
  }
}