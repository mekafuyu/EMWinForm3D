using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Numerics;
using System.Windows.Forms;

using FastEM3D.EMUtils;
using static FastEM3D.EMUtils.EMGeometry;

namespace FastEM3D;

public class FastEMEngineOld
{
  public float fNear = 0.1f;
  public float fFar = 1000.0f;
  public float fFov;
  public float fAspectRatio;
  public float fFovRad;
  public Matrix4x4 matProj;
  public Vector3 VCamera =
      new()
      {
        X = 0,
        Y = 0,
        Z = 0
      };
  public Vector3 LightDirection =
      new()
      {
        X = 0,
        Y = 0,
        Z = -1f
      };
  public Vector3 NLightDirection;
  private DateTime lastCheckTime;

  public FastEMEngineOld()
  {
    this.fAspectRatio = 1.333333f;
    this.SetFov(90);
    this.SetProjectionMatrix();
    NLightDirection = Vector3.Normalize(LightDirection);
  }

  public FastEMEngineOld(float height, float width)
  {
    this.fAspectRatio = height / width;
    this.SetFov(90);
    this.SetProjectionMatrix();
    NLightDirection = Vector3.Normalize(LightDirection);
  }

  public void SetFov(float angle)
  {
    this.fFov = angle;
    this.fFovRad = 1f / MathF.Tan(this.fFov * 0.5f / 180f * MathF.PI);
  }
  public void SetProjectionMatrix()
  {
    // matProj = new(
    //   this.fAspectRatio * this.fFovRad, 0, 0, 0,
    //   0, this.fFovRad, 0, 0,
    //   0, 0, this.fFar / (this.fFar - this.fNear), 1f,
    //   0, 0, (-this.fFar * this.fNear) / (this.fFar - this.fNear), 0
    // );
    matProj = new();
    matProj[0, 0] = this.fAspectRatio * this.fFovRad;
    matProj[1, 1] = this.fFovRad;
    matProj[2, 2] = this.fFar / (this.fFar - this.fNear);
    matProj[3, 2] = (-this.fFar * this.fNear) / (this.fFar - this.fNear);
    matProj[2, 3] = 1f;
    matProj[3, 3] = 0f;
  }

  
  public void Renderer(
    PictureBox pb,
    Form form,
    Graphics g,
    IEnumerable< Mesh > meshesToRender,
    (float x, float y, float z) rotation,
    (float x, float y, float z) translation
    )
  {
      int totalTriangles = 0;
      int[] rgb = new int[] { 128, 128, 255 };

      var rotationMatrix =  MatrixMath.GetRotationMatrix(rotation.x, rotation.y, rotation.z);

      List<Triangle> trianglesToRaster = new();
      foreach (var mesh in meshesToRender)
      {
        foreach (var meshTri in mesh.t)
        {
          if (meshTri is null)
            continue;
          var moddedTri = (Triangle)meshTri.Clone();
          moddedTri = TriangleMath.ScaledTriangleTransformation(moddedTri, rotationMatrix);
          moddedTri = TriangleMath.TranslateTriangle3D(moddedTri, (translation.x, translation.y, translation.z));

          var triProj = renderTriangle(
            moddedTri,
            this.LightDirection,
            this.VCamera,
            this.matProj,
            (form.Width, form.Height)
          );
          if (triProj is null)
            continue;
          trianglesToRaster.Add(triProj);
        }
        totalTriangles += mesh.t.Count();
      }
      trianglesToRaster = trianglesToRaster.OrderBy(t => t.ZPos).ToList();

      g.Clear(Color.Black);
      foreach (var triangle in trianglesToRaster)
      {
        RasterTriangle(triangle, g, rgb, true, false);
      }

      g.DrawString("EM3D v0.0.8", SystemFonts.DefaultFont, Brushes.White, new PointF(0f, 0f));
      g.DrawString("FPS: " + fpsCalculator().ToString(), SystemFonts.DefaultFont, Brushes.White, new PointF(0f, 10f));
      g.DrawString("Triangles: " + trianglesToRaster.Count.ToString() + "/" + totalTriangles, SystemFonts.DefaultFont, Brushes.White, new PointF(0f, 20f));
      pb.Refresh();
    }

    public int fpsCalculator()
    {
      double secondsElapsed = (DateTime.Now - lastCheckTime).TotalSeconds;
      lastCheckTime = DateTime.Now;
      return (int)(1 / secondsElapsed);
    }

    private Triangle renderTriangle(
      Triangle tr,
      Vector3 light,
      Vector3 camera,
      Matrix4x4 m,
      (float width, float height) size
    )
    {
      // Move farther
      Triangle trTranslated = (Triangle)tr.Clone();
      trTranslated = TriangleMath.TranslateTriangle3D(trTranslated, (0f, 0f, 10f));

      // Find normal vector and normalize
      Vector3 normal = VectorMath.FindNormal(trTranslated);
      normal = Vector3.Normalize(normal);

      // Test if needs to project triangle by similarity
      if (Vector3.Dot(normal, trTranslated.P.l1.V3 - camera) > 0)
        return null;

      // Project Triangle
      Triangle trProjected = TriangleMath
        .ScaledTriangleTransformation(trTranslated, m);

      // Scale triangle
      TriangleMath.ScaleTriangle(trProjected, size.width, size.height);

      // Calculate light intensity
      float dp = Vector3.Dot(normal, light);
      if (dp < 0)
        dp = 0;
      trProjected.lightIntensity = dp;

      return trProjected;
    }
    private void RasterTriangle(Triangle tr, Graphics g,int[] rgb, bool fillPoly, bool drawPolly)
    {
      Pen p = new Pen(Color.FromArgb(255, 0, 0, 0), 2);
      SolidBrush b =
        new(
          Color.FromArgb(
            (int)(rgb[0] * tr.lightIntensity),
            (int)(rgb[1] * tr.lightIntensity),
            (int)(rgb[2] * tr.lightIntensity)
          )
        );

      var trPoints = Utils.TriangleToPointFs(tr);

      if(fillPoly)
        Drawing.FillTriangleWithGraphics(b, g, trPoints);
      if(drawPolly)
        Drawing.DrawTriangleWithGraphics(p, g, trPoints);
    }
  }
