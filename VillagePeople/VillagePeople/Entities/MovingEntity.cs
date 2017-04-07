using System.Collections.Generic;
using System.Drawing;
using VillagePeople.Behaviours;
using VillagePeople.StateMachine;
using VillagePeople.Util;

namespace VillagePeople.Entities
{
    public abstract class MovingEntity : BaseGameEntity
    {
        public Color Color;

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

        private Pathfinder _pathFinder;
        private List<Node> _path = new List<Node>();
        private bool _possessed = false;
        private int _currentNodeInPath = -1;

        public MovingEntity(Vector2D position, World world) : base(position, world)
        {
            Mass = 150;
            MaxSpeed = 50;
            Radius = 30;
            Velocity = new Vector2D();
            Acceleration = new Vector2D();
            TargetSpeed = Velocity.Length();
            SteeringBehaviours = new List<SteeringBehaviour>();
        }

        public override void Update(float timeElapsed) {
            if (!_possessed)
            {
                Vector2D steering = SteeringBehaviour.CalculateWTS(SteeringBehaviours, MaxSpeed);
                steering /= Mass;

                Vector2D acceleration = steering;
                acceleration *= 0.8f;
                Velocity += acceleration;

                Velocity *= 0.8f;
                Position += Velocity;
            }
            else if(_path.Count > 0 && _currentNodeInPath != _path.Count && _currentNodeInPath != -1)
            {
                var diff = _path[_currentNodeInPath].WorldPosition - Position;
                Position += diff.Scale(10f);
                if (CloseEnough(Position, _path[_currentNodeInPath].WorldPosition, 10))
                {
                    _currentNodeInPath++;
                }
            }
        }

        public List<Node> EnterPossession(Graph g, Vector2D target)
        {
            _pathFinder = new Pathfinder();
            _possessed = true;
            _pathFinder.grid = g;
            UpdatePath(target);
            return _path;
        }

        public List<Node> UpdatePath(Vector2D target)
        {
            _pathFinder.seeker = Position;
            _pathFinder.target = target;
            _pathFinder.Update();
            _path = _pathFinder.path;
            _currentNodeInPath = 0;

            return _path;
        }

        public void ExitPossession()
        {
            _possessed = false;
            _pathFinder = null;
            _currentNodeInPath = -1;
        }

        public void SetNewTarget(Vector2D to)
        {
            SteeringBehaviours = new List<SteeringBehaviour> {
                new ArriveBehaviour(this, to)
            };
        }

        public void SetWander(float elapsedTime)
        {
            SteeringBehaviours = new List<SteeringBehaviour> {
                new WanderBehaviour(this, elapsedTime),
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
