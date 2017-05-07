using System;
using System.Collections.Generic;
using UnityEngine;
using ProceduralToolkit;

namespace HammerFingers.Anthro
{

    public class AnthroTile
    {
        public Biome biome { get; set; }
        public GameObject tileObject { get; set; }
    }

    public class BiomeAffinity
    {
        public Biome biome;
        public int factor;
    }

    public class Biome
    {
        public Biome()
        {
            affinities = new List<BiomeAffinity>();
        }
        public string name { get; set; }
        public AnthroLowPolyTerrainGenerator.Config config { get; set; }
        public int probability;
        public int continuityBias;
        public List<BiomeAffinity> affinities;
    }


    public class AnthroMap
    {
        public int bufferTiles;
        public int tileSize;

        List<List<AnthroTile>> NWQuad;
        List<List<AnthroTile>> NEQuad;
        List<List<AnthroTile>> SWQuad;
        List<List<AnthroTile>> SEQuad;

        Dictionary<string, Biome> biomes;

        AnthroLowPolyTerrainGenerator.Config terrainConfig = new AnthroLowPolyTerrainGenerator.Config();


        public AnthroMap(int _bufferTiles = 5, int _tileSize = 20)
        {
            bufferTiles = _bufferTiles;
            tileSize = _tileSize;

            NWQuad = new List<List<AnthroTile>>();
            NEQuad = new List<List<AnthroTile>>();
            SWQuad = new List<List<AnthroTile>>();
            SEQuad = new List<List<AnthroTile>>();


            biomes = new Dictionary<string, Biome>();

            var plainsConfig = new AnthroLowPolyTerrainGenerator.Config();
            plainsConfig.gradient = new Gradient();
            plainsConfig.gradient.SetKeys(new[] { new GradientColorKey(new Color(.3f, .2f, 0f), 0), new GradientColorKey(new Color(0.2f, 0.4f, 0), 1) }, new[] { new GradientAlphaKey(1, 0), new GradientAlphaKey(1, 1) });
            plainsConfig.noiseScale = 2.75f;
            plainsConfig.terrainSize.y = .75f;
            var plains = new Biome();
            plains.name = "plains";
            plains.config = plainsConfig;
            plains.probability = 30;
            plains.continuityBias = 1;
            biomes.Add("plains", plains);

            var hillsConfig = new AnthroLowPolyTerrainGenerator.Config();
            hillsConfig.gradient = new Gradient();
            hillsConfig.gradient.SetKeys(new[] { new GradientColorKey(new Color(.3f, .2f, 0f), 0), new GradientColorKey(new Color(0.1f, 0.6f, 0), 1) }, new[] { new GradientAlphaKey(1, 0), new GradientAlphaKey(1, 1) });
            hillsConfig.noiseScale = 20f;
            hillsConfig.terrainSize.y = 2f;
            var hills = new Biome();
            hills.name = "hills";
            hills.config = hillsConfig;
            hills.probability = 10;
            hills.continuityBias = 5;
            biomes.Add("hills", hills);


            var canyonsConfig = new AnthroLowPolyTerrainGenerator.Config();
            canyonsConfig.gradient = new Gradient();
            canyonsConfig.gradient.SetKeys(new[] { new GradientColorKey(new Color(.5f, .2f, 0f), 0), new GradientColorKey(new Color(.3f, .2f, 0f), 1) }, new[] { new GradientAlphaKey(1, 0), new GradientAlphaKey(1, 1) });
            canyonsConfig.noiseScale = 50f;
            canyonsConfig.terrainSize.y = 5f;
            canyonsConfig.heightOffset = -.75f;
            var canyons = new Biome();
            canyons.name = "canyons";
            canyons.config = canyonsConfig;
            canyons.probability = 0;
            canyons.continuityBias = 20;
            biomes.Add("canyons", canyons);

            BiomeAffinity canyonPlainsAffinity = new BiomeAffinity();
            canyonPlainsAffinity.biome = canyons;
            canyonPlainsAffinity.factor = 1;
            plains.affinities.Add(canyonPlainsAffinity);


            var rocksConfig = new AnthroLowPolyTerrainGenerator.Config();
            rocksConfig.gradient = new Gradient();
            rocksConfig.gradient.SetKeys(new[] { new GradientColorKey(new Color(.5f, .2f, 0f), 0), new GradientColorKey(new Color(.3f, .2f, 0f), 1) }, new[] { new GradientAlphaKey(1, 0), new GradientAlphaKey(1, 1) });
            rocksConfig.noiseScale = 50f;
            rocksConfig.terrainSize.y = 5f;
            rocksConfig.heightOffset = .2f;
            var rocks = new Biome();
            rocks.name = "rocks";
            rocks.config = rocksConfig;
            rocks.probability = 0;
            rocks.continuityBias = 20;
            biomes.Add("rocks", rocks);

            BiomeAffinity rocksHillsAffinity = new BiomeAffinity();
            rocksHillsAffinity.biome = rocks;
            rocksHillsAffinity.factor = 4;
            hills.affinities.Add(rocksHillsAffinity);

        }


