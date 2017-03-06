using System;
using System.Collections.Generic;
using System.Drawing;

namespace VillagePeople.Util
{
    class Graph
    {
        public List<Node> Nodes = new List<Node>();
        public int NodeSize = 50;

        public Node GetNodeByWorldPosition(Vector2D worldPos)
        {
            foreach (var node in Nodes)
            {
                if (worldPos.X >= node.WorldPosition.X && worldPos.X <= node.WorldPosition.X + 10 && worldPos.Y >= node.WorldPosition.Y && worldPos.Y <= node.WorldPosition.Y + 10)
                {
                    return node;
                }
            }
            return null;
        }

        public void Render(Graphics g)
        {
            Nodes.ForEach(e => e.Render(g));
        }

        public List<Vector2D> FindPath(Vector2D startWorldPos, Vector2D endWorldPos) { return FindPath(GetNodeByWorldPosition(startWorldPos), GetNodeByWorldPosition(endWorldPos)); }
        public List<Vector2D> FindPath(Node start, Node end)
        {
            throw new NotImplementedException();
        }
    }
}
