using FluidBehaviorTree.Runtime.BehaviorTree;
using FluidBehaviorTree.Runtime.BehaviorTree.Builder;
using FluidBehaviorTree.Runtime.Tasks;

using UnityEngine;

namespace CleverCrow.Fluid.BTs.Samples
{
    /// <summary>
    ///     Example script to test out BehaviorTrees, not actually compiled into the released package
    /// </summary>
    public class TreeTestSample : MonoBehaviour
    {
        [SerializeField]
        private BehaviorTree _treeA;

        [SerializeField]
        private BehaviorTree _treeB;

        [SerializeField]
        private BehaviorTree _treeC;

        [SerializeField]
        private bool _condition;

        private void Awake()
        {
            _treeA = new BehaviorTreeBuilder()
                     .Sequence()
                     .Condition("Custom Condition", () => true)
                     .Do("Custom Action A", () => TaskStatus.Success)
                     .Selector()
                     .Sequence("Nested Sequence")
                     .Condition("Custom Condition", () => _condition)
                     .Do("Custom Action", () => TaskStatus.Success)
                     .End()
                     .Sequence("Nested Sequence")
                     .Do("Custom Action", () => TaskStatus.Success)
                     .Sequence("Nested Sequence")
                     .Condition("Custom Condition", () => true)
                     .Do("Custom Action", () => TaskStatus.Success)
                     .End()
                     .End()
                     .Do("Custom Action", () => TaskStatus.Success)
                     .Condition("Custom Condition", () => true)
                     .End()
                     .Do("Custom Action B", () => TaskStatus.Success)
                     .End()
                     .Build();

            _treeB = new BehaviorTreeBuilder()
                     .Name("Basic")
                     .Sequence()
                     .Condition("Custom Condition", () => _condition)
                     .Do("Custom Action A", () => TaskStatus.Success)
                     .End()
                     .Build();

            _treeC = new BehaviorTreeBuilder()
                     .Name("Basic")
                     .Sequence()
                     .Condition("Custom Condition", () => _condition)
                     .Do("Continue", () => _condition ? TaskStatus.Process : TaskStatus.Success)
                     .End()
                     .Build();
        }

        private void Update()
        {
            // Update our tree every frame
            _treeA.Tick();
            _treeB.Tick();
            _treeC.Tick();
        }
    }
}
