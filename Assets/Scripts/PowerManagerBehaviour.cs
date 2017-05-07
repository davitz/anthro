using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace HammerFingers.Anthro
{

    public class PowerManagerBehaviour : MonoBehaviour
    {

        List<GenericAbility> abilities;

        GenericAbility activeAbility;

        // Use this for initialization
        void Start()
        {
            var plantTree = new GenericAbility("Plant Tree", GameObject.FindGameObjectWithTag("Player"), Resources.Load<GameObject>("Tree_002"), -1);


            abilities = new List<GenericAbility>();
            abilities.Add(plantTree);
            activeAbility = plantTree;

        }

        // Update is called once per frame
        void Update()
        {
            if(Input.GetMouseButtonUp(0) && activeAbility != null)
            {
                activeAbility.Cast();
            }
        }

        private void OnGUI()
        {
            var treeButton = GUI.Button(new Rect(0, 0, 30, 20), "Plant Tree");
        }
    }

}