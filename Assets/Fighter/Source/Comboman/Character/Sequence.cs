using System;
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor;
#endif
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
        private readonly CharacterData _char;

        /// <summary>
        /// Get the sequence
        /// </summary>
        /// <param name="start"></param>
        /// <param name="move"></param>
        public Sequence(float start, CharacterData _char, MoveData move)
        {
            _start = start;
            _data = move;
            this._char = _char;
        }

        /// <summary>
        /// Get the frame
        /// </summary>
        /// <returns></returns>
        public FrameData GetFrame()
        {
            return _data.GetFrameByTime(Now + _start, _char);
        }

        public float Start
        {
            get
            {
                return _start;
            }
        }


        public static float Now
        {
            get
            {
#if UNITY_EDITOR
                if (Application.isEditor)
                    return (float)EditorApplication.timeSinceStartup;
#endif
                return Time.fixedTime;
            }
        }

    }
}
