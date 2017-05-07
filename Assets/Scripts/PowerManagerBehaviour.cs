using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace HammerFingers.Anthro
{

    public class PowerManagerBehaviour : MonoBehaviour
    {

        List<GenericAbility> abilities;

        GenericAbility activeAbility;

        int buttonsX = 40;
        int buttonsY = 20;
        int buttonsHeight = 20;
        int buttonsWidth = 120;
        int buttonsGap = 10;

        GUIStyle selectedStyle;

        // Use this for initialization
        void Start()
        {
            
            selectedStyle = new GUIStyle();
            selectedStyle.fontStyle = FontStyle.Bold;

            var plantTree = new GenericAbility("Plant Tree", GameObject.FindGameObjectWithTag("Player"), Resources.Load<GameObject>("Tree_002"), 1);

            var plantOtherTree = new GenericAbility("Plant Other Tree", GameObject.FindGameObjectWithTag("Player"), Resources.Load<GameObject>("Tree_001"), 1);

            var summonRain = new GenericAbility("Summon Rain", GameObject.FindGameObjectWithTag("Player"), Resources.Load<GameObject>("RainCloud"), 1);

            var extractLimeStone = new GenericAbility("Extract Limestone", GameObject.FindGameObjectWithTag("Player"), Resources.Load<GameObject>("limestone rock 1"), -1);

            var poachTusk = new GenericAbility("Poach Tusks", GameObject.FindGameObjectWithTag("Player"), Resources.Load<GameObject>("lefttusk"), -1);


            abilities = new List<GenericAbility>();
            abilities.Add(plantTree);
            abilities.Add(plantOtherTree);
           // abilities.Add(summonRain);
            abilities.Add(extractLimeStone);
            abilities.Add(poachTusk);
            
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
            for(int i = 0; i < abilities.Count; i++)
            {
                bool button;
                
                
                if(activeAbility.Equals(abilities[i]))
                {
                    button = GUI.Button(new Rect(buttonsX, buttonsY + (i * (buttonsHeight + buttonsGap)), buttonsWidth, buttonsHeight), abilities[i].Name, selectedStyle);
                }
                else
                {
                    button = GUI.Button(new Rect(buttonsX, buttonsY + (i * (buttonsHeight + buttonsGap)), buttonsWidth, buttonsHeight), abilities[i].Name);
                }

                if(button)
                {
                    activeAbility = abilities[i];
                }
            }
        }
    }

}