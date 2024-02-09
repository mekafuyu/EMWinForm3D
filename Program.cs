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


var lvl2Mesh = EMFile.LoadObjectFile("./assets/models/FirstMapBetter4.obj");
Mesh.Scale(lvl2Mesh, 1.5f);
var lvl2Floor1 = new Floor(-1.5f, 1.5f, 12, -42, 3);

var lvl2Floor2 = new Floor(-5.5938f * 1.5f, 13.909f * 1.5f, -13.929f * 1.5f, 1.5f * 1.5f, -6.2f * 1.5f);
var lvl2Persp1 = new PerspectiveObstacle(-1.5f, 1.5f, -8.5f, -2.5f, 3, new(-79.70122f, 52.03208f, -111.0604f), false);

var lvl2Portal1 = new PerspectivePortal((-5.5938f + -6.1f) * 1.5f, 13.909f * 1.5f, -13.929f * 1.5f, 1.5f * 1.5f, -0.1f * 1.5f, new(-79.70122f, 52.03208f, -111.0604f), true);
// var lvl2Persp3 = new PerspectiveObstacle((-5.5938f + -5.9f) * 1.5f, 13.909f * 1.5f, -13.929f * 1.5f, 1.5f * 1.5f, -0.1f * 1.5f, new(-79.70122f, 52.03208f, -111.0604f), true);

var lvl2Portal3 = new PerspectivePortal((-5.5938f + -3.1f) * 1.5f, 13.909f * 1.5f, -13.929f * 1.5f, 1.5f * 1.5f, -0.1f * 1.5f, new(-79.70122f, 52.03208f, -111.0604f), true);

var lvl2Portal4 = new PerspectivePortal(-1 * 1.5f, 21 * 1.5f, -1 * 1.5f, -1.5f * 1.5f, 0.1f * 1.5f, new(-79.70122f, 52.03208f, -111.0604f), true);
// var lvl2Persp6 = new PerspectiveObstacle(-1 * 1.5f, 21 * 1.5f, -1 * 1.5f, -1.5f * 1.5f, 0.3f * 1.5f, new(-79.70122f, 52.03208f, -111.0604f), true);

// var lvl2Persp4 = new PerspectiveObstacle((-5.5938f + -2.9f) * 1.5f, 13.909f * 1.5f, -13.929f * 1.5f, 1.5f * 1.5f, -0.1f * 1.5f, new(-79.70122f, 52.03208f, -111.0604f), true);
var lvl2Persp5 = new PerspectiveObstacle((-5.5938f + -3.2f) * 1.5f, 13.909f * 1.5f, -13.929f * 1.5f, 1.5f * 1.5f, -1f * 1.5f, new(-79.70122f, 52.03208f, -111.0604f), false);

var lvl2Portal2 = new PerspectivePortal(-1.5f, 1.5f, 12, -2f, 3, new(-79.70122f, 52.03208f, -111.0604f), true);
// var lvl2Persp2 = new PerspectiveObstacle(-1.5f, 1.5f, 11, -2f, 3, new(-79.70122f, 52.03208f, -111.0604f), true);
lvl2Portal1.destiny = lvl2Portal2;
lvl2Portal2.destiny = lvl2Portal1;
lvl2Portal3.destiny = lvl2Portal4;
lvl2Portal4.destiny = lvl2Portal3;

var lvl2Book1 = new Book(-1, 1.5f, -5, 0.4f, 1f, 1f);
var lvl2Book2 = new Book(-8, 20.5f, -18.929f , 0.4f, 1f, 1f);
var lvl2Book3 = new Book(0, 31.6f, -6.929f , 0.4f, 1f, 1f);

var lvl2Entities = new List<Entity> {
    lvl2Floor1,
    lvl2Floor2,
    lvl2Persp1,
    // lvl2Persp2,
    // lvl2Persp3,
    // lvl2Persp4,
    lvl2Persp5,
    // lvl2Persp6,
    lvl2Portal1,
    lvl2Portal2,
    lvl2Portal3,
    lvl2Portal4,
    lvl2Book1,
    lvl2Book2,
    lvl2Book3};

float xCenter = (lvl2Floor1.Hitbox.X + lvl2Floor1.Hitbox.Width) / 2;
float zCenter = (lvl2Floor1.Hitbox.Y - lvl2Floor1.Hitbox.Height) / 2;
Level level2 = new();
level2.Initialize(new(-79.70122f, 52.03208f, -111.0604f),-0.6224661f, -0.24f, lvl2Entities, new List<Mesh>{lvl2Mesh}, bg1);
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

