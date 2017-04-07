﻿using System.Drawing;

namespace VillagePeople.Util
{
    public class Edge
    {
        public Node Target;
        public Node Origin;
        public int Cost;
        public Color Color = Color.Gray;

        public void Render(Graphics g)
        {
            Pen p = new Pen(Color, 2);
            g.DrawLine(
                p, 
                (int)Origin.WorldPosition.X, 
                (int)Origin.WorldPosition.Y, 
                (int)Target.WorldPosition.X, 
                (int)Target.WorldPosition.Y
                );
        }

        public override string ToString()
        {
            return "f:" + Origin + " - t:" + Target + " c:" + Cost;
        }
    }
}
