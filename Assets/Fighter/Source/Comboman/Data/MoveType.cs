using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Comboman
{
    public enum MoveType
    {
        IDLE,
        WALK,
        BASIC_PUNCH,
        BASIC_KICK,
        HURT,
        KNOCKDOWN,

        // The only non-basic move
        CUSTOM_ATTACK
    }

    public static class MoveTypeExtensions
    {
        public static bool IsBasicMove(this MoveType type)
        {
            if (type == MoveType.CUSTOM_ATTACK)
                return false;

            return true;
        }
    }
}
