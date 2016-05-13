using System;
using System.Collections.Generic;
using UnityEngine;

namespace Comboman
{
    /// <summary>
    /// This should be the transient 'current animation' class for the characters
    /// </summary>
    public class Sequence
    {
        private readonly float _start;
        private readonly MoveData _data;

        /// <summary>
        /// Get the sequence
        /// </summary>
        /// <param name="start"></param>
        /// <param name="move"></param>
        public Sequence(float start, MoveData move)
        {
            _start = start;
            _data = move;
        }

        public FrameData GetFrame()
        {
            return _data.GetFrameByTime(Time.fixedTime + _start);
        }
    }
}
