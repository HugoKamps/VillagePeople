﻿using System.Collections.Generic;
using System.Drawing;
using VillagePeople.Util;

namespace VillagePeople.Entities.Structures {
    internal class StoneMine : StaticEntity {
        public StoneMine(Vector2D position, World world) : base(position, world) {
            BaseAmount = 75;
            Resource.Stone = 75;
            Scale = 20;
            GatherRate = new Resource {Stone = 2};
            UnwalkableSpace = new List<Vector2D> {
                new Vector2D(position.X - Scale / 2 - 5, position.Y - Scale / 2 - 5), // Top Left
                new Vector2D(position.X + Scale / 2 + 5, position.Y + Scale / 2 + 5) // Bottom Right
            };

            ID = 1;
        }

        public void Gather(BaseGameEntity e) {
            if (Resource.Stone > GatherRate.Stone)
                Resource += e.AddResource(GatherRate);
            else
                Resource += e.AddResource(Resource);
        }

        public override void Update(float delta) {
            //if (Resource.Stone == 0)
            //    Walkable = false;
            //else
            //    Resource -= GatherRate;
        }

        public override void Render(Graphics g) {
            Image img;

            double size = Scale * 2;
            var leftCorner = Position.X - size / 2;
            var rightCorner = Position.Y - size / 2;

            if (Resource.Stone > 0) // Normal stone mine
                img = BitmapLoader.LoadBitmap(@"..\..\Resources\SE\stone.png", GetType() + "1");
            else // Broken stone mine
                img = BitmapLoader.LoadBitmap(@"..\..\Resources\SE\stone_broken.png", GetType() + "2");

            g.DrawImage(img, new Rectangle((int) leftCorner, (int) rightCorner, (int) size, (int) size));

            if (World.DebugText)
                g.DrawString(Resource.Stone.ToString(), new Font("Arial", 9), new SolidBrush(Color.Black),
                    Position.X + 10,
                    Position.Y + 10);
        }
    }
}