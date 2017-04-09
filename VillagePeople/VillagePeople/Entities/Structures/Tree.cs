using System.Collections.Generic;
using System.Drawing;
using VillagePeople.Util;

namespace VillagePeople.Entities.Structures
{
    class Tree : StaticEntity
    {
        public Tree(Vector2D position, World world) : base(position, world)
        {
            Scale = 20;
            Resource.Wood = 200;
            GatherRate = new Resource { Wood = 4 };
            UnwalkableSpace = new List<Vector2D> {
                new Vector2D(position.X - Scale / 2 - 5, position.Y - Scale / 2 - 5), // Top Left
                new Vector2D(position.X + Scale / 2 + 5, position.Y + Scale / 2 + 5), // Bottom Right
            };
        }

        public override void Update(float delta)
        {
            //if (Resource.Wood == 0)
            //    Walkable = false;
            //else
            //    Resource -= GatherRate;
        }

        public void Gather(BaseGameEntity e)
        {
            if (Resource.Wood > GatherRate.Wood)
                Resource += e.AddResource(GatherRate);
            else
                Resource += e.AddResource(Resource);
        }

        public override void Render(Graphics g)
        {
            var b = new System.Drawing.SolidBrush(Color.Brown);

            if (Resource.Wood > 100) // Normal tree
            {
                double size = Scale;
                double leftCorner = Position.X - size / 2;
                double rightCorner = Position.Y - size / 2;
                g.FillEllipse(b, new Rectangle((int)leftCorner, (int)rightCorner, (int)size, (int)size));
            }
            else if (Resource.Wood > 0) // Half cutdown tree
            {
                double size = Scale / 2;
                double leftCorner = Position.X - size / 2;
                double rightCorner = Position.Y - size / 2;
                g.FillEllipse(b, new Rectangle((int)leftCorner, (int)rightCorner, (int)size, (int)size));
            }
            else // Tree stump
            {
                b = new System.Drawing.SolidBrush(Color.Black);
                double size = Scale / 4;
                double leftCorner = Position.X - 5;
                double rightCorner = Position.Y - 5;
                g.FillEllipse(b, new Rectangle((int)leftCorner, (int)rightCorner, 10, 10));
            }

            g.DrawLine(new Pen(Color.Black, 4), new Point((int)UnwalkableSpace[0].X, (int)UnwalkableSpace[0].Y), new Point((int)UnwalkableSpace[1].X, (int)UnwalkableSpace[1].Y));
            g.DrawLine(new Pen(Color.Black, 4), new Point((int)UnwalkableSpace[1].X, (int)UnwalkableSpace[0].Y), new Point((int)UnwalkableSpace[0].X, (int)UnwalkableSpace[1].Y));

            g.DrawString(Resource.Wood.ToString(), new System.Drawing.Font("Arial", 9), new SolidBrush(Color.Black), Position.X, Position.Y);
        }
    }
}
