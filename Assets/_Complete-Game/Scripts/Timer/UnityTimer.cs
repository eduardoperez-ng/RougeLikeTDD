using Completed.Interfaces;
using UnityEngine;

namespace Completed.Timer
{
    public class UnityTimer : ITimer
    {
        public float ElapsedTime => Time.time;
    }
}