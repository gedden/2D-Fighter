using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Comboman
{
    [ExecuteInEditMode]
    [RequireComponent(typeof(Image))]
    public class Character : MonoBehaviour
    {
        public MoveType State = MoveType.IDLE;

        private CharacterData Data = null;

        private Image _image;

        private Dictionary<String, Frame> Frames;

        public float Speed = 10f;

        /// <summary>
        /// Image Reference
        /// </summary>
        private Image Image
        {
            get
            {
                if (_image == null)
                    _image = GetComponent<Image>();
                return _image;
            }
        }

        public static Character Create(CharacterData _data)
        {
            var instance = new GameObject();
            var c = instance.AddComponent<Character>();

            c.Data = _data;
            c.Frames = new Dictionary<string, Frame>();

            // Set the sprites
            var sprites = c.Data.LoadSprites();


            // Build up all the frame refrences
            foreach (var frameData in c.Data.Frames)
            {
                var frame = Frame.CreateFrame(frameData, sprites);
                c.Frames.Add(frame.Name, frame);
            }

            return c;
        }

        public void Start()
        {
            SetState(MoveType.IDLE);
        }

        /// <summary>
        /// Set the animation state
        /// </summary>
        /// <param name="next"></param>
        public void SetState(MoveType next)
        {
            //if (next == _state) return;
            State = next;

            if(State == MoveType.IDLE )
            {
                // Get the idle move
                var move = Data.GetMoveData(State.ToString());

                if (move == null)
                    return;

                Current = new Sequence(Time.fixedTime, Data, move);
            }
        }

        private Sequence _current;
        protected Sequence Current
        {
            get
            {
                return _current;
            }
            set
            {
                _current = value;
            }
        }


        /// <summary>
        /// Do it up
        /// </summary>
        public void Update()
        {
            if (_current == null) return;
            
            // Get the current frame
            var frameData = _current.GetFrame();
            var frame = Frames[frameData.SpriteName];

            // Set the image
            Image.sprite = frame.Sprite;
        }
    }
}
