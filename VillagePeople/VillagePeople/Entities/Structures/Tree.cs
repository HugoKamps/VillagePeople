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
            GatherRate = new Resource { Wood = 2 };
            UnwalkableSpace = new List<Vector2D> {
                new Vector2D(position.X - Scale / 2, position.Y - Scale / 2), // Top Left
                new Vector2D(position.X + Scale / 2, position.Y + Scale / 2) // Bottom Right
            };
        }

        public override void Update(float delta)
        {
            if (Resource.Wood == 0)
                Walkable = false;
            else
                Resource -= GatherRate;
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
            var p = new Pen(Color.Brown, 2);
            var b = new SolidBrush(Color.Brown);

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
                double size = Scale / 4;
                double leftCorner = Position.X - size / 2;
                double rightCorner = Position.Y - size / 2;
                g.FillEllipse(b, new Rectangle((int)leftCorner, (int)rightCorner, 10, 10));
            }
        }
    }
}
