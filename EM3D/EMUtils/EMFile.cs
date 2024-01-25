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
    // var (vCount, tCount) = CountVectorsAndTrianglesInObjFile(filepath);
    // Vector3[] vertexes = new Vector3[vCount];
    // Triangle[] triangles = new Triangle[tCount];
    List<Vertex> vertexes = new();
    List<Triangle> trList = new();
    trList.ToArray();

    using(FileStream fs = File.Open(filepath, FileMode.Open))
    {
      byte[] buffer = new byte[1024 * 1024 * 10];
      int countv = 0;
      int countf = 0;
      int countl = 0;

      UTF8Encoding temp = new UTF8Encoding(true);

      while (fs.Read(buffer, 0, buffer.Length) > 0) {
        var tempstr = temp.GetString(buffer);
        var lines = tempstr.Split('\n');
        foreach (var line in lines)
        {

          if(line[0] == 'v' && line[1] == ' ')
          {
            var vItems = line.Split(' ');
            vertexes.Add(new(){
              X = float.Parse(vItems[1].Replace('.',',')),
              Y = float.Parse(vItems[2].Replace('.',',')),
              Z = float.Parse(vItems[3].Replace('.',',')),
              // X = float.Parse(vItems[1], CultureInfo.InvariantCulture.NumberFormat),
              // Y = float.Parse(vItems[2], CultureInfo.InvariantCulture.NumberFormat),
              // Z = float.Parse(vItems[3], CultureInfo.InvariantCulture.NumberFormat),
            });
            countv++;
            continue;
          }

          if(line[0] == 'f')
          {
            var tItems = line.Split(' ');

            var vertexes1 = tItems[1].Split('/');
            var vertexes2 = tItems[2].Split('/');
            var vertexes3 = tItems[3].Split('/');

            trList.Add(
              new Triangle((
                vertexes[int.Parse(vertexes1[0]) - 1],
                vertexes[int.Parse(vertexes2[0]) - 1],
                vertexes[int.Parse(vertexes3[0]) - 1]
              ))
            );

            if(tItems.Length > 4)
            {
              var vertexes4 = tItems[4].Split('/');
              trList.Add(
                new Triangle((
                  vertexes[int.Parse(vertexes1[0]) - 1],
                  vertexes[int.Parse(vertexes3[0]) - 1],
                  vertexes[int.Parse(vertexes4[0]) - 1]
                ))
              );
            }

            // triangles[countf] = new Triangle((
            //   vertexes[int.Parse(vertexes1[0]) - 1],
            //   vertexes[int.Parse(vertexes2[1]) - 1],
            //   vertexes[int.Parse(vertexes3[2]) - 1]
            // ));
            countf++;
          }
          countl++;
        }
      }
    }
    return new(trList.ToArray());
  }
}