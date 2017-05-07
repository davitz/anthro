using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HammerFingers.Anthro
{

    /* This class is meant to test the world state via the Unity inspector */

    public class WorldStateTester : MonoBehaviour
    {
        [Range(-100, 100)]
        public int WorldState;

        private WorldStateManager stateManager;

        void Start()
        {
            stateManager = this.gameObject.GetComponent<WorldStateManager>();
        }

        void Update()
        {
            //stateManager.WorldState = WorldState;
        }
    }
}


