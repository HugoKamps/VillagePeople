using System;
using System.Collections.Generic;
using System.Drawing;
using VillagePeople.Entities;
using VillagePeople.Entities.NPC;
using VillagePeople.Entities.Structures;
using VillagePeople.Terrain;
using VillagePeople.Util;

namespace VillagePeople
{
    public class World
    {
        public List<MovingEntity> MovingEntities = new List<MovingEntity>();
        public List<StaticEntity> StaticEntities = new List<StaticEntity>();
        public List<GameTerrain> Terrains = new List<GameTerrain>();

        public int SelectedEntityIndex = -1;

        public Graph Graph;

        private Container _container;

        public int Width { get; set; }
        public int Height { get; set; }

        public List<Villager> Target = new List<Villager>();
        public Villager Leader { get; set; }

        public Resource Resources { get; set; }

        public bool DebugGraph = false;
        public bool DebugText = false;
        public bool AutoUpdate = false;

        public World(int width, int height, Container container)
        {
            Width = width;
            Height = height;

            _container = container;
            Resources = new Resource { Food = 0, Gold = 0, Stone = 0, Wood = 0 };

            Init();

            Graph = GenerateGraph();
        }

        public void Init()
        {
            GameTerrain grass = new GameTerrain(new Vector2D(0, 0));
            Terrains.Add(grass);
            GameTerrain water = new GameTerrain(new Vector2D(100, 0), TerrainType.Water);
            Terrains.Add(water);
            GameTerrain road = new GameTerrain(new Vector2D(0, 100), TerrainType.Road);
            Terrains.Add(road);

            Tree t1 = new Tree(new Vector2D(35, 35), this);
            Tree t2 = new Tree(new Vector2D(55, 35), this);
            Tree t3 = new Tree(new Vector2D(75, 35), this);

            StoneMine sm1 = new StoneMine(new Vector2D(350, 310), this);
            StoneMine sm2 = new StoneMine(new Vector2D(390, 310), this);
            StoneMine sm3 = new StoneMine(new Vector2D(430, 310), this);

            GoldMine gm1 = new GoldMine(new Vector2D(128, 280), this);
            GoldMine gm2 = new GoldMine(new Vector2D(168, 280), this);
            GoldMine gm3 = new GoldMine(new Vector2D(208, 280), this);

            StaticEntities = new List<StaticEntity> {t1, t2, t3, sm1, sm2, sm3, gm1, gm2, gm3};

            Villager v1 = new Villager(new Vector2D(150, 100), this)
            {
                Color = Color.CadetBlue,
                MaxInventorySpace = 12
            };

            Villager v2 = new Villager(new Vector2D(200, 90), this)
            {
                Color = Color.CadetBlue,
                MaxSpeed = 1200,
                MaxInventorySpace = 8
            };

            Villager v3 = new Villager(new Vector2D(200, 290), this) { Color = Color.CadetBlue };
            Villager v4 = new Villager(new Vector2D(450, 450), this) { Color = Color.CadetBlue };

            Sheep s1 = new Sheep(new Vector2D(700, 300), this) { Color = Color.CadetBlue };
            Sheep s2 = new Sheep(new Vector2D(800, 200), this) { Color = Color.Gray };
            Sheep s3 = new Sheep(new Vector2D(900, 700), this) { Color = Color.Gray };
            Sheep s4 = new Sheep(new Vector2D(600, 400), this) { Color = Color.Gray };

            MovingEntities = new List<MovingEntity> {v1, v2, v3, v4, s1, s2, s3, s4};

            /*Villager Target1 = new Villager(new Vector2D(), this)
            {
                Color = Color.DarkRed,
                Position = new Vector2D(40, 60, 40)
            };

            Target.Add(Target1);
            */
        }

        public void TrySelectEntity(Vector2D v)
        {
            for (int i = 0; i < MovingEntities.Count; i++)
            {
                if (MovingEntities[i].CloseEnough(MovingEntities[i].Position, v, 20))
                {
                    if (SelectedEntityIndex != -1)
                        MovingEntities[SelectedEntityIndex].ExitPossession();
                    SelectedEntityIndex = i;
                    Graph.path = MovingEntities[i].EnterPossession(Graph, Target[0].Position);
                    return;
                }
            }
            if (SelectedEntityIndex != -1)
                MovingEntities[SelectedEntityIndex].ExitPossession();

            SelectedEntityIndex = -1;
        }

        public void UpdatePath()
        {
            if (SelectedEntityIndex != -1)
            {
                Graph.path = MovingEntities[SelectedEntityIndex].UpdatePath(Target[0].Position);
            }
        }

        public void Update(float timeElapsed)
        {
            if (AutoUpdate)
            {
                if (timeElapsed % 20 == 0)
                {
                    Graph.path = new List<Node>();
                    UpdatePath();
                }

                foreach (MovingEntity me in MovingEntities)
                {
                    me.Update(timeElapsed);
                }

                foreach (StaticEntity se in StaticEntities)
                {
                    se.Update(timeElapsed);
                }

                _container.UpdateResourcesLabel();
            }
        }

        public Graph GenerateGraph()
        {
            return new Graph { Nodes = Graph.Generate(this, new Node { WorldPosition = new Vector2D(120, 120) }, new List<Node>()) };
        }

        public void Render(Graphics g)
        {
            Terrains.ForEach(e => e.Render(g));

            if (DebugGraph)
                Graph.Render(g);


            for (int i = 0; i < MovingEntities.Count; i++)
            {
                MovingEntities[i].Color = Color.CadetBlue;
                if (i == SelectedEntityIndex)
                    MovingEntities[i].Color = Color.Cyan;

                MovingEntities[i].Render(g);
            }
            StaticEntities.ForEach(e => e.Render(g));

            g.DrawRectangle(new Pen(Color.Green), Width/2, Height/2, 30, 30);

            if (SelectedEntityIndex != -1)
                Target.ForEach(e => e.Render(g));
        }

        public void NextStep(float timeElapsed)
        {
            foreach (MovingEntity me in MovingEntities)
            {
                me.NextStep(timeElapsed);
                _container.DebugInfo(DebugType.Velocity, me.Velocity.ToString());
            }
        }
    }
}
