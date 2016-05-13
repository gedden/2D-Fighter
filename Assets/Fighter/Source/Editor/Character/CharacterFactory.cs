using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Comboman
{
    public class CharacterFactory
    {
        public static void Write(CharacterData data)
        {
            TextAsset asset = new TextAsset();

            AssetDatabase.CreateAsset(asset, "Data/Characters/" + data.name);
        }
    }
}
