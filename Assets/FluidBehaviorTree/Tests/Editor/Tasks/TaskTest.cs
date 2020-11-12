using FluidBehaviorTree.Runtime.BehaviorTree;
using FluidBehaviorTree.Runtime.Tasks;
using FluidBehaviorTree.Runtime.Tasks.Actions;

using NSubstitute;

using NUnit.Framework;

using UnityEngine;

namespace FluidBehaviorTree.Tests.Editor.Tasks
{
    public class TaskTest
    {
        public class TaskExample : ActionBase
        {
            public TaskStatus status = TaskStatus.Success;
            public int StartCount { get; private set; }
            public int InitCount { get; private set; }
            public int ExitCount { get; private set; }

            protected override TaskStatus OnUpdate()
            {
                return status;
            }

            protected override void OnStart()
            {
                StartCount++;
            }

            protected override void OnInit()
            {
                InitCount++;
            }

            protected override void OnExit()
            {
                ExitCount++;
            }
        }

        public class UpdateMethod
        {
            private TaskExample _testNode;
            private ITask _node;

            [SetUp]
            public void SetUpNode()
            {
                _node = _testNode = new TaskExample();
            }

            public class RegisteringContinueNodes : UpdateMethod
            {
                [Test]
                public void It_should_call_BehaviorTree_AddActiveTask_on_continue()
                {
                    var tree = Substitute.For<IBehaviorTree>();
                    _testNode.status = TaskStatus.Process;
                    _node.ParentTree = tree;

                    _node.Update();

                    tree.Received(1).AddActiveTask(_node);
                }

                [Test]
                public void It_should_not_call_BehaviorTree_AddActiveTask_on_continue_twice()
                {
                    var tree = Substitute.For<IBehaviorTree>();
                    _testNode.status = TaskStatus.Process;
                    _node.ParentTree = tree;

                    _node.Update();
                    _node.Update();

                    tree.Received(1).AddActiveTask(_node);
                }

                [Test]
                public void It_should_call_BehaviorTree_AddActiveTask_again_after_Reset()
                {
                    var tree = Substitute.For<IBehaviorTree>();
                    _testNode.status = TaskStatus.Process;
                    _node.ParentTree = tree;

                    _node.Update();
                    _node.Reset();
                    _node.Update();

                    tree.Received(2).AddActiveTask(_node);
                }

                [Test]
                public void It_should_call_BehaviorTree_RemoveActiveTask_after_returning_continue()
                {
                    var tree = Substitute.For<IBehaviorTree>();
                    _testNode.status = TaskStatus.Process;
                    _node.ParentTree = tree;

                    _node.Update();

                    _testNode.status = TaskStatus.Success;
                    _node.Update();

                    tree.Received(1).RemoveActiveTask(_node);
                }

                [Test]
                public void It_should_not_call_BehaviorTree_AddActiveTask_if_continue_was_not_returned()
                {
                    var tree = Substitute.For<IBehaviorTree>();
                    _testNode.status = TaskStatus.Success;
                    _node.ParentTree = tree;

                    _node.Update();

                    tree.Received(0).RemoveActiveTask(_node);
                }
            }

            public class TreeTickCountChange : UpdateMethod
            {
                private GameObject _go;

                [SetUp]
                public void BeforeEach()
                {
                    _go = new GameObject();
                }

                [TearDown]
                public void AfterEach()
                {
                    Object.DestroyImmediate(_go);
                }

                [Test]
                public void Retriggers_start_if_tick_count_changes()
                {
                    var tree = new BehaviorTree();
                    tree.AddNode(tree.Root, _node);

                    tree.Tick();
                    tree.Tick();

                    Assert.AreEqual(2, _testNode.StartCount);
                }

                [Test]
                public void Does_not_retrigger_start_if_tick_count_stays_the_same()
                {
                    _testNode.status = TaskStatus.Process;

                    var tree = new BehaviorTree();
                    tree.AddNode(tree.Root, _node);

                    tree.Tick();
                    tree.Tick();

                    Assert.AreEqual(1, _testNode.StartCount);
                }
            }

            public class StartEvent : UpdateMethod
            {
                [Test]
                public void Trigger_on_1st_run()
                {
                    _node.Update();

                    Assert.AreEqual(1, _testNode.StartCount);
                }

                [Test]
                public void Triggers_on_reset()
                {
                    _testNode.status = TaskStatus.Process;

                    _node.Update();
                    _node.Reset();
                    _node.Update();

                    Assert.AreEqual(2, _testNode.StartCount);
                }
            }

            public class InitEvent : UpdateMethod
            {
                [SetUp]
                public void TriggerUpdate()
                {
                    _node.Update();
                }

                [Test]
                public void Triggers_on_1st_update()
                {
                    Assert.AreEqual(1, _testNode.InitCount);
                }

                [Test]
                public void Does_not_trigger_on_2nd_update()
                {
                    _node.Update();

                    Assert.AreEqual(1, _testNode.InitCount);
                }

                [Test]
                public void Does_not_trigger_on_reset()
                {
                    _node.Reset();
                    _node.Update();

                    Assert.AreEqual(1, _testNode.InitCount);
                }
            }

            public class ExitEvent : UpdateMethod
            {
                [Test]
                public void Does_not_trigger_on_continue()
                {
                    _testNode.status = TaskStatus.Process;
                    _node.Update();

                    Assert.AreEqual(0, _testNode.ExitCount);
                }

                [Test]
                public void Triggers_on_success()
                {
                    _testNode.status = TaskStatus.Success;
                    _node.Update();

                    Assert.AreEqual(1, _testNode.ExitCount);
                }

                [Test]
                public void Triggers_on_failure()
                {
                    _testNode.status = TaskStatus.Failure;
                    _node.Update();

                    Assert.AreEqual(1, _testNode.ExitCount);
                }
            }
        }

        public class EndMethod
        {
            private TaskExample testTask;
            private ITask task;

            [SetUp]
            public void CreateTask()
            {
                task = testTask = new TaskExample();
            }

            [Test]
            public void Does_not_call_exit_if_not_run()
            {
                task.End();

                Assert.AreEqual(0, testTask.ExitCount);
            }

            [Test]
            public void Does_not_call_exit_if_last_status_was_success()
            {
                testTask.status = TaskStatus.Success;

                task.Update();
                task.End();

                Assert.AreEqual(1, testTask.ExitCount);
            }

            [Test]
            public void Does_not_call_exit_if_last_status_was_failure()
            {
                testTask.status = TaskStatus.Failure;

                task.Update();
                task.End();

                Assert.AreEqual(1, testTask.ExitCount);
            }

            [Test]
            public void Calls_exit_if_last_status_was_continue()
            {
                testTask.status = TaskStatus.Process;

                task.Update();
                task.End();

                Assert.AreEqual(1, testTask.ExitCount);
            }

            [Test]
            public void Reset_prevents_exit_from_being_called_when_it_should()
            {
                testTask.status = TaskStatus.Process;

                task.Update();
                task.Reset();
                task.End();

                Assert.AreEqual(0, testTask.ExitCount);
            }
        }
    }
}
