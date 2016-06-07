using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sequencer
{
    public class ContinuousActionEnd : Action
    {
	    private IAction _action;

	    public ContinuousActionEnd(IAction action)
	    {
		    _action = action;
	    }

	    public override void ActionBegin(float secLate)
	    {
		    _action.ActionEnd();
	    }

        public override void ActionTick(float dt)
        {
            //throw new NotImplementedException();
        }

        public override void ActionEnd()
        {
            //throw new NotImplementedException();
        }
    }

}
