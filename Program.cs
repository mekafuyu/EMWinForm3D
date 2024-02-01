using System.Collections.Generic;
using System.Drawing;
using EM3D;
using EM3D.EMUtils;

Game game = new();

Mesh cube1 = EMFile.LoadObjectFile("./assets/models/longRoom.obj");
Mesh.Scale(cube1, 0.1f);
cube1.Color = new byte[] { 255, 255, 255 };
List<Mesh> level1Mesh = new List<Mesh>{cube1};

Floor floor = new Floor(-64f * 0.1f, 0, -256f * 0.1f, 256 * 0.1f, 128f * 0.1f);
List<Entity> level1Entity = new List<Entity>{floor};


Bitmap bg1 = (Bitmap) Bitmap.FromFile("./assets/imgs/bg.png");

Level level1 = new();
level1.Initialize(level1Entity, level1Mesh, bg1);
game.Levels.Add(level1);
game.Run();