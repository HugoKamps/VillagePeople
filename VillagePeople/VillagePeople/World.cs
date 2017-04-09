using System.Collections.Generic;
using System.Drawing;
using VillagePeople.Entities;
using VillagePeople.Entities.NPC;
using VillagePeople.Entities.Structures;
using VillagePeople.StateMachine;
using VillagePeople.StateMachine.States;
using VillagePeople.Terrain;
using VillagePeople.Util;

namespace VillagePeople
{
    public class World
    {
        private Container _container;
        public bool AutoUpdate = false;

        public bool DebugGraph = false;
        public bool DebugText = false;

        public Graph Graph;
        public List<MovingEntity> MovingEntities = new List<MovingEntity>();

        public int SelectedEntityIndex = -1;
        public List<StaticEntity> StaticEntities = new List<StaticEntity>();

        public List<Villager> Target = new List<Villager>();
        public Vector2D TargetLoc;
        public List<GameTerrain> Terrains = new List<GameTerrain>();

        public World(int width, int height, Container container)
        {
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

        public void Init()
        {
            TargetLoc = new Vector2D();
            GameTerrain.GenerateMap(Terrains);

            var t1 = new Tree(new Vector2D(25, 25), this);
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

            StaticEntities = new List<StaticEntity>
            {
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

            var v1 = new Villager(new Vector2D(150, 125), this)
            {
                MaxInventorySpace = 10,
                MaxSpeed = 400
            };

            var v2 = new Villager(new Vector2D(120, 200), this)
            {
                MaxSpeed = 300,
                MaxInventorySpace = 12
            };

            var v3 = new Villager(new Vector2D(200, 290), this)
            {
                MaxSpeed = 200,
                MaxInventorySpace = 14
            };

            var v4 = new Villager(new Vector2D(450, 450), this)
            {
                MaxSpeed = 500,
                MaxInventorySpace = 8
            };

            v1.StateMachine = new StateMachine<MovingEntity>(v1) {CurrentState = new CuttingWood()};
            v2.StateMachine = new StateMachine<MovingEntity>(v2) {CurrentState = new MiningStone()};
            v3.StateMachine = new StateMachine<MovingEntity>(v3) {CurrentState = new MiningGold()};
            v4.StateMachine = new StateMachine<MovingEntity>(v4) {CurrentState = new HerdingSheep()};

            var s1 = new Sheep(new Vector2D(700, 300), this) {Color = Color.CadetBlue};
            var s2 = new Sheep(new Vector2D(750, 200), this) {Color = Color.Gray};


            MovingEntities = new List<MovingEntity> {v1, v2, v3, v4, s1, s2};
        }

        public void InitTerrain()
        {
            float size = 50;
            float x = 0, y = 0;
            GameTerrain terrain;

            for (var i = 0; i < 12; i++)
            {
                for (var j = 0; j < 16; j++)
                {
                    if (j % 4 == 0) terrain = new GameTerrain(new Vector2D(x, y), TerrainType.Water);
                    else terrain = new GameTerrain(new Vector2D(x, y));
                    Terrains.Add(terrain);
                    x += size;
                }
                x = 0;
                y = i * size;
            }
        }

        public void TrySelectEntity(Vector2D v)
        {
            for (var i = 0; i < MovingEntities.Count; i++)
                if (MovingEntities[i].CloseEnough(MovingEntities[i].Position, v, 20))
                {
                    if (SelectedEntityIndex != -1)
                        MovingEntities[SelectedEntityIndex].ExitPossession();
                    SelectedEntityIndex = i;
                    Graph.Path = MovingEntities[i].EnterPossession(Graph, TargetLoc);
                    Graph.NonSmoothenedPath = MovingEntities[i].NonSmoothenedPath;
                    return;
                }
            if (SelectedEntityIndex != -1)
                MovingEntities[SelectedEntityIndex].ExitPossession();

            SelectedEntityIndex = -1;
        }

        public void UpdatePath()
        {
            if (SelectedEntityIndex != -1)
            {
                Graph.Path = MovingEntities[SelectedEntityIndex].UpdatePath(TargetLoc);
                Graph.NonSmoothenedPath = MovingEntities[SelectedEntityIndex].NonSmoothenedPath;
            }
        }

        public void Update(float timeElapsed)
        {
            if (AutoUpdate)
            {
                if (timeElapsed % 20 == 0)
                {
                    Graph.Path = new List<Node>();
                    Graph.NonSmoothenedPath = new List<Node>();
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

        public Graph GenerateGraph()
        {
            var g = new Graph(this);
            g.Nodes = g.Generate(new Node {WorldPosition = new Vector2D(20, 20)}, new List<Node>());
            return g;
        }

        public void Render(Graphics g)
        {
            Terrains.ForEach(e => e.Render(g));

            if (DebugGraph)
                Graph.Render(g);


            for (var i = 0; i < MovingEntities.Count; i++)
            {
                MovingEntities[i].Color = Color.CadetBlue;
                if (i == SelectedEntityIndex)
                    MovingEntities[i].Color = Color.Cyan;

                MovingEntities[i].Render(g);
            }
            StaticEntities.ForEach(e => e.Render(g));

            if (SelectedEntityIndex != -1)
                Target.ForEach(e => e.Render(g));
        }

        public void NextStep(float timeElapsed)
        {
            foreach (var me in MovingEntities)
            {
                me.NextStep(timeElapsed);
                _container.DebugInfo(DebugType.Velocity, me.Velocity.ToString());
            }
        }
    }
}