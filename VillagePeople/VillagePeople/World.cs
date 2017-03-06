using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
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
        public int NodeSize = 50;
        public bool Debug = true;

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
        }

        public void Update(float timeElapsed)
        {
            foreach (MovingEntity me in _movingEntities)
            {
                me.Update(timeElapsed);
            }
        }

        public bool IsValidWorldPosition(Vector2D v1)
        {
            return (v1.X >= 0 && v1.X < Width && v1.Y >= 0 && v1.Y < Height);
        }

        public Graph GenerateGraph()
        {
            var g = new Graph();

            var startNode = new Node() { WorldPosition = new Vector2D(10, 10) };

            var notInspectedNodes = new List<Node>();
            var inspectedNodes = new List<Node>();

            notInspectedNodes.Add(startNode);

            while (notInspectedNodes.Count != 0)
            {
                var curr = notInspectedNodes.FirstOrDefault();

                List<Vector2D> WorldPositions = new List<Vector2D>();

                WorldPositions.Add(new Vector2D(curr.WorldPosition.X - NodeSize, curr.WorldPosition.Y - NodeSize));
                WorldPositions.Add(new Vector2D(curr.WorldPosition.X, curr.WorldPosition.Y - NodeSize));
                WorldPositions.Add(new Vector2D(curr.WorldPosition.X + NodeSize, curr.WorldPosition.Y - NodeSize));
                WorldPositions.Add(new Vector2D(curr.WorldPosition.X - NodeSize, curr.WorldPosition.Y));
                WorldPositions.Add(new Vector2D(curr.WorldPosition.X + NodeSize, curr.WorldPosition.Y));
                WorldPositions.Add(new Vector2D(curr.WorldPosition.X - NodeSize, curr.WorldPosition.Y + NodeSize));
                WorldPositions.Add(new Vector2D(curr.WorldPosition.X, curr.WorldPosition.Y + NodeSize));
                WorldPositions.Add(new Vector2D(curr.WorldPosition.X + NodeSize, curr.WorldPosition.Y + NodeSize));

                foreach (var worldPos in WorldPositions)
                {
                    if (IsValidWorldPosition(worldPos))
                    {
                        var node = CheckIfWorldPosExists(notInspectedNodes, worldPos);

                        if (node == null)
                        {
                            node = CheckIfWorldPosExists(inspectedNodes, worldPos);
                        }

                        if (node == null)
                        {
                            var n = new Node() { WorldPosition = worldPos };
                            notInspectedNodes.Add(n);
                            curr.TwoWayConnect(n);
                        }
                    }
                }

                notInspectedNodes.Remove(curr);
                inspectedNodes.Add(curr);
            }

            g.Nodes = inspectedNodes;
            return g;
        }

        private Node CheckIfWorldPosExists(List<Node> list, Vector2D worldPos)
        {
            Node n2 = null;
            foreach (var node in list)
            {
                if (worldPos.X == node.WorldPosition.X && worldPos.Y == node.WorldPosition.Y)
                {
                    n2 = node;
                }
            }

            return n2;
        }

        public void Render(Graphics g)
        {
            _movingEntities.ForEach(e => e.Render(g));
            if (Debug)
            {
                _graph.Render(g);
            }
        }

        public void NextStep(float timeElapsed)
        {
            foreach (MovingEntity me in _movingEntities)
            {
                me.NextStep(timeElapsed);
                _container.DebugInfo(DebugType.Velocity, me.Position.ToString() + "---" + me.Velocity.ToString() + me.targetSpeed + " " + me.Acceleration.ToString() + me.Acceleration.Length().ToString() + " " + (me.targetSpeed + me.Acceleration.Length()).ToString());
            }
        }
    }
}
