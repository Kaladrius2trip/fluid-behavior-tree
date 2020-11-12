using FluidBehaviorTree.Runtime.Decorators;
using FluidBehaviorTree.Runtime.TaskParents;
using FluidBehaviorTree.Runtime.Tasks;
using FluidBehaviorTree.Tests.Editor.Builders;

using NUnit.Framework;

namespace FluidBehaviorTree.Tests.Editor.Decorators
{
    public class DecoratorGenericTest
    {
        public class UpdateMethod
        {
            [Test]
            public void Can_invert_status_of_child()
            {
                ITaskComposite task = new DecoratorGeneric
                {
                    updateLogic = child =>
                    {
                        if (child.Update() == TaskStatus.Success)
                        {
                            return TaskStatus.Failure;
                        }

                        return TaskStatus.Success;
                    }
                };
                task.AddChild(A.TaskStub().Build());

                Assert.AreEqual(TaskStatus.Failure, task.Update());
            }

            [Test]
            public void Returns_child_status_without_update_logic()
            {
                ITaskComposite task = new DecoratorGeneric();
                task.AddChild(A.TaskStub().Build());

                Assert.AreEqual(TaskStatus.Success, task.Update());
            }
        }
    }
}
