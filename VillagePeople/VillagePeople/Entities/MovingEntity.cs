using System.Collections.Generic;
using System.Drawing;
using System.Net;
using VillagePeople.Behaviours;
using VillagePeople.Entities.NPC;
using VillagePeople.StateMachine;
using VillagePeople.StateMachine.States;
using VillagePeople.Util;

namespace VillagePeople.Entities {
    public abstract class MovingEntity : BaseGameEntity {
        private int _currentNodeInPath = -1;
        private List<Node> _path = new List<Node>();
        private Pathfinder _pathFinder;
        protected bool _possessed;
        public Color Color;
        public List<Node> ConsideredEdges = new List<Node>();
        public List<Node> NonSmoothenedPath = new List<Node>();
        public double Radius;
        public StateMachine<MovingEntity> StateMachine;
        public Sheep TargetSheep;
        public double TargetSpeed;

        public MovingEntity(Vector2D position, World world) : base(position, world) {
            Mass = 150;
            MaxSpeed = 900;
            Radius = 30;
            Velocity = new Vector2D();
            Acceleration = new Vector2D();
            TargetSpeed = Velocity.Length();
            Heading = Velocity.Normalize();
            SteeringBehaviours = new List<SteeringBehaviour>();
        }

        public List<MovingEntity> Neighbours { get; set; }
        public List<SteeringBehaviour> SteeringBehaviours { get; set; }
        public Vector2D Velocity { get; set; }
        public Vector2D Acceleration { get; set; }
        public Vector2D Heading { get; set; }
        public float Mass { get; set; }
        public float MaxSpeed { get; set; }
        public int MaxInventorySpace { get; set; }

        public override void Update(float timeElapsed) {
            if (_path.Count > 0 && _currentNodeInPath != _path.Count && _currentNodeInPath != -1 && _possessed) {
                var diff = _path[_currentNodeInPath].WorldPosition - Position;

                SetNewTarget(Position += diff.Scale(10f));
                if (CloseEnough(Position, _path[_currentNodeInPath].WorldPosition, 10))
                    _currentNodeInPath++;
            }
            else {
                var steering = SteeringBehaviour.CalculateWeightedAverage(SteeringBehaviours, MaxSpeed);
                Heading = Velocity.Normalize();

                steering = steering.Truncate(MaxSpeed);

                var acceleration = steering / Mass;
                Velocity = (Velocity + acceleration).Truncate(MaxSpeed);
                Position += Velocity;
            }
        }

        public List<Node> EnterPossession(Graph g, Vector2D target) {
            _pathFinder = new Pathfinder();
            _pathFinder.Grid = g;
            UpdatePath(target);
            _possessed = true;

            return _path;
        }

        public List<Node> UpdatePath(Vector2D target) {
            _pathFinder.Seeker = Position;
            _pathFinder.Target = target;
            _pathFinder.Update();
            NonSmoothenedPath = _pathFinder.Path;
            ConsideredEdges = _pathFinder.ConsideredEdges;
            _pathFinder.PathSmoothing();
            _path = _pathFinder.Path;
            _currentNodeInPath = 0;

            return _path;
        }

        public void ExitPossession() {
            _pathFinder.NodesWithSmoothEdges.ForEach(n => n.SmoothEdges = new List<Edge>());
            _pathFinder = null;
            _currentNodeInPath = -1;
            _possessed = false;
            StateMachine.ChangeState(new ReturningResources());
        }

        public void SetNewTarget(Vector2D to) {
            SteeringBehaviours = new List<SteeringBehaviour> {
                new SeekBehaviour(this, to),
                new Separation(this)
            };
        }

        public void NextStep(float timeElapsed) {
            Update(timeElapsed);
        }

        public override string ToString() {
            return $"{Velocity}";
        }
    }
}