using System;
using UnityEngine;

[Serializable]
public class LevelBuilder
{
    [SerializeField] private Transform groundPrefab;
    [SerializeField] private Transform ceilingPrefab;
    [SerializeField] private Transform backgroundPrefab;
    [SerializeField] private Transform platformPrefab;
    [SerializeField] private Transform platformContainer;
    [SerializeField] private Transform rightLevelEdge;
    [SerializeField] private Transform leftLevelEdge;

    [SerializeField] private float platformMaxXOffset = 3f;
    [SerializeField] private float platformMaxYOffset = 3f;

    private float groundWidth;
    private float groundHeight;

    // Main method for level building. Controls the process of creating ground layers, platforms, exit to the next level, spawning enemies, etc.
    public void InstantiateLevel(int levelWidth, int levelHeight)
    {        
        groundWidth = groundPrefab.GetComponent<SpriteRenderer>().size.x;
        groundHeight = groundPrefab.GetComponent<SpriteRenderer>().size.y;

        // Creating background
        Transform backgroundInstance = GameObject.Instantiate(backgroundPrefab, new Vector3(0, (levelHeight + groundHeight) / 2), groundPrefab.rotation);
        backgroundInstance.GetComponent<SpriteRenderer>().size = new Vector2(levelWidth, levelHeight);

        // Creating ground layer
        Transform groundInstance = GameObject.Instantiate(groundPrefab, new Vector3(0, 0), groundPrefab.rotation);
        groundInstance.GetComponent<SpriteRenderer>().size = new Vector2(levelWidth, groundHeight);
        groundInstance.GetComponent<EdgeCollider2D>().points = new Vector2[] { new Vector2(levelWidth / 2, 0f), new Vector2(-1 * levelWidth / 2, 0f)};

        // Creating roof layer
        Transform ceilingInstance = GameObject.Instantiate(ceilingPrefab, new Vector3(0, levelHeight + groundHeight), ceilingPrefab.rotation);
        ceilingInstance.GetComponent<SpriteRenderer>().size = new Vector2(levelWidth, groundHeight);
        ceilingInstance.GetComponent<EdgeCollider2D>().points = new Vector2[] { new Vector2(levelWidth / 2, 0f), new Vector2(-1 * levelWidth / 2, 0f) };

        // Creating level edges
        CreateEdge(rightLevelEdge, levelWidth, levelHeight, 1);
        CreateEdge(leftLevelEdge, levelWidth, levelHeight, -1);

        // Creating platforms
        CreatePlatforms(levelWidth, levelHeight);
    }

    private void CreateEdge(Transform edgePrefab, int levelWidth, int levelHeight, int sideOffset)
    {
        Transform levelEdgeInstance = GameObject.Instantiate(edgePrefab,
            new Vector3(sideOffset * (levelWidth + groundWidth) / 2, (levelHeight + groundHeight) / 2), edgePrefab.rotation);
        Transform levelEdgeFloorCorner = levelEdgeInstance.Find("GroundCorner");
        Transform levelEdgeCeilingCorner = levelEdgeInstance.Find("CeilingCorner");

        levelEdgeInstance.GetComponent<SpriteRenderer>().size = new Vector2(groundWidth, levelHeight);
        levelEdgeInstance.GetComponent<BoxCollider2D>().size = new Vector2(0.1f, levelHeight);

        levelEdgeFloorCorner.position = new Vector3(levelEdgeInstance.position.x, levelEdgeInstance.position.y - 0.5f *
            (levelEdgeInstance.GetComponent<SpriteRenderer>().size.y + levelEdgeFloorCorner.GetComponent<SpriteRenderer>().size.y));
        levelEdgeCeilingCorner.position = new Vector3(levelEdgeInstance.position.x, levelEdgeInstance.position.y + 0.5f *
            (levelEdgeInstance.GetComponent<SpriteRenderer>().size.y + levelEdgeFloorCorner.GetComponent<SpriteRenderer>().size.y));
    }

    private void CreatePlatforms(int levelWidth, int levelHeight)
    {
        float platformWidth = platformPrefab.GetComponent<SpriteRenderer>().size.x + platformMaxXOffset;
        float platformHeight = platformPrefab.GetComponent<SpriteRenderer>().size.y + platformMaxYOffset;

        int xPlatformCount = Mathf.FloorToInt(levelWidth / platformWidth);
        int yPlatformCount = Mathf.FloorToInt(levelHeight / platformHeight);

        for(int y = 0; y < yPlatformCount; y++)
        {
            float yPos = platformMaxYOffset + platformHeight * y;
            for(int x = 0; x < xPlatformCount; x++)
            {
                float xPos = -1 * levelWidth / 2 + (UnityEngine.Random.Range(0f, platformMaxXOffset) + platformWidth * x);
                GameObject.Instantiate(platformPrefab, new Vector3(xPos, yPos), platformPrefab.rotation, platformContainer);
            }
        }
    }
}
