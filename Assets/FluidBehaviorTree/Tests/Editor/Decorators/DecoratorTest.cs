using FluidBehaviorTree.Runtime.Decorators;
using FluidBehaviorTree.Runtime.TaskParents;
using FluidBehaviorTree.Runtime.Tasks;
using FluidBehaviorTree.Tests.Editor.Builders;

using NSubstitute;

using NUnit.Framework;

namespace FluidBehaviorTree.Tests.Editor.Decorators
{
    public class DecoratorTest
    {
        private DecoratorExample _testDecorator;
        private ITaskComposite _decorator;

        [SetUp]
        public void BeforeEach()
        {
            _testDecorator = new DecoratorExample();
            _decorator = _testDecorator;
            _decorator.AddChild(A.TaskStub().Build());
        }

        public class DecoratorExample : DecoratorBase
        {
            public TaskStatus status;

            protected override TaskStatus OnUpdate()
            {
                return status;
            }
        }

        public class EnabledProperty : DecoratorTest
        {
            [Test]
            public void Returns_true_if_child_is_set()
            {
                Assert.IsTrue(_decorator.Enabled);
            }

            [Test]
            public void Returns_false_if_child_is_set_but_set_to_false()
            {
                _decorator.Enabled = false;

                Assert.IsFalse(_decorator.Enabled);
            }
        }

        public class UpdateMethod : DecoratorTest
        {
            [Test]
            public void Sets_LastUpdate_to_returned_status_value()
            {
                _testDecorator.status = TaskStatus.Failure;

                _decorator.Update();

                Assert.AreEqual(TaskStatus.Failure, _decorator.LastStatus);
            }
        }

        public class EndMethod : DecoratorTest
        {
            [Test]
            public void Calls_end_on_child()
            {
                _decorator.End();

                _testDecorator.Child.Received(1).End();
            }
        }
    }
}
