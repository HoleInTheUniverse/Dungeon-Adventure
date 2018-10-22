using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponBehaviour : MonoBehaviour
{
    private Animator weaponAnimator;

    private void Start()
    {
        weaponAnimator = GetComponent<Animator>();
    }

    public void Attack()
    {
        weaponAnimator.SetTrigger("Attack");
    }
}
