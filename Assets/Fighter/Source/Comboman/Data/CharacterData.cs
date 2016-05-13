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



        public void Write()
        {
            var writer = new System.Xml.Serialization.XmlSerializer(typeof(CharacterData));
            var wfile = new System.IO.StreamWriter("Assets/Fighter/Data/"+name+".xml");
            writer.Serialize(wfile, this);
            
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

    }
}
