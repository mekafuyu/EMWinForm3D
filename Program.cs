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

//labirinto

Mesh labirinto = EMFile.LoadObjectFile("./assets/models/labirinto.obj");

//puzzle portas
Mesh portas = EMFile.LoadObjectFile("./assets/models/portas puzzle_portas.obj");
Mesh chaoPortas = EMFile.LoadObjectFile("./assets/models/portas puzzle_chao.obj");
Mesh objetivoPortas = EMFile.LoadObjectFile("./assets/models/portas puzzle_objetivo.obj");
// Mesh.Scale(cube1, 0.1f);
Mesh.Scale(casa1, 10f);
Mesh.Scale(casa2, 10f);
Mesh.Scale(casa3, 10f);
Mesh.Scale(casa5, 10f);
Mesh.Scale(cerca, 10f);
Mesh.Scale(chao, 10f);
Mesh.Scale(grama, 10f);
Mesh.Scale(portas, 10f);
Mesh.Scale(chaoPortas, 10f);
Mesh.Scale(objetivoPortas, 10f);
Mesh.Scale(labirinto, 2.1f);

casa1.Color = new byte[] { 179, 125, 255 };
casa2.Color = new byte[] { 125, 229, 255 };
casa3.Color = new byte[] { 0, 0, 255 };
casa5.Color = new byte[] { 255, 0, 0 };
cerca.Color = new byte[] { 143, 62, 0 };
chao.Color = new byte[] { 135, 135, 135 };
grama.Color = new byte[] { 144, 255, 140 };
//labirinto 
labirinto.Color = new byte[] {255, 255, 255};
//portas
portas.Color = new byte[] { 255, 234, 171};
chaoPortas.Color = new byte[] { 214, 255, 251};
objetivoPortas.Color = new byte[] { 255, 0, 200};

// List<Mesh> level1Mesh = new List<Mesh>{cube1};

List<Mesh> level1Mesh = new List<Mesh>{casa1, casa2, casa3, casa5, cerca, chao, grama};

Floor floor = new Floor(-64f * 0.1f, 0, -256f * 0.1f, 256 * 0.1f, 128f * 0.1f);
List<Entity> level1Entity = new List<Entity>{floor};


Bitmap bg1 = (Bitmap) Bitmap.FromFile("./assets/imgs/bggen.jpg");
Bitmap bg2 = (Bitmap) Bitmap.FromFile("./assets/imgs/bg.png");

Level level1 = new();
level1.Initialize(level1Entity, level1Mesh, bg1);


var lvl2Mesh = EMFile.LoadObjectFile("./assets/models/blender/FirstMapBetter3.obj");
Mesh.Scale(lvl2Mesh, 1.5f);
var lvl2Floor1 = new Floor(-1.5f, 1.5f, 12, -24, 3);
var lvl2Floor2 = new Floor(-5.5938f * 1.5f, 13.909f * 1.5f, -9.9293f * 1.5f, 1.5f * 1.5f, -2 * 1.5f);
var lvl2Persp = new PerspectiveObstacle(-1.5f, 1.5f, -2.5f, -2.5f, 3, new(-68.1943f, 44.6857f, -92.364105f));
// -0.6 12.903 0.9305  -0.6 12.903 -0.6
var lvl2Portal1 = new Portal(-5.5938f * 1.5f, 13.909f * 1.5f, -9.9293f * 1.5f, 1.5f * 1.5f, -1 * 1.5f, true);
var lvl2Portal2 = new Portal(-1.5f, 1.5f, 12, -2f, 3, true);
lvl2Portal1.destiny = lvl2Portal2;
lvl2Portal2.destiny = lvl2Portal1;

var lvl2Entities = new List<Entity> { lvl2Floor1, lvl2Floor2, lvl2Persp, lvl2Portal1, lvl2Portal2 };

float xCenter = (lvl2Floor1.Hitbox.X + lvl2Floor1.Hitbox.Width) / 2;
float zCenter = (lvl2Floor1.Hitbox.Y - lvl2Floor1.Hitbox.Height) / 2;
Level level2 = new();
level2.Initialize(new(-68.1943f, 44.6857f, -92.364105f),-0.6359842f, -0.24f, lvl2Entities, new List<Mesh>{lvl2Mesh}, bg1);
level2.Amelia.Anchor3D = new(-xCenter, 1.5f, lvl2Floor1.Hitbox.Height + lvl2Floor1.Hitbox.Y);

var lvl3Floor1 = new Floor(37.5f, 0, 46.5f, -88, 0.5f);
var lvl3Floor2 = new Floor(36.5f, 0, 40.5f, 0.5f, -80);
var lvl3Entities = new List<Entity> { lvl3Floor1, lvl3Floor2 };
Level level3 = new();
List<Mesh> level3Mesh = new List<Mesh>{labirinto};
level3.Initialize(new(-122.6943f, 113.1857f, -122.3641f), -0.805f, -0.6668f, lvl3Entities, level3Mesh, bg2);
xCenter = lvl3Floor1.Hitbox.X + lvl3Floor1.Hitbox.Width / 2;
zCenter = lvl3Floor1.Hitbox.Y + lvl3Floor1.Hitbox.Height;
level3.Amelia.Anchor3D = new(xCenter, 0, zCenter);

var lvl4Floor1 = new Floor(-0.8f, -0f, -0.35f, 1, 3);
var lvl4Entities = new List<Entity> { lvl4Floor1 };
Level level4 = new();
List<Mesh> level4Mesh = new List<Mesh>{chaoPortas, portas, objetivoPortas};
level4.Initialize(new(-122.0044f, 34.5814f, 9.1700f), -1.5789f, -0.2279f, lvl4Entities, level4Mesh, bg2);

game.Levels.Add(level4);
game.Levels.Add(level3);
game.Run();