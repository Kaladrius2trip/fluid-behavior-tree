using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

using FluidBehaviorTree.Runtime.BehaviorTree;
using FluidBehaviorTree.Runtime.Tasks;

namespace FluidBehaviorTree.Runtime.TaskParents
{
    public abstract class TaskParentBase : CommonTaskBase, ITaskComposite
    {
        private int _lastTickCount;

        private readonly List<ITask> _children = new List<ITask>();

        protected virtual int MaxChildren { get; } = -1;

#region ITask Implementation

        public override IBehaviorTree ParentTree { get; set; }
        public override string Name { get; set; }
        public override bool Enabled { get; set; } = true;

        public override void End()
        {
            throw new NotImplementedException();
        }

        public override void Reset() { }

#endregion

#region ITaskComposite Implementation

        public IReadOnlyList<ITask> Children => _children;

        public virtual ITaskComposite AddChild(ITask child)
        {
            if (!child.Enabled)
            {
                return this;
            }

            if (_children.Count < MaxChildren || MaxChildren < 0)
            {
                _children.Add(child);
            }

            return this;
        }

        public void RemoveAllChild()
        {
            _children.Clear();
        }

        public void RemoveChildAt(int idx)
        {
            _children.RemoveAt(idx);
        }

        public void SwapChild(int idxFrom, int idxTo)
        {
            if (idxFrom == idxTo)
            {
                return;
            }

            CheckRangeAndThrow(idxFrom);
            CheckRangeAndThrow(idxTo);

            ITask tmp = _children[idxFrom];
            _children[idxFrom] = _children[idxTo];
            _children[idxTo] = tmp;
        }

#endregion

        protected sealed override TaskStatus Update()
        {
            base.Update();
            UpdateTicks();

            TaskStatus status = OnUpdate();
            LastStatus = status;
            if (status != TaskStatus.Process)
            {
                Reset();
            }

            return status;
        }

        protected virtual TaskStatus OnUpdate()
        {
            return TaskStatus.Success;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void CheckRangeAndThrow(int idx)
        {
            if (idx < 0 || idx >= _children.Count)
            {
                throw new IndexOutOfRangeException($"Can't remove child with by index {idx}");
            }
        }

        private void UpdateTicks()
        {
            if (ParentTree == null)
            {
                return;
            }

            if (_lastTickCount != ParentTree.TickCount)
            {
                Reset();
            }

            _lastTickCount = ParentTree.TickCount;
        }
    }
}
