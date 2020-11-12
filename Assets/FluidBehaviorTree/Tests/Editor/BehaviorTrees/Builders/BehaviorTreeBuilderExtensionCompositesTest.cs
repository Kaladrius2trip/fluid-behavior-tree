using FluidBehaviorTree.Runtime.BehaviorTree;
using FluidBehaviorTree.Runtime.BehaviorTree.Builder;
using FluidBehaviorTree.Runtime.TaskParents.Composites;
using FluidBehaviorTree.Runtime.Tasks;

using NUnit.Framework;

namespace FluidBehaviorTree.Tests.Editor.BehaviorTrees.Builders
{
    public static class BehaviorTreeExtensionCompositeExamples
    {
        public static BehaviorTreeBuilder CustomSequence(this BehaviorTreeBuilder builder, string name)
        {
            return builder.ParentTask<Sequence>(name);
        }
    }

    public class BehaviorTreeExtensionCompositesTest
    {
        [Test]
        public void It_should_run_the_custom_action()
        {
            var result = false;
            BehaviorTree tree = new BehaviorTreeBuilder()
                                .CustomSequence("test")
                                .Do(() =>
                                {
                                    result = true;
                                    return TaskStatus.Success;
                                })
                                .End()
                                .Build();

            tree.Tick();

            Assert.IsTrue(result);
        }
    }
}
