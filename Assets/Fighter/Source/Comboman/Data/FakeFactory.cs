using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Comboman
{
    public static class FakeFactory
    {
        /// <summary>
        /// A wild Ryu has appeared!
        /// </summary>
        /// <returns></returns>
        public static CharacterData CreateRyu()
        {
            var ryu = new CharacterData();
            ryu.name = "Ryu";
            ryu.spriteSheet = "RyuCE";

            // Create some fake frames
            for (int x = 0; x < 5; x++)
            {
                var frame = new FrameData("idle_" + x);
                ryu.Frames.Add(frame);
            }

            // Create a fake animation
            var move = new MoveData(MoveType.IDLE);
            foreach (var frame in ryu.Frames)
            {
                var m = new MoveFrame(frame.SpriteName, 0.1f);

                move.MoveFrames.Add(m);
            }
            ryu.Moves.Add(move);

            ryu.Write();
            return ryu;
        }

        public static CharacterData LoadRyu()
        {
            return CharacterData.Read("Assets/Fighter/Data/Ryu.xml");
        }
    }
}
