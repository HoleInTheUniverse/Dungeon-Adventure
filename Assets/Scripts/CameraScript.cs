using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour
{
    [SerializeField] private Transform player;                          // Reference to the player

    private LevelBuilder level;

    private void Start()
    {
        level = GameObject.Find("GameManager").GetComponent<GameHandler>().Level;
    }

    private void Update()
    {
        Vector3 newPosition = new Vector3(player.position.x, player.position.y, transform.position.z);
        float xPos = newPosition.x;
        float yPos = newPosition.y;

        if (newPosition.y - Camera.main.orthographicSize < level.LevelBoundaries.BottomBoundary)
            yPos += level.LevelBoundaries.BottomBoundary - (newPosition.y - Camera.main.orthographicSize);
        else if (newPosition.y + Camera.main.orthographicSize > level.LevelBoundaries.TopBoundary)
            yPos -= (newPosition.y + Camera.main.orthographicSize) - level.LevelBoundaries.TopBoundary;

        if (newPosition.x - Camera.main.orthographicSize * Camera.main.aspect < level.LevelBoundaries.LeftBoundary)
            xPos += level.LevelBoundaries.LeftBoundary - (newPosition.x - Camera.main.orthographicSize * Camera.main.aspect);
        else if (newPosition.x + Camera.main.orthographicSize * Camera.main.aspect > level.LevelBoundaries.RightBoundary)
            xPos -= (newPosition.x + Camera.main.orthographicSize * Camera.main.aspect) - level.LevelBoundaries.RightBoundary;

        newPosition = new Vector3(xPos, yPos, newPosition.z);
        transform.position = newPosition;
    }
}
