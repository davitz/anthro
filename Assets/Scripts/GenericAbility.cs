using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HammerFingers.Anthro
{

    // Needs to inherit from MonoBehaviour so that we can use Invoke
    // This is not good practice but it is necessary
    public class GenericAbility// : MonoBehaviour
    {


        // We use properties because ideally this is not an editor component
        // Should really only be used by a PlayerController when a power is selected
        public GameObject Player { get; set; }
        public GameObject SpawnObj { get; set; }
        public int PointValue { get; set; }
        public string Name;

        private List<string> CanSpawnOnObjectsWithTag = new List<string>();

        private WorldStateManager worldstate;

        #region Constructors

        public GenericAbility() { }
        public GenericAbility(string name, GameObject player, GameObject ObjectToSpawn, int pointValue)
        {
            CanSpawnOnObjectsWithTag.Add("Terrain");

            this.Name = name;
            this.Player = player;
            this.SpawnObj = ObjectToSpawn;
            this.PointValue = pointValue;

            worldstate = GameObject.Find("World State").GetComponent<WorldStateManager>();
        }

        #endregion

        #region Cast Definitions

        public void Cast()
        {
            SpawnUsingRayCast();
            worldstate.WorldState += PointValue;
        }

        public void CastAt(Vector3 pos)
        {
            SpawnAt(pos);
            worldstate.WorldState += PointValue;
        }

        public void CastAt(float x, float y, float z)
        {
            CastAt(new Vector3(x, y, z));
            worldstate.WorldState += PointValue;
        }

        #endregion

        #region Spawning with raycast

        private GameObject SpawnUsingRayCast()
        {
            

            Ray mouseRay = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(mouseRay, out hit))
            {
                if (CanSpawnOnObjectsWithTag.Contains(hit.transform.gameObject.tag))
                {
                    return SpawnAt(hit.point);
                }
            }

            return null; // no hit, so return nothing
        }


        #endregion

        #region Static SpawnAt Definitions

        private GameObject SpawnAt(float x, float y, float z)
        {
            GameObject ret = SpawnAt(new Vector3(x, y, z)); ;
            return ret;
        }

        private GameObject SpawnAt(Vector3 pos)
        {
            GameObject ret = GameObject.Instantiate(SpawnObj, pos, Quaternion.Euler(new Vector3(0,Random.Range(0,360),0)));
            return ret;
        }

        private GameObject SpawnAt(Vector3 pos, Quaternion rotation)
        {
            GameObject ret = GameObject.Instantiate(SpawnObj, pos, rotation);
            return ret;
        }

        private GameObject SpawnAt(float x, float y, float z, Quaternion rotation)
        {
            GameObject ret = SpawnAt(new Vector3(x, y, z), rotation);
            return ret;
        }

        #endregion
    }

}