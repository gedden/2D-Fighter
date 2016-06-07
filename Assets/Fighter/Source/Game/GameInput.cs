using UnityEngine;

namespace Comboman
{
    public class GameInput : MonoBehaviour
    {
        void Update()
        {
            float horizontal = Input.GetAxis("Horizontal");

            Debug.Log(horizontal);

            Game.Instance.Walk(horizontal);
        }
    }
}