using System;

using FluidBehaviorTree.Runtime.BehaviorTree;

using UnityEngine.Assertions;

namespace FluidBehaviorTree.Runtime.Tasks
{
    public abstract class CommonTaskBase : ITask
    {
#if UNITY_EDITOR
        protected const string PACKAGE_ROOT = "ROOT/Editor/Icons/Tasks";

        public event Action<TaskStatus> StatusChanged;

        /// <summary>
        ///     For custom project icons provide a path from assets to a Texture2D asset. Example `Assets/MyIcon.png`.
        ///     Be sure to NOT include the keyword `ROOT` in your path name. This will be replaced with a path
        ///     to the package root for Fluid Behavior Tree.
        /// </summary>
        public virtual string IconPath => $"{PACKAGE_ROOT}/Play.png";

        public virtual float IconPadding { get; } = 10;
#endif
        public TaskStatus LastStatus { get; protected set; }
        public abstract IBehaviorTree ParentTree { get; set; }

        public abstract string Name { get; set; }
        public abstract bool Enabled { get; set; }

        public bool HasBeenActive { get; private set; }

#if UNITY_EDITOR

        TaskStatus ITask.Update()
        {
            TaskStatus result = Update();
            Assert.AreEqual(result, LastStatus);

            StatusChanged?.Invoke(LastStatus);
            return result;
        }

        protected virtual TaskStatus Update()
        {
            HasBeenActive = true;

            return TaskStatus.Success;
        }

#else
        public virtual TaskStatus Update()
        {
            HasBeenActive = true;
            return TaskStatus.Success;
        }
#endif

        public abstract void End();
        public abstract void Reset();
    }
}
