using System.Drawing;

namespace VillagePeople.Util
{
    class Edge
    {
        public Node Target;
        public Node Origin;
        public int Cost;
        public Color color = Color.Black;

        public void Render(Graphics g)
        {
            Pen p = new Pen(color, 2);
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
            return "f:" + Origin.ToString() + " - t:" + Target.ToString() + " c:" + Cost.ToString();
        }
    }
}
