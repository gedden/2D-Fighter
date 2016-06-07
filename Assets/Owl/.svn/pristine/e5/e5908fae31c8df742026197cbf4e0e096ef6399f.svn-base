//------------------------------------------------------------------------------
// The ActionSequencer allows Action objects to be scheduled to run
// at a given time. These objects will be sent three messages throughout
// their lifetime: actionBegin, actionTick, and actionEnd. 
// (See the Action interface for more information on these three messages.)
//
//
// If you need to schedule a cluster of related actions, see the ActionGroup and ActionChain classes.
//
// All actions scheduled with the ActionScheduler should be scheduled
// in ActionSequencer time. ActionSequencer time's 'zero' exists at
// the moment when the ActionSequencer is first initialized. The method
// ActionSequencer.getCurrentTime() will return the current time in
// ActionSequencer time. Use this to calculate schedule times.
//------------------------------------------------------------------------------
using System;
using Owl.Pump;
// using Spacewars.Network;
using UnityEngine;

namespace Sequencer
{
	public class ActionSequencer : IPumpable
	{
        // Class variables
        public static readonly float CONTINUOUS = float.MinValue;
        private static ActionSequencer _instance;

        // Member variables
        private ActionGroup[] sequence;
		private float blockOffset = 0;

        /// <summary>
        /// Gets the instance.
        /// </summary>
        /// <returns>The instance.</returns>
        public static ActionSequencer GetInstance()
        {
            return Instance;
        }

        /// <summary>
        /// Singleton accessor
        /// </summary>
	    public static ActionSequencer Instance
	    {
	        get
	        {
                if(_instance == null )
                    _instance = new ActionSequencer();
	            return _instance;
	        }
	    }

        /// <summary>
        /// Class Constructor
        /// </summary>
        private ActionSequencer()
        {
            sequence = new ActionGroup[2];

            // Initialize the actions
            ClearActions();
        }

        /// <summary>
        /// Clears the actions.
        /// </summary>
        public void ClearActions()
        {
            foreach (SequenceQueue queue in GameUtil.GetValues<SequenceQueue>())
            {
                sequence[(int)queue] = new ActionGroup();
            }
        }

        /// <summary>
        /// Get the number of actions in one of the queues
        /// </summary>
        /// <param name="queue"></param>
        /// <returns></returns>
        public ActionGroup GetQueue(SequenceQueue queue)
        {
            try
            {
                return sequence[(int)queue];
            }
            catch (Exception e)
            {
                Debug.Log(e);
            }
            return null;
        }

        /// <summary>
        /// Get the number of actions in one of the queues
        /// </summary>
        /// <param name="queue"></param>
        /// <returns></returns>
        public int GetActionCount(SequenceQueue queue)
        {
            try
            {
                GetQueue(queue).GetActionCount();
            }
            catch( Exception e )
            {
                Debug.Log(e);
            }
            return 0;
        }

		/// <summary>
		/// Get the total number of actions currently in the sequencer
		/// </summary>
		/// <returns>The action count.</returns>
		public int GetActionCount()
		{
			int total = 0;

            foreach (SequenceQueue queue in GameUtil.GetValues<SequenceQueue>())
                total += GetActionCount(queue);
			
			return total;
		}

        /// <summary>
        /// Schedule an action to be performed at startTime secs after
        /// the ActionSequencer was initialized. See the ActionSequencer
        /// class docs for more info on ActionSequencer time.
        /// 
        /// Also see: ActionSequencer.GetCurrentTime().
        /// </summary>
        /// <param name="startTime">The time in seconds that this action should begin at</param>
        /// <param name="action">The action to be performed</param>
		public void Schedule(float startTime, IAction action, SequenceQueue queue = SequenceQueue.Game)
		{
			GetQueue(queue).AddAction(startTime, action);
		}

        /// <summary>
        /// Schedule an action to performed when all other actions are complete
        /// </summary>
        /// <param name="action"></param>
        /// <param name="queue"></param>
        public void ScheduleNext(IAction action, SequenceQueue queue = SequenceQueue.Game)
	    {
			var nextTime = Mathf.Max (CurrentTime, GetActionDuration (queue));
            Schedule(nextTime, action, queue);
	    }

		public void ScheduleEffectiveNext(IAction action, SequenceQueue queue = SequenceQueue.Game)
		{
			var nextTime = Mathf.Max (CurrentTime, GetEffectiveDuration (queue));
			Schedule(nextTime, action, queue);
		}

        /// <summary>
        /// Schedule an action to be performed immediately.
        /// </summary>
        /// <param name="action"></param>
        /// <param name="queue"></param>
        public void Schedule(IAction action, SequenceQueue queue = SequenceQueue.Game)
		{
			var time = Mathf.Max(CurrentTime, blockOffset);
			Schedule(time, action, queue);
		}

        /// <summary>
        /// The time at the beginning of this frame (Read Only). This is the time in seconds since the start of the game.
        /// </summary>
        public float CurrentTime { get; protected set; }

        /// <summary>
        /// Pump the ActionSequencer.
        /// 
        /// This should be called once per game-level tick in order
        /// to properly update and dispatch all scheduled actions.
        /// If this is never called, actions will never be executed!
        /// </summary>
        public void Pump()
        {
            // This is done so items can be scheduled from external threads, as you cannot called Time.time directly from other threads!
            CurrentTime = Time.time;

            foreach (SequenceQueue queue in GameUtil.GetValues<SequenceQueue>())
                GetQueue(queue).ActionTick(CurrentTime);
                //GetQueue(queue).ActionTick(Time.fixedDeltaTime);
		}

		public float GetActionDuration(SequenceQueue queue = SequenceQueue.Game)
		{
            return GetQueue(queue).Duration;
		}

		public float GetEffectiveDuration(SequenceQueue queue = SequenceQueue.Game)
		{
			return GetQueue(queue).EffectiveDuration;
		}
	}

    public enum SequenceQueue
    {
        Game,
        // Non-game actions are actions that have no direct effect on game resolution, such as idle animation
        Nongame
    }
}

