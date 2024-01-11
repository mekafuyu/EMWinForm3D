using System;

namespace EM3D;

public class Triangule : ICloneable
{
    public Vec3D[] P = new Vec3D[3];
    
    public Triangule () { }
    public Triangule (Vec3D[] p)
    {
        this.P = p;
    }

    public object Clone()
    {
        Triangule nt = new Vec3D[] { (Vec3D)this.P[0].Clone(), (Vec3D)this.P[1].Clone(), (Vec3D)this.P[2].Clone(), };
        return nt;
    }

    public static implicit operator Triangule(Vec3D[] vec)
    {
        if(vec.Length != 3)
            throw new Exception("KKKKKKK sla");

        Triangule output = new(){
            P = vec
        };
        return output;
    }
}
