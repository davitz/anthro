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

        #region Constructors

        public GenericAbility() { }
        public GenericAbility(string name, GameObject player, GameObject ObjectToSpawn, int pointValue)
        {
            CanSpawnOnObjectsWithTag.Add("Terrain");

            this.Name = name;
            this.Player = player;
            this.SpawnObj = ObjectToSpawn;
            this.PointValue = pointValue;
        }

        #endregion

        #region Cast Definitions

        public void Cast()
        {
            SpawnUsingRayCast();
        }

        public void CastAt(Vector3 pos)
        {
            SpawnAt(pos);
        }

        public void CastAt(float x, float y, float z)
        {
            CastAt(new Vector3(x, y, z));
        }

        #endregion

        #region Spawning with raycast

        private GameObject SpawnUsingRayCast()
        {
            

            Ray mouseRay = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(mouseRay, out hit))
            {
                Debug.Log("hit " + hit.transform.gameObject.name);
                if (CanSpawnOnObjectsWithTag.Contains(hit.transform.gameObject.tag))
                {
                    Debug.Log("in spawn block");
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