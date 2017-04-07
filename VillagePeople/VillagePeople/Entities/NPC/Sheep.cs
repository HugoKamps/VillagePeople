using System.Drawing;
using VillagePeople.Behaviours;
using VillagePeople.Util;

namespace VillagePeople.Entities.NPC
{
    class Sheep : MovingEntity
    {
        public string Path;
        public Resource GatherRate;

        public Sheep(Vector2D position, World world) : base(position, world)
        {
            Velocity = new Vector2D(1, 1);
            Acceleration = new Vector2D(1, 1);
            TargetSpeed = Velocity.Length();
            MaxSpeed = 50;
            Scale = 20;
            Resource.Food = 50;
            GatherRate = new Resource { Food = 4 };
            Path = @"..\..\Resources\NPC\sheep.png";
        }

        public override void Render(Graphics g)
        {
            double leftCorner = Position.X - Scale;
            double rightCorner = Position.Y - Scale;
            double size = Scale * 2;

            var p = new Pen(Color, 4);
            var b = new SolidBrush(Color);

            g.DrawImage(new Bitmap(Path), new Rectangle((int)leftCorner, (int)rightCorner, (int)size, (int)size));

            g.DrawLine(p, (int)Position.X, (int)Position.Y, (int)Position.X + (int)Velocity.X, (int)Position.Y + (int)Velocity.Y);
            g.DrawString(Resource.Food.ToString(), new Font("Arial", 9), new SolidBrush(Color.Black), Position.X + 10, Position.Y + 10);

        }

        public override void Update(float timeElapsed) {
            SetWander(timeElapsed);
            SteeringBehaviour.TagNeighbors(this, World.MovingEntities, 5);
            if (World.MovingEntities.FindAll(m => m.Tagged && m.GetType() == typeof(Villager)).Count > 0) {
                Path = @"..\..\Resources\NPC\sheep_dead.png";
                MaxSpeed = 0;
            }

            if (Resource.Food == 0) {
                World.MovingEntities.Remove(this);
            }
            base.Update(timeElapsed);
        }

        public void Gather(BaseGameEntity e)
        {
            if (Resource.Food > GatherRate.Food)
                Resource += e.AddResource(GatherRate);
            else
                Resource += e.AddResource(Resource);
        }
    }
}
