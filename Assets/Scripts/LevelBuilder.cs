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

    [SerializeField] private float platformMinXOffset = 1f;
    [SerializeField] private float platformMaxXOffset = 5f;
    [SerializeField] private float platformMinYOffset = 1f;
    [SerializeField] private float platformMaxYOffset = 3f;
    [SerializeField] private int platformSpawnRate = 70;
    [SerializeField] private int platformAdaptionRate = 30;

    private float groundWidth;
    private float groundHeight;

    public Boundaries LevelBoundaries;

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
        groundInstance.GetComponent<BoxCollider2D>().size = new Vector2(levelWidth, groundHeight);
        LevelBoundaries.BottomBoundary = groundInstance.position.y - groundHeight / 2;

        // Creating roof layer
        Transform ceilingInstance = GameObject.Instantiate(ceilingPrefab, new Vector3(0, levelHeight + groundHeight), ceilingPrefab.rotation);
        ceilingInstance.GetComponent<SpriteRenderer>().size = new Vector2(levelWidth, groundHeight);
        ceilingInstance.GetComponent<BoxCollider2D>().size = new Vector2(levelWidth, groundHeight);
        LevelBoundaries.TopBoundary = ceilingInstance.position.y + groundHeight / 2;

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

        if (sideOffset < 0)
            LevelBoundaries.LeftBoundary = levelEdgeInstance.position.x - groundWidth / 2;
        else
            LevelBoundaries.RightBoundary = levelEdgeInstance.position.x + groundWidth / 2;
    }

    private void CreatePlatforms(int levelWidth, int levelHeight)
    {
        float platformWidth = platformPrefab.GetComponent<SpriteRenderer>().size.x + platformMaxXOffset;
        float platformHeight = platformPrefab.GetComponent<SpriteRenderer>().size.y + platformMaxYOffset;

        int xPlatformCount = Mathf.FloorToInt(levelWidth / platformWidth);
        int yPlatformCount = Mathf.FloorToInt(levelHeight / platformHeight);

        for(int y = 0; y < yPlatformCount; y++)
        {
            float yPos;

            for (int x = 0; x < xPlatformCount; x++)
            {
                int chance = Mathf.CeilToInt(UnityEngine.Random.Range(0f, 100f));

                if (chance < platformSpawnRate)
                {
                    yPos = platformHeight * y;
                    if (y == 0)
                        yPos += platformMaxYOffset;
                    else
                        yPos += UnityEngine.Random.Range(platformMinYOffset, platformMaxYOffset);

                    float xOffset = UnityEngine.Random.Range(platformMinXOffset, platformMaxXOffset);
                    float xPos = -1 * levelWidth / 2 + (xOffset + platformWidth * x);
                    Transform platformInstance =  GameObject.Instantiate(platformPrefab, new Vector3(xPos, yPos), platformPrefab.rotation, platformContainer);

                    chance = Mathf.CeilToInt(UnityEngine.Random.Range(0f, 100f));
                    if(chance < platformAdaptionRate)
                    {
                        SpriteRenderer platformSprite = platformInstance.GetComponent<SpriteRenderer>();
                        platformSprite.drawMode = SpriteDrawMode.Tiled;
                        platformSprite.tileMode = SpriteTileMode.Adaptive;
                        platformSprite.size = new Vector2(platformSprite.size.x + xOffset, platformSprite.size.y);

                        Vector2[] platformColliderPoints = platformInstance.GetComponent<EdgeCollider2D>().points;
                        platformColliderPoints[0] = new Vector2(platformColliderPoints[0].x - xOffset / 2, platformColliderPoints[0].y);
                        platformColliderPoints[1] = new Vector2(platformColliderPoints[1].x + xOffset / 2, platformColliderPoints[1].y);

                        platformInstance.GetComponent<EdgeCollider2D>().points = platformColliderPoints;
                    }
                }
            }
        }
    }
}

public struct Boundaries
{
    public float TopBoundary;
    public float BottomBoundary;
    public float LeftBoundary;
    public float RightBoundary;
}