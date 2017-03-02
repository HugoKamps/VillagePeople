using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using VillagePeople.Entities;
using VillagePeople.Entities.NPC;
using VillagePeople.Util;

namespace VillagePeople
{
    class World
    {
        private List<MovingEntity> _movingEntities = new List<MovingEntity>();
        private Container _container;
        public int Width { get; set; }
        public int Height { get; set; }

        public World(int width, int height, Container container)
        {
            Width = width;
            Height = height;
            _container = container;
            Init();
        }

        public void Init()
        {
            Villager v = new Villager(new Vector2D(300, 300), this) { Color = Color.Red };
            _movingEntities.Add(v);
        }

        public void Update(float timeElapsed)
        {
            foreach (MovingEntity me in _movingEntities)
            {
                me.Update(timeElapsed);
            }
        }

        public void Render(Graphics g)
        {
            _movingEntities.ForEach(e => e.Render(g));
        }

        public void NextStep(float timeElapsed)
        {
            foreach (MovingEntity me in _movingEntities)
            {
                me.NextStep(timeElapsed);
                _container.DebugInfo(DebugType.Velocity, me.Position.ToString() +  "---" + me.Velocity.ToString() + me.targetSpeed + " " + me.Acceleration.ToString() + me.Acceleration.Length().ToString() + " " + (me.targetSpeed + me.Acceleration.Length()).ToString());
            }
        }
    }
}
