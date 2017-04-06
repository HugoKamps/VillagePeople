using System.Collections.Generic;
using System.Drawing;

namespace VillagePeople.Util
{
    public class Node
    {
        public List<Edge> Edges;
        public Vector2D WorldPosition;

        public Color Color = Color.Black;
        public int Size;

        public Node parent; // Used for path planning => points to the previous node in path

        public int gCost; // Travel cost
        public int hCost; // Heuristic cost
        public int fCost  // Actual cost
        {
            get { return gCost + hCost; }
        }

        public Node(int size = 10)
        {
            Size = size;
            Edges = new List<Edge>();
            WorldPosition = new Vector2D();
        }

        public void Render(Graphics g)
        {
            var leftCorner = WorldPosition.X - Size / 2;
            var rightCorner = WorldPosition.Y - Size / 2;

            Pen p = new Pen(Color, 2);
            g.DrawEllipse(p, new Rectangle((int)leftCorner, (int)rightCorner, Size, Size));
        }

        public void RenderEdges(Graphics g)
        {
            foreach (var e in Edges)
            {
                e.Render(g);
                e.Color = Color.Black;
            }
        }

        public void Connect(Node n1, int cost = 1)
        {
            var edge = new Edge() { Origin = this, Target = n1, Cost = cost };
            Edges.Add(edge);
            n1.Edges.Add(edge);
        }

        public bool IsConnected(Node n1)
        {
            if (n1 == null) { return true; } // A node is always connected with 'nothing'

            foreach (var edge in Edges)
                if ((edge.Origin == this && edge.Target == n1) || (edge.Origin == n1 && edge.Target == this))
                    return true;

            return false;
        }

        public override string ToString()
        {
            return "(" + WorldPosition.X + ", " + WorldPosition.Y + ")";
        }
    }
}
