using System.Collections.Generic;
using System.Drawing;
using VillagePeople.Util;

namespace VillagePeople.Entities.Structures
{
    class GoldMine : StaticEntity
    {
        
        public GoldMine(Vector2D position, World world) : base(position, world)
        {
            BaseAmount = 75;
            Resource.Gold = BaseAmount;
            Scale = 20;
            GatherRate = new Resource { Gold = 2 };
            UnwalkableSpace = new List<Vector2D>
            {
                new Vector2D(position.X - Scale / 2 - 5, position.Y - Scale / 2 - 5), // Top Left
                new Vector2D(position.X + Scale / 2 + 5, position.Y + Scale / 2 + 5) // Bottom Right
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
            Image img;

            double size = Scale * 2;
            double leftCorner = Position.X - size / 2;
            double rightCorner = Position.Y - size / 2;

            if (Resource.Gold > 0) // Normal gold mine
                img = BitmapLoader.LoadBitmap(@"..\..\Resources\SE\gold.png", GetType() + "1");
            else // Broken gold mine
                img = BitmapLoader.LoadBitmap(@"..\..\Resources\SE\gold_broken.png", GetType() + "2");

            g.DrawImage(img, new Rectangle((int)leftCorner, (int)rightCorner, (int)size, (int)size));
            
            g.DrawLine(new Pen(Color.Black, 4), new Point((int)UnwalkableSpace[0].X, (int)UnwalkableSpace[0].Y), new Point((int)UnwalkableSpace[1].X, (int)UnwalkableSpace[1].Y));
            g.DrawLine(new Pen(Color.Black, 4), new Point((int)UnwalkableSpace[1].X, (int)UnwalkableSpace[0].Y), new Point((int)UnwalkableSpace[0].X, (int)UnwalkableSpace[1].Y));
            g.DrawString(Resource.Gold.ToString(), new Font("Arial", 9), new SolidBrush(Color.Black), Position.X + 10, Position.Y + 10);
        }
    }
}
