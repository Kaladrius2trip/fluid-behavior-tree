using System.Collections.Generic;

using FluidBehaviorTree.Runtime.TaskParents;
using FluidBehaviorTree.Runtime.TaskParents.Composites;
using FluidBehaviorTree.Runtime.Tasks;
using FluidBehaviorTree.Tests.Editor.Builders;

using NSubstitute;

using NUnit.Framework;

namespace FluidBehaviorTree.Tests.Editor.TaskParents.Composites
{
    public class SelectorTest
    {
        public class UpdateMethod
        {
            private ITaskComposite _selector;

            [SetUp]
            public void SetSelector()
            {
                _selector = new Selector();
            }

            public void CheckUpdateCalls(ITaskComposite selector, List<int> updateCalls)
            {
                for (var i = 0; i < updateCalls.Count; i++)
                {
                    ITask child = selector.Children[i];
                    if (updateCalls[i] >= 0)
                    {
                        child.Received(updateCalls[i]).Update();
                    }
                }
            }

            public class SingleNode : UpdateMethod
            {
                [Test]
                public void Returns_success_if_a_child_task_returns_success()
                {
                    _selector.AddChild(A.TaskStub().Build());

                    Assert.AreEqual(TaskStatus.Success, _selector.Update());
                }

                [Test]
                public void Returns_failure_if_a_child_task_returns_failure()
                {
                    ITask child = A.TaskStub().WithUpdateStatus(TaskStatus.Failure).Build();
                    _selector.AddChild(child);

                    Assert.AreEqual(TaskStatus.Failure, _selector.Update());
                }

                [Test]
                public void Returns_continue_if_a_child_task_returns_continue()
                {
                    ITask child = A.TaskStub().WithUpdateStatus(TaskStatus.Process).Build();
                    _selector.AddChild(child);

                    Assert.AreEqual(TaskStatus.Process, _selector.Update());
                }
            }

            public class MultipleNodes : UpdateMethod
            {
                [Test]
                public void Stops_on_continue()
                {
                    ITask childFailure = A.TaskStub().WithUpdateStatus(TaskStatus.Failure).Build();
                    _selector.AddChild(childFailure);

                    ITask childContinue = A.TaskStub().WithUpdateStatus(TaskStatus.Process).Build();
                    _selector.AddChild(childContinue);

                    ITask childSuccess = A.TaskStub().Build();
                    _selector.AddChild(childSuccess);

                    _selector.Update();

                    var updateCalls = new List<int> {1, 1, 0};
                    CheckUpdateCalls(_selector, updateCalls);
                }

                [Test]
                public void Reruns_the_same_node_if_it_returns_continue()
                {
                    ITask childFailure = A.TaskStub().WithUpdateStatus(TaskStatus.Failure).Build();
                    _selector.AddChild(childFailure);

                    ITask childContinue = A.TaskStub().WithUpdateStatus(TaskStatus.Process).Build();
                    _selector.AddChild(childContinue);

                    _selector.Update();
                    _selector.Update();

                    var updateCalls = new List<int> {1, 2};
                    CheckUpdateCalls(_selector, updateCalls);
                }

                [Test]
                public void Returns_failure_if_all_return_failure()
                {
                    ITask childA = A.TaskStub().WithUpdateStatus(TaskStatus.Failure).Build();
                    _selector.AddChild(childA);

                    ITask childB = A.TaskStub().WithUpdateStatus(TaskStatus.Failure).Build();
                    _selector.AddChild(childB);

                    Assert.AreEqual(TaskStatus.Failure, _selector.Update());
                }

                [Test]
                public void Stops_on_first_success_node()
                {
                    ITask childFailure = A.TaskStub().WithUpdateStatus(TaskStatus.Failure).Build();
                    _selector.AddChild(childFailure);

                    ITask childSuccessA = A.TaskStub().Build();
                    _selector.AddChild(childSuccessA);

                    ITask childSuccessB = A.TaskStub().Build();
                    _selector.AddChild(childSuccessB);

                    _selector.Update();

                    var updateCalls = new List<int> {1, 1, 0};
                    CheckUpdateCalls(_selector, updateCalls);
                }
            }
        }
    }
}
