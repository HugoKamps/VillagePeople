using System;
using System.Collections.Generic;
using System.Drawing;

namespace VillagePeople.Util
{
    class Graph
    {
        public List<Node> Nodes = new List<Node>();
        public const int NodeSize = 50;

        public Node getNodeByWorldPosition(Vector2D worldPos)
        {
            foreach (var node in Nodes)
            {
                if (worldPos.X > node.WorldPosition.X && worldPos.X < node.WorldPosition.X + 10 && worldPos.Y > node.WorldPosition.Y && worldPos.Y < node.WorldPosition.Y + 10)
                {
                    return node;
                }
            }
            return null;
        }

        public static List<Node> Generate(World w, Node n, List<Node> nodes)
        {
            if (n != null && Graph.GetNodeAtWorldPosition(nodes, n.WorldPosition) == null)
            {
                var Nodes = new List<Node>();
                getNeighborWorldPositions(n).ForEach(e => getValidWorldPositions(w, e, Nodes));
                nodes.Add(n);
                foreach (var node in Nodes)
                {
                    var diff = new Vector2D(Math.Abs(n.WorldPosition.X - node.WorldPosition.X), Math.Abs(n.WorldPosition.Y - node.WorldPosition.Y));
                    var smallest = new Vector2D(Math.Min(n.WorldPosition.X ,node.WorldPosition.X), Math.Min(n.WorldPosition.Y, node.WorldPosition.Y));
                    var center = smallest + (diff / 2);

                    if (isValidWorldPosition(w, center))
                    {
                        n.Connect(node);
                    }
                    Generate(w, node, nodes);
                }
                return nodes;
            }
            else return null;
        }

        public static bool isValidWorldPosition(World w, Vector2D v1)
        {
            if (!(v1.X >= 0 && v1.X < w.Width && v1.Y >= 0 && v1.Y < w.Height))
                return false;

            foreach (var se in w.staticEntities)
                if (se.IsWalkable(v1) == false)
                    return false;

            return true;
        }

        public static void getValidWorldPositions(World w, Vector2D worldPos, List<Node> Nodes)
        {
            if (!isValidWorldPosition(w, worldPos))
                return;

            if (Graph.GetNodeAtWorldPosition(Nodes, worldPos) == null) // Node at world position does not yet exist 
                Nodes.Add(new Node() { WorldPosition = worldPos });
        }

        public static List<Vector2D> getNeighborWorldPositions(Node n)
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
            Nodes.ForEach(e => e.Render(g));
        }

        public List<Vector2D> FindPath(Vector2D startWorldPos, Vector2D endWorldPos) { return FindPath(getNodeByWorldPosition(startWorldPos), getNodeByWorldPosition(endWorldPos)); }
        public List<Vector2D> FindPath(Node start, Node end)
        {
            throw new NotImplementedException();
        }
    }
}
