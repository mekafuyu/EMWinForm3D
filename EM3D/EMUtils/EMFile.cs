using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Numerics;
using System.Text;

namespace EM3D.EMUtils;

public static class EMFile
{
  public static (int vecCount, int faceCount) CountVectorsAndTrianglesInObjFile(string filepath)
  {
    int vecCount = 0;
    int faceCount = 0;
    long lineCount = 0;
    using (FileStream fs = new FileStream(filepath, FileMode.Open, FileAccess.Read, FileShare.None, 1024 * 1024))
    {
      byte[] buffer = new byte[1024 * 1024];
      int bytesRead;
      bool isComment = false;

      do
      {
        bytesRead = fs.Read(buffer, 0, buffer.Length);
        for (int i = 0; i < bytesRead; i++)
        {

          if (buffer[i] == '\n')
          {
            lineCount++;
            isComment = false;
            continue;
          };

          if (isComment)
            continue;

          if (buffer[i] == '#')
          {
            isComment = true;
            continue;
          }

          if (buffer[i] == 'v' && buffer[i + 1] == ' ')
            vecCount++;
          if (buffer[i] == 'f' && buffer[i + 1] == ' ')
            faceCount++;
        }
      }
      while (bytesRead > 0);
    }
    return (vecCount, faceCount);
  }

  public static Mesh LoadObjectFile(string filepath)
  {
    List<Vertex> vertexes = new();
    List<Vector2> vertexes2D = new();
    List<Triangle> trList = new();
    trList.ToArray();

    using (FileStream fs = File.Open(filepath, FileMode.Open))
    {
      byte[] buffer = new byte[1024 * 1024 * 10];
      int countv = 0;
      int countvt = 0;
      int countf = 0;
      int countl = 0;

      UTF8Encoding temp = new UTF8Encoding(true);

      while (fs.Read(buffer, 0, buffer.Length) > 0)
      {
        var tempstr = temp.GetString(buffer);
        var lines = tempstr.Split('\n');
        foreach (var line in lines)
        {
          if (string.IsNullOrEmpty(line))
            continue;
          if (line[0] == 'v' && line[1] == ' ')
          {
            var vItems = line.Split(' ');
            vertexes.Add(new()
            {
              X = float.Parse(vItems[1].Replace('.', ',')),
              Y = float.Parse(vItems[2].Replace('.', ',')),
              Z = float.Parse(vItems[3].Replace('.', ','))
            });
            countv++;
            continue;
          }

          if (line[0] == 'v' && line[1] == 't')
          {
            var v2Items = line.Split(' ');
            vertexes2D.Add(new()
            {
              X = float.Parse(v2Items[1].Replace('.', ',')),
              Y = float.Parse(v2Items[2].Replace('.', ',')),
            });
            countvt++;
            continue;
          }

          if (line[0] == 'f')
          {
            var tItems = line.Split(' ');

            var vertexes1 = tItems[1].Split('/');
            var vertexes2 = tItems[2].Split('/');
            var vertexes3 = tItems[3].Split('/');

            var nt1 = new Triangle((
              vertexes[int.Parse(vertexes1[0]) - 1],
              vertexes[int.Parse(vertexes2[0]) - 1],
              vertexes[int.Parse(vertexes3[0]) - 1]
            ));

            if(vertexes1.Length > 1)
              nt1.T = (
                vertexes2D[int.Parse(vertexes1[1]) - 1],
                vertexes2D[int.Parse(vertexes2[1]) - 1],
                vertexes2D[int.Parse(vertexes3[1]) - 1]
              );

            trList.Add(nt1);
            countf++;

            if (tItems.Length < 5)
              continue;
            
            var vertexes4 = tItems[4].Split('/');

            var nt2 = new Triangle((
              vertexes[int.Parse(vertexes1[0]) - 1],
              vertexes[int.Parse(vertexes3[0]) - 1],
              vertexes[int.Parse(vertexes4[0]) - 1]
              ));

            if (vertexes4.Length > 1)
              nt2.T = ((
                vertexes2D[int.Parse(vertexes1[1]) - 1],
                vertexes2D[int.Parse(vertexes3[1]) - 1],
                vertexes2D[int.Parse(vertexes4[1]) - 1]
              ));
              
            trList.Add(nt2);
            countf++;
          }
          countl++;
        }
      }
    }
    return new(trList.ToArray());
  }
}