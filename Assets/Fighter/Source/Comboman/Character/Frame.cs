using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Comboman
{
    [Serializable]
    public class Frame
    {
        public FrameData Data;
        public Sprite Sprite; 

        public Frame(Sprite sprite, FrameData data)
        {
            this.Sprite = sprite;
            this.Data = data;
        }

        /// <summary>
        /// Create a frame from the data
        /// </summary>
        /// <param name="data"></param>
        /// <param name="sprites"></param>
        /// <returns></returns>
        public static Frame CreateFrame(FrameData data, Sprite [] sprites)
        {
            Sprite s = null;
            // Find the sprite
            foreach( var sprite in sprites )
                if( sprite.name == data.SpriteName )
                {
                    s = sprite;
                    break;
                }

            if (s == null)
                return null;

            return new Frame(s, data);
        }

        public String Name { get { return Data.SpriteName; } }
    }
}
