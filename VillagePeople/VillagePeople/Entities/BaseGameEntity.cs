﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VillagePeople.Util;

namespace VillagePeople.Entities
{
    abstract class BaseGameEntity
    {
        public Vector2D Position { get; set; }
        public float Scale { get; set; }
        public World World { get; set; }

        public BaseGameEntity(Vector2D position, World world)
        {
            Position = position;
            World = world;
        }

        public abstract void Update(float delta);

        public virtual void Render(Graphics g)
        {
            
        }
    }
}