        public AnthroTile GetTile(int x, int z)
        {
            List<List<AnthroTile>> selectedQuad;
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


        public AnthroTile AddTile(int x, int z)
        {
            List<List<AnthroTile>> selectedQuad;
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
                selectedQuad.Add(new List<AnthroTile>());
            }

            while(selectedQuad[i].Count < j + 1)
            {
                selectedQuad[i].Add(null);
            }
            
            if(selectedQuad[i][j] == null)
            {
                selectedQuad[i][j] = new AnthroTile();
                selectedQuad[i][j].tileObject = (GameObject)UnityEngine.Object.Instantiate(Resources.Load("AnthroTile"));
                selectedQuad[i][j].biome = SelectBiome(x, z);

                generateTerrain(selectedQuad[i][j].biome, selectedQuad[i][j], GetTile(x, z + 1), GetTile(x, z - 1), GetTile(x - 1, z), GetTile(x + 1, z));

                Vector3 newPos = new Vector3(x * tileSize, selectedQuad[i][j].tileObject.transform.position.y, z * tileSize);
                selectedQuad[i][j].tileObject.transform.position = newPos;

                return selectedQuad[i][j];


            }

            return null;

        }


        public List<AnthroTile> GetNeighbors(int x, int z)
        {
            List<AnthroTile> neighbors = new List<AnthroTile>();

            AnthroTile neighbor = GetTile(x + 1, z);
            if (neighbor != null)
                neighbors.Add(neighbor);

            neighbor = GetTile(x - 1, z);
            if (neighbor != null)
                neighbors.Add(neighbor);

            neighbor = GetTile(x, z + 1);
            if (neighbor != null)
                neighbors.Add(neighbor);

            neighbor = GetTile(x, z - 1);
            if (neighbor != null)
                neighbors.Add(neighbor);

            return neighbors;

        }


        protected string SelectRandom(List<string> items)
        {
            return items[UnityEngine.Random.Range(0, items.Count)];
        }

        protected Biome SelectBiome(int x, int z)
        {
            List<string> selectionList = new List<string>();

            foreach(string biomeKey in biomes.Keys)
            {
                for(int i = 0; i < biomes[biomeKey].probability; i++)
                {
                    selectionList.Add(biomes[biomeKey].name);
                }
            }

            foreach(AnthroTile neighbor in GetNeighbors(x,z))
            {
                for(int i = 0; i < neighbor.biome.continuityBias; i++)
                    selectionList.Add(neighbor.biome.name);
                foreach(BiomeAffinity affinity in neighbor.biome.affinities)
                {
                    for(int i = 0; i < affinity.factor; i++)
                    {
                        selectionList.Add(affinity.biome.name);
                    }
                }
            }

            return biomes[SelectRandom(selectionList)];
            
        }


        protected void generateTerrain(Biome biome, AnthroTile newTile, AnthroTile northNeighbor = null, AnthroTile southNeighbor = null, AnthroTile westNeighbor = null, AnthroTile eastNeighbor = null)
        {

            Mesh northNeighborMesh = (northNeighbor != null) ? northNeighbor.tileObject.GetComponent<MeshFilter>().mesh : null;
            Mesh southNeighborMesh = (southNeighbor != null) ? southNeighbor.tileObject.GetComponent<MeshFilter>().mesh : null;
            Mesh westNeighborMesh = (westNeighbor != null) ? westNeighbor.tileObject.GetComponent<MeshFilter>().mesh : null;
            Mesh eastNeighborMesh = (eastNeighbor != null) ? eastNeighbor.tileObject.GetComponent<MeshFilter>().mesh : null;


            var draft = AnthroLowPolyTerrainGenerator.TerrainDraft(biome.config, northNeighborMesh, southNeighborMesh, westNeighborMesh, eastNeighborMesh);
            AssignDraftToMeshFilter(draft, newTile.tileObject);


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
