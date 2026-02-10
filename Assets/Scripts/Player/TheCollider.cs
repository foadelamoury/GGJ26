using System;
using UnityEngine;

public class TheCollider : MonoBehaviour, IDamageable
{

   
    public void Die(Collision2D collision)
    {
        GameObject collider = collision.gameObject;
     
    }

    public void TakeDamage(float damage)
    {
    }
}
