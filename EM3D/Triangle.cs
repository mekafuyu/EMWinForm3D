using System;

namespace EM3D;

public class Triangle : ICloneable
{
    public Vec3D[] P = new Vec3D[3];
    
    public Triangle () { }
    public Triangle (Vec3D[] p)
    {
        this.P = p;
    }

    public object Clone()
    {
        Triangle nt = (Triangle) new Vec3D[] { (Vec3D)this.P[0].Clone(), (Vec3D)this.P[1].Clone(), (Vec3D)this.P[2].Clone(), };
        return nt;
    }

    public static explicit operator Triangle(Vec3D[] vec)
    {
        if(vec.Length != 3)
            throw new Exception("KKKKKKK sla");

        Triangle output = new(){
            P = vec
        };
        return output;
    }
}
