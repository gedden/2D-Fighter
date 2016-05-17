using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using UnityEngine;

namespace Comboman
{
    [Serializable]
    public class FrameData
    {
        public String SpriteName;

        public bool HasHitbox = false;
        public Rect Hitbox;

        public bool HasAttackbox = false;
        public Rect Attackbox;

        /// <summary>
        /// Class Constructor
        /// </summary>
        public FrameData(String sprite)
        {
            Hitbox = new Rect();
            Attackbox = new Rect();
            SpriteName = sprite;
        }

        public FrameData() : this("")
        {
        }
    }
}
