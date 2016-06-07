//------------------------------------------------------------------------------
// The interface of an action. This will preform tasks!
//
//------------------------------------------------------------------------------
using System;
namespace Sequencer
{
	public interface IAction
	{
		/// <summary>
		/// Called to signal the beginning of an action. Use this for any sort
		/// of setup that the action requires. 
		/// 
		/// i.e. an effect action might use this to initialize new effect objects
		/// on the board (and keep local references to them).
		/// </summary>
		/// <param name="msLate">Ms late. The number of milliseconds between when we were scheduled to begin and when we actually did begin</param>
		void ActionBegin(float secLate);
		
		/// <summary>
		/// Called during each tick of the action's execution.
		/// 
		/// NOTE: There is no guarantee that this will ever be called
		/// with timeOffset zero. This is ok and normal.
		/// </summary>
		/// <param name="timeOffset">Time offset.</param>
		void ActionTick(float secOffset);
		

		/// <summary>
		/// Called to signal that the action's duration period is over. The action will receive no more ticks after this is called. Use this to perform any sort of cleanup after an action.		/// </summary>
		void ActionEnd();
		
		/// <summary>
		/// Query for the action's total duration in seconds from start to finish.
		/// </summary>
		/// <returns>The action duration.</returns>
        float Duration { get; set; }
		
		/// <summary>
		/// Gets the effective end time.
		/// 
		/// This value is the time at which the action can be considered effectively finished for the purposes of scheduling the next action in a GameActionResponse tree or similar.
		/// </summary>
		/// <returns>The effective end time.</returns>
        float EffectiveDuration { get; set; }


        /// <summary>
        /// Accessor for when this action began
        /// </summary>
        float StartTime { get; set; }

	}
}

