using System;
using System.Collections.Generic;
using UnityEngine;
using ProceduralToolkit;

namespace HammerFingers.Anthro
{

    public class AnthroMap
    {
        public int bufferTiles;
        public int tileSize;

        List<List<GameObject>> NWQuad;
        List<List<GameObject>> NEQuad;
        List<List<GameObject>> SWQuad;
        List<List<GameObject>> SEQuad;

        AnthroLowPolyTerrainGenerator.Config terrainConfig = new AnthroLowPolyTerrainGenerator.Config();


        public AnthroMap(int _bufferTiles = 5, int _tileSize = 20)
        {
            bufferTiles = _bufferTiles;
            tileSize = _tileSize;

            NWQuad = new List<List<GameObject>>();
            NEQuad = new List<List<GameObject>>();
            SWQuad = new List<List<GameObject>>();
            SEQuad = new List<List<GameObject>>();


            terrainConfig.gradient = new Gradient();
            terrainConfig.gradient.SetKeys(new[] { new GradientColorKey(new Color(0, 0, 0), 0), new GradientColorKey(new Color(0, 0.5f, 0), 1) }, new[] { new GradientAlphaKey(1, 0), new GradientAlphaKey(1, 1) });
            terrainConfig.noiseScale = 2.75f;
            terrainConfig.cellSize = 2f;
            terrainConfig.terrainSize.y = 2;
        }


        public GameObject GetTile(int x, int z)
        {
            List<List<GameObject>> selectedQuad;
            int i = x;
            int j = z;

            if (x >= 0)
            {
                if (z >= 0)
                {
                    selectedQuad = NEQuad;
                }
                else
                {
                    selectedQuad = SEQuad;
                    j = Math.Abs(z + 1);
                }
            }
            else
            {
                if (z >= 0)
                {
                    selectedQuad = NWQuad;
                    i = Math.Abs(x + 1); ;
                }
                else
                {
                    selectedQuad = SWQuad;
                    i = Math.Abs(x + 1); ;
                    j = Math.Abs(z + 1); ;
                }
            }

            if (selectedQuad.Count < i + 1)
            {
                return null;
            }

            if(selectedQuad[i].Count < j + 1)
            {
                return null;
            }

            return selectedQuad[i][j];
        }


        public void GenerateTilesAroundPosition(Vector3 position)
        {
            GenerateTilesAroundIndex((int)(position.x / tileSize), (int)(position.z / tileSize));
        }


        public void GenerateTilesAroundIndex(int x, int z)
        {
            AddTile(x, z);
            
            for(int i = 0; i < bufferTiles + 1; i++)
            {
                for(int j = 0; j < bufferTiles + 1; j++)
                {
                    AddTile(x + i, z + j);
                    AddTile(x - i, z + j);
                    AddTile(x + i, z - j);
                    AddTile(x - i, z - j);
                }
            }

        }


        public GameObject AddTile(int x, int z)
        {
            List<List<GameObject>> selectedQuad;
            int i = x;
            int j = z;

            if(x >= 0)
            {
                if(z >= 0)
                {
                    selectedQuad = NEQuad;
                }
                else
                {
                    selectedQuad = SEQuad;
                    j = Math.Abs(z + 1);
                }
            }
            else
            {
                if (z >= 0)
                {
                    selectedQuad = NWQuad;
                    i = Math.Abs(x + 1); ;
                }
                else
                {
                    selectedQuad = SWQuad;
                    i = Math.Abs(x + 1); ;
                    j = Math.Abs(z + 1); ;
                }
            }


            while(selectedQuad.Count < i + 1)
            {
                selectedQuad.Add(new List<GameObject>());
            }

            while(selectedQuad[i].Count < j + 1)
            {
                selectedQuad[i].Add(null);
            }
            
            if(selectedQuad[i][j] == null)
            {
                selectedQuad[i][j] = (GameObject)UnityEngine.Object.Instantiate(Resources.Load("AnthroTile"));
      
                generateTerrain(selectedQuad[i][j], GetTile(x, z + 1), GetTile(x, z - 1), GetTile(x - 1, z), GetTile(x + 1, z));

                Vector3 newPos = new Vector3(x * tileSize, selectedQuad[i][j].transform.position.y, z * tileSize);
                selectedQuad[i][j].transform.position = newPos;

                return selectedQuad[i][j];


            }

            return null;

        }


        protected void generateTerrain(GameObject terrain, GameObject northNeighbor = null, GameObject southNeighbor = null, GameObject westNeighbor = null, GameObject eastNeighbor = null)
        {

            Mesh northNeighborMesh = (northNeighbor != null) ? northNeighbor.GetComponent<MeshFilter>().mesh : null;
            Mesh southNeighborMesh = (southNeighbor != null) ? southNeighbor.GetComponent<MeshFilter>().mesh : null;
            Mesh westNeighborMesh = (westNeighbor != null) ? westNeighbor.GetComponent<MeshFilter>().mesh : null;
            Mesh eastNeighborMesh = (eastNeighbor != null) ? eastNeighbor.GetComponent<MeshFilter>().mesh : null;

            var draft = AnthroLowPolyTerrainGenerator.TerrainDraft(terrainConfig, northNeighborMesh, southNeighborMesh, westNeighborMesh, eastNeighborMesh);
            AssignDraftToMeshFilter(draft, terrain);


        }

        protected static void AssignDraftToMeshFilter(MeshDraft draft, GameObject terrain)
        {
            var mesh = draft.ToMesh();
            mesh.RecalculateBounds();


            var meshFilter = terrain.GetComponent<MeshFilter>();
            meshFilter.mesh = mesh;
            meshFilter.sharedMesh = mesh;

            var meshCollider = terrain.GetComponent<MeshCollider>();
            meshCollider.sharedMesh = mesh;
        }

    }
}
