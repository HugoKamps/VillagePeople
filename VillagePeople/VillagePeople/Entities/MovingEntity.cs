using System.Collections.Generic;
using System.Drawing;
using VillagePeople.Behaviours;
using VillagePeople.StateMachine;
using VillagePeople.Util;

namespace VillagePeople.Entities
{
    public abstract class MovingEntity : BaseGameEntity
    {
        private int _currentNodeInPath = -1;
        private List<Node> _path = new List<Node>();
        private Pathfinder _pathFinder;
        private bool _possessed;
        public Color Color;

        public List<Node> NonSmoothenedPath = new List<Node>();
        public double Radius;

        public StateMachine<MovingEntity> StateMachine;
        public double TargetSpeed;

        public MovingEntity(Vector2D position, World world) : base(position, world)
        {
            Mass = 150;
            MaxSpeed = 900;
            Radius = 30;
            Velocity = new Vector2D();
            Acceleration = new Vector2D();
            TargetSpeed = Velocity.Length();
            SteeringBehaviours = new List<SteeringBehaviour>();
        }

        public Vector2D Velocity { get; set; }
        public Vector2D Acceleration { get; set; }
        public Vector2D Heading { get; set; }


        public List<SteeringBehaviour> SteeringBehaviours { get; set; }

        public float Mass { get; set; }
        public float MaxSpeed { get; set; }
        public int MaxInventorySpace { get; set; }
        public List<MovingEntity> Neighbours { get; set; }

        public override void Update(float timeElapsed)
        {

            if (_path.Count > 0 && _currentNodeInPath != _path.Count && _currentNodeInPath != -1)
            {
                var diff = _path[_currentNodeInPath].WorldPosition - Position;

                SetNewTarget(Position += diff.Scale(10f));
                if (CloseEnough(Position, _path[_currentNodeInPath].WorldPosition, 10))
                    _currentNodeInPath++;
            }

            var steering = SteeringBehaviour.CalculateWts(SteeringBehaviours, MaxSpeed);
            steering /= Mass;

            var acceleration = steering;
            acceleration *= 0.8f;
            Velocity += acceleration;

            Velocity *= 0.8f;
            Position += Velocity;
        }

        /*
        public override void Update(float timeElapsed)
        {
            if (!_possessed)
            {
                var steering = SteeringBehaviour.CalculateWts(SteeringBehaviours, MaxSpeed);
                steering /= Mass;

                var acceleration = steering;
                acceleration *= 0.8f;
                Velocity += acceleration;

                Velocity *= 0.8f;
                Position += Velocity;
            }
            else if (_path.Count > 0 && _currentNodeInPath != _path.Count && _currentNodeInPath != -1)
            {
                var diff = _path[_currentNodeInPath].WorldPosition - Position;
                Position += diff.Scale(10f);
                if (CloseEnough(Position, _path[_currentNodeInPath].WorldPosition, 10))
                    _currentNodeInPath++;
            }
        }*/

        public List<Node> EnterPossession(Graph g, Vector2D target)
        {
            _pathFinder = new Pathfinder();
            _possessed = true;
            _pathFinder.Grid = g;
            UpdatePath(target);

            return _path;
        }

        public List<Node> UpdatePath(Vector2D target)
        {
            _pathFinder.Seeker = Position;
            _pathFinder.Target = target;
            _pathFinder.Update();
            NonSmoothenedPath = _pathFinder.Path;
            _pathFinder.PathSmoothing();
            _path = _pathFinder.Path;
            _currentNodeInPath = 0;

            return _path;
        }

        public void ExitPossession()
        {
            _possessed = false;
            _pathFinder.NodesWithSmoothEdges.ForEach(n => n.SmoothEdges = new List<Edge>());
            _pathFinder = null;
            _currentNodeInPath = -1;
        }

        public void SetNewTarget(Vector2D to)
        {
            SteeringBehaviours = new List<SteeringBehaviour>
            {
                new ArriveBehaviour(this, to)
            };
        }

        public void SetWander(float elapsedTime)
        {
            SteeringBehaviours = new List<SteeringBehaviour>
            {
                new WanderBehaviour(this, elapsedTime),
                new Alignment(this, World.MovingEntities),
                new Cohesion(this, World.MovingEntities),
                new Separation(this, World.MovingEntities)
            };
        }

        public void UpdateFlocking()
        {
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