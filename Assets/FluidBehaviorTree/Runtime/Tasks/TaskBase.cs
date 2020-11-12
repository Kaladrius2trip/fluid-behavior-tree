using FluidBehaviorTree.Runtime.BehaviorTree;

namespace FluidBehaviorTree.Runtime.Tasks
{
    public abstract class TaskBase : CommonTaskBase
    {
        private bool _init;
        private bool _start;
        private bool _exit;
        private int _lastTickCount;
        private bool _active;

        public override string Name { get; set; }
        public override bool Enabled { get; set; } = true;
        public override IBehaviorTree ParentTree { get; set; }

        /// <summary>
        ///     Reset the node to be re-used
        /// </summary>
        public sealed override void Reset()
        {
            _active = false;
            _start = false;
            _exit = false;
        }

        public sealed override void End()
        {
            Exit();
        }

        protected sealed override TaskStatus Update()
        {
            base.Update();
            UpdateTicks();

            if (!_init)
            {
                Init();
                _init = true;
            }

            if (!_start)
            {
                Start();
                _start = true;
                _exit = true;
            }

            LastStatus = GetUpdate();

            if (LastStatus != TaskStatus.Process)
            {
                if (_active)
                {
                    ParentTree?.RemoveActiveTask(this);
                }

                Exit();
            }
            else if (!_active)
            {
                ParentTree?.AddActiveTask(this);
                _active = true;
            }

            return LastStatus;
        }

        protected virtual TaskStatus GetUpdate()
        {
            return TaskStatus.Failure;
        }

        /// <summary>
        ///     Triggers the first time this node is run or after a hard reset
        /// </summary>
        protected virtual void OnInit() { }

        /// <summary>
        ///     Run every time this node begins
        /// </summary>
        protected virtual void OnStart() { }

        /// <summary>
        ///     Triggered when this node is complete
        /// </summary>
        protected virtual void OnExit() { }

        private void UpdateTicks()
        {
            if (ParentTree == null)
            {
                return;
            }

            if (_lastTickCount != ParentTree.TickCount)
            {
                Reset();
            }

            _lastTickCount = ParentTree.TickCount;
        }

        private void Init()
        {
            OnInit();
        }

        private void Start()
        {
            OnStart();
        }

        private void Exit()
        {
            if (_exit)
            {
                OnExit();
            }

            Reset();
        }
    }
}
