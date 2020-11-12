using System.Collections.Generic;

namespace FluidBehaviorTree.Editor.BehaviorTree.Printer.Containers
{
    public class GraphContainerVertical : GraphContainerHorizontal
    {
        public override void AddBox(IGraphBox child)
        {
            CalculateChild(child);
            _childContainers.Add(child);
        }

        public override void CenterAlignChildren()
        {
            List<float> positions = GetCenterAlignLocalPositions();

            for (var i = 0; i < _childContainers.Count; i++)
            {
                IGraphBox child = _childContainers[i];
                if (child.SkipCentering)
                {
                    continue;
                }

                child.AddGlobalPosition(positions[i], 0);
                child.CenterAlignChildren();
            }
        }

        private void CalculateChild(IGraphBox child)
        {
            child.SetLocalPosition(0, Height);
            child.AddGlobalPosition(GlobalPositionX + child.LocalPositionX, GlobalPositionY + child.LocalPositionY);

            Height += child.Height;
            if (child.Width > Width)
            {
                Width = child.Width;
            }
        }

        private List<float> GetCenterAlignLocalPositions()
        {
            var list = new List<float>();
            foreach (IGraphBox child in _childContainers)
            {
                float gap = Width - child.Width;
                float shift = gap / 2f;

                list.Add(shift);
            }

            return list;
        }
    }
}
