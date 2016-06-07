using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace Comboman
{
    public class MoveData
    {
        public String Name;
        public List<MoveFrame> MoveFrames;
        public MoveType MoveType;

        public float Startup = 0f;
        public float Recovery = 0f;

        /// <summary>
        /// Class Constructor
        /// </summary>
        /// <param name="name"></param>
        /// <param name="_data"></param>
        public MoveData(String name)
        {
            this.Name = name;
            MoveFrames = new List<MoveFrame>();
            MoveType = MoveType.CUSTOM_ATTACK;
        }

        public MoveData(MoveType type) : this(type.ToString())
        {
            MoveType = type;
        }

        /// <summary>
        /// Set the move data
        /// </summary>
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
        public FrameData GetFrameByTime(float timeFromStart, CharacterData _char)
        {
            if (MoveFrames == null)
                return null;

            var t = timeFromStart % Duration;

            var c = 0.0f;
            foreach( var f in MoveFrames )
            {
                var next = c + f.Duration;

                if (t < next)
                    return f.GetFrame(_char);
                c = next;
            }

            try
            {
                return MoveFrames[0].GetFrame(_char);
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// Add the frame
        /// </summary>
        /// <param name="frame"></param>
        public void AddFrame(MoveFrame frame)
        {
            MoveFrames.Add(frame);
        }

        public void Remove(MoveFrame frame)
        {
            MoveFrames.Remove(frame);
        }
    }

    public class MoveFrame
    {
        public string FrameName;
        public float Duration;

        /// <summary>
        /// Class Constructor
        /// </summary>
        /// <param name="name"></param>
        /// <param name="Duration"></param>
        public MoveFrame(string name, float Duration)
        {
            this.FrameName = name;
            this.Duration = Duration;
        }

        /// <summary>
        /// Class Constructor
        /// </summary>
        private MoveFrame() : this("", 0)
        {

        }

        public FrameData GetFrame(CharacterData _char)
        {
            return _char.GetFrame(FrameName);
        }

    }

}
