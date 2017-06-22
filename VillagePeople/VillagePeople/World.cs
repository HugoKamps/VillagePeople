using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using VillagePeople.Entities;
using VillagePeople.Entities.NPC;
using VillagePeople.Entities.Structures;
using VillagePeople.StateMachine;
using VillagePeople.StateMachine.States;
using VillagePeople.Terrain;
using VillagePeople.Util;

namespace VillagePeople {
    public class World {
        private Container _container;
        public bool AutoUpdate = false;

        public bool DebugGraph = false;
        public bool DebugText = false;

        public Graph Graph;

        public List<MovingEntity> MovingEntities;

        public int SelectedEntityIndex = -1;
        public List<StaticEntity> StaticEntities;

        public List<Villager> Target = new List<Villager>();
        public Vector2D TargetLoc;
        public List<GameTerrain> Terrains = new List<GameTerrain>();
        public List<Wall> Walls;

        public World(int width, int height, Container container) {
            Width = width;
            Height = height;

            _container = container;
            Resources = new Resource {Food = 0, Gold = 0, Stone = 0, Wood = 0};

            Init();

            Graph = GenerateGraph();
        }

        public int Width { get; set; }
        public int Height { get; set; }
        public Villager Leader { get; set; }

        public Resource Resources { get; set; }

        public void Init() {
            TargetLoc = new Vector2D();
            GameTerrain.GenerateMap(Terrains);
            SetWalls();
            var t1 = new Tree(new Vector2D(175, 25), this);
            var t2 = new Tree(new Vector2D(75, 25), this);
            var t3 = new Tree(new Vector2D(125, 25), this);
            var t4 = new Tree(new Vector2D(50, 75), this);
            var t5 = new Tree(new Vector2D(100, 75), this);
            var t6 = new Tree(new Vector2D(150, 75), this);

            var sm1 = new StoneMine(new Vector2D(325, 275), this);
            var sm2 = new StoneMine(new Vector2D(325, 325), this);
            var sm3 = new StoneMine(new Vector2D(325, 375), this);

            var gm1 = new GoldMine(new Vector2D(30, 570), this);
            var gm2 = new GoldMine(new Vector2D(130, 570), this);
            var gm3 = new GoldMine(new Vector2D(230, 570), this);

            StaticEntities = new List<StaticEntity> {
                t1,
                t2,
                t3,
                t4,
                t5,
                t6,
                sm1,
                sm2,
                sm3,
                gm1,
                gm2,
                gm3
            };

            var v1 = new Villager(new Vector2D(150, 125), this) {
                MaxInventorySpace = 10,
                MaxSpeed = 400
            };

            var v2 = new Villager(new Vector2D(120, 200), this) {
                MaxSpeed = 300,
                MaxInventorySpace = 12
            };

            var v3 = new Villager(new Vector2D(200, 290), this) {
                MaxSpeed = 200,
                MaxInventorySpace = 14
            };

            var v4 = new Villager(new Vector2D(450, 450), this) {
                MaxSpeed = 500,
                MaxInventorySpace = 8
            };

            v1.StateMachine = new StateMachine<MovingEntity>(v1) {CurrentState = new ReturningResources()};
            v2.StateMachine = new StateMachine<MovingEntity>(v2) {CurrentState = new ReturningResources()};
            v3.StateMachine = new StateMachine<MovingEntity>(v3) {CurrentState = new ReturningResources()};
            v4.StateMachine = new StateMachine<MovingEntity>(v4) {CurrentState = new ReturningResources()};

            MovingEntities = new List<MovingEntity> {v1, v2, v3, v4};
        }

        public void SetWalls() {
            Walls = new List<Wall> {
                new Wall(new Vector2D(2, 2), new Vector2D(Width - 2, 2), this),
                new Wall(new Vector2D(Width - 2, 2), new Vector2D(Width - 2, Height - 2), this),
                new Wall(new Vector2D(Width - 2, Height - 2), new Vector2D(2, Height - 2), this),
                new Wall(new Vector2D(2, Height - 2), new Vector2D(2, 2), this)
            };
        }

        public void TrySelectEntity(Vector2D v) {
            for (var i = 0; i < MovingEntities.Count; i++)
                if (MovingEntities[i].CloseEnough(MovingEntities[i].Position, v, 20)) {
                    if (SelectedEntityIndex != -1)
                        MovingEntities[SelectedEntityIndex].ExitPossession();
                    SelectedEntityIndex = i;
                    Graph.Path = MovingEntities[i].EnterPossession(Graph, TargetLoc);
                    Graph.NonSmoothenedPath = MovingEntities[i].NonSmoothenedPath;
                    Graph.ConsideredEdges = MovingEntities[i].ConsideredEdges;
                    return;
                }
            if (SelectedEntityIndex != -1)
                MovingEntities[SelectedEntityIndex].ExitPossession();

            SelectedEntityIndex = -1;
        }

        public void UpdatePath() {
            if (SelectedEntityIndex != -1) {
                Graph.Path = MovingEntities[SelectedEntityIndex].UpdatePath(TargetLoc);
                Graph.NonSmoothenedPath = MovingEntities[SelectedEntityIndex].NonSmoothenedPath;
                Graph.ConsideredEdges = MovingEntities[SelectedEntityIndex].ConsideredEdges;
            }
        }

        public void Update(float timeElapsed) {
            if (AutoUpdate) {
                if (timeElapsed % 20 == 0) {
                    Graph.Path = new List<Node>();
                    Graph.NonSmoothenedPath = new List<Node>();
                    Graph.ConsideredEdges = new List<Node>();
                    UpdatePath();
                }

                foreach (var me in MovingEntities)
                    me.Update(timeElapsed);

                foreach (var se in StaticEntities)
                    se.Update(timeElapsed);

                if (Resource.NoResources(this))
                    Resource.ResetResources(this);
                _container.UpdateResourcesLabel();
            }
        }

        public Graph GenerateGraph() {
            var g = new Graph(this);
            g.Nodes = g.Generate(new Node {WorldPosition = new Vector2D(20, 20)}, new List<Node>());
            return g;
        }

        public void Render(Graphics g) {
            Terrains.ForEach(e => e.Render(g));

            if (DebugGraph)
                Graph.Render(g);

            for (var i = 0; i < MovingEntities.Count; i++) MovingEntities[i].Render(g);

            foreach (var se in StaticEntities) se.Render(g);
            foreach (var w in Walls) w.Render(g);

            if (SelectedEntityIndex != -1)
                Target.ForEach(e => e.Render(g));
        }

        public void NextStep(float timeElapsed) {
            foreach (var me in MovingEntities) {
                me.NextStep(timeElapsed);
                _container.DebugInfo(DebugType.Velocity, me.Velocity.ToString());
            }
        }

        public List<Sheep> GetLivingSheep() {
            return MovingEntities.FindAll(me => me.GetType() == typeof(Sheep))
                .Cast<Sheep>()
                .ToList()
                .FindAll(s => s.Alive);
        }
    }
}