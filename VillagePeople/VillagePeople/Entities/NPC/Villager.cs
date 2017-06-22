using System.Drawing;
using SteeringCS.fuzzylogic;
using VillagePeople.Entities.Structures;
using VillagePeople.StateMachine;
using VillagePeople.StateMachine.States;
using VillagePeople.Util;

namespace VillagePeople.Entities.NPC {
    public class Villager : MovingEntity {
        private FuzzyModule _fm = new FuzzyModule();

        public Villager(Vector2D position, World world) : base(position, world) {
            var fVrc = _fm.CreateFlv("ResourceStatus");
            var low = fVrc.AddLeftShoulderSet("Low", 0, 20, 40);
            var some = fVrc.AddTriangularSet("Some", 20, 40, 60);
            var much = fVrc.AddTriangularSet("Much", 40, 60, 80);
            var veryMuch = fVrc.AddRightShoulderSet("VeryMuch", 60, 80, 100);

            var fVdist = _fm.CreateFlv("Distance");
            var close = fVdist.AddLeftShoulderSet("Close", 0, 100, 500);
            var far = fVdist.AddTriangularSet("Far", 100, 500, 900);
            var veryFar = fVdist.AddRightShoulderSet("VeryFar", 500, 900, 1000);

            var fVdes = _fm.CreateFlv("Desirability");
            var undesirable = fVdes.AddLeftShoulderSet("Undesirable", 0, 5, 10);
            var desirable = fVdes.AddTriangularSet("Desirable", 5, 10, 15);
            var veryDesirable = fVdes.AddRightShoulderSet("VeryDesirable", 10, 15, 20);

            _fm.AddRule(new FzAnd(low, close), veryDesirable);
            _fm.AddRule(new FzAnd(low, far), desirable);
            _fm.AddRule(new FzAnd(low, veryFar), desirable);

            _fm.AddRule(new FzAnd(some, close), veryDesirable);
            _fm.AddRule(new FzAnd(some, far), desirable);
            _fm.AddRule(new FzAnd(some, veryFar), undesirable);

            _fm.AddRule(new FzAnd(much, close), desirable);
            _fm.AddRule(new FzAnd(much, far), desirable);
            _fm.AddRule(new FzAnd(much, veryFar), undesirable);

            _fm.AddRule(new FzAnd(veryMuch, close), desirable);
            _fm.AddRule(new FzAnd(veryMuch, far), undesirable);
            _fm.AddRule(new FzAnd(veryMuch, veryFar), undesirable);

            StateMachine = new StateMachine<MovingEntity>(this);
            StateMachine.ChangeState(new ReturningResources());

            Velocity = new Vector2D(1, 1);
            Acceleration = new Vector2D(1, 1);
            TargetSpeed = Velocity.Length();
            Scale = 20;
            MaxInventorySpace = 10;

            Color = Color.Black;
        }

        public override Resource AddResource(Resource r) {
            var capacity = MaxInventorySpace - Resource.TotalResources();

            if (capacity <= 0)
                return r;

            if (capacity >= r.TotalResources())
                return base.AddResource(r);
            return base.AddResource(r.Cap(capacity));
        }

        public override void Render(Graphics g) {

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

            if (_possessed)
            {
                g.DrawRectangle(new Pen(Brushes.Red, 1), new Rectangle((int)leftCorner + 4, (int)rightCorner, img.Width - 2, img.Height + 6));
            }
        }

        public int GetNextResource() {
            double highest = -1;
            BaseGameEntity winner = null;
            foreach (var r in Resource.ClosestResources(World)) {
                if (r == null) continue;
                double value = 0;
                if (r.GetType() == typeof(Tree))
                    value = World.Resources.Wood;
                else if (r.GetType() == typeof(StoneMine))
                    value = World.Resources.Stone;
                else if (r.GetType() == typeof(GoldMine))
                    value = World.Resources.Gold;
                else if (r.GetType() == typeof(Sheep))
                    value = World.Resources.Food;

                _fm.Fuzzify("ResourceStatus", value);
                _fm.Fuzzify("Distance", Vector2D.Distance(Position, r.Position));

                var score = _fm.DeFuzzify("Desirability");
                if (!(score > highest)) continue;
                highest = score;
                winner = r;
            }

            return winner?.ID ?? 0;
        }

        public override void Update(float timeElapsed) {
            if (timeElapsed % 20 == 0) StateMachine.Update();
            base.Update(timeElapsed);
        }
    }
}