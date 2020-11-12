using System.Collections.Generic;

using FluidBehaviorTree.Runtime.Tasks;

namespace FluidBehaviorTree.Runtime.TaskParents
{
    public interface ITaskComposite : ITask
    {
        IReadOnlyList<ITask> Children { get; }

        ITaskComposite AddChild(ITask child);
        void RemoveAllChild();
        void RemoveChildAt(int idx);
        void SwapChild(int idxFrom, int idxTo);
    }
}
