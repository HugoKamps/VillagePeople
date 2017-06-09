using System.Collections.Generic;
using System.Drawing;
using VillagePeople.Util;

namespace VillagePeople.Terrain
{
    public class GameTerrain
    {
        public Vector2D Position;
        public float Speed;
        public TerrainType Type;

        public GameTerrain(Vector2D position, TerrainType t = TerrainType.Grass)
        {
            Position = position;
            Type = t;
            switch (Type)
            {
                case TerrainType.Grass:
                    Speed = 1.0f;
                    break;
                case TerrainType.Road:
                    Speed = 1.2f;
                    break;
            }
        }

        public bool Walkable => Speed > 0.0f;

        public void Render(Graphics g)
        {
            Image img;
            switch (Type)
            {
                case TerrainType.Road:
                    img = BitmapLoader.LoadBitmap(@"..\..\Resources\Terrain\road.png", GetType().ToString() + Type);
                    break;
                case TerrainType.Townhall:
                    img = BitmapLoader.LoadBitmap(@"..\..\Resources\Terrain\town_hall.png", GetType().ToString() + Type);
                    break;
                default: // case TerrainType.Grass
                    img = BitmapLoader.LoadBitmap(@"..\..\Resources\Terrain\grass.png", GetType().ToString() + Type);
                    break;
            }

            g.DrawImage(img, new RectangleF(Position.X, Position.Y, 50, 50));
        }

        public static void GenerateMap(List<GameTerrain> terrain)
        {
            // Row 1 & 2
            var y = 0;
            for (var i = 0; i < 2; i++)
            {
                CreateTerrain(terrain, new GameTerrain(new Vector2D(0, y)), 4);
                CreateTerrain(terrain, new GameTerrain(new Vector2D(200, y)), 2);
                CreateTerrain(terrain, new GameTerrain(new Vector2D(300, y)), 2);
                CreateTerrain(terrain, new GameTerrain(new Vector2D(400, y)), 8);
                y += 50;
            }

            // Row 3
            CreateTerrain(terrain, new GameTerrain(new Vector2D(0, y)), 4);
            CreateTerrain(terrain, new GameTerrain(new Vector2D(200, y)), 12);

            // Row 4
            y += 50;
            CreateTerrain(terrain, new GameTerrain(new Vector2D(0, y)), 5);
            CreateTerrain(terrain, new GameTerrain(new Vector2D(250, y)), 7);
            CreateTerrain(terrain, new GameTerrain(new Vector2D(600, y)), 4);

            // Row 5
            y += 50;
            CreateTerrain(terrain, new GameTerrain(new Vector2D(0, y)), 8);
            CreateTerrain(terrain, new GameTerrain(new Vector2D(400, y)), 3);
            CreateTerrain(terrain, new GameTerrain(new Vector2D(550, y)), 5);

            // Row 6
            y += 50;
            CreateTerrain(terrain, new GameTerrain(new Vector2D(0, y)), 12);
            CreateTerrain(terrain, new GameTerrain(new Vector2D(600, y), TerrainType.Road), 4);

            // Row 7
            y += 50;
            CreateTerrain(terrain, new GameTerrain(new Vector2D(0, y)), 11);
            CreateTerrain(terrain, new GameTerrain(new Vector2D(550, y), TerrainType.Road), 5);

            // Row 8
            y += 50;
            CreateTerrain(terrain, new GameTerrain(new Vector2D(0, y)), 1);
            CreateTerrain(terrain, new GameTerrain(new Vector2D(50, y), TerrainType.Townhall), 1);
            CreateTerrain(terrain, new GameTerrain(new Vector2D(100, y)), 8);
            CreateTerrain(terrain, new GameTerrain(new Vector2D(500, y), TerrainType.Road), 3);
            CreateTerrain(terrain, new GameTerrain(new Vector2D(650, y)), 3);

            // Row 9
            y += 50;
            CreateTerrain(terrain, new GameTerrain(new Vector2D(0, y), TerrainType.Road), 12);
            CreateTerrain(terrain, new GameTerrain(new Vector2D(600, y)), 4);

            // Row 10
            y += 50;
            CreateTerrain(terrain, new GameTerrain(new Vector2D(0, y), TerrainType.Road), 11);
            CreateTerrain(terrain, new GameTerrain(new Vector2D(550, y)), 3);
            CreateTerrain(terrain, new GameTerrain(new Vector2D(700, y)), 2);

            // Row 11
            y += 50;
            CreateTerrain(terrain, new GameTerrain(new Vector2D(0, y)), 12);
            CreateTerrain(terrain, new GameTerrain(new Vector2D(600, y)), 4);

            // Row 11
            y += 50;
            CreateTerrain(terrain, new GameTerrain(new Vector2D(0, y)), 11);
            CreateTerrain(terrain, new GameTerrain(new Vector2D(550, y)), 5);
        }

        public static void CreateTerrain(List<GameTerrain> terrains, GameTerrain terrain, int amount)
        {
            var type = terrain.Type;
            float x = terrain.Position.X, y = terrain.Position.Y;
            for (var i = 0; i < amount; i++)
            {
                terrains.Add(new GameTerrain(new Vector2D(x, y), type));
                x += 50;
            }
        }
    }

    public enum TerrainType
    {
        Grass,
        Road,
        Townhall
    }
}