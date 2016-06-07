using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEngine;

namespace Comboman
{
    public class DragManager
    {
        private static DragManager _instance = null;
        private System.Object _current;
        private Vector2 _lastMouse;

        public DragManager()
        {
        }

        public void Update()
        {
            if (IsDragging && Event.current.type == EventType.MouseUp)
            {
                Clear();
            }

            if( IsDragging && _lastMouse != Event.current.mousePosition )
            {
                _lastMouse = Event.current.mousePosition;
                CombomanEditor.Instance.RequestRepaint();
            }


            if (IsDragging && Frame != null)
            {
                Texture t = Frame.Sprite.texture;
                Rect tr = Frame.Sprite.textureRect;
                Rect r = new Rect(tr.x / t.width, tr.y / t.height, tr.width / t.width, tr.height / t.height);
                var tip = new Rect(Event.current.mousePosition.x, Event.current.mousePosition.y, tr.width, tr.height);

                GUI.DrawTextureWithTexCoords(tip, Frame.Sprite.texture, r);
            }



        }

        public IDragTarget Target { get; set; }

        /// <summary>
        /// Get the instnace
        /// </summary>
        public static DragManager Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new DragManager();
                return _instance;
            }
        }

        public bool IsDragging
        {
            get
            {
                return _current != null;
            }
        }

        /// <summary>
        /// Set the frame data
        /// </summary>
        public void Clear()
        {
            if (Target != null)
                Target.OnBeforeDragComplete();
            Target = null;

            _current = null;
            CombomanEditor.Instance.RequestRepaint();
        }

        /// <summary>
        /// Check the frame
        /// </summary>
        public Frame Frame
        {
            get
            {
                if (_current is Frame)
                    return _current as Frame;
                return null;
            }
            set
            {
                _current = value;
            }
        }


        public Bar Bar
        {
            get
            {
                if (_current is Bar)
                    return _current as Bar;
                return null;
            }
            set
            {
                _current = value;
            }
        }

        public bool IsDraggingOver(Rect rect)
        {
            //var screenPoint = GUIUtility.ScreenToGUIPoint(Event.current.mousePosition);

            return Event.current.type == EventType.mouseDrag && (rect.Contains(Event.current.mousePosition));
        }
    }
}