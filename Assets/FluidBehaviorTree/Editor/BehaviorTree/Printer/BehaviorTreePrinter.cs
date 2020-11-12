using FluidBehaviorTree.Editor.BehaviorTree.Printer.Containers;
using FluidBehaviorTree.Editor.BehaviorTree.Printer.Graphics;
using FluidBehaviorTree.Runtime.BehaviorTree;

using UnityEngine;

namespace FluidBehaviorTree.Editor.BehaviorTree.Printer
{
    public class BehaviorTreePrinter
    {
        private const float SCROLL_PADDING = 40;

        private readonly VisualTask _root;
        private readonly Rect _containerSize;

        private Vector2 _scrollPosition;

        public BehaviorTreePrinter(IBehaviorTree tree, Vector2 windowSize)
        {
            StatusIcons = new StatusIcons();
            SharedStyles = new GuiStyleCollection();

            var container = new GraphContainerVertical();
            container.SetGlobalPosition(SCROLL_PADDING, SCROLL_PADDING);
            _root = new VisualTask(tree.Root, container);
            container.CenterAlignChildren();

            _containerSize = new Rect(0, 0,
                                      container.Width + SCROLL_PADDING * 2,
                                      container.Height + SCROLL_PADDING * 2);

            CenterScrollView(windowSize, container);
        }

        public static StatusIcons StatusIcons { get; private set; }
        public static GuiStyleCollection SharedStyles { get; private set; }

        public void Print(Vector2 windowSize)
        {
            _scrollPosition = GUI.BeginScrollView(
                new Rect(0, 0, windowSize.x, windowSize.y),
                _scrollPosition,
                _containerSize);
            _root.Print();
            GUI.EndScrollView();
        }

        public void Unbind()
        {
            _root.RecursiveTaskUnbind();
        }

        private void CenterScrollView(Vector2 windowSize, GraphContainerVertical container)
        {
            float scrollOverflow = container.Width + SCROLL_PADDING * 2 - windowSize.x;
            float centerViewPosition = scrollOverflow / 2;
            _scrollPosition.x = centerViewPosition;
        }
    }
}
