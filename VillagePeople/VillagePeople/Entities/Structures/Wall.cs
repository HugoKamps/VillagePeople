using System.Drawing;
using VillagePeople.Util;

namespace VillagePeople.Entities.Structures {
    public class Wall : BaseGameEntity {
        public Vector2D EndPosition;
        public Vector2D Normal;

        public Wall(Vector2D start, Vector2D end, World world)
            : base(start, world) {
            EndPosition = end;
            Normal = new Vector2D(-(end.Y - start.Y), end.X - start.X);
            Normal.Normalize();
        }

        public override void Update(float delta) { }

        public override void Render(Graphics g) {
            g.DrawLine(Pens.Red, Position.X, Position.Y, EndPosition.X, EndPosition.Y);
        }
    }
}