using System.Collections.Generic;

using FluidBehaviorTree.Editor.BehaviorTree.Printer.Containers;
using FluidBehaviorTree.Runtime.TaskParents;
using FluidBehaviorTree.Runtime.Tasks;

namespace FluidBehaviorTree.Editor.BehaviorTree.Printer
{
    public class VisualTask
    {
        private readonly List<VisualTask> _children = new List<VisualTask>();
        private readonly NodePrintController _printer;
        private TaskStatus _lastTaskStatus;

        public VisualTask(ITask task, IGraphContainer parentContainer)
        {
            Task = task;
            BindTask();

            var container = new GraphContainerVertical();

            AddBox(container);

            if (task is ITaskComposite parentTask && parentTask.Children != null)
            {
                var childContainer = new GraphContainerHorizontal();
                foreach (ITask child in parentTask.Children)
                {
                    _children.Add(new VisualTask(child, childContainer));
                }

                AddDivider(container, childContainer);
                container.AddBox(childContainer);
            }

            parentContainer.AddBox(container);

            _printer = new NodePrintController(this);
        }

        public ITask Task { get; }
        public IReadOnlyList<VisualTask> Children => _children;

        public float Width { get; } = 70;
        public float Height { get; } = 50;

        public IGraphBox Box { get; private set; }
        public IGraphBox Divider { get; private set; }
        public float DividerLeftOffset { get; private set; }

        public void RecursiveTaskUnbind()
        {
            Task.StatusChanged -= UpdateTaskActiveStatus;

            foreach (VisualTask child in _children)
            {
                child.RecursiveTaskUnbind();
            }
        }

        public void Print()
        {
            _printer.Print(_lastTaskStatus);
            _lastTaskStatus = TaskStatus.None;

            foreach (VisualTask child in _children)
            {
                child.Print();
            }
        }

        private void BindTask()
        {
            Task.StatusChanged += UpdateTaskActiveStatus;
        }

        private void UpdateTaskActiveStatus(TaskStatus status)
        {
            _lastTaskStatus = status;
        }

        private void AddDivider(IGraphContainer parent, IGraphContainer children)
        {
            Divider = new GraphBox
            {
                SkipCentering = true
            };

            DividerLeftOffset = children.ChildContainers[0].Width / 2;
            float dividerRightOffset = children.ChildContainers[children.ChildContainers.Count - 1].Width / 2;
            float width = children.Width - DividerLeftOffset - dividerRightOffset;

            Divider.SetSize(width, 1);

            parent.AddBox(Divider);
        }

        private void AddBox(IGraphContainer parent)
        {
            Box = new GraphBox();
            Box.SetSize(Width, Height);
            Box.SetPadding(10, 10);
            parent.AddBox(Box);
        }
    }
}
