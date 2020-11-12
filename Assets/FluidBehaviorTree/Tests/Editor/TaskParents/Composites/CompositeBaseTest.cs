using FluidBehaviorTree.Runtime.BehaviorTree;
using FluidBehaviorTree.Runtime.TaskParents.Composites;
using FluidBehaviorTree.Runtime.Tasks;
using FluidBehaviorTree.Tests.Editor.Builders;

using NSubstitute;

using NUnit.Framework;

using UnityEngine;

namespace FluidBehaviorTree.Tests.Editor.TaskParents.Composites
{
    public class CompositeBaseTest
    {
        private CompositeExample _composite;

        [SetUp]
        public void Set_composite()
        {
            _composite = new CompositeExample();
        }

        public class CompositeExample : CompositeBase
        {
            public TaskStatus status;

            public void SetChildIndex(int index)
            {
                ChildIndex = index;
            }

            protected override TaskStatus OnUpdate()
            {
                return status;
            }
        }

        public class AddChild : CompositeBaseTest
        {
            [Test]
            public void Does_not_add_disabled_nodes()
            {
                ITask child = A.TaskStub()
                               .WithEnabled(false)
                               .Build();

                _composite.AddChild(child);

                Assert.AreEqual(0, _composite.Children.Count);
            }
        }

        public class EndMethod : CompositeBaseTest
        {
            [Test]
            public void Does_not_fail_if_children_empty()
            {
                _composite.End();
            }

            [Test]
            public void Calls_end_on_current_child()
            {
                ITask child = A.TaskStub().Build();
                _composite.AddChild(child);

                _composite.End();
                child.Received(1).End();
            }
        }

        public class ResetMethod : CompositeBaseTest
        {
            private GameObject _go;

            [SetUp]
            public void BeforEach()
            {
                _go = new GameObject();
            }

            [TearDown]
            public void AfterEach()
            {
                Object.DestroyImmediate(_go);
            }

            [Test]
            public void Resets_child_node_pointer()
            {
                _composite.SetChildIndex(2);

                _composite.Reset();

                Assert.AreEqual(0, _composite.ChildIndex);
            }

            [Test]
            public void Resets_pointer_if_tick_count_changes()
            {
                var tree = new BehaviorTree();
                tree.AddNode(tree.Root, _composite);
                tree.AddNode(_composite, Substitute.For<ITask>());

                tree.Tick();
                _composite.SetChildIndex(2);
                tree.Tick();

                Assert.AreEqual(0, _composite.ChildIndex);
            }

            [Test]
            public void Does_not_reset_pointer_if_tick_count_does_not_change()
            {
                var task = Substitute.For<ITask>();
                var tree = new BehaviorTree();
                tree.AddNode(tree.Root, _composite);
                tree.AddNode(_composite, task);
                _composite.status = TaskStatus.Process;

                tree.Tick();
                _composite.SetChildIndex(1);
                tree.Tick();

                Assert.AreEqual(1, _composite.ChildIndex);
            }
        }
    }
}
