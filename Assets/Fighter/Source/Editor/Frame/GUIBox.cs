using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEngine;

namespace Comboman
{
    public class GUIBox
    {
        private static Texture2D BoxTexture = null;

        public Rect Data = new Rect(0,0,10,10);
        private Color _color, _selected;


        private Rect _lastBounds;
        public bool Selected;
        public bool Enabled;
        private bool _dragging;
        private Vector2 dragStart;
        private Vector2 drag;
        private DragType type = DragType.None;
        private string name = "";
        private Texture2D buttonTexture;

        private enum DragType
        {
            None,
            Draw,
            TopLeft,
            TopRight,
            BottomLeft,
            BottomRight,
        }


        public GUIBox(string name, Color color, Texture2D buttonTexture)
        {
            if(BoxTexture==null)
                BoxTexture = AssetDatabase.LoadAssetAtPath<Texture2D>("Assets/Fighter/Artwork/Editor/Box.png");

            _color = color;
            _color.a = 0.5f;
            Selected = false;
            Enabled = false;

            _selected.r = Mathf.Min(_color.r * 4f, 1f);
            _selected.g = Mathf.Min(_color.g * 4f, 1f);
            _selected.b = Mathf.Min(_color.b * 4f, 1f);
            _selected.a = 0.8f;

            this.name = name;
            this.buttonTexture = buttonTexture;
        }

        public Rect ScaledData
        {
            get
            {
                var s = CombomanEditor.Instance.ViewScale;
                return new Rect(Data.x * s, Data.y * s, Data.width * s, Data.height * s);
            }
            set
            {
                var s = CombomanEditor.Instance.ViewScale;
                Data = new Rect(value.x / s, value.y / s, value.width / s, value.height / s);
            }
        }

        public bool DrawToolButton()
        {
            Enabled = GUILayout.Toggle(Enabled, name);

            GUI.color = Selected ? Color.gray : Color.white;

            
            GUI.enabled = Enabled;

            var next = GUILayout.Button(buttonTexture);

            GUI.enabled = true;

            GUI.color = Color.white;
            return next ? !Selected : Selected;
        }

        /// <summary>
        /// Draw the current box
        /// </summary>
        /// <param name="bounds"></param>
        public void Draw(Rect bounds)
        {
            if( bounds.x > 0 )
                _lastBounds = bounds;

            GUI.color = Selected?_selected:_color;

            Rect pos = new Rect(ScaledData);
            pos.x += bounds.x;
            pos.y += bounds.y;

            if (_dragging)
            {
                //Debug.Log(dragStart + " --> " + drag);
                var updated = new Rect(dragStart.x, dragStart.y, drag.x - dragStart.x, drag.y - dragStart.y);
                GUI.DrawTexture(updated, BoxTexture);
            }
            else
            {
                GUI.DrawTexture(pos, BoxTexture);
            }

            GUI.color = Color.white;
            if (!_dragging && Selected)//(Event.current.type == EventType.Repaint)
            {
                var s = 12f;
                var h = s / 2f;

                // Top Left Handle
                var r = new Rect(pos.x + _lastBounds.x - h, pos.y + _lastBounds.y - h, s, s);
                GUILayout.BeginArea(r);
                if (GUILayout.RepeatButton("", GUILayout.Width(s * 2), GUILayout.Height(s * 2)))
                {
                    type = DragType.TopLeft;
                    DoBeginDrag();
                }
                EditorGUIUtility.AddCursorRect(GUILayoutUtility.GetLastRect(), MouseCursor.ResizeUpLeft);
                GUILayout.EndArea();
                

                // Bottom Right Handle
                r = new Rect(pos.x + _lastBounds.x - h + ScaledData.width, pos.y + _lastBounds.y - h + ScaledData.height, s, s);
                GUILayout.BeginArea(r);
                if (GUILayout.RepeatButton("", GUILayout.Width(s * 2), GUILayout.Height(s * 2)))
                {
                    type = DragType.BottomRight;
                    DoBeginDrag();
                }
                EditorGUIUtility.AddCursorRect(GUILayoutUtility.GetLastRect(), MouseCursor.ResizeUpLeft);
                GUILayout.EndArea();                
            }

            if (Selected && Event.current.isMouse)
            {
                Handle(Event.current);
            }
        }

        /// <summary>
        /// Handle the mouse events
        /// </summary>
        /// <param name="e"></param>
        private void Handle(Event e)
        {
            if (!Event.current.isMouse)
                return;

            if (Event.current.type == EventType.mouseDown && GUILayoutUtility.GetLastRect().Contains(Event.current.mousePosition))
            {
                type = DragType.Draw;
                DoBeginDrag();
            }

            if (type == DragType.None)
                return;

            if (Event.current.type == EventType.mouseUp)
                DoEndDrag();
            else if (Event.current.type == EventType.mouseDrag)
                DoDrag();
        }

        private void DoBeginDrag()
        {
            // Last Rect
            var rect = new Rect(GUILayoutUtility.GetLastRect());
            switch (type)
            {
                case DragType.TopLeft:
                    dragStart = new Vector2(rect.x + Event.current.mousePosition.x + _lastBounds.x, rect.y + Event.current.mousePosition.y + _lastBounds.y);
                    drag = new Vector2(_lastBounds.x + ScaledData.x + ScaledData.width, _lastBounds.y + ScaledData.y + ScaledData.height);           
                    break;
                case DragType.TopRight:
                case DragType.BottomLeft:
                case DragType.BottomRight:
                    dragStart = new Vector2(_lastBounds.x + ScaledData.x, _lastBounds.y + ScaledData.y);
                    drag = new Vector2(rect.x + Event.current.mousePosition.x + _lastBounds.x, rect.y + Event.current.mousePosition.y + _lastBounds.y);
                    
                    break;
                case DragType.Draw:
                    dragStart = Event.current.mousePosition;
                    drag = dragStart;
                    break;
            }


        }

        /// <summary>
        /// Update the dragging state
        /// </summary>
        private void DoDrag()
        {
            Debug.Log("DRAGGING : " + type);
            if (!_dragging)
            {
                var mag = Vector2.Distance(dragStart, drag);
                _dragging = (mag > 10);
            }

            switch( type )
            {
                case DragType.TopLeft:
                    dragStart = Event.current.mousePosition;
                    break;

                case DragType.TopRight:
                case DragType.BottomLeft:
                case DragType.BottomRight:
                case DragType.Draw:
                    drag = Event.current.mousePosition;
                    break;
            }
            

            if (_dragging)
                CombomanEditor.Instance.RequestRepaint();
        }

        /// <summary>
        /// End the dragging event
        /// </summary>
        private void DoEndDrag()
        {
            DoDrag();
            if (_dragging)
            {
                _dragging = false;
                ScaledData = new Rect(dragStart.x - _lastBounds.x, dragStart.y - _lastBounds.y, drag.x - dragStart.x, drag.y - dragStart.y);
            }
            type = DragType.None;
        }
    }
}
