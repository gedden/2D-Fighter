using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Comboman
{
    public class MoveData
    {
        public String Name;
        public List<MoveFrame> MoveFrames;

        public float Startup = 0f;
        public float Recovery = 0f;

        public MoveData(String name)
        {
            this.Name = name;
            MoveFrames = new List<MoveFrame>();
        }

        public MoveData() : this("")
        {
        }

        /// <summary>
        /// Get the total duration of this animation
        /// </summary>
        public float Duration
        {
            get
            {
                float result = 0.0f;
                foreach( var frame in MoveFrames )
                    result += frame.Duration;
                return result;
            }
        }

        /// <summary>
        /// Get the total time this animation is active
        /// </summary>
        public float Active
        {
            get
            {
                return Duration - Startup - Recovery;
            }
        }

        /// <summary>
        /// Get the frame by the current time
        /// </summary>
        /// <param name="timeFromStart"></param>
        /// <returns></returns>
        public FrameData GetFrameByTime(float timeFromStart)
        {
            var t = timeFromStart % Duration;

            var c = 0.0f;
            foreach( var f in MoveFrames )
            {
                var next = c + f.Duration;

                if (t < next)
                    return f.Frame;
                c = next;
            }
            return MoveFrames[0].Frame;
        }

    }

    public struct MoveFrame
    {
        public FrameData Frame;
        public float Duration;
    }

}
