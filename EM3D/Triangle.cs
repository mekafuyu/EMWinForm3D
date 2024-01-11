using System;
using System.Numerics;

namespace EM3D;

public class Triangule : ICloneable
{
    public Vector3[] P = new Vector3[3];
    
    public Triangule () { }
    public Triangule (Vector3[] p)
    {
        this.P = p;
    }

    public object Clone()
    {
        Triangule nt = new Triangule(
            this.P
            // new Vector3[]{
            //     this.P[0],
            //     this.P[1],
            //     this.P[2]
            // }
            );
        return nt;
    }

    public static implicit operator Triangule(Vector3[] vec)
    {
        if(vec.Length != 3)
            throw new Exception("KKKKKKK sla");

        Triangule output = new(){
            P = vec
        };
        return output;
    }
}
