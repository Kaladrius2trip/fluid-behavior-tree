using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

using FluidBehaviorTree.Runtime.BehaviorTree;
using FluidBehaviorTree.Runtime.TaskParents;
using FluidBehaviorTree.Runtime.Tasks;

using UnityEngine.Assertions;

namespace FluidBehaviorTree.Runtime.Decorators
{
    public abstract class DecoratorBase : CommonTaskBase, ITaskComposite
    {
        private ITask[] _children = Array.Empty<ITask>();

        public ITask Child { get; private set; }

#region ITask Implementation

        public override string Name { get; set; }

        public override bool Enabled { get; set; } = true;

        public override IBehaviorTree ParentTree { get; set; }

        public sealed override void End()
        {
            Assert.IsNotNull(Child);

            Child.End();
        }

        public sealed override void Reset() { }

#endregion

#region ITaskComposite Implementation

        public IReadOnlyList<ITask> Children => _children;

        public ITaskComposite AddChild(ITask child)
        {
            if (Child != null)
            {
                throw new InvalidOperationException("Can't add more than one decorated node in decorator node");
            }

            Child = child;
            _children = new ITask[1] {child};

            return this;
        }

        public void RemoveAllChild()
        {
            Child = null;
            _children = Array.Empty<ITask>();
        }

        public void RemoveChildAt(int idx)
        {
            CheckRangeAndThrow(idx);

            RemoveAllChild();
        }

        public void SwapChild(int idxFrom, int idxTo)
        {
            throw new InvalidOperationException("Decorator node doesn't support this swap child operation.");
        }

#endregion

        protected sealed override TaskStatus Update()
        {
            base.Update();
            Assert.IsNotNull(Child);

            TaskStatus status = OnUpdate();
            LastStatus = status;

            return status;
        }

        protected abstract TaskStatus OnUpdate();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void CheckRangeAndThrow(int idx)
        {
            if (idx < 0 || idx >= _children.Length)
            {
                throw new IndexOutOfRangeException($"Can't remove child with by index {idx}");
            }
        }
    }
}
