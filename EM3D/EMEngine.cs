using System;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace EM3D;

public class EMEngine
{
    public float fNear = 0.1f;
    public float fFar = 1000.0f;
    public float fFov = 90.0f;
    public float fAspectRatio = 1.333333f;
    public float fFovRad;
    public Mat4x4 matProj;
    public Vec3D VCamera = new(){ X = 0, Y = 0, Z = 0};
    public Vec3D LightDirection = new(){ X = 0, Y = 0, Z = -1f};
    public Vec3D NLightDirection;


    public EMEngine()
    {
        setFovAndMatrix();

        float length = MathF.Sqrt(LightDirection.X * LightDirection.X + LightDirection.Y * LightDirection.Y + LightDirection.Z * LightDirection.Z);
        LightDirection.X /= length;
        LightDirection.Y /= length;
        LightDirection.Z /= length;
    }

    public EMEngine(float height, float width)
    {
        this.fAspectRatio = height / width;
        setFovAndMatrix();

        float length = MathF.Sqrt(LightDirection.X * LightDirection.X + LightDirection.Y * LightDirection.Y + LightDirection.Z * LightDirection.Z);
        LightDirection.X /= length;
        LightDirection.Y /= length;
        LightDirection.Z /= length;
    }

    public static Vec3D getVectorFromVec3D(Vec3D p1, Vec3D p2)
    {
        return new(){
            X = p1.X - p2.X,
            Y = p1.Y - p2.Y,
            Z = p1.Z - p2.Z
        };
    }
    private void setFovAndMatrix()
    {
        fFovRad =  1f / MathF.Tan(this.fFov * 0.5f / 180f * MathF.PI);
        this.matProj = new();
        matProj.m[0,0] = this.fAspectRatio * this.fFovRad;
        matProj.m[1,1] = this.fFovRad;
        matProj.m[2,2] = this.fFar / (this.fFar - this.fNear);
        matProj.m[3,2] = (-this.fFar * this.fNear) / (this.fFar - this.fNear);
        matProj.m[2,3] = 1f;
        matProj.m[3,3] = 0f;
    }

    public static Vec3D FindNormal(Vec3D l1, Vec3D l2)
    {
        Vec3D res = new(){
            X = l1.Y * l2.Z - l1.Z * l2.Y,
            Y = l1.Z * l2.X - l1.X * l2.Z,
            Z = l1.X * l2.Y - l1.Y * l2.X
        };

        return res;
    }

    public static Vec3D MultiplyMatrixVector(Vec3D i, Mat4x4 m)
    {
        Vec3D res = new();
        res.X = i.X * m.m[0,0] + i.Y * m.m[1,0] + i.Z * m.m[2,0] + m.m[3,0];
        res.Y = i.X * m.m[0,1] + i.Y * m.m[1,1] + i.Z * m.m[2,1] + m.m[3,1];
        res.Z = i.X * m.m[0,2] + i.Y * m.m[1,2] + i.Z * m.m[2,2] + m.m[3,2];
        float w = i.X * m.m[0,3] + i.Y * m.m[1,3] + i.Z * m.m[2,3] + m.m[3,3];

        if(w != 0.0f)
        {
            res.X /= w;
            res.Y /= w;
            res.Z /= w;
        }

        return res;
    }

    public static Triangule RotateTriangle(Triangule tr, Mat4x4 rotationMatrix)
    {
        Triangule rotatedTri = new();
        rotatedTri.P[0] = MultiplyMatrixVector(tr.P[0], rotationMatrix);
        rotatedTri.P[1] = MultiplyMatrixVector(tr.P[1], rotationMatrix);
        rotatedTri.P[2] = MultiplyMatrixVector(tr.P[2], rotationMatrix);
        
        return rotatedTri;
    }
    public static (Triangule? t, float lightInt) ProjectTriangle(Triangule tr, Vec3D light, Vec3D camera, Mat4x4 m, (float width, float height) size)
    {
        Triangule trTranslated = (Triangule) tr.Clone();
        trTranslated.P[0].Z = tr.P[0].Z + 3f;
        trTranslated.P[1].Z = tr.P[1].Z + 3f;
        trTranslated.P[2].Z = tr.P[2].Z + 3f;

        Vec3D l1 = getVectorFromVec3D(trTranslated.P[1], trTranslated.P[0]);
        Vec3D l2 = getVectorFromVec3D(trTranslated.P[2], trTranslated.P[0]);
        Vec3D normal = FindNormal(l1, l2);
        float length = MathF.Sqrt(normal.X * normal.X + normal.Y * normal.Y + normal.Z * normal.Z);
        normal.X /= length; 
        normal.Y /= length; 
        normal.Z /= length; 

        if (
            normal.X * ( trTranslated.P[0].X - camera.X ) +
            normal.Y * ( trTranslated.P[0].Y - camera.Y ) +
            normal.Z * ( trTranslated.P[0].Z - camera.Z ) > 0)
        // if(normal.Z > 0)
            return (null, 0);

        float dp = normal.X * light.X + normal.Y * light.Y + normal.Z * light.Z;

        Triangule trProjected = new();

        trProjected.P[0] = MultiplyMatrixVector(trTranslated.P[0], m);
        trProjected.P[1] = MultiplyMatrixVector(trTranslated.P[1], m);
        trProjected.P[2] = MultiplyMatrixVector(trTranslated.P[2], m);

        trProjected.P[0].X += 1f; trProjected.P[0].Y += 1f;
        trProjected.P[1].X += 1f; trProjected.P[1].Y += 1f;
        trProjected.P[2].X += 1f; trProjected.P[2].Y += 1f;

        trProjected.P[0].X *= 0.5f * size.width;
        trProjected.P[0].Y *= 0.5f * size.height;
        trProjected.P[1].X *= 0.5f * size.width;
        trProjected.P[1].Y *= 0.5f * size.height;
        trProjected.P[2].X *= 0.5f * size.width;
        trProjected.P[2].Y *= 0.5f * size.height;

        return (trProjected, dp);
    }

    public static void FillTriangleWithGraphics(Brush b, Graphics g, Triangule tr)
    {
        var path = new GraphicsPath();
        PointF[] pts =  new PointF[]{
            new(tr.P[0].X, tr.P[0].Y),
            new(tr.P[1].X, tr.P[1].Y),
            new(tr.P[2].X, tr.P[2].Y),
        };

        path.AddLines(pts);
        path.CloseFigure();

        g.FillPath(b, path);
    }
    public static void DrawTriangleWithGraphics(Pen p, Graphics g, Triangule tr)
    {
        var path = new GraphicsPath();
        PointF[] pts =  new PointF[]{
            new(tr.P[0].X, tr.P[0].Y),
            new(tr.P[1].X, tr.P[1].Y),
            new(tr.P[2].X, tr.P[2].Y),
        };

        path.AddLines(pts);
        path.CloseFigure();

        g.DrawPath(p, path);
    }

    
}