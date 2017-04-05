using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace VillagePeople.Util
{
    public class Graph
    {
        public List<Node> Nodes = new List<Node>();
        public List<Node> path;
        public const int NodeSize = 50;

        public Node GetNodeByWorldPosition(Vector2D worldPos)
        {
            foreach (var node in Nodes)
            {
                if (worldPos.X > node.WorldPosition.X && worldPos.X < node.WorldPosition.X + 10 && worldPos.Y > node.WorldPosition.Y && worldPos.Y < node.WorldPosition.Y + 10)
                {
                    return node;
                }
            }
            return null;//GetClosestNode(worldPos);
        }

        public Node GetClosestNode(Vector2D worldPos)
        {
            return Nodes.Select(x => x).OrderBy(x => GetDistance(x.WorldPosition.X, x.WorldPosition.Y, worldPos.X, worldPos.Y)).First();
        }

        float GetDistance(float oX, float oY, float tX, float tY)
        {
            double xSqr = Math.Pow(Math.Abs(oX - tX), 2);
            double ySqr = Math.Pow(Math.Abs(oY - tY), 2);
            return (int)Math.Sqrt(xSqr + ySqr);
        }

        public static List<Node> Generate(World w, Node n, List<Node> nodes) {
            if (n != null && GetNodeAtWorldPosition(nodes, n.WorldPosition) == null)
            {
                var Nodes = new List<Node>();
                GetNeighborWorldPositions(n).ForEach(e => GetValidWorldPositions(w, e, Nodes));
                nodes.Add(n);
                foreach (var node in Nodes)
                {
                    var diff = new Vector2D(Math.Abs(n.WorldPosition.X - node.WorldPosition.X), Math.Abs(n.WorldPosition.Y - node.WorldPosition.Y));
                    var smallest = new Vector2D(Math.Min(n.WorldPosition.X ,node.WorldPosition.X), Math.Min(n.WorldPosition.Y, node.WorldPosition.Y));
                    var center = smallest + (diff / 2);

                    if (IsValidWorldPosition(w, center))
                    {
                        n.Connect(node);
                    }
                    Generate(w, node, nodes);
                }
                return nodes;
            }
            return null;
        }

        public static bool IsValidWorldPosition(World w, Vector2D v1)
        {
            if (!(v1.X >= 0 && v1.X < w.Width && v1.Y >= 0 && v1.Y < w.Height))
                return false;

            foreach (var se in w.StaticEntities)
                if (se.IsWalkable(v1) == false)
                    return false;

            return true;
        }

        public static void GetValidWorldPositions(World w, Vector2D worldPos, List<Node> nodes)
        {
            if (!IsValidWorldPosition(w, worldPos))
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
                if (path.FirstOrDefault(e => e.WorldPosition == n.WorldPosition) != null)
                {
                    n.Color = Color.Red;
                }

                n.Render(g);
            }
        }

        public List<Vector2D> FindPath(Vector2D startWorldPos, Vector2D endWorldPos) { return FindPath(GetNodeByWorldPosition(startWorldPos), GetNodeByWorldPosition(endWorldPos)); }
        public List<Vector2D> FindPath(Node start, Node end)
        {
            throw new NotImplementedException();
        }
    }
}
