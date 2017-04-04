﻿using System;
using System.Collections.Generic;
using System.Linq;
using VillagePeople.Behaviours;
using VillagePeople.Entities;
using VillagePeople.Util;

namespace VillagePeople.StateMachine.States
{
    class ReturningResources : State<MovingEntity>
    {
        public override void Enter(MovingEntity me)
        {
            // Move to townhall
            me.SteeringBehaviours = new List<SteeringBehaviour> {new SeekBehaviour(me, new Vector2D((float)me.World.Width/2, (float)me.World.Height/2))};
            Console.WriteLine("Return resources");
        }

        public override void Execute(MovingEntity me) {
            if (me.CloseEnough(me.Position, new Vector2D((float)me.World.Width / 2, (float)me.World.Height / 2)))
            {
                me.World.Resources.Wood += me.Resource.Wood;
                me.Resource.Wood = 0;

                me.World.Resources.Stone += me.Resource.Stone;
                me.Resource.Stone = 0;

                me.World.Resources.Gold += me.Resource.Gold;
                me.Resource.Gold = 0;

                me.World.Resources.Food += me.Resource.Food;
                me.Resource.Food = 0;

                List<int> resourceNumbers = new List<int> {
                    me.World.Resources.Wood,
                    me.World.Resources.Stone,
                    me.World.Resources.Gold,
                    me.World.Resources.Food
                };

                int index = resourceNumbers.FindIndex(m => m == resourceNumbers.Min());

                switch (index) {
                    case 0:
                        me.StateMachine.ChangeState(new CuttingWood());
                        break;
                    case 1:
                        me.StateMachine.ChangeState(new MiningStone());
                        break;
                    case 2:
                        me.StateMachine.ChangeState(new MiningGold());
                        break;
                    case 3:
                        me.StateMachine.ChangeState(new HerdingSheep());
                        break;
                    default:
                        me.StateMachine.ChangeState(new CuttingWood());
                        break;
                }
            }
        }

        public override void Exit(MovingEntity me) {
            Console.WriteLine("Go to resource");
        }
    }
}