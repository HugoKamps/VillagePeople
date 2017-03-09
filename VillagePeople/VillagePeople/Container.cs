using System;
using System.Reflection;
using System.Timers;
using System.Windows.Forms;
using VillagePeople.Util;
using Timer = System.Timers.Timer;

namespace VillagePeople
{
    public partial class Container : Form
    {
        private World _world;

        public const float Delta = 0.8f;

        public Container()
        {
            InitializeComponent();

            typeof(Panel).InvokeMember("DoubleBuffered", BindingFlags.SetProperty | BindingFlags.Instance | BindingFlags.NonPublic, null, GamePanel, new object[] { true });

            _world = new World(GamePanel.Width, GamePanel.Height, this);

            var timer = new Timer();
            timer.Elapsed += Timer_Elapsed;
            timer.Interval = 20;
            timer.Enabled = true;
        }

        public void DebugInfo(DebugType type, string value)
        {
            switch (type)
            {
                case DebugType.Velocity:
                    lblVelocity.Text = value;
                    Console.WriteLine(@"---" + value);
                    break;
                case DebugType.Position:
                    break;
            }
        }

        private void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            _world.Update(Delta);
            GamePanel.Invalidate();
        }

        private void GamePanel_Paint(object sender, PaintEventArgs e)
        {
            _world.Render(e.Graphics);
        }

        private void GamePanel_MouseClick(object sender, MouseEventArgs e)
        {
            _world.Target.Position = new Vector2D(e.X, e.Y);
        }

        private void cbDebug_CheckedChanged(object sender, EventArgs e)
        {
            _world.Debug = cbDebug.Checked;
        }

        private void cbUpdate_CheckedChanged(object sender, EventArgs e)
        {
            _world.AutoUpdate = cbUpdate.Checked;
        }
    }

    public enum DebugType
    {
        Velocity,
        Position
    }
}
