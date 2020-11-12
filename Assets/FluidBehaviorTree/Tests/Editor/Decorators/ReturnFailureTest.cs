using FluidBehaviorTree.Runtime.Decorators;
using FluidBehaviorTree.Runtime.TaskParents;
using FluidBehaviorTree.Runtime.Tasks;
using FluidBehaviorTree.Tests.Editor.Builders;

using NUnit.Framework;

using Assert = UnityEngine.Assertions.Assert;

namespace FluidBehaviorTree.Tests.Editor.Decorators
{
    public class ReturnFailureTest
    {
        public class UpdateMethod
        {
            [Test]
            public void Returns_failure_on_child_failure()
            {
                ITaskComposite returnSuccess = new ReturnFailure();
                returnSuccess.AddChild(A.TaskStub().WithUpdateStatus(TaskStatus.Failure).Build());

                Assert.AreEqual(TaskStatus.Failure, returnSuccess.Update());
            }

            [Test]
            public void Returns_failure_on_child_success()
            {
                ITaskComposite returnSuccess = new ReturnFailure();
                returnSuccess.AddChild(A.TaskStub().WithUpdateStatus(TaskStatus.Success).Build());

                Assert.AreEqual(TaskStatus.Failure, returnSuccess.Update());
            }

            [Test]
            public void Returns_continue_on_child_continue()
            {
                ITaskComposite returnSuccess = new ReturnFailure();
                returnSuccess.AddChild(A.TaskStub().WithUpdateStatus(TaskStatus.Process).Build());

                Assert.AreEqual(TaskStatus.Process, returnSuccess.Update());
            }
        }
    }
}
