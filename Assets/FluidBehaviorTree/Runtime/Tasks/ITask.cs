using System;

using FluidBehaviorTree.Runtime.BehaviorTree;

namespace FluidBehaviorTree.Runtime.Tasks
{
    public interface ITask
    {
        /// <summary>
        ///     Used for debugging and identification purposes
        /// </summary>
        string Name { get; set; }

        /// <summary>
        ///     Is this task enabled or not? Disabled tasks are excluded from the runtime
        /// </summary>
        bool Enabled { get; set; }

        /// <summary>
        ///     Tree this node belongs to
        /// </summary>
        IBehaviorTree ParentTree { get; set; }

        /// <summary>
        ///     Last status returned by Update
        /// </summary>
        TaskStatus LastStatus { get; }

        bool HasBeenActive { get; }

        /// <summary>
        ///     Triggered every tick
        /// </summary>
        /// <returns></returns>
        TaskStatus Update();

        /// <summary>
        ///     Forcibly end this task. Firing all necessary completion logic
        /// </summary>
        void End();

        /// <summary>
        ///     Reset this task back to its initial state to run again. Triggered after the behavior
        ///     tree finishes with a task status other than continue.
        /// </summary>
        void Reset();

#if UNITY_EDITOR
        event Action<TaskStatus> StatusChanged;
        string IconPath { get; }
        float IconPadding { get; }
#endif
    }
}
