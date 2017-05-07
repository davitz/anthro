using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;

namespace HammerFingers.Anthro
{
    public class WorldStateManager : MonoBehaviour {

        private int worldState; // instance property
        public int WorldState // public property
        {
            get { return this.worldState; }
            set
            {
                this.worldState = value;
                WorldStateChanged();
            }
        }

        [Header("Main World Lighting (Directional Light)")]
        public GameObject MainLight; // world lighting
        private Light worldLight; // MainLight's "Light" component

        [Header("Health Bar")]
        public Texture Healthbar;
        public Texture HealthbarNeedle;
        public bool isHealthbarVisible;

        [Header("Point Range (e.g. 100 means -100 to 100)")]
        public int MaxPointsRange = 100; // points can't go past 100 or -100

        [Header("Lighting Colour States")]
        public Color PositiveColour;
        public Color NeutralColour;
        public Color NegativeColour;

        void Start()
        {
            worldLight = MainLight.GetComponent<Light>();
        }

        void Update()
        {
            Debug.Log(WorldState);
        }

        private void WorldStateChanged()
        {

            // World lighting changes based on accumulated points

            if (WorldState > 0 && WorldState <= 100)
            {
                worldLight.color = Color.Lerp(NeutralColour, PositiveColour, WorldState / 100f);
                Debug.Log("good light");
            }
            else if (WorldState < 0 && WorldState >= -100)
            {
                worldLight.color = Color.Lerp(NeutralColour, NegativeColour, Mathf.Abs(WorldState) / 100f);
            }
            else // if WorldState is zero
            {
                worldLight.color = NeutralColour;
            }


        }

        void OnGUI()
        {
            if (isHealthbarVisible == true)
            {
                GUI.DrawTexture(new Rect((Screen.width / 2) - (Healthbar.width / 2), 5, Healthbar.width, Healthbar.height), Healthbar);
                DrawNeedleOnBar();
            }
        }

        private void DrawNeedleOnBar()
        {
            float segment = (Healthbar.width/2) / MaxPointsRange;

            float xOrigin = Screen.width / 2;
            float xDraw = 0.0f;

            if (WorldState == 0) xDraw = xOrigin;
            else xDraw = xOrigin + (segment * WorldState);

            GUI.DrawTexture(new Rect(xDraw, 0, HealthbarNeedle.width, HealthbarNeedle.height), HealthbarNeedle);
        }

    }
}

