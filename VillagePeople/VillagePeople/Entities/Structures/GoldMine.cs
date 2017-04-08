using System;
using System.Collections.Generic;
using System.Drawing;
using VillagePeople.Util;

namespace VillagePeople.Entities.Structures
{
    class GoldMine : StaticEntity
    {
        public GoldMine(Vector2D position, World world) : base(position, world)
        {
            Scale = 20;
            Resource.Gold = 75;
            GatherRate = new Resource() { Gold = 2 };
            UnwalkableSpace = new List<Vector2D>()
            {
                new Vector2D(position.X - Scale / 2 - 5, position.Y - Scale / 2 - 5), // Top Left
                new Vector2D(position.X + Scale / 2 + 5, position.Y + Scale / 2 + 5), // Bottom Right
            };
        }

        public void Gather(BaseGameEntity e)
        {
            if (Resource.Gold > GatherRate.Gold)
                Resource += e.AddResource(GatherRate);
            else
                Resource += e.AddResource(Resource);
        }

        public override void Update(float delta)
        {
            //if (Resource.Gold == 0)
            //    Walkable = false;
            //else
            //    Resource -= GatherRate;
        }

        public override void Render(Graphics g)
        {
            double size = Scale * 2;
            double leftCorner = Position.X - size / 2;
            double rightCorner = Position.Y - size / 2;
            if (Resource.Gold > 0) // Normal tree
            {
                g.DrawImage(new Bitmap(@"..\..\Resources\SE\gold.png"), new Rectangle((int)leftCorner, (int)rightCorner, (int)size, (int)size));
            } else // Tree stump
            {
                g.DrawImage(new Bitmap(@"..\..\Resources\SE\gold_broken.png"), new Rectangle((int)leftCorner, (int)rightCorner, (int)size, (int)size));
            }

            g.DrawString(Resource.Gold.ToString(), new System.Drawing.Font("Arial", 9), new SolidBrush(Color.Black), Position.X + 10, Position.Y + 10);
        }
    }
}
