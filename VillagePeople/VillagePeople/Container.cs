using System;
using System.Windows.Forms;

namespace VillagePeople
{
    public partial class Container : Form
    {
        private World _world;

        private System.Timers.Timer timer;
        public const float delta = 0.8f;

        public Container()
        {
            InitializeComponent();

            _world = new World(GamePanel.Width, GamePanel.Height, this);

            timer = new System.Timers.Timer();
            timer.Elapsed += Timer_Elapsed;
            timer.Interval = 60;
            timer.Enabled = true;
        }

        public void DebugInfo(DebugType type, string value)
        {
            switch (type)
            {
                case DebugType.Velocity:
                    //lblVelocity.Text = value;
                    Console.WriteLine("---" + value);
                    break;
                case DebugType.Position:
                    break;
                default:
                    break;
            }
        }

        private void Timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            _world.Update(delta);
            GamePanel.Invalidate();
        }

        private void GamePanel_Paint(object sender, PaintEventArgs e)
        {
            _world.Render(e.Graphics);
        }

        private void GamePanel_MouseClick(object sender, MouseEventArgs e)
        {
            _world.NextStep(delta);
        }
    }

    public enum DebugType
    {
        Velocity,
        Position
    }
}
