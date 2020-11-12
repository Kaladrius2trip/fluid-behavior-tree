using System;
using System.Collections.Generic;

using FluidBehaviorTree.Runtime.TaskParents;
using FluidBehaviorTree.Runtime.Tasks;

namespace FluidBehaviorTree.Runtime.BehaviorTree
{
    public interface IBehaviorTree
    {
        string Name { get; }
        TaskRoot Root { get; }
        int TickCount { get; }

        void AddActiveTask(ITask task);
        void RemoveActiveTask(ITask task);
    }

    [Serializable]
    public class BehaviorTree : IBehaviorTree
    {
        private readonly List<ITask> _tasks = new List<ITask>();

        public BehaviorTree()
        {
            SyncNodes(Root);
        }

        public ITask RootTask => Root;

        public IReadOnlyList<ITask> ActiveTasks => _tasks;

        public int TickCount { get; private set; }

        public string Name { get; set; }
        public TaskRoot Root { get; } = new TaskRoot();

        public void AddActiveTask(ITask task)
        {
            _tasks.Add(task);
        }

        public void RemoveActiveTask(ITask task)
        {
            _tasks.Remove(task);
        }

        public TaskStatus Tick()
        {
            TaskStatus status = RootTask.Update();
            if (status != TaskStatus.Process)
            {
                Reset();
            }

            return status;
        }

        public void Reset()
        {
            foreach (ITask task in _tasks)
            {
                task.End();
            }

            _tasks.Clear();
            TickCount++;
        }

        public void AddNode(ITaskComposite parent, ITask child)
        {
            parent.AddChild(child);
            child.ParentTree = this;
        }

        public void Splice(ITaskComposite parent, BehaviorTree tree)
        {
            parent.AddChild(tree.Root);

            SyncNodes(tree.Root);
        }

        private void SyncNodes(ITaskComposite taskParent)
        {
            taskParent.ParentTree = this;

            foreach (ITask child in taskParent.Children)
            {
                child.ParentTree = this;

                if (child is ITaskComposite parent)
                {
                    SyncNodes(parent);
                }
            }
        }
    }
}
