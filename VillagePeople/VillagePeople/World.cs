using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VillagePeople.Entities;
using VillagePeople.Entities.NPC;
using VillagePeople.Util;

namespace VillagePeople
{
    class World
    {
        private List<MovingEntity> _movingEntities = new List<MovingEntity>();
        public int Width { get; set; }
        public int Height { get; set; }

        public World(int width, int height)
        {
            Width = width;
            Height = height;
            Init();
        }

        public void Init()
        {
            Villager v = new Villager(new Vector2D(10, 10), this) { Color = Color.Red };
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
    }
}
