using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    public float speed = 6.0F;

    private Vector3 moveDirection = Vector3.zero;

    void Update()
    {
        CharacterController controller = GetComponent<CharacterController>();
       

        moveDirection = new Vector3(Input.GetAxis("Horizontal"), -5, Input.GetAxis("Vertical"));
        controller.Move((moveDirection * speed) * Time.deltaTime);

        



        
    }
}
