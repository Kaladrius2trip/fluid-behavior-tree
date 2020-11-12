using FluidBehaviorTree.Runtime.Decorators;
using FluidBehaviorTree.Runtime.TaskParents;
using FluidBehaviorTree.Runtime.Tasks;
using FluidBehaviorTree.Tests.Editor.Builders;

using NUnit.Framework;

namespace FluidBehaviorTree.Tests.Editor.Decorators
{
    public class RepeatForeverTest
    {
        public class UpdateMethod
        {
            private RepeatForever Setup(ITask child)
            {
                var repeat = new RepeatForever();
                repeat.AddChild(child);

                return repeat;
            }

            public class WhenChildReturnsFailure : UpdateMethod
            {
                [Test]
                public void It_should_return_continue()
                {
                    ITask stub = A.TaskStub().WithUpdateStatus(TaskStatus.Failure).Build();

                    ITaskComposite repeater = Setup(stub);
                    TaskStatus status = repeater.Update();

                    Assert.AreEqual(TaskStatus.Process, status);
                }
            }

            public class WhenChildReturnsSuccess : UpdateMethod
            {
                [Test]
                public void It_should_return_continue()
                {
                    ITask stub = A.TaskStub().WithUpdateStatus(TaskStatus.Success).Build();

                    ITaskComposite repeater = Setup(stub);
                    TaskStatus status = repeater.Update();

                    Assert.AreEqual(TaskStatus.Process, status);
                }
            }

            public class WhenChildReturnsContinue : UpdateMethod
            {
                [Test]
                public void It_should_return_continue()
                {
                    ITask stub = A.TaskStub().WithUpdateStatus(TaskStatus.Process).Build();

                    ITaskComposite repeater = Setup(stub);
                    TaskStatus status = repeater.Update();

                    Assert.AreEqual(TaskStatus.Process, status);
                }
            }
        }
    }
}
