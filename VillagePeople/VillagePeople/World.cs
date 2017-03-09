using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using VillagePeople.Entities;
using VillagePeople.Entities.NPC;
using VillagePeople.Util;

namespace VillagePeople
{
    class World
    {
        private List<MovingEntity> _movingEntities = new List<MovingEntity>();
        private Container _container;
        private Graph _graph;

        public int Width { get; set; }
        public int Height { get; set; }

        public Villager Target { get; set; }
        public Villager Leader { get; set; }

        public int NodeSize = 50;

        public bool Debug = false;
        public bool AutoUpdate = true;

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
            Villager v1 = new Villager(new Vector2D(10, 10), this) { Color = Color.Red };
            _movingEntities.Add(v1);
            
            Villager v2 = new Villager(new Vector2D(150, 150), this) { Color = Color.Blue };
            _movingEntities.Add(v2);

            Villager v3 = new Villager(new Vector2D(200, 290), this) { Color = Color.Brown };
            _movingEntities.Add(v3);

            Villager v4 = new Villager(new Vector2D(450, 450), this) { Color = Color.Yellow };
            _movingEntities.Add(v4);

            Target = new Villager(new Vector2D(300, 300), this)
            {
                Color = Color.DarkRed,
                Position = new Vector2D(300, 300, 40)
            };
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
            }
        }

        public bool IsValidWorldPosition(Vector2D v1)
        {
            return v1.X >= 0 && v1.X < Width && v1.Y >= 0 && v1.Y < Height;
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

                if (curr != null)
                {
                    List<Vector2D> worldPositions = new List<Vector2D>
                    {
                        new Vector2D(curr.WorldPosition.X - NodeSize, curr.WorldPosition.Y - NodeSize),
                        new Vector2D(curr.WorldPosition.X, curr.WorldPosition.Y - NodeSize),
                        new Vector2D(curr.WorldPosition.X + NodeSize, curr.WorldPosition.Y - NodeSize),
                        new Vector2D(curr.WorldPosition.X - NodeSize, curr.WorldPosition.Y),
                        new Vector2D(curr.WorldPosition.X + NodeSize, curr.WorldPosition.Y),
                        new Vector2D(curr.WorldPosition.X - NodeSize, curr.WorldPosition.Y + NodeSize),
                        new Vector2D(curr.WorldPosition.X, curr.WorldPosition.Y + NodeSize),
                        new Vector2D(curr.WorldPosition.X + NodeSize, curr.WorldPosition.Y + NodeSize)
                    };

                    // Top Left
                    // Top Middle
                    // Top Right

                    // Middle Left
                    // Middle Right

                    // Bottom Left
                    // Bottom Middle
                    // Bottom Right

                    foreach (var worldPos in worldPositions)
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
            Target.Render(g);
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
