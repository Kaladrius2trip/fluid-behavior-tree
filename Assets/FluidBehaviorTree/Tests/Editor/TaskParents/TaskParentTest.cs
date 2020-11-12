using FluidBehaviorTree.Runtime.TaskParents;
using FluidBehaviorTree.Runtime.Tasks;
using FluidBehaviorTree.Runtime.Tasks.Actions;
using FluidBehaviorTree.Tests.Editor.Builders;

using NUnit.Framework;

namespace FluidBehaviorTree.Tests.Editor.TaskParents
{
    public class TaskParentTest
    {
        private TaskParentExample _exampleParent;
        private ITaskComposite _taskParent;

        [SetUp]
        public void SetTaskParent()
        {
            _exampleParent = new TaskParentExample();
            _taskParent = _exampleParent;
        }

        public class TaskParentExample : TaskParentBase
        {
            public int childCount = -1;
            public int resetCount;
            public TaskStatus status = TaskStatus.Success;

            protected override int MaxChildren => childCount;

            public override void Reset()
            {
                resetCount += 1;
            }

            protected override TaskStatus OnUpdate()
            {
                return status;
            }
        }

        public class TaskExample : ActionBase
        {
            protected override TaskStatus OnUpdate()
            {
                return TaskStatus.Success;
            }
        }

        public class TriggeringReset : TaskParentTest
        {
            [Test]
            public void It_should_trigger_Reset_on_success()
            {
                _exampleParent.status = TaskStatus.Success;

                _taskParent.Update();

                Assert.AreEqual(1, _exampleParent.resetCount);
            }

            [Test]
            public void It_should_trigger_Reset_on_failure()
            {
                _exampleParent.status = TaskStatus.Failure;

                _taskParent.Update();

                Assert.AreEqual(1, _exampleParent.resetCount);
            }

            [Test]
            public void It_should_not_trigger_Reset_on_continue()
            {
                _exampleParent.status = TaskStatus.Process;

                _taskParent.Update();

                Assert.AreEqual(0, _exampleParent.resetCount);
            }
        }

        public class EnabledProperty : TaskParentTest
        {
            [Test]
            public void Returns_enabled_if_child()
            {
                _taskParent.AddChild(A.TaskStub().Build());

                Assert.IsTrue(_taskParent.Enabled);
            }

            [Test]
            public void Returns_disabled_if_child_and_set_to_disabled()
            {
                _taskParent.AddChild(A.TaskStub().Build());
                _taskParent.Enabled = false;

                Assert.IsFalse(_taskParent.Enabled);
            }
        }

        public class AddChildMethod : TaskParentTest
        {
            [Test]
            public void Adds_a_child()
            {
                _taskParent.AddChild(new TaskExample());

                Assert.AreEqual(1, _taskParent.Children.Count);
            }

            [Test]
            public void Adds_two_children()
            {
                _taskParent.AddChild(new TaskExample());
                _taskParent.AddChild(new TaskExample());

                Assert.AreEqual(2, _taskParent.Children.Count);
            }

            [Test]
            public void Ignores_overflowing_children()
            {
                _exampleParent.childCount = 1;

                _taskParent.AddChild(new TaskExample());
                _taskParent.AddChild(new TaskExample());

                Assert.AreEqual(1, _taskParent.Children.Count);
            }

            [Test]
            public void Does_not_add_disabled_children()
            {
                ITask child = A.TaskStub()
                               .WithEnabled(false)
                               .Build();

                _taskParent.AddChild(child);

                Assert.AreEqual(0, _taskParent.Children.Count);
            }
        }
    }
}
