using System.Collections.Generic;
using System.Drawing;
using VillagePeople.Util;

namespace VillagePeople.Entities.Structures
{
    class StoneMine : StaticEntity
    {
        public StoneMine(Vector2D position, World world) : base(position, world)
        {
            Scale = 20;
            Resource.Stone = 75;
            GatherRate = new Resource { Stone = 2 };
            UnwalkableSpace = new List<Vector2D> {
                new Vector2D(position.X - Scale / 2 - 5, position.Y - Scale / 2 - 5), // Top Left
                new Vector2D(position.X + Scale / 2 + 5, position.Y + Scale / 2 + 5) // Bottom Right
            };
        }

        public void Gather(BaseGameEntity e)
        {
            if (Resource.Stone > GatherRate.Stone)
                Resource += e.AddResource(GatherRate);
            else
                Resource += e.AddResource(Resource);
        }

        public override void Update(float delta)
        {
            //if (Resource.Stone == 0)
            //    Walkable = false;
            //else
            //    Resource -= GatherRate;
        }

        public override void Render(Graphics g)
        {
            double size = Scale * 2;
            double leftCorner = Position.X - size / 2;
            double rightCorner = Position.Y - size / 2;
            if (Resource.Stone > 0) // Normal tree
            {
                g.DrawImage(new Bitmap(@"..\..\Resources\SE\stone.png"), new Rectangle((int)leftCorner, (int)rightCorner, (int)size, (int)size));
            } else // Tree stump
            {
                g.DrawImage(new Bitmap(@"..\..\Resources\SE\stone_broken.png"), new Rectangle((int)leftCorner, (int)rightCorner, (int)size, (int)size));
            }

            g.DrawString(Resource.Stone.ToString(), new Font("Arial", 9), new SolidBrush(Color.Black), Position.X + 10, Position.Y + 10);
        }
    }
}
