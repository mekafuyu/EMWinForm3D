using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

public class TriangleImageForm : Form
{
    private PictureBox pictureBox;

    public TriangleImageForm()
    {
        InitializeComponents();
    }

    private void InitializeComponents()
    {
        pictureBox = new PictureBox();
        pictureBox.Dock = DockStyle.Fill;
        pictureBox.Paint += PictureBox_Paint;

        Controls.Add(pictureBox);
    }

    private void PictureBox_Paint(object sender, PaintEventArgs e)
    {
        // Clear the previous drawings
        e.Graphics.Clear(Color.White);

        // Define the vertices of the triangle
        Point[] trianglePoints = { new Point(50, 200), new Point(150, 50), new Point(250, 200) };

        // Create a GraphicsPath from the triangle vertices
        GraphicsPath path = new GraphicsPath();
        path.AddPolygon(trianglePoints);

        // Set the clipping region to the triangle
        e.Graphics.SetClip(path);

        // Load an image to draw inside the triangle
        Image image = Image.FromFile("path_to_your_image.jpg");

        // Calculate the position and size for the image
        RectangleF imageBounds = path.GetBounds();
        float scale = Math.Min(imageBounds.Width / image.Width, imageBounds.Height / image.Height);
        PointF imageLocation = new PointF(imageBounds.X, imageBounds.Y);
        SizeF imageSize = new SizeF(image.Width * scale, image.Height * scale);

        // Draw the clipped image inside the triangle
        e.Graphics.DrawImage(image, new RectangleF(imageLocation, imageSize));

        // Reset the clipping region to the entire PictureBox
        e.Graphics.ResetClip();
    }

    [STAThread]
    static void Main()
    {
        Application.EnableVisualStyles();
        Application.SetCompatibleTextRenderingDefault(false);
        Application.Run(new TriangleImageForm());
    }
}
