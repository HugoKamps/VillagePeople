using System.Collections.Generic;
using System.Linq;
using VillagePeople.Entities;
using VillagePeople.Entities.NPC;
using VillagePeople.Entities.Structures;

namespace VillagePeople.Util {
    public class Resource {
        public int Food;
        public int Gold;
        public int Stone;
        public int Wood;

        public int TotalResources() {
            return Wood + Food + Gold + Stone;
        }

        public Resource Cap(int cap) {
            var w = 0;
            var f = 0;
            var g = 0;
            var s = 0;

            if (Wood > 0)
                if (Wood > cap) {
                    w = cap;
                    Wood -= cap;
                    cap = 0;
                }
                else {
                    w += Wood;
                    Wood = 0;
                    cap -= Wood;
                }
            if (Food > 0)
                if (Food > cap) {
                    f = cap;
                    cap = 0;
                }
                else {
                    f += Food;
                    cap -= Food;
                }
            if (Gold > 0)
                if (Gold > cap) {
                    g = cap;
                    cap = 0;
                }
                else {
                    g += Gold;
                    cap -= Gold;
                }
            if (Stone > 0)
                if (Stone > cap) {
                    s = cap;
                    cap = 0;
                }
                else {
                    s += Stone;
                    cap -= Stone;
                }

            return new Resource {Wood = w, Food = f, Gold = g, Stone = s};
        }

        public static Resource operator +(Resource r1, Resource r2) {
            return new Resource {
                Wood = r1.Wood + r2.Wood,
                Food = r1.Food + r2.Food,
                Gold = r1.Gold + r2.Gold,
                Stone = r1.Stone + r2.Stone
            };
        }

        public static Resource operator -(Resource r1, Resource r2) {
            return new Resource {
                Wood = r1.Wood - r2.Wood,
                Food = r1.Food - r2.Food,
                Gold = r1.Gold - r2.Gold,
                Stone = r1.Stone - r2.Stone
            };
        }

        public static int GetLowestResource(MovingEntity me) {
            var resourceNumbers = new List<int> {
                me.World.Resources.Wood,
                me.World.Resources.Stone,
                me.World.Resources.Gold,
                me.World.Resources.Food
            };

            return resourceNumbers.FindIndex(m => m == resourceNumbers.Min());
        }

        public static void DepositResources(MovingEntity me) {
            me.World.Resources.Wood += me.Resource.Wood;
            me.Resource.Wood = 0;

            me.World.Resources.Stone += me.Resource.Stone;
            me.Resource.Stone = 0;

            me.World.Resources.Gold += me.Resource.Gold;
            me.Resource.Gold = 0;

            me.World.Resources.Food += me.Resource.Food;
            me.Resource.Food = 0;
        }

        public static bool IsResourceAvailable(World world, int index) {
            switch (index) {
                case 0:
                    return
                        world.StaticEntities.FindAll(m => m.GetType() == typeof(Tree))
                            .Count(x => x.Resource.Wood > 0) > 0;
                case 1:
                    return
                        world.StaticEntities.FindAll(m => m.GetType() == typeof(StoneMine))
                            .Count(x => x.Resource.Stone > 0) > 0;
                case 2:
                    return
                        world.StaticEntities.FindAll(m => m.GetType() == typeof(GoldMine))
                            .Count(x => x.Resource.Gold > 0) > 0;
                case 3:
                    return
                        world.MovingEntities.FindAll(m => m.GetType() == typeof(Sheep))
                            .Count(x => x.Resource.Food > 0) > 0;
                default:
                    return false;
            }
        }

        public static bool NoResources(World world) {
            var woodAvailable =
                world.StaticEntities.FindAll(m => m.GetType() == typeof(Tree)).Count(x => x.Resource.Wood > 0) > 0;
            var stoneAvailable =
                world.StaticEntities.FindAll(m => m.GetType() == typeof(StoneMine)).Count(x => x.Resource.Stone > 0) >
                0;
            var goldAvailable =
                world.StaticEntities.FindAll(m => m.GetType() == typeof(GoldMine)).Count(x => x.Resource.Gold > 0) > 0;
            var foodAvailable =
                world.MovingEntities.FindAll(m => m.GetType() == typeof(Sheep)).Count(x => x.Resource.Food > 0) > 0;

            return !(woodAvailable || stoneAvailable || goldAvailable || foodAvailable);
        }

        public static void ResetResources(World world) {
            foreach (var se in world.StaticEntities) {
                if (se.GetType() == typeof(Tree)) se.Resource.Wood = se.BaseAmount;
                if (se.GetType() == typeof(StoneMine)) se.Resource.Stone = se.BaseAmount;
                if (se.GetType() == typeof(GoldMine)) se.Resource.Gold = se.BaseAmount;
            }

            foreach (var me in world.MovingEntities.FindAll(me => me.GetType() == typeof(Sheep))) {
                var s = (Sheep) me;
                me.Resource.Food = s.BaseAmount;
            }
        }

        public static List<BaseGameEntity> ClosestResources(World world) {
            var tree = world.StaticEntities.Find(m => m.GetType() == typeof(Tree));
            var stone = world.StaticEntities.Find(m => m.GetType() == typeof(StoneMine));
            var gold = world.StaticEntities.Find(m => m.GetType() == typeof(GoldMine));
            var sheep = world.MovingEntities.Find(m => m.GetType() == typeof(Sheep));

            var resources = new List<BaseGameEntity>();
            if (IsResourceAvailable(world, 0)) resources.Add(tree);
            if (IsResourceAvailable(world, 1)) resources.Add(stone);
            if (IsResourceAvailable(world, 2)) resources.Add(gold);
            if (IsResourceAvailable(world, 3)) resources.Add(sheep);
            return resources;
        }
    }
}