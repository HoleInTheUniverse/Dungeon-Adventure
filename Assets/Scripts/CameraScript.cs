using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour
{
    [SerializeField] private Transform player;                          // Reference to the player

    private void Update()
    {
        Vector3 newPosition = new Vector3(player.position.x, player.position.y, transform.position.z);
        transform.position = newPosition;
    }
}
