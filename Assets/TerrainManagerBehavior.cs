using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ProceduralToolkit;

namespace HammerFingers.Anthro
{

    public class TerrainManagerBehavior : MonoBehaviour
    {
        

        

        public AnthroMap map;

        AnthroLowPolyTerrainGenerator.Config config = new AnthroLowPolyTerrainGenerator.Config();

        Gradient grad = new Gradient();

        protected static void AssignDraftToMeshFilter(MeshDraft draft, GameObject terrain)
        {
            var mesh = draft.ToMesh();
            mesh.RecalculateBounds();

            var meshFilter = terrain.GetComponent<MeshFilter>();
            meshFilter.mesh = mesh;
            meshFilter.sharedMesh = mesh;
        }


        protected void generateTerrain(GameObject terrain, GameObject northNeighbor = null, GameObject southNeighbor = null, GameObject westNeighbor = null, GameObject eastNeighbor = null)
        {

            Mesh northNeighborMesh = (northNeighbor != null) ? northNeighbor.GetComponent<MeshFilter>().mesh : null;
            Mesh southNeighborMesh = (southNeighbor != null) ? southNeighbor.GetComponent<MeshFilter>().mesh : null;
            Mesh westNeighborMesh = (westNeighbor != null) ? westNeighbor.GetComponent<MeshFilter>().mesh : null;
            Mesh eastNeighborMesh = (eastNeighbor != null) ? eastNeighbor.GetComponent<MeshFilter>().mesh : null;

            var draft = AnthroLowPolyTerrainGenerator.TerrainDraft(config, northNeighborMesh, southNeighborMesh, westNeighborMesh, eastNeighborMesh);
            AssignDraftToMeshFilter(draft, terrain);
        }

        // Use this for initialization
        void Start()
        {

            map = new AnthroMap();

        }

        // Update is called once per frame
        void Update()
        {
            var player = GameObject.FindWithTag("Player");
            map.GenerateTilesAroundPosition(player.transform.position);

        }
    }

}