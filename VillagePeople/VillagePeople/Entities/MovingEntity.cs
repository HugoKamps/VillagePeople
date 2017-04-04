using System;
using System.Collections.Generic;
using VillagePeople.Behaviours;
using VillagePeople.Entities.NPC;
using VillagePeople.StateMachine;
using VillagePeople.StateMachine.States;
using VillagePeople.Util;

namespace VillagePeople.Entities
{
    public abstract class MovingEntity : BaseGameEntity
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
            Mass = 150;
            MaxSpeed = 500;
            Radius = 30;
            Velocity = new Vector2D();
            Acceleration = new Vector2D();
            TargetSpeed = Velocity.Length();
            SteeringBehaviours = new List<SteeringBehaviour>();
        }

        public override void Update(float timeElapsed) {
            _elapsedTicks += 1;
            if (_elapsedTicks % 50 == 0) StateMachine.Update();

            FlockingBehaviour.TagNeighbors(this, World.MovingEntities, 5);
            Neighbours = World.MovingEntities.FindAll(m => m.Tagged);

            /*SteeringBehaviours = new List<SteeringBehaviour> {
                new ArriveBehaviour(this, World.Target.Position),
                new SeekBehaviour(this, World.Target.Position),
                new Alignment(this, Neighbours),
                new Cohesion(this, Neighbours),
                new Separation(this, Neighbours)
            };*/

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
