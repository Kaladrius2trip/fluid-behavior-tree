using FluidBehaviorTree.Runtime.Tasks;
using FluidBehaviorTree.Runtime.Tasks.Actions;

using NUnit.Framework;

namespace FluidBehaviorTree.Tests.Editor.Tasks.Actions
{
    public class ActionGenericTest
    {
        public class UpdateMethod
        {
            [Test]
            public void It_should_execute_a_generic_function()
            {
                ITask task = new ActionGeneric
                {
                    updateLogic = () => TaskStatus.Failure
                };

                Assert.AreEqual(TaskStatus.Failure, task.Update());
            }

            [Test]
            public void It_should_not_fail_without_a_generic_function()
            {
                ITask task = new ActionGeneric();
                task.Update();
            }

            [Test]
            public void It_should_execute_a_start_hook()
            {
                var test = 0;
                ITask task = new ActionGeneric
                {
                    startLogic = () => { test++; }
                };

                task.Update();

                Assert.AreEqual(1, test);
            }

            [Test]
            public void It_should_execute_a_init_hook()
            {
                var test = 0;
                ITask task = new ActionGeneric
                {
                    initLogic = () => { test++; }
                };

                task.Update();

                Assert.AreEqual(1, test);
            }

            [Test]
            public void It_should_execute_a_exit_hook()
            {
                var test = 0;
                ITask task = new ActionGeneric
                {
                    exitLogic = () => { test++; }
                };

                task.Update();

                Assert.AreEqual(1, test);
            }
        }
    }
}
