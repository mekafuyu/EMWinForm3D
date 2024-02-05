using System.Collections.Generic;
using System.Drawing;
using EM3D;
using EM3D.EMUtils;

Game game = new();

// Mesh cube1 = EMFile.LoadObjectFile("./assets/models/longRoom.obj");
Mesh casa1 = EMFile.LoadObjectFile("./assets/models/mapa principal casa1.obj");
Mesh casa2 = EMFile.LoadObjectFile("./assets/models/mapa principal casa2.obj");
Mesh casa3 = EMFile.LoadObjectFile("./assets/models/mapa principal casa3.obj");
Mesh casa5 = EMFile.LoadObjectFile("./assets/models/mapa principal casa5.obj");
Mesh cerca = EMFile.LoadObjectFile("./assets/models/mapa principal cerca.obj");
Mesh chao = EMFile.LoadObjectFile("./assets/models/mapa principal chao.obj");
Mesh grama = EMFile.LoadObjectFile("./assets/models/mapa principal grama.obj");

// Mesh.Scale(cube1, 0.1f);
Mesh.Scale(casa1, 10f);
Mesh.Scale(casa2, 10f);
Mesh.Scale(casa3, 10f);
Mesh.Scale(casa5, 10f);
Mesh.Scale(cerca, 10f);
Mesh.Scale(chao, 10f);
Mesh.Scale(grama, 10f);

casa1.Color = new byte[] { 179, 125, 255 };
casa2.Color = new byte[] { 125, 229, 255 };
casa3.Color = new byte[] { 0, 0, 255 };
casa5.Color = new byte[] { 255, 0, 0 };
cerca.Color = new byte[] { 143, 62, 0 };
chao.Color = new byte[] { 135, 135, 135 };
grama.Color = new byte[] { 144, 255, 140 };
// List<Mesh> level1Mesh = new List<Mesh>{cube1};
List<Mesh> level1Mesh = new List<Mesh>{casa1, casa2, casa3, casa5, cerca, chao, grama};

Floor floor = new Floor(-64f * 0.1f, 0, -256f * 0.1f, 256 * 0.1f, 128f * 0.1f);
List<Entity> level1Entity = new List<Entity>{floor};


Bitmap bg1 = (Bitmap) Bitmap.FromFile("./assets/imgs/bggen.jpg");

Level level1 = new();
level1.Initialize(level1Entity, level1Mesh, bg1);


var lvl2Mesh = EMFile.LoadObjectFile("./assets/models/FirstMap2.obj");
Mesh.Scale(lvl2Mesh, 1.5f);
var lvl2Floor = new Floor(-1.5f, 1.5f, 0, -24, 3);
var lvl2Door = new Door(-1.5f, 1.5f, -14.5f, -2.5f, 3, false);
var lvl2Entities = new List<Entity> { lvl2Floor, lvl2Door };

float xCenter = (lvl2Floor.Hitbox.X + lvl2Floor.Hitbox.Width) / 2;
float zCenter = (lvl2Floor.Hitbox.Y - lvl2Floor.Hitbox.Height) / 2;
Level level2 = new();
level2.Initialize(lvl2Entities, new List<Mesh>{lvl2Mesh}, bg1);
level2.Amelia.Anchor3D = new(-xCenter, 1.5f, lvl2Floor.Hitbox.Height);
game.Levels.Add(level2);
game.Levels.Add(level1);
game.Run();