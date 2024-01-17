using System.Globalization;
using System.IO;
using System.Numerics;
using System.Text;

namespace FastEM3D.EMUtils;

public static class EMFile
{
public static (int vecCount, int faceCount) CountVectorsAndTrianglesInObjFile(string filepath)
  {
    int vecCount = 0;
    int faceCount = 0;
    long lineCount = 0;
    using(FileStream fs = new FileStream(filepath, FileMode.Open, FileAccess.Read, FileShare.None, 1024 * 1024))
    {
      byte[] buffer = new byte[1024 * 1024];
      int bytesRead;
      bool isComment = false;

      do
      {
        bytesRead = fs.Read(buffer, 0, buffer.Length);
        for (int i = 0; i < bytesRead; i++){
          
          if (buffer[i] == '\n'){
            lineCount++;
            isComment = false;
            continue;
          };

          if (isComment)
            continue;

          if (buffer[i] == '#'){
            isComment = true;
            continue;
          }

          if (buffer[i] == 'v')
            vecCount++;
          if (buffer[i] == 'f')
            faceCount++;
        }
      }
      while (bytesRead > 0);
    }

    return (vecCount, faceCount);
  }

  public static Mesh LoadObjectFile(string filepath)
  {
    var (vCount, tCount) = CountVectorsAndTrianglesInObjFile(filepath);
    Vector3[] vertexes = new Vector3[vCount];
    Triangle[] triangles = new Triangle[tCount];

    using(FileStream fs = File.Open(filepath, FileMode.Open))
    {
      byte[] buffer = new byte[1024 * 1024];
      int countv = 0;
      int countf = 0;

      UTF8Encoding temp = new UTF8Encoding(true);

      while (fs.Read(buffer, 0, buffer.Length) > 0) {
        var tempstr = temp.GetString(buffer);
        var lines = tempstr.Split('\n');
        foreach (var line in lines)
        {
          if(line[0] == 'v')
          {
            var vItems = line.Split(' ');
            vertexes[countv] = new(){
              X = float.Parse(vItems[1], CultureInfo.InvariantCulture.NumberFormat),
              Y = float.Parse(vItems[2], CultureInfo.InvariantCulture.NumberFormat),
              Z = float.Parse(vItems[3], CultureInfo.InvariantCulture.NumberFormat),
            };
            countv++;
            continue;
          }

          if(line[0] == 'f')
          {
            var tItems = line.Split(' ');
            triangles[countf] = new Triangle((
              vertexes[int.Parse(tItems[1]) - 1],
              vertexes[int.Parse(tItems[2]) - 1],
              vertexes[int.Parse(tItems[3]) - 1]
            ));
            countf++;
          }
        }
      }
    }
    return new(triangles);
  }
}