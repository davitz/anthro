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
        public GameObject MainLight; // world lighting
        public Texture Healthbar;
        public Texture HealthbarNeedle;
        public int MaxPointsRange = 100; // points can't go past 100 or -100


        private void WorldStateChanged()
        {
            // do things with light (using MainLight object)
        }

        void Update()
        {
            
        }

        void temp()
        {
            WorldState = (new System.Random()).Next(-100, 100);
        }

        void Start()
        {
            //WorldState = 10;
        }

        void OnGUI()
        {
            GUI.DrawTexture(new Rect((Screen.width / 2) - (Healthbar.width /2), 5, Healthbar.width, Healthbar.height), Healthbar);
            DrawNeedleOnBar();
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

