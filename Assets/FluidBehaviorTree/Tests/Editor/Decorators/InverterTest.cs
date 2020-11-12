using FluidBehaviorTree.Runtime.Decorators;
using FluidBehaviorTree.Runtime.TaskParents;
using FluidBehaviorTree.Runtime.Tasks;
using FluidBehaviorTree.Tests.Editor.Builders;

using NUnit.Framework;

namespace FluidBehaviorTree.Tests.Editor.Decorators
{
    public class InverterTest
    {
        public class UpdateMethod
        {
            [Test]
            public void Returns_failure_when_child_returns_success()
            {
                ITaskComposite inverter = new Inverter();
                inverter.AddChild(A.TaskStub().Build());

                Assert.AreEqual(TaskStatus.Failure, inverter.Update());
            }

            [Test]
            public void Returns_true_when_child_returns_false()
            {
                ITaskComposite inverter = new Inverter();
                inverter.AddChild(A.TaskStub().WithUpdateStatus(TaskStatus.Failure).Build());

                Assert.AreEqual(TaskStatus.Success, inverter.Update());
            }

            [Test]
            public void Returns_continue_when_child_returns_continue()
            {
                ITaskComposite inverter = new Inverter();
                inverter.AddChild(A.TaskStub().WithUpdateStatus(TaskStatus.Process).Build());

                Assert.AreEqual(TaskStatus.Process, inverter.Update());
            }
        }
    }
}
