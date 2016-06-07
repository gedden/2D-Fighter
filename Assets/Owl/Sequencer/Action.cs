using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sequencer
{
    public abstract class Action : IAction
    {
        public abstract void ActionBegin(float secLate);
        public abstract void ActionTick(float dt);
        public abstract void ActionEnd();


        private float effective = -1.0f;


        /// <summary>
        /// Query for the action's total duration in seconds from start to finish.
        /// </summary>
        /// <returns>The action duration.</returns>
        public float Duration { get; set; }

        /// <summary>
        /// Gets the effective end time.
        /// 
        /// This value is the time at which the action can be considered effectively finished for the purposes of scheduling the next action in a GameActionResponse tree or similar.
        /// </summary>
        /// <returns>The effective end time.</returns>
        public float EffectiveDuration
        {
            get
            {
                if (effective < 0 )
                    return Duration;
                return effective;
            }
            set
            {
                effective = value;
            }
        }

		protected float RawEffective
		{
			get
			{
				return effective;
			}
		}

        /// <summary>
        /// Accessor for when this action began
        /// </summary>
        public float StartTime { get; set; }

        /// <summary>
        /// The percent complete for this action
        /// </summary>
        protected float Percent
        {
            get
            {
                return ActionTime/Duration;
            }
        }

        /// <summary>
        /// The time in seconds that have passed since this
        /// action began
        /// </summary>
        protected float ActionTime
        {
            get
            {
                return Time - StartTime;
            }
        }

        protected float Time
        {
            get { return ActionSequencer.GetInstance().CurrentTime; }
        }

    }
}
