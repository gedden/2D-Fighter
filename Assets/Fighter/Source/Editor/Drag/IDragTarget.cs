using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Comboman
{
    public interface IDragTarget
    {
        void OnBeforeDragComplete();
    }
}
