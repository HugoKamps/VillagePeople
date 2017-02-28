using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VillagePeople.Entities;

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

        }

        public void Update(float delta)
        {

        }

        public void Render(Graphics graphics)
        {

        }
    }
}
