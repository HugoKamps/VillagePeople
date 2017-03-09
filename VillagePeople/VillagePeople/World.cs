using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using VillagePeople.Entities;
using VillagePeople.Entities.NPC;
using VillagePeople.Entities.Structures;
using VillagePeople.Util;

namespace VillagePeople
{
    class World
    {
        private List<MovingEntity> _movingEntities = new List<MovingEntity>();
        private List<StaticEntity> _staticEntities = new List<StaticEntity>();
        private Container _container;
        private Graph _graph;

        public int Width { get; set; }
        public int Height { get; set; }

        public int NodeSize = 50;

        public bool Debug = false;
        public bool AutoUpdate = false;

        public World(int width, int height, Container container)
        {
            Width = width;
            Height = height;

            _container = container;

            Init();

            _graph = GenerateGraph();
        }

        public void Init()
        {
            Villager v = new Villager(new Vector2D(300, 300), this) { Color = Color.Red };
            _movingEntities.Add(v);

            Tree t = new Tree(new Vector2D(200, 200), this);
            _staticEntities.Add(t);
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

                foreach (StaticEntity se in _staticEntities)
                {
                    se.Update(timeElapsed);
                }
            }
        }

        public bool IsValidWorldPosition(Vector2D v1)
        {
            return (v1.X >= 0 && v1.X < Width && v1.Y >= 0 && v1.Y < Height);
        }

        public Graph GenerateGraph()
        {
            var startNode = new Node() { WorldPosition = new Vector2D(10, 10) };

            var notInspectedNodes = new List<Node>();
            var inspectedNodes = new List<Node>();

            notInspectedNodes.Add(startNode);

            while (notInspectedNodes.Count != 0)
            {
                var curr = notInspectedNodes.FirstOrDefault();

                List<Vector2D> WorldPositions = new List<Vector2D>();

                WorldPositions.Add(new Vector2D(curr.WorldPosition.X - NodeSize, curr.WorldPosition.Y - NodeSize)); // Top Left
                WorldPositions.Add(new Vector2D(curr.WorldPosition.X, curr.WorldPosition.Y - NodeSize));            // Top Middle
                WorldPositions.Add(new Vector2D(curr.WorldPosition.X + NodeSize, curr.WorldPosition.Y - NodeSize)); // Top Right

                WorldPositions.Add(new Vector2D(curr.WorldPosition.X - NodeSize, curr.WorldPosition.Y));            // Middle Left
                WorldPositions.Add(new Vector2D(curr.WorldPosition.X + NodeSize, curr.WorldPosition.Y));            // Middle Right

                WorldPositions.Add(new Vector2D(curr.WorldPosition.X - NodeSize, curr.WorldPosition.Y + NodeSize)); // Bottom Left
                WorldPositions.Add(new Vector2D(curr.WorldPosition.X, curr.WorldPosition.Y + NodeSize));            // Bottom Middle
                WorldPositions.Add(new Vector2D(curr.WorldPosition.X + NodeSize, curr.WorldPosition.Y + NodeSize)); // Bottom Right

                foreach (var worldPos in WorldPositions)
                {
                    if (!IsValidWorldPosition(worldPos))
                        continue;

                    var n1 = CheckIfWorldPosExists(notInspectedNodes, worldPos);
                    var n2 = CheckIfWorldPosExists(inspectedNodes, worldPos);

                    if (!curr.IsConnected(n2)) // If n2 is null it returns true
                    {
                        var cost = new Vector2D(Math.Abs(curr.WorldPosition.X - n2.WorldPosition.X), Math.Abs(curr.WorldPosition.Y - n2.WorldPosition.Y)).Length();
                        curr.Connect(n2, (int)cost);
                        continue;
                    }

                    if (n1 == null) // WorldPos is not in the not inspected nodes nor is it in the inspected nodes
                    {
                        var node = new Node() { WorldPosition = worldPos };
                        notInspectedNodes.Add(node);

                        var cost = new Vector2D(Math.Abs(curr.WorldPosition.X - worldPos.X), Math.Abs(curr.WorldPosition.Y - worldPos.Y)).Length();

                        curr.Connect(node, (int)cost);
                    }
                }

                notInspectedNodes.Remove(curr);
                inspectedNodes.Add(curr);
            }

            return new Graph() { Nodes = inspectedNodes };
        }

        private Node CheckIfWorldPosExists(List<Node> list, Vector2D worldPos)
        {
            Node n2 = null;
            foreach (var node in list)
                if (worldPos.X == node.WorldPosition.X && worldPos.Y == node.WorldPosition.Y)
                    n2 = node;

            return n2;
        }

        public void Render(Graphics g)
        {
            if (Debug)
                _graph.Render(g);

            _movingEntities.ForEach(e => e.Render(g));
            _staticEntities.ForEach(e => e.Render(g));
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
