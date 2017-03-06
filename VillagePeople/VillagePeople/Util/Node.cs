using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VillagePeople.Util
{
    class Node
    {
        public List<Edge> Edges;
        public Vector2D WorldPosition;
        
        public Color Color = Color.Black;
        public int Size;

        public Node(int size = 20)
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

            Edges.ForEach(e => e.Render(g));
        }

        public void Connect(Node n1, int cost = 1)
        {
            Edges.Add(new Edge() { Origin = this, Target = n1, Cost = cost});
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
