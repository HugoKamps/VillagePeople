﻿using System.Collections.Generic;
using System.Drawing;
using System.Runtime.InteropServices;
using VillagePeople.StateMachine;
using VillagePeople.StateMachine.States;
using VillagePeople.Util;

namespace VillagePeople.Entities.NPC
{
    public class Villager : MovingEntity
    {
        public Villager(Vector2D position, World world) : base(position, world)
        {
            StateMachine = new StateMachine<MovingEntity>(this);
            StateMachine.ChangeState(new ReturningResources());

            Velocity = new Vector2D(1, 1);
            Acceleration = new Vector2D(1, 1);
            TargetSpeed = Velocity.Length();
            Scale = 20;
            MaxInventorySpace = 10;

            Color = Color.Black;
        }

        public override Resource AddResource(Resource r)
        {
            int capacity = MaxInventorySpace - Resource.TotalResources();

            if (capacity <=0)
            {
                return r;
            }

            if (capacity >= r.TotalResources())
            {
                return base.AddResource(r);
            }
            return base.AddResource(r.Cap(capacity));
        }

        public override void Render(Graphics g)
        {
            double leftCorner = Position.X - 0.5 * Scale;
            double rightCorner = Position.Y - 0.5 * Scale;
            double size = Scale;

            var p = new Pen(Color, 4);
            var b = new SolidBrush(Color);

            g.FillEllipse(b, new Rectangle((int)leftCorner, (int)rightCorner, (int)size, (int)size));
            g.DrawEllipse(new Pen(Color.Black, 1), new Rectangle((int)leftCorner, (int)rightCorner, (int)size, (int)size));
            g.DrawLine(p, (int)Position.X, (int)Position.Y, (int)Position.X + (int)Velocity.X, (int)Position.Y + (int)Velocity.Y);
        }
    }
}
