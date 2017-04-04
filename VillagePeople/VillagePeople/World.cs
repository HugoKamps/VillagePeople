using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using VillagePeople.Entities;
using VillagePeople.Entities.NPC;
using VillagePeople.Entities.Structures;
using VillagePeople.Terrain;
using VillagePeople.Util;

namespace VillagePeople
{
    public class World
    {
        private List<MovingEntity> _movingEntities = new List<MovingEntity>();
        public List<StaticEntity> staticEntities = new List<StaticEntity>();
        public List<GameTerrain> terrains = new List<GameTerrain>();
        private Container _container;
        private Graph _graph;

        private Pathfinder pf;

        public int Width { get; set; }
        public int Height { get; set; }

        public Villager Target { get; set; }
        public Villager Leader { get; set; }

        public bool Debug = false;
        public bool AutoUpdate = false;

        public World(int width, int height, Container container)
        {
            Width = width;
            Height = height;

            _container = container;

            _graph = GenerateGraph();
            pf = new Pathfinder();
            pf.grid = _graph;

            Init();
        }

        public void Init()
        {
            //GameTerrain grass = new GameTerrain(new Vector2D(0, 0), TerrainType.Grass);
            //terrains.Add(grass);
            //GameTerrain water = new GameTerrain(new Vector2D(100, 0), TerrainType.Water); 
            //terrains.Add(water);
            //GameTerrain road = new GameTerrain(new Vector2D(0, 100), TerrainType.Road);
            //terrains.Add(road);

            //Tree t1 = new Tree(new Vector2D(35, 35), this);
            //staticEntities.Add(t1);
            //Tree t2 = new Tree(new Vector2D(350, 310), this);
            //staticEntities.Add(t2);
            //Tree t3 = new Tree(new Vector2D(128, 280), this);
            //staticEntities.Add(t3);

            //Villager v1 = new Villager(new Vector2D(200, 100), this) { Color = Color.Red };
            //_movingEntities.Add(v1);

            //Villager v2 = new Villager(new Vector2D(150, 150), this) { Color = Color.Blue };
            //_movingEntities.Add(v2);

            //Villager v3 = new Villager(new Vector2D(200, 200), this) { Color = Color.Brown };
            //_movingEntities.Add(v3);

            //Villager v4 = new Villager(new Vector2D(30, 30), this) { Color = Color.Yellow };
            //_movingEntities.Add(v4);

            //Target = new Villager(new Vector2D(300, 300), this)
            //{
            //    Color = Color.DarkRed,
            //    Position = new Vector2D(40, 60, 40)
            //};


            Villager v1 = new Villager(new Vector2D(10, 10), this) { Color = Color.Yellow };
            _movingEntities.Add(v1);

            Villager v2 = new Villager(new Vector2D(170, 130), this) { Color = Color.Purple };
            _movingEntities.Add(v2);

            pf.seeker = v1;
            pf.target = v2;
            pf.Update();
        }

        public void Update(float timeElapsed)
        {
            if (AutoUpdate)
            {
                foreach (MovingEntity me in _movingEntities)
                {
                    me.Update(timeElapsed);
                    _container.DebugInfo(DebugType.Velocity, me.Velocity.ToString());
                }

                foreach (StaticEntity se in staticEntities)
                {
                    se.Update(timeElapsed);
                }
            }
        }

        public Graph GenerateGraph()
        {
            return new Graph() { Nodes = Graph.Generate(this, new Node() { WorldPosition = new Vector2D(120, 120) }, new List<Node>()) };
        }

        public void Render(Graphics g)
        {
            terrains.ForEach(e => e.Render(g));

            if (Debug)
                _graph.Render(g);

            _movingEntities.ForEach(e => e.Render(g));
            staticEntities.ForEach(e => e.Render(g));
            //Target.Render(g);
            //Leader.Render(g);
        }

        public void NextStep(float timeElapsed)
        {
            foreach (MovingEntity me in _movingEntities)
            {
                me.NextStep(timeElapsed);
                _container.DebugInfo(DebugType.Velocity, me.Velocity.ToString());
            }
        }
    }
}
