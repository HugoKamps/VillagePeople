using System.Drawing;
using SteeringCS.fuzzylogic;
using VillagePeople.StateMachine;
using VillagePeople.StateMachine.States;
using VillagePeople.Util;

namespace VillagePeople.Entities.NPC
{
    public class Villager : MovingEntity {
        FuzzyModule fm = new FuzzyModule();

        public Villager(Vector2D position, World world) : base(position, world)
        {
            StateMachine = new StateMachine<MovingEntity>(this);
            StateMachine.ChangeState(new ReturningResources());

            Velocity = new Vector2D(1, 1);
            Acceleration = new Vector2D(1, 1);
            TargetSpeed = Velocity.Length();
            Scale = 20;
            MaxInventorySpace = 10;

            Color = Color.Black;
        }

        public override Resource AddResource(Resource r)
        {
            var capacity = MaxInventorySpace - Resource.TotalResources();

            if (capacity <= 0)
                return r;

            if (capacity >= r.TotalResources())
                return base.AddResource(r);
            return base.AddResource(r.Cap(capacity));
        }

        public override void Render(Graphics g)
        {
            var img = BitmapLoader.LoadBitmap(@"..\..\Resources\NPC\villager.png", GetType().ToString());

            double leftCorner = Position.X - Scale;
            double rightCorner = Position.Y - Scale;
            double size = Scale * 2;

            var p = new Pen(Color, 4);
            var b = new SolidBrush(Color);

            var fontFamily = new FontFamily("Arial");
            var font = new Font(fontFamily, 10, FontStyle.Regular, GraphicsUnit.Pixel);

            var text = "Current state: " + StateMachine.CurrentState.GetType().Name + "\n" +
                       "Wood:" + Resource.Wood + "\n" +
                       "Stone: " + Resource.Stone + "\n" +
                       "Gold: " + Resource.Gold + "\n" +
                       "Food: " + Resource.Food;

            g.DrawImage(img, new Rectangle((int) leftCorner, (int) rightCorner, (int) size, (int) size));
            if (World.DebugText) g.DrawString(text, font, new SolidBrush(Color.Black), Position.X, Position.Y);
        }

        public override void Update(float timeElapsed)
        {
            if (timeElapsed % 20 == 0) StateMachine.Update();
            base.Update(timeElapsed);
        }
    }
}