var lvl4Floor1 = new Floor(-1.4f, -0f, -0.35f, 1, 3);
var lvl4Floor2 = new Floor(-11.5f, -6f, 1.2f, 1, 3);
var lvl4Floor3 = new Floor(-12f, -8.9f, -9.2f, 0.6F, 2);
var lvl4Floor4 = new Floor(-12f, 2.3f, -5.2f, 0.6F, 2);
var lvl4Floor5 = new Floor(-8.5f, 6.6f, -13.5f, 0.6F, 2);
var lvl4Floor6 = new Floor(9, 6.6f, 1.2f, 0.6F, 2);
var lvl4Floor7 = new Floor(4, 22.6f, -2.3f, 0.6F, 2);
var lvl4Floor8 = new Floor(-12.5f, 22.6f, 13.7f, 0.6F, 8);
var lvl4Floor9 = new Floor(-1.5f, -2.3f, 11.7f, 0.6F, 2);
var lvl4Floor10 = new Floor(-6.5f, 9f, 12f, -0.6F, 2);
var lvl4Floor11 = new Floor(3.4f, 11.5f, -8f, -0.6F, 3);

var lvl4Portal1 = new Portal(1, 0, 0.2f, 0.4f, 1, true); //chao 1
var lvl4Portal2 = new Portal(-1.3f, -2.3f, 11.5f, 0.4f, 1, true); //chao 9
var lvl4Portal3 = new Portal(-12.2f, 2.3f, -5f, 0.4f, 1, true); //chao 4
var lvl4Portal4 = new Portal(4.2f, 22.6f, -2, 0.4f, 1, true); //chao 7
var lvl4Portal5 = new Portal(-11.3f, -6f, 1, 0.4f, 1, true); //chao 2
var lvl4Portal6 = new Portal(-8.1f, 6.6f, -13.3f, 0.4f, 1, true); //chao 5
var lvl4Portal7 = new Portal(-12.3f, -8.9f, -9, 0.4f, 1, true); //chao 3
var lvl4Portal8 = new Portal(-9.3f, -6f, 1f, 0.4f, 1, true); //chao 2
var lvl4Portal9 = new Portal(-6.6f, 9f, 12.3f, 0.4f, 1, true); //chao 10
var lvl4Portal10 = new Portal(8.6f, 6.6f, 0.8f, 0.4f, 1, true); //chao 6
var lvl4Portal11 = new Portal(-12.3f, 22.6f, 13.6f, 0.4f, 1, true); //chao 8
var lvl4Portal12 = new Portal(5f, 22.6f, -2, 0.4f, 1, true); //chao 7
var lvl4Portal13 = new Portal(4f, 11.5f, -8, 0.4f, 1, true); //chao 11
var lvl4Portal14 = new Portal(2f, 0, 0.2f, 0.4f, 1, true); //chao 1

var lvl4Book1 = new Book(-1, 0, -1, 0.4f, 0.5f, 0.5f);
var lvl4Book2 = new Book(-11.7f, -6f, 1.2f, 0.4f, 0.5f, 0.5f);
var lvl4Book3 = new Book(-8, 22.6f, 13.9f, 0.4f, 0.5f, 0.5f);
var lvl4Book4 = new Book(10, 6.6f, 1.3f, 0.4f, 0.5f, 0.5f);
lvl4Portal1.destiny = lvl4Portal2;
lvl4Portal2.destiny = lvl4Portal3;
lvl4Portal3.destiny = lvl4Portal4;
lvl4Portal4.destiny = lvl4Portal5;
lvl4Portal5.destiny = lvl4Portal6;
lvl4Portal6.destiny = lvl4Portal7;
lvl4Portal7.destiny = lvl4Portal8;
lvl4Portal8.destiny = lvl4Portal9;
lvl4Portal9.destiny = lvl4Portal10;
lvl4Portal10.destiny = lvl4Portal4;
lvl4Portal13.destiny = lvl4Portal14;
lvl4Portal14.destiny = lvl4Portal1;


lvl4Portal11.destiny = lvl4Portal1; //chegada
lvl4Portal12.destiny = lvl4Portal11;

var lvl4Entities = new List<Entity> { lvl4Floor1, lvl4Floor2, lvl4Floor3, lvl4Floor4, lvl4Floor5, lvl4Floor6, lvl4Floor7, lvl4Floor8, lvl4Floor9, lvl4Floor10, lvl4Floor11,
                                      lvl4Portal1, lvl4Portal2, lvl4Portal3, lvl4Portal4, lvl4Portal5, lvl4Portal6, lvl4Portal7, lvl4Portal8, lvl4Portal9, lvl4Portal10, lvl4Portal11,
                                      lvl4Portal12, lvl4Portal13, lvl4Portal14, lvl4Book1, lvl4Book2, lvl4Book3, lvl4Book4
                                    };
Level level4 = new();
List<Mesh> level4Mesh = new List<Mesh>{chaoPortas, portas, objetivoPortas};
level4.Initialize(new(-122.0044f, 34.5814f, 9.1700f), -1.5789f, -0.2279f, lvl4Entities, level4Mesh, bg2);
level4.Amelia.Anchor3D = new(0.5f, 0, 0);
level4.Amelia.Height = 5;

game.Levels.Add(level2);
game.Levels.Add(level4);
game.Run();