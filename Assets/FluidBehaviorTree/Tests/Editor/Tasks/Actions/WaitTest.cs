using FluidBehaviorTree.Runtime.BehaviorTree;
using FluidBehaviorTree.Runtime.Tasks;
using FluidBehaviorTree.Runtime.Tasks.Actions;

using NUnit.Framework;

namespace FluidBehaviorTree.Tests.Editor.Tasks.Actions
{
    public class WaitTest
    {
        [Test]
        public void It_should_trigger_continue_on_first_tick()
        {
            ITask wait = new Wait();

            Assert.AreEqual(TaskStatus.Process, wait.Update());
        }

        [Test]
        public void It_should_trigger_success_on_2nd_tick()
        {
            ITask wait = new Wait();

            Assert.AreEqual(TaskStatus.Process, wait.Update());
            Assert.AreEqual(TaskStatus.Success, wait.Update());
        }

        [Test]
        public void It_should_trigger_success_after_2_ticks()
        {
            ITask wait = new Wait
            {
                turns = 2
            };

            Assert.AreEqual(TaskStatus.Process, wait.Update());
            Assert.AreEqual(TaskStatus.Process, wait.Update());
            Assert.AreEqual(TaskStatus.Success, wait.Update());
        }

        [Test]
        public void It_should_trigger_continue_after_tree_restarts()
        {
            var tree = new BehaviorTree();
            tree.AddNode(tree.Root, new Wait());

            Assert.AreEqual(TaskStatus.Process, tree.Tick());
            Assert.AreEqual(TaskStatus.Success, tree.Tick());
            Assert.AreEqual(TaskStatus.Process, tree.Tick());
        }
    }
}
