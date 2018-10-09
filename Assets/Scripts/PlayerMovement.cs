using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float speed = 3f;                          // The speed of the player
    [SerializeField] private float jumpForce = 300f;                    // The jumping force    
    [SerializeField] private LayerMask groundLayers;                    // Determines what is ground for the ground collider

    private Rigidbody2D playerRB;                                       // Reference to the player's RigidBody
    private Transform groundCollider;                                   // Reference to player's ground check
    private bool isJumping;                                             // Shows whether player is in jump already

    private void Awake()
    {
        playerRB = GetComponent<Rigidbody2D>();
        groundCollider = transform.Find("GroundCollider");
    }

    private void FixedUpdate()
    {
        isJumping = true;

        Collider2D[] groundColliders = Physics2D.OverlapCircleAll(groundCollider.position, 0.16f, groundLayers);
        foreach (Collider2D collider in groundColliders)
        {
            if(collider.gameObject != gameObject)
                isJumping = false;
        }
    }

    public void Move(float xAxis, bool jump)
    {
        Vector2 movement = new Vector2(xAxis * speed, playerRB.velocity.y);
        playerRB.velocity = movement;

        if (jump && !isJumping)
        {
            isJumping = true;
            playerRB.AddForce(new Vector2(0f, jumpForce));
        }        
    }
}
