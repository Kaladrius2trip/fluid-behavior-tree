using FluidBehaviorTree.Runtime.Tasks;

using UnityEngine;

namespace FluidBehaviorTree.Editor.BehaviorTree.Printer.Graphics
{
    public sealed class ColorFader
    {
        private const float FADE_DURATION = 0.8f;

        private float _fadeTime;

        private readonly Color[] _statusColor;

        private TaskStatus _targetStatus;

        public ColorFader(Color neutral, Color success, Color failure, Color process)
        {
            _statusColor = new Color[4]
            {
                success, failure, process, neutral
            };
        }

        public Color CurrentColor { get; private set; }

        public void Update(TaskStatus status)
        {
            if (status != TaskStatus.None)
            {
                _fadeTime = FADE_DURATION;
                _targetStatus = status;
            }
            else
            {
                _fadeTime -= Time.deltaTime;

                if (_fadeTime < 0)
                {
                    _fadeTime = 0;
                    _targetStatus = TaskStatus.None;
                }
            }

            CurrentColor = Color.Lerp(_statusColor[3], _statusColor[(int) _targetStatus], _fadeTime / FADE_DURATION);
        }
    }
}
