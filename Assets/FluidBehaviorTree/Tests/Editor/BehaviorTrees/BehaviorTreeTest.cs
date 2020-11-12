using System.Collections.Generic;
using System.Linq;

using FluidBehaviorTree.Runtime.BehaviorTree;
using FluidBehaviorTree.Runtime.TaskParents.Composites;
using FluidBehaviorTree.Runtime.Tasks;
using FluidBehaviorTree.Tests.Editor.Builders;

using NSubstitute;

using NUnit.Framework;

using UnityEngine;

namespace FluidBehaviorTree.Tests.Editor.BehaviorTrees
{
    public class BehaviorTreeTest
    {
        private GameObject _gameObject;
        private BehaviorTree _tree;

        [SetUp]
        public void BeforeEach()
        {
            _gameObject = new GameObject("BT");
            _tree = new BehaviorTree();
        }

        [TearDown]
        public void AfterEach()
        {
            Object.DestroyImmediate(_gameObject);
        }

        public class SpliceMethod : BehaviorTreeTest
        {
            private GameObject _owner;
            private BehaviorTree _treeAlt;

            [SetUp]
            public void SetupTreeAlt()
            {
                _owner = new GameObject("BT");
                _treeAlt = new BehaviorTree();
            }

            [TearDown]
            public void TeardownTreeAlt()
            {
                Object.DestroyImmediate(_owner);
            }

            [Test]
            public void It_should_set_the_trees_root_as_a_child()
            {
                _tree.Splice(_tree.Root, _treeAlt);

                Assert.AreEqual(_treeAlt.Root, _tree.Root.Children[0]);
            }

            [Test]
            public void It_should_update_all_child_ParentTree_variables()
            {
                var task = Substitute.For<ITask>();
                task.Enabled.Returns(true);

                _treeAlt.AddNode(_treeAlt.Root, task);
                var nodes = new List<ITask> {_treeAlt.Root, task};

                _tree.Splice(_tree.Root, _treeAlt);

                nodes.ForEach(node => Assert.AreEqual(_tree, node.ParentTree));
            }
        }

        public class AddNodeMethod : BehaviorTreeTest
        {
            [Test]
            public void Parent_adds_the_child_reference()
            {
                var action = Substitute.For<ITask>();
                action.Enabled.Returns(true);

                _tree.AddNode(_tree.Root, action);

                Assert.AreEqual(action, _tree.Root.Children[0]);
            }
        }

        public class ResetMethod : BehaviorTreeTest
        {
            [Test]
            public void It_should_increase_the_tick_count()
            {
                _tree.Reset();

                Assert.AreEqual(1, _tree.TickCount);
            }

            [Test]
            public void It_should_call_End_on_active_nodes()
            {
                ITask task = A.TaskStub().Build();

                _tree.AddActiveTask(task);
                _tree.Reset();

                task.Received(1).End();
            }

            [Test]
            public void It_should_not_call_End_on_active_nodes_twice_if_called_again()
            {
                ITask task = A.TaskStub().Build();

                _tree.AddActiveTask(task);
                _tree.Reset();
                _tree.Reset();

                task.Received(1).End();
            }
        }

        public class TickMethod : BehaviorTreeTest
        {
            private ITask _action;

            [SetUp]
            public void ConfigureAction()
            {
                _action = Substitute.For<ITask>();
                _action.Enabled.Returns(true);
            }

            public class RootWithSingleNode : TickMethod
            {
                [SetUp]
                public void AddNode()
                {
                    _tree.AddNode(_tree.Root, _action);
                }

                [Test]
                public void Update_the_first_child_task_on_update()
                {
                    _tree.Tick();

                    _action.Received(1).Update();
                }

                [Test]
                public void Does_not_run_reset_on_2nd_run_if_continue_was_returned()
                {
                    _action.Update().Returns(TaskStatus.Process);

                    _tree.Tick();
                    _tree.Tick();

                    _action.Received(0).Reset();
                }

                [Test]
                public void Update_reruns_node_on_status_failure()
                {
                    _action.Update().Returns(TaskStatus.Failure);

                    _tree.Tick();
                    _tree.Tick();

                    _action.Received(2).Update();
                }

                [Test]
                public void Update_reruns_node_on_status_success()
                {
                    _action.Update().Returns(TaskStatus.Failure);

                    _tree.Tick();
                    _tree.Tick();

                    _action.Received(2).Update();
                }
            }

            public class RootWithSequenceTasks : TickMethod
            {
                private ITask _actionAlt;
                private Sequence _sequence;

                [SetUp]
                public void AddNode()
                {
                    _sequence = new Sequence();

                    _actionAlt = Substitute.For<ITask>();
                    _actionAlt.Enabled.Returns(true);

                    _tree.AddNode(_tree.Root, _sequence);
                    _tree.AddNode(_sequence, _action);
                    _tree.AddNode(_sequence, _actionAlt);
                }

                [Test]
                public void Runs_all_actions()
                {
                    _tree.Tick();

                    foreach (ITask c in _sequence.Children)
                    {
                        c.Received(1).Update();
                    }
                }

                [Test]
                public void Runs_all_actions_again_on_2nd_pass()
                {
                    _tree.Tick();
                    _tree.Tick();

                    foreach (ITask c in _sequence.Children)
                    {
                        c.Received(2).Update();
                    }
                }

                [Test]
                public void Does_not_run_reset_on_2nd_action_if_1st_fails()
                {
                    _action.Update().Returns(TaskStatus.Failure);

                    _tree.Tick();
                    _tree.Tick();

                    _actionAlt.Received(0).Reset();
                }
            }
        }

        public class AddActiveTaskMethod : BehaviorTreeTest
        {
            [Test]
            public void It_should_add_an_active_task()
            {
                ITask task = A.TaskStub().Build();

                _tree.AddActiveTask(task);

                Assert.IsTrue(_tree.ActiveTasks.Contains(task));
            }
        }

        public class RemoveActiveTaskMethod : BehaviorTreeTest
        {
            [Test]
            public void It_should_add_an_active_task()
            {
                ITask task = A.TaskStub().Build();

                _tree.AddActiveTask(task);
                _tree.RemoveActiveTask(task);

                Assert.IsFalse(_tree.ActiveTasks.Contains(task));
            }
        }
    }
}
