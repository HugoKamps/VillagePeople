using System.Drawing;
using System.Runtime.InteropServices;
using VillagePeople.Behaviours;
using VillagePeople.StateMachine;
using VillagePeople.StateMachine.States;
using VillagePeople.Util;

namespace VillagePeople.Entities.NPC
{
    class Sheep : MovingEntity
    {
        public Color Color;

        public Sheep(Vector2D position, World world) : base(position, world)
        {
            Velocity = new Vector2D(1, 1);
            Acceleration = new Vector2D(1, 1);
            TargetSpeed = Velocity.Length();
            Scale = 20;
        }
        
        public override void Render(Graphics g)
        {
            double leftCorner = Position.X - Scale;
            double rightCorner = Position.Y - Scale;
            double size = Scale * 4;

            var p = new Pen(Color, 4);
            var b = new SolidBrush(Color);

            g.DrawImage(new Bitmap(@"..\..\Resources\spel.png"), new Rectangle((int)leftCorner, (int)rightCorner, (int)size, (int)size));

            //g.FillEllipse(b, new Rectangle((int)leftCorner, (int)rightCorner, (int)size, (int)size));
            g.DrawLine(p, (int)Position.X, (int)Position.Y, (int)Position.X + (int)Velocity.X, (int)Position.Y + (int)Velocity.Y);
        }

        public override void Update(float timeElapsed) {
            //SteeringBehaviours.Add(new WanderBehaviour(this, new Vector2D(500, 500)));
            //Vector2D steering = SteeringBehaviour.CalculateDithered(SteeringBehaviours);
            ////steering.Truncate(MaxSpeed);
            //steering /= Mass;

            //Vector2D acceleration = steering;
            //acceleration *= timeElapsed;
            //Velocity += acceleration;
            ////Velocity.Truncate(MaxSpeed);

            //Velocity *= timeElapsed;
            //Position += Velocity;
        }
    }
}
