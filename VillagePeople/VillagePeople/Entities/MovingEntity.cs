using System;
using VillagePeople.Behaviours;
using VillagePeople.Util;

namespace VillagePeople.Entities
{
    abstract class MovingEntity : BaseGameEntity
    {
        public Vector2D Velocity { get; set; }
        public Vector2D Acceleration { get; set; }
        public float Mass { get; set; }
        public float MaxSpeed { get; set; }
        public int MaxInventorySpace { get; set; }
        public double TargetSpeed;

        public MovingEntity(Vector2D position, World world) : base(position, world)
        {
            Mass = 150;
            MaxSpeed = 100;
            Velocity = new Vector2D();
            Acceleration = new Vector2D();
            TargetSpeed = Velocity.Length();
        }

        public override void Update(float timeElapsed)
        {
            /*Position.Add(Velocity);

            if (Position.X < 0 || Position.X > World.Width || Position.Y < 0 || Position.Y > World.Height)
            {
                Position = new Vector2D(300, 300);
                Velocity = new Vector2D(1, 1);
                Acceleration = new Vector2D(1, 1);
            }

            var rotationMatrix = Matrix.Identity().Rotate(30);
            Vector2D targetVelocity;

            TargetSpeed += Acceleration.Length();
            if (TargetSpeed > MaxSpeed)
            {
                targetVelocity = Velocity * rotationMatrix;
                TargetSpeed = MaxSpeed;
            }
            else
            {
                targetVelocity = Velocity.Add(Acceleration) * rotationMatrix;
            }

            Velocity = targetVelocity.Scale(TargetSpeed);*/

            var steering = new ArriveBehaviour(this, World.Target.Position).Calculate();

            steering /= Mass;
            var acceleration = steering;

            acceleration *= timeElapsed;
            Velocity += acceleration;
            Velocity.Truncate(MaxSpeed);

            Velocity *= timeElapsed;
            Position += Velocity;
        }

        public void NextStep(float timeElapsed)
        {
            Update(timeElapsed);
        }

        public override string ToString()
        {
            return $"{Velocity}";
        }
    }
}
