//------------------------------------------------------------------------------
// Represents a group of actions to be performed at specific
// time offsets from the point at which this ActionGroup
// begins its own execution.
//
//------------------------------------------------------------------------------
using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Sequencer
{
	public class ActionGroup : Action
	{
		private LinkedList<ScheduledAction> _actions;
		private LinkedList<ScheduledAction> _activeActions;
		private LinkedList<ScheduledAction> _workingBeginList;
		
		public ActionGroup()
		{
			StartTime 			= 0;
			_actions            = new LinkedList<ScheduledAction>();
			_activeActions      = new LinkedList<ScheduledAction>();
			_workingBeginList   = new LinkedList<ScheduledAction>();
		}
		
		public int GetActionCount()
		{
			return _actions.Count + _activeActions.Count + _workingBeginList.Count;		
		}

        /// <summary>
        /// Add a new action to be performed at timeOffset seconds after the beginning of this ActionGroup's execution.
        /// </summary>
        /// <param name="timeOffset">Scheduled time for this action to start in SECONDS after this ActionGroup begins.</param>
        /// <param name="action">The action to be performed</param>
		public void AddAction(float timeOffset, IAction action)
		{
			var act = new ScheduledAction(timeOffset, action); 
			
			lock (_actions)
            {
                _actions.AddLast(act);
            }
			
			lock (this)
			{
				if (act.EndTime > Duration )
                    Duration = act.EndTime;
				if (act.EffectiveEndTime > RawEffective )
                    EffectiveDuration = act.EffectiveEndTime;
				
			}
		}

	    public void AddAction(IAction action)
	    {
	        AddAction(EffectiveDuration, action);
	    }
		
        /// <summary>
        /// Remove an action from this group!
        /// </summary>
        /// <param name="timeOffset"></param>
        /// <param name="action"></param>
		public void RemoveAction(float timeOffset, IAction action)
		{
			if (action.Duration != ActionSequencer.CONTINUOUS)
				throw new ArgumentException("ActionGroup.RemoveAction(): Can only remove actions with a continuous duration");
			AddAction(timeOffset, new ContinuousActionEnd(action));
		}

        /// <summary>
        /// Clean up any extra actions sitting around.
        /// 
        /// Actions are guaranteed to never receive an actionTick() after their
        /// time is elapsed. To maintain this guarantee, it is possible (almost guaranteed)
        /// that an ActionGroup (an action itself) will never receive the final tick
        /// that it would normally use to call actionEnd on its last executing action(s).
        /// Therefore, we do that here.
        /// </summary>
		public override void ActionEnd()
		{
            //Debug.Log("ENDING AN ENTIRE ACTION GROUP!");
			lock (_actions)
			{		
				lock (_activeActions)
				{
                    foreach( ScheduledAction next in _activeActions )
					{
						try
						{
							next.GetAction().ActionEnd();
						}
						catch (Exception e)
						{
                            Debug.Log(e);
						}
					}
				}
				
				foreach( ScheduledAction schedule in _actions )
				{
					try
					{
                        IAction action = schedule.GetAction();
                        action.StartTime = Time;//schedule.StartTime;
                        schedule.GetAction().ActionBegin(schedule.EndTime - schedule.StartTime);
					}
					catch (Exception e)
					{
						Debug.Log(e);
					}
					
					try
					{
                        schedule.GetAction().ActionEnd();
					}
					catch (Exception e)
					{
						Debug.Log(e);
					}
				}
			}
		}
		
		
        /// <summary>
        /// Pump the actions
        /// </summary>
        /// <param name="timeOffset"></param>
        public sealed override void ActionTick(float secOffset)
		{
			/*
		    * WARNING: This block holds two synch locks! Right now this is the only
		    * place this happens. Beware of creating a 'dining philosophers' scenario
		    * with future development!
		    */
			lock (_actions)
			{
				lock (_activeActions)
				{
                    // Clone the list in question
                    var clone = new List<ScheduledAction>(_activeActions);

                    //Debug.Log("THERE ARE " + _activeActions.Count + " ACTIVE ACTIONS");
					//Process existing actions
					foreach( ScheduledAction act in clone )
                    {
						try
						{
                            //Debug.Log("END TIME : " + act.EndTime + " <= " + secOffset + " : secOffset");
							if (act.EndTime <= secOffset && !act.Continious)
							{
                                _activeActions.Remove(act);
								act.GetAction().ActionEnd();
							}
							else
                                act.GetAction().ActionTick(secOffset - act.StartTime);
						}
						catch (Exception e)
						{
							Debug.Log(e);
						}
					}

                    // Clone the list in question
                    clone = new List<ScheduledAction>(_actions);
                    foreach( ScheduledAction act in clone )
                    {
                        //Debug.Log("   >>>  " + secOffset + " >= " + act.StartTime);
                        if (secOffset >= act.StartTime)
						{
                            _actions.Remove(act);
							_activeActions.AddLast(act);
							_workingBeginList.AddLast(act);
						}
					}
					

				    // This is done outside of the loop above to prevent comodifications
                    // while an iterator is out. Otherwise adding new actions as part
                    // of an actionBegin will not work properly.
					while (_workingBeginList.Count > 0)
					{
						var schedule = _workingBeginList.First.Value;
                        _workingBeginList.RemoveFirst();
						
						try
						{
                            var action = schedule.GetAction();
                            action.StartTime = Time;// schedule.StartTime;
                            action.ActionBegin(secOffset - schedule.StartTime);
						}
						catch (Exception e)
						{
							Debug.LogError(e);
						}
					}
				}
			}
		}


        public override void ActionBegin(float secLate)
        {
            //throw new NotImplementedException();
        }
    }


	class ScheduledAction
	{
		private IAction _action;
		
		public ScheduledAction(float startTime, IAction action)
		{
            
            StartTime = startTime;
			_action = action;

            //Debug.Log(" SCHEDULE ACTION: " + StartTime + " End Time: " + EndTime + " Duration" + action.Duration + " ~ " + action.GetType() );
		}

        public float StartTime { get; set; }
		
        public float EndTime
        {
            get
            {
                return StartTime + _action.Duration;
            }
        }
		
        public float EffectiveEndTime
        {
            get
            {
                return StartTime + _action.EffectiveDuration;
            }
        }
		
		public IAction GetAction()
		{
			return _action;
		}

		public bool Continious
		{
			get
			{
				return _action.Duration == ActionSequencer.CONTINUOUS;
			}
		}
	}
}

