using System;

namespace EM3D;

public class Vec3D : ICloneable
{
    public float X, Y, Z;

    public Vec3D() { }
    public Vec3D(float x, float y, float z)
    {
        this.X = x;
        this.Y = y;
        this.Z = z;
    }

    public object Clone()
    {
        return new Vec3D(){X = this.X, Y = this.Y, Z = this.Z};
    }

    public static explicit operator Vec3D(float[] obj)
    {
        Vec3D output = new(
            obj[0],
            obj[1],
            obj[2]
        );
        return output;
    }
}
