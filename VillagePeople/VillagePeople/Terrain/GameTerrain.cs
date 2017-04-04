using System.Drawing;
using VillagePeople.Util;

namespace VillagePeople.Terrain
{
    public class GameTerrain
    {
        public float Speed;
        public TerrainType Type;
        public Vector2D Position;

        public bool Walkable => Speed > 0.0f;

        public GameTerrain(Vector2D position, TerrainType t = TerrainType.Grass)
        {
            Position = position;
            Type = t;
            switch (Type)
            {
                case TerrainType.Grass:
                    Speed = 1.0f;
                    break;
                case TerrainType.Water:
                    Speed = 0.2f;
                    break;
                case TerrainType.Road:
                    Speed = 1.2f;
                    break;
            }
        }

        public void Render(Graphics g)
        {
            switch (Type)
            {
                case TerrainType.Grass:
                    g.FillRectangle(new SolidBrush(Color.Green), new RectangleF(Position.X, Position.Y, 50, 50));
                    break;
                case TerrainType.Water:
                    g.FillRectangle(new SolidBrush(Color.Blue), new RectangleF(Position.X, Position.Y, 50, 50));
                    break;
                case TerrainType.Road:
                    g.FillRectangle(new SolidBrush(Color.Gray), new RectangleF(Position.X, Position.Y, 50, 50));
                    break;
            }
        }
    }

    public enum TerrainType
    {
        Grass,
        Water,
        Road
    }
}
