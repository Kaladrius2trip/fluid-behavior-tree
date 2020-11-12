using FluidBehaviorTree.Runtime.TaskParents;
using FluidBehaviorTree.Runtime.TaskParents.Composites;
using FluidBehaviorTree.Runtime.Tasks;
using FluidBehaviorTree.Tests.Editor.Builders;

using NSubstitute;

using NUnit.Framework;

namespace FluidBehaviorTree.Tests.Editor.TaskParents.Composites
{
    public class ParallelTest
    {
        private ITaskComposite parallel;

        [SetUp]
        public void SetParallel()
        {
            parallel = new Parallel();
        }

        public class UpdateMethod : ParallelTest
        {
            public class OnSuccess : UpdateMethod
            {
                [SetUp]
                public void CreateTree()
                {
                    parallel
                        .AddChild(A.TaskStub().Build())
                        .AddChild(A.TaskStub().Build());
                }

                [Test]
                public void Returns_success_when_all_children_return_success()
                {
                    Assert.AreEqual(TaskStatus.Success, parallel.Update());
                }

                [Test]
                public void Ticks_all_children_at_once()
                {
                    parallel.Update();

                    foreach (ITask c in parallel.Children)
                    {
                        c.Received(1).Update();
                    }
                }

                [Test]
                public void Runs_end_on_all_children_when_complete()
                {
                    parallel.Update();

                    foreach (ITask c in parallel.Children)
                    {
                        c.Received(1).End();
                    }
                }
            }

            public class OnContinue : UpdateMethod
            {
                [SetUp]
                public void CreateTree()
                {
                    parallel
                        .AddChild(A.TaskStub().WithUpdateStatus(TaskStatus.Process).Build())
                        .AddChild(A.TaskStub().Build());
                }

                [Test]
                public void Returns_continue_if_any_child_returns_continue()
                {
                    Assert.AreEqual(TaskStatus.Process, parallel.Update());
                }

                [Test]
                public void Reruns_continue_tasks()
                {
                    parallel.Update();
                    parallel.Update();

                    parallel.Children[0].Received(2).Update();
                }

                [Test]
                public void Does_not_rerun_a_task_after_it_returns_success()
                {
                    parallel.Update();
                    parallel.Update();

                    parallel.Children[1].Received(1).Update();
                }

                [Test]
                public void Returns_success_when_continue_changes_to_success()
                {
                    parallel.Update();
                    parallel.Children[0].Update().Returns(TaskStatus.Success);
                    parallel.Update();

                    Assert.AreEqual(TaskStatus.Success, parallel.Update());
                }

                [Test]
                public void Runs_end_on_all_children_after_changing_to_success()
                {
                    parallel.Update();
                    parallel.Children[0].Update().Returns(TaskStatus.Success);
                    parallel.Update();

                    foreach (ITask c in parallel.Children)
                    {
                        c.Received(1).End();
                    }
                }
            }

            public class OnFailure : UpdateMethod
            {
                [SetUp]
                public void CreateTree()
                {
                    parallel
                        .AddChild(A.TaskStub().WithUpdateStatus(TaskStatus.Failure).Build())
                        .AddChild(A.TaskStub().Build());
                }

                [Test]
                public void Returns_failure_if_any_child_returns_failure()
                {
                    Assert.AreEqual(TaskStatus.Failure, parallel.Update());
                }

                [Test]
                public void Runs_failed_tasks_after_the_failed_node()
                {
                    parallel.Update();

                    parallel.Children[1].Received(1).Update();
                }

                [Test]
                public void Runs_end_on_all_children_when_complete()
                {
                    parallel.Update();
                    foreach (ITask c in parallel.Children)
                    {
                        c.Received(1).End();
                    }
                }
            }
        }

        public class EndMethod : ParallelTest
        {
            [Test]
            public void Ends_all_ongoing_tasks()
            {
                parallel
                    .AddChild(A.TaskStub().Build())
                    .AddChild(A.TaskStub().Build());

                parallel.End();

                foreach (ITask c in parallel.Children)
                {
                    c.Received(1).End();
                }
            }
        }

        public class ResetMethod : ParallelTest
        {
            [Test]
            public void Does_not_recall_previous_node_status_after_usage()
            {
                parallel
                    .AddChild(A.TaskStub().WithUpdateStatus(TaskStatus.Process).Build())
                    .AddChild(A.TaskStub().Build());

                parallel.Update();
                parallel.Reset();
                parallel.Update();

                foreach (ITask c in parallel.Children)
                {
                    c.Received(2).Update();
                }
            }

            [Test]
            public void Does_not_trigger_end_on_children()
            {
                parallel
                    .AddChild(A.TaskStub().Build())
                    .AddChild(A.TaskStub().Build());

                parallel.Reset();

                foreach (ITask c in parallel.Children)
                {
                    c.Received(0).End();
                }
            }
        }
    }
}
