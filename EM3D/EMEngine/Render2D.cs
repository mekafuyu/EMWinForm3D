using System.Collections.Generic;
using System.Drawing;
using System.Numerics;

using static EM3D.EMUtils.EMGeometry;

namespace EM3D;

public partial class EMEngine
{
    private (Vertex, float) renderPoint(
        Vertex p,
        (float width, float height) size
      )
    {
        var pClone = p;
        // Move farther
        pClone.Z += 8;

        // Convert World Space to view Space
        pClone = Vector4.Transform(pClone.V4, this.VirtualCamera.ViewMatrix);

        // The point to test is zNear, the plane is the Z axis, 
        float distance = TriangleMath.distancePointPlane(pClone.V3, new(0f, 0f, this.fNear), new(0f, 0f, 1f));
        if (distance <= 0)
            return (p, 0);

        pClone.V4 = Vector4.Transform(pClone.V4, this.VirtualCamera.ProjectionMatrix);
        pClone.V3 /= pClone.W;

        // Scale triangle
        pClone.X += 1f;
        pClone.Y += 1f;
        pClone.X *= 0.5f * size.width;
        pClone.Y *= 0.5f * size.height;

        // trProjected.lightIntensity =
        // trianglesToRasterBuffer.Add(trProjected);      

        return (pClone, distance);
    }

    public static void RenderAmelia(Amelia amelia, Graphics g)
    {
        amelia.Draw(g);
    }
}
