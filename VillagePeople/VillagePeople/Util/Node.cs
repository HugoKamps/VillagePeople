﻿using System.Collections.Generic;
using System.Drawing;

namespace VillagePeople.Util {
    public class Node {
        public Color Color = Color.Gray;
        public List<Edge> Edges;

        public int GCost; // Travel cost
        public int HCost; // Heuristic cost

        public Node Parent; // Used for path planning => points to the previous node in path
        public int Size;
        public List<Edge> SmoothEdges;
        public Vector2D WorldPosition;

        public Node(int size = 10) {
            Size = size;
            Edges = new List<Edge>();
            SmoothEdges = new List<Edge>();
            WorldPosition = new Vector2D();
        }

        public int FCost // Actual cost
            => GCost + HCost;

        public void Render(Graphics g) {
            var leftCorner = WorldPosition.X - (float) Size / 2;
            var rightCorner = WorldPosition.Y - (float) Size / 2;

            var p = new Pen(Color, 2);
            g.DrawEllipse(p, new Rectangle((int) leftCorner, (int) rightCorner, Size, Size));
        }

        public void RenderEdges(Graphics g) {
            foreach (var e in Edges) {
                e.Render(g);
                e.Color = Color.Gray;
            }
        }

        public void Connect(Node n1, int cost = 1) {
            var edge = new Edge {Origin = this, Target = n1, Cost = cost};
            Edges.Add(edge);
            n1.Edges.Add(edge);
        }

        public bool IsConnected(Node n1) {
            if (n1 == null) return true;

            foreach (var edge in Edges)
                if (edge.Origin == this && edge.Target == n1 || edge.Origin == n1 && edge.Target == this)
                    return true;

            foreach (var edge in SmoothEdges)
                if (edge.Origin == this && edge.Target == n1 || edge.Origin == n1 && edge.Target == this)
                    return true;

            return false;
        }

        public override string ToString() {
            return "(" + WorldPosition.X + ", " + WorldPosition.Y + ")";
        }

        internal void ConnectSmoothEdge(Node n1, int cost = 1) {
            var edge = new Edge {Origin = this, Target = n1, Cost = cost};
            SmoothEdges.Add(edge);
            n1.SmoothEdges.Add(edge);
        }
    }
}