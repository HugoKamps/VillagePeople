using System;
using System.Reflection;
using System.Timers;
using System.Windows.Forms;
using VillagePeople.Entities.NPC;
using VillagePeople.Util;
using Timer = System.Timers.Timer;

namespace VillagePeople {
    public partial class Container : Form {
        public const float Delta = 1.0f;

        private Timer _timer = new Timer();
        private World _world;
        public float TimeElapsed;

        public Container() {
            InitializeComponent();

            typeof(Panel).InvokeMember("DoubleBuffered",
                BindingFlags.SetProperty | BindingFlags.Instance | BindingFlags.NonPublic, null, GamePanel,
                new object[] {true});

            _world = new World(GamePanel.Width, GamePanel.Height, this);

            _timer.Elapsed += Timer_Elapsed;
            _timer.Interval = 20;
            _timer.Enabled = true;
        }

        public void DebugInfo(DebugType type, string value) {
            switch (type) {
                case DebugType.Velocity:
                    //lblVelocity.Text = value;
                    Console.WriteLine("---" + value);
                    break;
                case DebugType.Position:
                    break;
            }
        }

        private void Timer_Elapsed(object sender, ElapsedEventArgs e) {
            TimeElapsed += Delta;
            _world.Update(TimeElapsed);
            GamePanel.Invalidate();
        }

        private void GamePanel_Paint(object sender, PaintEventArgs e) {
            _world.Render(e.Graphics);
        }

        private void GamePanel_MouseClick(object sender, MouseEventArgs e) {
            if (e.Button == MouseButtons.Left)
                _world.TrySelectEntity(new Vector2D(e.X, e.Y));
            else
                _world.TargetLoc = new Vector2D(e.X, e.Y);
        }

        private void cbDebug_CheckedChanged(object sender, EventArgs e) {
            _world.DebugGraph = cbDebug.Checked;
        }

        private void cbUpdate_CheckedChanged(object sender, EventArgs e) {
            _world.AutoUpdate = cbUpdate.Checked;
        }

        public void UpdateResourcesLabel() {
            var txt = "Wood: " + _world.Resources.Wood + " - " + "Stone: " + _world.Resources.Stone + " - " +
                      "Gold: " + _world.Resources.Gold + " - " + "Food: " + _world.Resources.Food;
            if (resourcesLabel.InvokeRequired)
                resourcesLabel.BeginInvoke((MethodInvoker) delegate { resourcesLabel.Text = txt; });
            else
                resourcesLabel.Text = txt;
        }

        private void cbDebugText_CheckedChanged(object sender, EventArgs e) {
            _world.DebugText = cbDebugText.Checked;
        }

        private void addSheepButton_Click(object sender, EventArgs e) {
            _world.MovingEntities.Add(new Sheep(new Vector2D(Width / 2.0f, Height / 2.0f), _world));
        }
    }

    public enum DebugType {
        Velocity,
        Position,
        Neighbours
    }
}