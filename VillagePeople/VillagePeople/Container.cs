using System;
using System.Collections.Generic;
using System.Reflection;
using System.Timers;
using System.Windows.Forms;
using VillagePeople.Entities;
using VillagePeople.Util;
using Timer = System.Timers.Timer;

namespace VillagePeople
{
    public partial class Container : Form
    {
        private World _world;

        public const float Delta = 1.0f;
        public float TimeElapsed = 0;

        Timer timer = new Timer();

        public Container()
        {
            InitializeComponent();

            typeof(Panel).InvokeMember("DoubleBuffered", BindingFlags.SetProperty | BindingFlags.Instance | BindingFlags.NonPublic, null, GamePanel, new object[] { true });

            _world = new World(GamePanel.Width, GamePanel.Height, this);

            timer.Elapsed += Timer_Elapsed;
            timer.Interval = 20;
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
            }
        }

        private void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            TimeElapsed += Delta;
            _world.Update(TimeElapsed);
            GamePanel.Invalidate();
        }

        private void GamePanel_Paint(object sender, PaintEventArgs e)
        {
            _world.Render(e.Graphics);
        }

        private void GamePanel_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
                _world.Target[0].Position = new Vector2D(e.X, e.Y);
            else
                _world.Target[1].Position = new Vector2D(e.X, e.Y);
        }

        private void cbDebug_CheckedChanged(object sender, EventArgs e)
        {
            _world.Debug = cbDebug.Checked;
        }

        private void cbUpdate_CheckedChanged(object sender, EventArgs e)
        {
            _world.AutoUpdate = cbUpdate.Checked;
        }

        public void UpdateResourcesLabel()
        {
            var txt = "Wood: " + _world.Resources.Wood + " - " + "Stone: " + _world.Resources.Stone + " - " +
                                  "Gold: " + _world.Resources.Gold + " - " + "Food: " + _world.Resources.Food;
            if (this.resourcesLabel.InvokeRequired)
            {
                this.resourcesLabel.BeginInvoke((MethodInvoker)delegate () { this.resourcesLabel.Text = txt; });
            }
            else
            {
                this.resourcesLabel.Text = txt;
            }
        }
    }

    public enum DebugType
    {
        Velocity,
        Position,
        Neighbours
    }
}
