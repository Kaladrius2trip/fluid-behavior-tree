namespace FluidBehaviorTree.Editor.BehaviorTree.Printer.Containers
{
    public interface IGraphContainer : IGraphBox
    {
        void AddBox(IGraphBox container);
    }
}
