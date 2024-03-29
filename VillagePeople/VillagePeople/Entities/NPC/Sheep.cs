﻿using System.Drawing;
using VillagePeople.Behaviours;
using VillagePeople.StateMachine.States;
using VillagePeople.Util;

namespace VillagePeople.Entities.NPC {
    public class Sheep : MovingEntity {
        public bool Alive;
        public int BaseAmount;
        public Resource GatherRate;


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

            var alignment = new Alignment(this);
            var separation = new Separation(this);
            var wanderBehaviour = new WanderBehaviour(this);
            var wallAvoidance = new WallAvoidance(this);

            SteeringBehaviours.Add(wanderBehaviour);
            SteeringBehaviours.Add(wallAvoidance);
            SteeringBehaviours.Add(separation);
            SteeringBehaviours.Add(alignment);

            ID = 3;
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
                foreach (var sb in SteeringBehaviours) sb.RenderSB(g);

            g.DrawString(Resource.Food.ToString(), new Font("Arial", 9), new SolidBrush(Color.Black), Position.X + 10,
                Position.Y + 10);
        }

        public override void Update(float timeElapsed) {
            if (World.MovingEntities.FindAll(m => m.GetType() == typeof(Villager) &&
                                                  m.StateMachine.CurrentState.GetType() == typeof(HerdingSheep) &&
                                                  Vector2D.Distance(Position, m.Position) < 10 && m.TargetSheep == this)
                    .Count > 0) Alive = false;

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