using FluidBehaviorTree.Runtime.TaskParents;
using FluidBehaviorTree.Runtime.TaskParents.Composites;
using FluidBehaviorTree.Runtime.Tasks;
using FluidBehaviorTree.Tests.Editor.Builders;

using NSubstitute;

using NUnit.Framework;

namespace FluidBehaviorTree.Tests.Editor.TaskParents.Composites
{
    public class TaskSequenceTest
    {
        private ITask _childA;
        private ITask _childB;
        private ITaskComposite _sequence;

        [SetUp]
        public void SetupChild()
        {
            _sequence = new Sequence();

            _childA = CreateTaskStub();
            _sequence.AddChild(_childA);

            _childB = CreateTaskStub();
            _sequence.AddChild(_childB);
        }

        private ITask CreateTaskStub()
        {
            ITask task = A.TaskStub().Build();

            return task;
        }

        public class UpdateMethod : TaskSequenceTest
        {
            public class UpdateMethodMisc : UpdateMethod
            {
                [Test]
                public void It_should_run_update_on_all_success_child_tasks()
                {
                    _sequence.Update();

                    foreach (ITask c in _sequence.Children)
                    {
                        c.Received(1).Update();
                    }
                }

                [Test]
                public void It_should_retick_a_continue_node_when_update_is_rerun()
                {
                    _childB.Update().Returns(TaskStatus.Process);

                    _sequence.Update();
                    _sequence.Update();

                    _childB.Received(2).Update();
                }

                [Test]
                public void It_should_not_update_previous_nodes_when_a_continue_node_is_rerun()
                {
                    _childB.Update().Returns(TaskStatus.Process);

                    _sequence.Update();
                    _sequence.Update();

                    _childA.Received(1).Update();
                }
            }

            public class ReturnedStatusType : UpdateMethod
            {
                [Test]
                public void Returns_success_if_no_children()
                {
                    _sequence.RemoveAllChild();

                    Assert.AreEqual(TaskStatus.Success, _sequence.Update());
                }

                [Test]
                public void Should_be_success_if_all_child_tasks_pass()
                {
                    _sequence.Update();

                    Assert.AreEqual(TaskStatus.Success, _sequence.Update());
                }

                [Test]
                public void Should_be_failure_if_a_child_task_fails()
                {
                    _childA.Update().Returns(TaskStatus.Failure);

                    Assert.AreEqual(TaskStatus.Failure, _sequence.Update());
                }

                [Test]
                public void Should_be_continue_if_a_child_returns_continue()
                {
                    _childA.Update().Returns(TaskStatus.Process);

                    Assert.AreEqual(TaskStatus.Process, _sequence.Update());
                }
            }
        }
    }
}
