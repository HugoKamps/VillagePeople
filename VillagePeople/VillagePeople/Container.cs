using System.Windows.Forms;

namespace VillagePeople {
    public partial class Container : Form
    {
        private World _world;

        private System.Timers.Timer timer;
        public const float delta = 0.8f;

        public Container() {
            InitializeComponent();

            _world = new World(GamePanel.Width, GamePanel.Height);

            timer = new System.Timers.Timer();
            timer.Elapsed += Timer_Elapsed;
            timer.Interval = 20;
            timer.Enabled = true;
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
    }
}
