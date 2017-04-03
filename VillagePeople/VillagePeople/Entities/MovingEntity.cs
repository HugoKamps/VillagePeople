using System;
using System.Collections.Generic;
using VillagePeople.Behaviours;
using VillagePeople.Entities.NPC;
using VillagePeople.StateMachine;
using VillagePeople.StateMachine.States;
using VillagePeople.Util;

namespace VillagePeople.Entities
{
    abstract class MovingEntity : BaseGameEntity
    {
        public Vector2D Velocity { get; set; }
        public Vector2D Acceleration { get; set; }
        public Vector2D Heading { get; set; }

        public List<SteeringBehaviour> SteeringBehaviours { get; set; }

        public float Mass { get; set; }
        public float MaxSpeed { get; set; }
        public int MaxInventorySpace { get; set; }
        public double TargetSpeed;
        public double Radius;
        public List<MovingEntity> Neighbours { get; set; }

        public StateMachine<MovingEntity> StateMachine;

        private int _elapsedTicks;

        public MovingEntity(Vector2D position, World world) : base(position, world)
        {
            StateMachine = new StateMachine<MovingEntity>(this);
            StateMachine.ChangeState(new ReturningResources());

            Mass = 150;
            MaxSpeed = 100;
            Radius = 30;
            Velocity = new Vector2D();
            Acceleration = new Vector2D();
            TargetSpeed = Velocity.Length();
        }

        public override void Update(float timeElapsed) {
            _elapsedTicks += 1;
            if(_elapsedTicks % 50 == 0) StateMachine.Update();
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

            FlockingBehaviour.TagNeighbors(this, World.MovingEntities, 5);
            Neighbours = World.MovingEntities.FindAll(m => m.Tagged);

            SteeringBehaviours = new List<SteeringBehaviour> {
                new ArriveBehaviour(this, World.Target.Position),
                new SeekBehaviour(this, World.Target.Position),
                new Alignment(this, Neighbours),
                new Cohesion(this, Neighbours),
                new Separation(this, Neighbours)
            };


            Vector2D steering = SteeringBehaviour.CalculateDithered(SteeringBehaviours);
            //steering.Truncate(MaxSpeed);
            steering /= Mass;

            Vector2D acceleration = steering;
            acceleration *= timeElapsed;
            Velocity += acceleration;
            //Velocity.Truncate(MaxSpeed);

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
