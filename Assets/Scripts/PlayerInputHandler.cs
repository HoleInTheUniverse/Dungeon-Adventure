using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInputHandler : MonoBehaviour
{
    private bool jump;
    private PlayerMovement player;

    private void Awake()
    {
        player = GetComponent<PlayerMovement>();
    }

    private void Update()
    {
        if (!jump)
            jump = Input.GetButtonDown("Jump");
    }

    private void FixedUpdate()
    {
        float xAxis = Input.GetAxis("Horizontal");
        player.Move(xAxis, jump);
        jump = false;
    }
}
