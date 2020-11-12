using FluidBehaviorTree.Runtime.Tasks;
using FluidBehaviorTree.Runtime.Tasks.Conditions;

using NUnit.Framework;

namespace FluidBehaviorTree.Tests.Editor.Tasks.Conditions
{
    public class RandomChanceTest
    {
        [Test]
        public void It_should_create_a_random_chance()
        {
            ITask randomChance = new RandomChance
            {
                chance = 1,
                outOf = 2
            };

            Assert.IsNotNull(randomChance.Update());
        }
    }
}
