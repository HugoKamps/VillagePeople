namespace VillagePeople.Util
{
    public class Resource
    {
        public int Wood;
        public int Food;
        public int Gold;
        public int Stone;

        public int TotalResources()
        {
            return Wood + Food + Gold + Stone;
        }

        public Resource Cap(int cap)
        {
            int w = 0;
            int f = 0;
            int g = 0;
            int s = 0;

            if (Wood > 0)
            {
                if (Wood > cap)
                { w = cap; Wood -= cap; cap = 0; }
                else
                { w += Wood; Wood = 0; cap -= Wood; }
            }
            if (Food > 0)
            {
                if (Food > cap)
                { f = cap; cap = 0; }
                else
                { f += Food; cap -= Food; }
            }
            if (Gold > 0)
            {
                if (Gold > cap)
                { g = cap; cap = 0; }
                else
                { g += Gold; cap -= Gold; }
            }
            if (Stone > 0)
            {
                if (Stone > cap)
                { s = cap; cap = 0; }
                else
                { s += Stone; cap -= Stone; }
            }

            return new Resource { Wood = w, Food = f, Gold = g, Stone = s };
        }

        public static Resource operator +(Resource r1, Resource r2)
        {
            return new Resource {
                Wood = r1.Wood + r2.Wood,
                Food = r1.Food + r2.Food,
                Gold = r1.Gold + r2.Gold,
                Stone = r1.Stone + r2.Stone
            };
        }

        public static Resource operator -(Resource r1, Resource r2)
        {
            return new Resource {
                Wood = r1.Wood - r2.Wood,
                Food = r1.Food - r2.Food,
                Gold = r1.Gold - r2.Gold,
                Stone = r1.Stone - r2.Stone
            };
        }
    }
}
