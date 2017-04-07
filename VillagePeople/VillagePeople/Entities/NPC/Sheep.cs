using System.Drawing;
using VillagePeople.Behaviours;
using VillagePeople.Util;

namespace VillagePeople.Entities.NPC
{
    class Sheep : MovingEntity
    {
        public Color Color;
        public string Path;
        public Sheep(Vector2D position, World world) : base(position, world)
        {
            Velocity = new Vector2D(1, 1);
            Acceleration = new Vector2D(1, 1);
            TargetSpeed = Velocity.Length();
            MaxSpeed = 50;
            Scale = 20;
            Resource.Food = 50;
            Path = @"..\..\Resources\sheep.png";
            SetWander();
        }

        public override void Render(Graphics g)
        {
            double leftCorner = Position.X - Scale;
            double rightCorner = Position.Y - Scale;
            double size = Scale * 4;

            var p = new Pen(Color, 4);
            var b = new SolidBrush(Color);

            g.DrawImage(new Bitmap(Path), new Rectangle((int)leftCorner, (int)rightCorner, (int)size, (int)size));

            //g.FillEllipse(b, new Rectangle((int)leftCorner, (int)rightCorner, (int)size, (int)size));
            g.DrawLine(p, (int)Position.X, (int)Position.Y, (int)Position.X + (int)Velocity.X, (int)Position.Y + (int)Velocity.Y);
            g.DrawString(Resource.Food.ToString(), new Font("Arial", 9), new SolidBrush(Color.Black), Position.X, Position.Y);

        }

        public override void Update(float timeElapsed) {
            SetWander();
            SteeringBehaviour.TagNeighbors(this, World.MovingEntities, 5);
            if (World.MovingEntities.FindAll(m => m.Tagged && m.GetType() == typeof(Villager)).Count > 0) {
                Path = @"..\..\Resources\sheep_dead.png";
                MaxSpeed = 0;
            }

            if (Resource.Food == 0) {
                World.MovingEntities.Remove(this);
            }
            Vector2D steering = SteeringBehaviour.CalculateWTS(SteeringBehaviours, MaxSpeed);
            steering /= Mass;

            Vector2D acceleration = steering;
            acceleration *= timeElapsed;
            Velocity += acceleration;

            Velocity *= timeElapsed;
            Position += Velocity;
        }

        public void Gather(BaseGameEntity e)
        {
            if (Resource.Gold > e.World.StaticEntities[0].GatherRate.Gold)
                Resource += e.AddResource(e.World.StaticEntities[0].GatherRate);
            else
                Resource += e.AddResource(Resource);
        }
    }
}
