using System.Collections.Generic;
using System.Numerics;

using static EM3D.EMUtils.EMGeometry;

namespace EM3D;

public partial class EMEngine
{
  private int transformMeshToBuffer(Mesh m, Matrix4x4 rotationMatrix, (float x, float y, float z) translation, (float width, float height) size, List<Triangle> bufferList)
  {
    int countTr = 0;
    foreach (var tri in m.t)
    {
      if (tri is null)
        continue;

      var moddedTri = (Triangle)tri.Clone();
      moddedTri.Color = m.Color;

      moddedTri = TriangleMath
        .ScaledTriangleTransformation(moddedTri, rotationMatrix);

      moddedTri = TriangleMath
        .TranslateTriangle3D(moddedTri, (translation.x, translation.y, translation.z));

      countTr += renderTriangle(
        moddedTri,
        size,
        bufferList
      );
    }
    return countTr;
  }
}