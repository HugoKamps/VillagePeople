using System.Collections.Generic;
using System.Windows.Forms.VisualStyles;
using VillagePeople.Behaviours;
using VillagePeople.StateMachine;
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
            Mass = 150;
            MaxSpeed = 100;
            Radius = 30;
            Velocity = new Vector2D();
            Acceleration = new Vector2D();
            TargetSpeed = Velocity.Length();
        }

        public override void Update(float timeElapsed) {
            _elapsedTicks += 1;
            if (_elapsedTicks % 50 == 0) StateMachine.Update();

            Neighbours = new List<MovingEntity>();
            SteeringBehaviour.TagNeighbors(this, World.MovingEntities, 5);

            Vector2D steering = SteeringBehaviour.CalculateWTS(SteeringBehaviours, MaxSpeed);
            steering /= Mass;

            Vector2D acceleration = steering;
            acceleration *= timeElapsed;
            Velocity += acceleration;
            
            Velocity *= timeElapsed;
            Position += Velocity;
        }

        public void SetNewTarget(Vector2D from, Vector2D to)
        {
            SteeringBehaviours = new List<SteeringBehaviour> {
                new ArriveBehaviour(this, to)
            };
        }

        public void SetWander()
        {
            SteeringBehaviours = new List<SteeringBehaviour> {
                new WanderBehaviour(this, _elapsedTicks),
                new Alignment(this, World.MovingEntities),
                new Cohesion(this, World.MovingEntities),
                new Separation(this, World.MovingEntities)
            };
        }

        public void UpdateFlocking() {
            SteeringBehaviours[1] = new Alignment(this, World.MovingEntities);
            SteeringBehaviours[2] = new Cohesion(this, World.MovingEntities);
            SteeringBehaviours[3] = new Separation(this, World.MovingEntities);
        }

        public void NextStep(float timeElapsed)
        {
            Update(timeElapsed);
        }

        public override string ToString() => $"{Velocity}";
    }
}
