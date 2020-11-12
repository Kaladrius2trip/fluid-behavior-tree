using System;

using FluidBehaviorTree.Runtime.Tasks;

namespace FluidBehaviorTree.Runtime.TaskParents.Composites
{
    /// <summary>
    ///     Randomly selects a child node with a shuffle algorithm
    /// </summary>
    public class SelectorRandom : CompositeBase
    {
        private bool _init;

        public override string IconPath { get; } = $"{PACKAGE_ROOT}/LinearScale.png";

        public override void Reset()
        {
            base.Reset();

            ShuffleChildren();
        }

        protected override TaskStatus OnUpdate()
        {
            if (!_init)
            {
                ShuffleChildren();
                _init = true;
            }

            for (int i = ChildIndex; i < Children.Count; i++)
            {
                ITask child = Children[ChildIndex];

                switch (child.Update())
                {
                    case TaskStatus.Success:
                        return TaskStatus.Success;
                    case TaskStatus.Process:
                        return TaskStatus.Process;
                }

                ChildIndex++;
            }

            return TaskStatus.Failure;
        }

        private void ShuffleChildren()
        {
            var rng = new Random();
            int n = Children.Count;
            while (n > 1)
            {
                n--;

                int k = rng.Next(n + 1);
                SwapChild(k, n);
            }
        }
    }
}
