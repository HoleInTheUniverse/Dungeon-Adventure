using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInputHandler : MonoBehaviour
{
    private bool jump;
    private PlayerMovement player;
    private WeaponBehaviour weapon;

    private void Awake()
    {
        player = GetComponent<PlayerMovement>();
        weapon = transform.Find("Weapon").GetComponentInChildren<WeaponBehaviour>();
    }

    private void Update()
    {
        if (!jump)
            jump = Input.GetButtonDown("Jump");

        if (Input.GetButtonDown("Fire1"))
            weapon.Attack();
    }

    private void FixedUpdate()
    {
        float xAxis = Input.GetAxis("Horizontal");
        player.Move(xAxis, jump);
        jump = false;
    }
}
