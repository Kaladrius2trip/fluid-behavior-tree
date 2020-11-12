using FluidBehaviorTree.Runtime.Tasks;
using FluidBehaviorTree.Runtime.Tasks.Conditions;

using NUnit.Framework;

namespace FluidBehaviorTree.Tests.Editor.Tasks.Conditions
{
    public class ConditionGenericTest
    {
        public class UpdateMethod
        {
            [Test]
            public void It_should_execute_a_generic_function()
            {
                ITask task = new ConditionGeneric
                {
                    updateLogic = () => false
                };

                Assert.AreEqual(TaskStatus.Failure, task.Update());
            }

            [Test]
            public void It_should_return_success_without_a_lambda()
            {
                ITask task = new ConditionGeneric();

                Assert.AreEqual(TaskStatus.Success, task.Update());
            }

            [Test]
            public void It_should_execute_a_start_hook()
            {
                var test = 0;
                ITask task = new ConditionGeneric
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
                ITask task = new ConditionGeneric
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
                ITask task = new ConditionGeneric
                {
                    exitLogic = () => { test++; }
                };

                task.Update();

                Assert.AreEqual(1, test);
            }
        }
    }
}
