﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VillagePeople.Util;

namespace VillagePeople.Entities.Structures
{
    class Tree : StaticEntity
    {
        public Tree(Vector2D position, World world) : base(position, world)
        {
            Scale = 40;
            Resource.Wood = 200;
            GatherRate = new Resource() { Wood = 2 };
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
                double size = Scale / 4;
                double leftCorner = Position.X - size / 2;
                double rightCorner = Position.Y - size / 2;
                g.FillEllipse(b, new Rectangle((int)leftCorner, (int)rightCorner, (int)10, (int)10));
            }
        }
    }
}