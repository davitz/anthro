using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace HammerFingers.Anthro
{
    public class PlayerController : MonoBehaviour
    {

        public float speed = 6.0F;

        private Vector3 moveDirection = Vector3.zero;

        private GameObject Power1SpawnObj;

        void Start()
        {
            Power1SpawnObj = Resources.Load("Prefabs/BlueBall") as GameObject;
        }

        void Update()
        {
            CharacterController controller = GetComponent<CharacterController>();


            moveDirection = new Vector3(Input.GetAxis("Horizontal"), -5, Input.GetAxis("Vertical"));
            controller.Move((moveDirection * speed) * Time.deltaTime);




            /*if (Input.GetKey("joystick button 0"))
            {
                GenericAbility power1 = new GenericAbility(this.gameObject, Power1SpawnObj, 0);
                power1.Cast();
            }*/

        }
    }
}