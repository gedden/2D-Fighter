using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Comboman
{
    public class Game : UnityEngine.MonoBehaviour
    {
        public Character Player { get; private set; }

        public static Game Instance { get; private set; }


        public void Start()
        {
            NewGame();
            Instance = this;

            transform.gameObject.AddComponent<GameInput>();
        }

        public void NewGame()
        {
            // Add the main fighter
            var data = CharacterData.Read(CharacterData.CHARACTER_DATA_PATH + "/Ryu.xml");
            Player = Character.Create(data);

            Player.transform.SetParent(this.transform, false);
            Player.transform.localPosition = new Vector3(0, -60, 0);
            Player.name = "Player";
        }

        public void Walk(float dir)
        {
            var current = Player.transform.localPosition;

            current.x += dir * Time.deltaTime * Player.Speed;

            Player.transform.localPosition = current;
        }
    }
}
