using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using UnityEngine;

namespace Comboman
{
    [Serializable]
    public class CharacterData
    {
        public String name;
        public String spriteSheet;

        public List<FrameData> Frames;
        public List<MoveData> Moves;

        [XmlIgnoreAttribute]
        public bool Dirty = false;

        [XmlIgnoreAttribute]
        public static readonly string CHARACTER_DATA_PATH = "Assets/Fighter/Data";

        /// <summary>
        /// Simple constructor
        /// </summary>
        public CharacterData()
        {
            Frames = new List<FrameData>();
            Moves = new List<MoveData>();
        }

        public MoveData GetMoveData(String name)
        {
            foreach( var move in Moves )
                if (move.Name == name)
                    return move;
            return null;
        }


        /// <summary>
        /// Write this xml to the data path
        /// </summary>
        public void Write()
        {
            var writer = new System.Xml.Serialization.XmlSerializer(typeof(CharacterData));
            var wfile = new System.IO.StreamWriter(CHARACTER_DATA_PATH +"/"+ name+".xml");
            writer.Serialize(wfile, this);
            Dirty = false;
            wfile.Close();
        }

        public static CharacterData Read(string path)
        {
            System.Xml.Serialization.XmlSerializer reader = new System.Xml.Serialization.XmlSerializer(typeof(CharacterData));
            System.IO.StreamReader file = new System.IO.StreamReader(path);
            var data = (CharacterData)reader.Deserialize(file);
            file.Close();
            return data;
        }

        /// <summary>
        /// Create a new character with the sprite at the path
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static CharacterData Create(string path)
        {
            var c = new CharacterData();
            c.spriteSheet = path;
            c.name = path;

            c.DoAddMissingFrames();

            return c;
        }

        public void DoAddMissingFrames()
        {
            // Find a missing frame data
            foreach (var sprite in LoadSprites())
            {
                var frame = GetFrameForSprite(sprite);

                // Create msising sprite frame
                if (frame == null)
                    CreateFrame(sprite);
            }
        }


        public Sprite [] LoadSprites()
        {
            return Resources.LoadAll<Sprite>("Characters/"+spriteSheet);
        }

        /// <summary>
        /// Gets the frame for the 
        /// passed in sprite. Null if there is none
        /// </summary>
        /// <param name="sprite"></param>
        /// <returns></returns>
        public FrameData GetFrameForSprite(Sprite sprite)
        {
            foreach( var frame in Frames )
                if (frame.SpriteName.ToLower() == sprite.name.ToLower())
                    return frame;
            return null;
        }

        public FrameData CreateFrame(Sprite sprite)
        {
            var result = new FrameData(sprite.name);
            Frames.Add(result);
            Dirty = true;
            return result;
        }

        public FrameData UpdateFrame(Sprite sprite, Rect hitbox, Rect attackbox)
        {
            var frame = GetFrameForSprite(sprite);
            frame.Hitbox = hitbox;
            frame.Attackbox = attackbox;
            Dirty = true;
            return frame;
        }
    }
}
