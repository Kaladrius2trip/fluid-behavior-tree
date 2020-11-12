using FluidBehaviorTree.Runtime.Tasks;

using UnityEngine;

namespace FluidBehaviorTree.Editor.BehaviorTree.Printer.Graphics
{
    public sealed class NodeFaders
    {
        public ColorFader BackgroundFader { get; } = new ColorFader(new Color(0.65f, 0.65f, 0.65f),
                                                                    new Color(0.39f, 0.78f, 0.39f),
                                                                    new Color(0.78f, 0.34f, 0.29f),
                                                                    new Color(0.78f, 0.75f, 0.38f));

        public ColorFader TextFader { get; } = new ColorFader(Color.white, Color.black, Color.black, Color.black);

        public ColorFader MainIconFader { get; } = new ColorFader(new Color(1, 1, 1, 0.3f),
                                                                  new Color(1, 1, 1, 1f),
                                                                  new Color(1, 1, 1, 1f),
                                                                  new Color(1, 1, 1, 1f));

        public void Update(TaskStatus status)
        {
            BackgroundFader.Update(status);
            TextFader.Update(status);
            MainIconFader.Update(status);
        }
    }
}
