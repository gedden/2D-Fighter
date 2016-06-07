using Sequencer;
//using Spacewars.Network;
//using Spacewars.Cache;
using UnityEngine;

namespace Owl.Pump
{
    /// <summary>
    /// The pump master is a class that controls the major systems which need to be pumped in different scenes. 
    /// 
    /// Currently this is a requirement to add add to any scene within the game. Examples of this include the ActionSequencer and
    /// network controller
    /// </summary>
    public class PumpMaster : MonoBehaviour
    {
        public void FixedUpdate()
        {
            ActionSequencer.Instance.Pump();
            // NetworkController.Instance.Pump();
			// Fetcher.Instance.Pump();
        }
    }
}
