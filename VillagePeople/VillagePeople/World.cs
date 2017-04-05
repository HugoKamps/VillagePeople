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
        private Container _container;
        public Graph _graph;

        public Pathfinder pf;

        public int Width { get; set; }
        public int Height { get; set; }

        public Villager Target { get; set; }
        public Villager Leader { get; set; }

        public Resource Resources { get; set; }

        public bool Debug = false;
        public bool AutoUpdate = false;

        public World(int width, int height, Container container)
        {
            Width = width;
            Height = height;

            _container = container;
            Resources = new Resource { Food = 0, Gold = 0, Stone = 0, Wood = 0 };
            Target = new Villager(new Vector2D(Width/2, Height/2), this);

            Init();

            _graph = GenerateGraph();
            pf = new Pathfinder();
            pf.grid = _graph;
            pf.seeker = MovingEntities[0].Position;
            pf.target = Target.Position;
            pf.Update();
            _graph.path = pf.path;
        }

        public void Init()
        {
            //GameTerrain grass = new GameTerrain(new Vector2D(0, 0), TerrainType.Grass);
            //Terrains.Add(grass);
            //GameTerrain water = new GameTerrain(new Vector2D(100, 0), TerrainType.Water);
            //Terrains.Add(water);
            //GameTerrain road = new GameTerrain(new Vector2D(0, 100), TerrainType.Road);
            //Terrains.Add(road);

            Tree t1 = new Tree(new Vector2D(40, 40), this);
            StaticEntities.Add(t1);
            StoneMine t2 = new StoneMine(new Vector2D(345, 320), this);
            StaticEntities.Add(t2);
            GoldMine t3 = new GoldMine(new Vector2D(128, 280), this);
            StaticEntities.Add(t3);

            Villager v1 = new Villager(new Vector2D(20, 20), this) { Color = Color.Red };
            MovingEntities.Add(v1);

            //Villager v2 = new Villager(new Vector2D(200, 90), this) { Color = Color.Blue };
            //MovingEntities.Add(v2);

            //Villager v3 = new Villager(new Vector2D(200, 290), this) { Color = Color.Brown };
            //MovingEntities.Add(v3);

            //Villager v4 = new Villager(new Vector2D(450, 450), this) { Color = Color.Yellow };
            //MovingEntities.Add(v4);

            Sheep s1 = new Sheep(new Vector2D(700, 300), this) { Color = Color.Gray };
            MovingEntities.Add(s1);

            //Villager v3 = new Villager(new Vector2D(200, 200), this) { Color = Color.Brown };
            //_movingEntities.Add(v3);

            //Villager v4 = new Villager(new Vector2D(30, 30), this) { Color = Color.Yellow };
            //_movingEntities.Add(v4);

            Target = new Villager(new Vector2D(300, 300), this)
            {
                Color = Color.DarkRed,
                Position = new Vector2D(40, 60, 40)
            };
        }

        public void UpdatePath()
        {
            pf.target = Target.Position;
            pf.Update();
            _graph.path = pf.path;
        }

        public void Update(float timeElapsed)
        {
            if (AutoUpdate)
            {
                foreach (MovingEntity me in MovingEntities)
                {
                    if(me.GetType() == typeof(Villager)) me.Update(timeElapsed);
                    //_container.DebugInfo(DebugType.Velocity, me.Velocity.ToString());
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

            if (Debug)
                _graph.Render(g);
            
            MovingEntities.ForEach(e => e.Render(g));
            StaticEntities.ForEach(e => e.Render(g));
            Target.Render(g);
            //Leader.Render(g);
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
