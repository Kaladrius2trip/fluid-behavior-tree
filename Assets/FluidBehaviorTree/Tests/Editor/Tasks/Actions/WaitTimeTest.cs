using FluidBehaviorTree.Runtime.Tasks;
using FluidBehaviorTree.Runtime.Tasks.Actions.WaitTime;

using NSubstitute;

using NUnit.Framework;

namespace FluidBehaviorTree.Tests.Editor.Tasks.Actions
{
    public class WaitTimeTest
    {
        public class UpdateMethod
        {
            [Test]
            public void It_should_return_continue_if_time_has_not_passed()
            {
                var timeMonitor = Substitute.For<ITimeMonitor>();
                timeMonitor.DeltaTime.Returns(0);
                ITask waitTime = new WaitTime(timeMonitor) {time = 1};

                Assert.AreEqual(TaskStatus.Process, waitTime.Update());
            }

            [Test]
            public void It_should_return_success_if_time_has_passed()
            {
                var timeMonitor = Substitute.For<ITimeMonitor>();
                timeMonitor.DeltaTime.Returns(2);
                ITask waitTime = new WaitTime(timeMonitor) {time = 1};

                Assert.AreEqual(TaskStatus.Success, waitTime.Update());
            }
        }

        public class ResetMethod
        {
            [Test]
            public void It_should_reset_time()
            {
                var timeMonitor = Substitute.For<ITimeMonitor>();
                ITask waitTime = new WaitTime(timeMonitor) {time = 1};

                timeMonitor.DeltaTime.Returns(2);
                waitTime.Update();
                waitTime.Reset();
                timeMonitor.DeltaTime.Returns(0);

                Assert.AreEqual(TaskStatus.Process, waitTime.Update());
            }
        }
    }
}
