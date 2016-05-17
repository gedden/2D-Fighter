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
        public Anim State = Anim.None;
        private Anim _state = Anim.None;

        private CharacterData Data = null;

        private Image _image;

        private Dictionary<String, Frame> Frames;


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

        public void Start()
        {
            //Data = FakeFactory.CreateRyu();
            Data = FakeFactory.LoadRyu();
            Frames = new Dictionary<string, Frame>();

            // Set the sprites
            var sprites = Data.LoadSprites();


            // Build up all the frame refrences
            foreach( var frameData in Data.Frames )
            {
                var frame = Frame.CreateFrame(frameData, sprites);
                Frames.Add(frame.Name, frame);
            }
            SetState(Anim.Idle);
        }

        /// <summary>
        /// Set the animation state
        /// </summary>
        /// <param name="next"></param>
        public void SetState(Anim next)
        {
            if (next == _state)
                return;
            _state = next;

            if( _state == Anim.Idle )
            {
                // Get the idle move
                var move = Data.GetMoveData(Anim.Idle.ToString());

                if (move == null)
                    return;

                Current = new Sequence(Time.fixedTime, move);
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

    public enum Anim
    {
        None,
        Idle,
        WalkForward
    }
}
