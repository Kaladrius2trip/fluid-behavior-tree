using System.Collections.Generic;

using FluidBehaviorTree.Runtime.TaskParents;
using FluidBehaviorTree.Runtime.Tasks;

using UnityEngine;

namespace FluidBehaviorTree.Editor.BehaviorTree.Printer.GraphNode
{
    public class GraphNode
    {
        private readonly IGraphNodePrinter _printer;

        public GraphNode(ITask task, IGraphNodePrinter printer, GraphNodeOptions options)
        {
            _printer = printer;
            Task = task;

            Size = options.Size;
            VerticalConnectorBottomHeight = options.VerticalConnectorBottomHeight;
            VerticalConnectorTopHeight = options.VerticalConnectorTopHeight;
            HorizontalConnectorHeight = options.HorizontalConnectorHeight;

            if (task is ITaskComposite parentTask)
            {
                if (parentTask.Children == null)
                {
                    return;
                }

                foreach (ITask child in parentTask.Children)
                {
                    Children.Add(new GraphNode(child, printer, options));
                }
            }
        }

        public float ContainerHeight =>
            Size.y
            + VerticalConnectorBottomHeight
            + HorizontalConnectorHeight
            + VerticalConnectorTopHeight;

        public Vector2 Position { get; private set; }
        public List<GraphNode> Children { get; } = new List<GraphNode>();
        public Vector2 Size { get; }
        public ITask Task { get; }
        public int VerticalConnectorBottomHeight { get; }
        public int VerticalConnectorTopHeight { get; }
        public int HorizontalConnectorHeight { get; }

        public void SetPosition(Vector2 position)
        {
            Position = position;

            for (var i = 0; i < Children.Count; i++)
            {
                GraphNode child = Children[i];
                var childPos = new Vector2(position.x, position.y + ContainerHeight);

                // Center the child, then align it to the expected position
                childPos.x += Size.x / 2 + Size.x * i;

                // Shift the child as if it were in a container so it lines up properly
                childPos.x -= Size.x * (Children.Count / 2f);

                child.SetPosition(childPos);
            }
        }

        public void Print()
        {
            _printer.Print(this);

            foreach (GraphNode child in Children)
            {
                child.Print();
            }
        }
    }
}
