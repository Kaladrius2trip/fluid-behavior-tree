using FluidBehaviorTree.Editor.BehaviorTree.Printer.Containers;

using NSubstitute;

namespace FluidBehaviorTree.Tests.Editor.Builders
{
    public class GraphBoxStubBuilder
    {
        private float _width;
        private float _height;

        public GraphBoxStubBuilder WithSize(float width, float height)
        {
            _width = width;
            _height = height;
            return this;
        }

        public IGraphBox Build()
        {
            var task = Substitute.For<IGraphBox>();
            task.Width.Returns(_width);
            task.Height.Returns(_height);

            return task;
        }
    }
}
