using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace VillagePeople.Util
{
    public class Graph
    {
        public const int NodeSize = 50;
        public List<Node> Nodes = new List<Node>();
        public List<Node> NonSmoothenedPath = new List<Node>();
        public List<Node> ConsideredEdges = new List<Node>();
        public List<Node> Path = new List<Node>();
        public World W;

        public Graph(World w)
        {
            W = w;
        }

        public Node GetNodeByWorldPosition(Vector2D worldPos)
            =>
                Nodes.FirstOrDefault(
                    node =>
                        worldPos.X > node.WorldPosition.X && worldPos.X < node.WorldPosition.X + 10 &&
                        worldPos.Y > node.WorldPosition.Y && worldPos.Y < node.WorldPosition.Y + 10);

        public Node GetClosestNode(Vector2D worldPos)
            => Nodes.Select(x => x).OrderBy(x => GetDistance(x.WorldPosition, worldPos)).First();

        // Pythagorean Theorem
        private float GetDistance(float a, float b) => (int)Math.Sqrt(Math.Pow(a, 2) + Math.Pow(b, 2));

        private float GetDistance(float oX, float oY, float tX, float tY)
            => GetDistance(Math.Abs(oX - tX), Math.Abs(oY - tY));

        private float GetDistance(Vector2D v1, Vector2D v2) => GetDistance(v1.X, v1.Y, v2.X, v2.Y);

        public List<Node> Generate(Node n, List<Node> nodes)
        {
            if (n != null && GetNodeAtWorldPosition(nodes, n.WorldPosition) == null)
            {
                var tempNodes = new List<Node>();
                GetNeighborWorldPositions(n).ForEach(e => GetValidWorldPositions(e, tempNodes));
                nodes.Add(n);
                foreach (var node in tempNodes)
                {
                    if (!IntersectsStaticObjects(n.WorldPosition, node.WorldPosition))
                        n.Connect(node);

                    Generate(node, nodes);
                }
                return nodes;
            }
            return null;
        }

        public bool IsValidWorldPosition(Vector2D v1)
        {
            if (!(v1.X >= 0 && v1.X < W.Width && v1.Y >= 0 && v1.Y < W.Height))
                return false;

            foreach (var se in W.StaticEntities)
                if (se.IsWalkable(v1) == false)
                    return false;

            return true;
        }


        public bool IntersectsStaticObjects(Vector2D begin, Vector2D end)
        {
            var line = new LinearEquation(begin, end);

            foreach (var entity in W.StaticEntities)
            {
                if (entity.Walkable)
                    continue;

                var unwalkableBox = new List<LinearEquation>
                {
                    // top left to bottom left
                    new LinearEquation(entity.UnwalkableSpace[0],
                        new Vector2D(entity.UnwalkableSpace[0].X, entity.UnwalkableSpace[1].Y)),
                    // top left to top right
                    new LinearEquation(entity.UnwalkableSpace[0],
                        new Vector2D(entity.UnwalkableSpace[1].X, entity.UnwalkableSpace[0].Y)),
                    // bottom right to bottom left
                    new LinearEquation(entity.UnwalkableSpace[1],
                        new Vector2D(entity.UnwalkableSpace[0].X, entity.UnwalkableSpace[1].Y)),
                    // bottom right to top right
                    new LinearEquation(entity.UnwalkableSpace[1],
                        new Vector2D(entity.UnwalkableSpace[1].X, entity.UnwalkableSpace[0].Y))
                };

                foreach (var func in unwalkableBox)
                    if (line.Intersects(func))
                        return true;
            }

            return false;
        }


        public void GetValidWorldPositions(Vector2D worldPos, List<Node> nodes)
        {
            if (!IsValidWorldPosition(worldPos))
                return;

            if (GetNodeAtWorldPosition(nodes, worldPos) == null) // Node at world position does not yet exist 
                nodes.Add(new Node { WorldPosition = worldPos });
        }

        public static List<Vector2D> GetNeighborWorldPositions(Node n)
        {
            return new List<Vector2D>
            {
                new Vector2D(n.WorldPosition.X - NodeSize, n.WorldPosition.Y - NodeSize),
                new Vector2D(n.WorldPosition.X, n.WorldPosition.Y - NodeSize),
                new Vector2D(n.WorldPosition.X + NodeSize, n.WorldPosition.Y - NodeSize),
                new Vector2D(n.WorldPosition.X - NodeSize, n.WorldPosition.Y),
                new Vector2D(n.WorldPosition.X + NodeSize, n.WorldPosition.Y),
                new Vector2D(n.WorldPosition.X - NodeSize, n.WorldPosition.Y + NodeSize),
                new Vector2D(n.WorldPosition.X, n.WorldPosition.Y + NodeSize),
                new Vector2D(n.WorldPosition.X + NodeSize, n.WorldPosition.Y + NodeSize)
            };
        }

        public static Node GetNodeAtWorldPosition(List<Node> list, Vector2D worldPos)
        {
            Node n2 = null;
            foreach (var node in list)
                if (worldPos.X == node.WorldPosition.X && worldPos.Y == node.WorldPosition.Y)
                    n2 = node;

            return n2;
        }

        public void Render(Graphics g)
        {
            foreach (var n in Nodes)
            {
                n.Render(g);
                n.RenderEdges(g);
            }

            DrawPath(g, ConsideredEdges, Color.Purple);
            DrawPath(g, NonSmoothenedPath, Color.Blue);
            DrawSmoothPath(g);
        }

        private void DrawSmoothPath(Graphics g)
        {
            foreach (var n in Path)
            {
                for (var k = 0; k <= n.SmoothEdges.Count - 1; k++)
                {
                    var e = n.SmoothEdges[k];
                    e.Color = Color.Red;
                    e.Render(g);
                    e.Color = Color.Gray;
                }

                n.Color = Color.Red;
                n.Render(g);
                n.Color = Color.Gray;
            }
        }

        private void DrawPath(Graphics g, List<Node> path, Color c)
        {
            foreach (var n in path)
            {
                foreach(var e in n.Edges)
                {
                    var pathContainsTarget =
                        path.Contains(path.FirstOrDefault(l => l.WorldPosition == e.Target.WorldPosition));
                    var pathContainsOrigin =
                        path.Contains(path.FirstOrDefault(l => l.WorldPosition == e.Origin.WorldPosition));
                    if (pathContainsOrigin && pathContainsTarget)
                    {
                        e.Color = c;
                        e.Render(g);
                        e.Color = Color.Gray;
                    }
                }

                n.Color = c;
                n.Render(g);
                n.Color = Color.Gray;
            }
        }
    }
}