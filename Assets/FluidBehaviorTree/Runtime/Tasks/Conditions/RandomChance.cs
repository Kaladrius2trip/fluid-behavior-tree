using UnityEngine;

namespace FluidBehaviorTree.Runtime.Tasks.Conditions
{
    public class RandomChance : ConditionBase
    {
        public float chance = 1;
        public float outOf = 1;
        public int seed;

        protected override bool OnUpdate()
        {
            Random.State oldState = Random.state;

            if (seed != 0)
            {
                Random.InitState(seed);
            }

            float percentage = chance / outOf;
            float rng = Random.value;

            if (seed != 0)
            {
                Random.state = oldState;
            }

            return rng <= percentage;
        }
    }
}
