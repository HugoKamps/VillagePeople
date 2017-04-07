﻿using System.Collections.Generic;
using System.Drawing;
using VillagePeople.Entities;
using VillagePeople.Entities.NPC;
using VillagePeople.Entities.Structures;
using VillagePeople.Terrain;
using VillagePeople.Util;

namespace VillagePeople
{
    class World
    {
        public List<MovingEntity> MovingEntities = new List<MovingEntity>();
        public List<StaticEntity> StaticEntities = new List<StaticEntity>();
        public List<GameTerrain> Terrains = new List<GameTerrain>();
        private Container _container;
        private Graph _graph;

        public int Width { get; set; }
        public int Height { get; set; }

        public Villager Target { get; set; }

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

            _graph = GenerateGraph();
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

            Villager v1 = new Villager(new Vector2D(10, 10), this) { Color = Color.Red };
            MovingEntities.Add(v1);

            Villager v2 = new Villager(new Vector2D(150, 150), this) { Color = Color.Blue };
            MovingEntities.Add(v2);

            Villager v3 = new Villager(new Vector2D(200, 290), this) { Color = Color.Brown };
            MovingEntities.Add(v3);

            Villager v4 = new Villager(new Vector2D(450, 450), this) { Color = Color.Yellow };
            MovingEntities.Add(v4);

            Sheep s1 = new Sheep(new Vector2D(700, 300), this) { Color = Color.Gray };
            MovingEntities.Add(s1);

            Sheep s2 = new Sheep(new Vector2D(800, 200), this) { Color = Color.Gray };
            MovingEntities.Add(s2);

            Sheep s3 = new Sheep(new Vector2D(900, 700), this) { Color = Color.Gray };
            MovingEntities.Add(s3);

            Sheep s4 = new Sheep(new Vector2D(600, 400), this) { Color = Color.Gray };
            MovingEntities.Add(s4);
        }

        public void Update(float timeElapsed)
        {
            if (AutoUpdate)
            {
                foreach (MovingEntity me in MovingEntities)
                {
                    me.Update(timeElapsed);
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
            return new Graph { Nodes = Graph.Generate(this, new Node { WorldPosition = new Vector2D(10, 10) }, new List<Node>()) };
        }

        public void Render(Graphics g)
        {
            Terrains.ForEach(e => e.Render(g));

            if (DebugGraph)
                _graph.Render(g);

            MovingEntities.ForEach(e => e.Render(g));
            StaticEntities.ForEach(e => e.Render(g));

            g.DrawRectangle(new Pen(Color.Green), Width/2, Height/2, 30, 30);
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
