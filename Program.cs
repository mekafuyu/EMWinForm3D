using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using EM3D;


float[] f = {1f, 2f, 3f};
Vec3D[] v = new Vec3D[]{(Vec3D) f};
Mesh cube = new(
    new Triangle[]{

        // SOUTH
        (Triangle) new Vec3D[] { new Vec3D(0f, 0f, 0f), new Vec3D(0f, 1f, 0f), new Vec3D(1f, 1f, 0f) },
        (Triangle) new Vec3D[] { new Vec3D(0f, 0f, 0f), new Vec3D(0f, 1f, 0f), new Vec3D(1f, 0f, 0f) },

        // EAST
        (Triangle) new Vec3D[] { new Vec3D(1f, 0f, 0f), new Vec3D(1f, 1f, 0f), new Vec3D(1f, 1f, 1f) },
        (Triangle) new Vec3D[] { new Vec3D(1f, 0f, 0f), new Vec3D(1f, 1f, 1f), new Vec3D(1f, 0f, 1f) },

        // NORTH
        (Triangle) new Vec3D[] { new Vec3D(1f, 0f, 1f), new Vec3D(1f, 1f, 1f), new Vec3D(0f, 1f, 1f) },
        (Triangle) new Vec3D[] { new Vec3D(1f, 0f, 1f), new Vec3D(0f, 1f, 1f), new Vec3D(0f, 0f, 1f) },

        // WEST
        (Triangle) new Vec3D[] { new Vec3D(0f, 0f, 1f), new Vec3D(0f, 1f, 1f), new Vec3D(0f, 1f, 0f) },
        (Triangle) new Vec3D[] { new Vec3D(0f, 0f, 1f), new Vec3D(0f, 1f, 0f), new Vec3D(0f, 0f, 0f) },

        // TOP
        (Triangle) new Vec3D[] { new Vec3D(0f, 1f, 0f), new Vec3D(0f, 1f, 1f), new Vec3D(1f, 1f, 1f) },
        (Triangle) new Vec3D[] { new Vec3D(0f, 1f, 0f), new Vec3D(1f, 1f, 1f), new Vec3D(1f, 1f, 0f) },

        // BOTTOM
        (Triangle) new Vec3D[] { new Vec3D(1f, 0f, 1f), new Vec3D(0f, 0f, 1f), new Vec3D(0f, 0f, 0f) },
        (Triangle) new Vec3D[] { new Vec3D(1f, 0f, 1f), new Vec3D(0f, 0f, 0f), new Vec3D(1f, 0f, 0f) },
    }
);

Bitmap bmp = null;
Graphics g = null;

PictureBox pb = new PictureBox{
    Dock = DockStyle.Fill
};

var timer = new Timer{
    Interval = 40
};

var form = new Form
{
    WindowState = FormWindowState.Maximized,
    Controls = { pb }
};
var eng = new EMEngine(form.Width, form.Height);

form.Load += (o, e) =>
{
    bmp = new Bitmap(
        pb.Width,
        pb.Height
    );
    g = Graphics.FromImage(bmp);
    pb.Image = bmp;
    timer.Start();
};


float theta = 0;
Mat4x4 matRotZ = new(), matRotX = new();

timer.Tick += (o, e) => {
    g.Clear(Color.White);

    matRotZ.m[0,0] = MathF.Cos(theta);
    matRotZ.m[0,1] = MathF.Sin(theta);
    matRotZ.m[1,0] = -MathF.Sin(theta);
    matRotZ.m[1,1] = MathF.Cos(theta);
    matRotZ.m[2,2] = 1;
    matRotZ.m[3,3] = 1;

    matRotX.m[0,0] = 1;
    matRotX.m[1,1] = MathF.Cos(theta * 0.5f);
    matRotX.m[1,2] = MathF.Sin(theta * 0.5f);
    matRotX.m[2,1] = -MathF.Sin(theta * 0.5f);
    matRotX.m[2,2] = MathF.Cos(theta * 0.5f);
    matRotX.m[3,3] = 1;

    foreach (var tri in cube.t)
    {
        var rotatedTri = EMEngine.RotateTriangle(tri, matRotZ);
        rotatedTri = EMEngine.RotateTriangle(rotatedTri, matRotX);
        var triProj = EMEngine.ProjectTriangle(rotatedTri, eng.matProj, (form.Width, form.Height));
        EMEngine.DrawTriangleWithGraphics(
            Pens.Blue,
            g,
            triProj
        );
    }
    
    theta += 0.05f;
    
    pb.Refresh();
};



Application.Run(form);