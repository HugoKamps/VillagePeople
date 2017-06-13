using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms.VisualStyles;
using VillagePeople.Behaviours;
using VillagePeople.StateMachine.States;
using VillagePeople.Util;

namespace VillagePeople.Entities.NPC {
    public class Sheep : MovingEntity {
        public bool Alive;
        public int BaseAmount;
        public Resource GatherRate;

        private Alignment _alignment;
        private Cohesion _cohesion;
        private Separation _separation;
        private WanderBehaviour _wanderBehaviour;
        private WallAvoidance _wallAvoidance;

        public Sheep(Vector2D position, World world) : base(position, world) {
            BaseAmount = 50;
            Velocity = new Vector2D(1, 1);
            Acceleration = new Vector2D(1, 1);
            TargetSpeed = Velocity.Length();
            MaxSpeed = 50;
            Scale = 20;
            Resource.Food = BaseAmount;
            GatherRate = new Resource {Food = 4};
            Alive = true;

            _alignment = new Alignment(this);
            _cohesion = new Cohesion(this);
            _separation = new Separation(this);
            _wanderBehaviour = new WanderBehaviour(this);
            _wallAvoidance = new WallAvoidance(this);

            SteeringBehaviours.Add(_wanderBehaviour);
            SteeringBehaviours.Add(_wallAvoidance);
            SteeringBehaviours.Add(_separation);
            SteeringBehaviours.Add(_alignment);
        }

        public override void Render(Graphics g) {
            var img =
                BitmapLoader.LoadBitmap(
                    Alive ? @"..\..\Resources\NPC\sheep.png" : @"..\..\Resources\NPC\sheep_dead.png",
                    GetType().ToString() + Alive);

            double leftCorner = Position.X - Scale;
            double rightCorner = Position.Y - Scale;
            double size = Scale * 2;

            g.DrawImage(img, new Rectangle((int) leftCorner, (int) rightCorner, (int) size, (int) size));

            if (World.DebugText && Alive)
                foreach (var sb in SteeringBehaviours) {
                    sb.RenderSB(g);
                }

            g.DrawString(Resource.Food.ToString(), new Font("Arial", 9), new SolidBrush(Color.Black), Position.X + 10,
                Position.Y + 10);
        }

        public override void Update(float timeElapsed) {
            if (World.MovingEntities.FindAll(m => m.GetType() == typeof(Villager) &&
                                                  m.StateMachine.CurrentState.GetType() == typeof(HerdingSheep) &&
                                                  Vector2D.Distance(Position, m.Position) < 10 && m.TargetSheep == this)
                    .Count > 0) {
                Alive = false;
            }

            if (Position.X < 0 || Position.X > World.Width || Position.Y < 0 || Position.Y > World.Width)
                World.MovingEntities.Remove(this);

            if (Alive) base.Update(timeElapsed);
        }

        public void Gather(BaseGameEntity e) {
            if (Resource.Food > GatherRate.Food)
                Resource += e.AddResource(GatherRate);
            else
                Resource += e.AddResource(Resource);
        }
    }
}