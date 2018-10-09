using UnityEngine;

public class GameHandler : MonoBehaviour
{
    [SerializeField] private LevelBuilder level;
    [SerializeField] private int levelWidthScale = 16;
    [SerializeField] private int levelHeightScale = 9;

    private static int levelWidth;
    private static int levelHeight;

    private void Start()
    {
        levelWidth = (Screen.currentResolution.width / 100) * levelWidthScale;
        levelHeight = (Screen.currentResolution.height / 100) * levelHeightScale;

        level.InstantiateLevel(levelWidth, levelHeight);
    }
}
