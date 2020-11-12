using System;

using FluidBehaviorTree.Runtime.BehaviorTree;
using FluidBehaviorTree.Runtime.BehaviorTree.Builder;
using FluidBehaviorTree.Runtime.Tasks;
using FluidBehaviorTree.Runtime.Tasks.Actions;

using NUnit.Framework;

namespace FluidBehaviorTree.Tests.Editor.BehaviorTrees.Builders
{
    public static class BehaviorTreeExtensionActionExamples
    {
        public static BehaviorTreeBuilder ExampleAction(this BehaviorTreeBuilder builder, string name, Action callback)
        {
            return builder.AddNode(new BehaviorTreeBuilderExtensionActionTest.ExtensionAction
            {
                Name = name,
                callback = callback
            });
        }
    }

    public class BehaviorTreeBuilderExtensionActionTest
    {
        [Test]
        public void It_should_run_the_custom_action()
        {
            var result = false;
            BehaviorTree tree = new BehaviorTreeBuilder()
                                .Sequence()
                                .ExampleAction("test", () => result = true)
                                .End()
                                .Build();

            tree.Tick();

            Assert.IsTrue(result);
        }

        public class ExtensionAction : ActionBase
        {
            public Action callback;

            protected override TaskStatus OnUpdate()
            {
                callback();
                return TaskStatus.Success;
            }
        }
    }
}
