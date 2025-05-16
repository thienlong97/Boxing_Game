using System;
using System.Collections.Generic;
using UnityEngine;

public class AttackHitbox : MonoBehaviour
{
    [SerializeField] private List<Hitbox> hitboxes = new();
    private Fighter owner;
    public bool canDamage;

    public void Initialize(Fighter fighter)
    {
        owner = fighter;
        foreach (var hitbox in hitboxes)
        {
            hitbox.Initialize(owner,this, OnHitBoxTakeDamage);
            hitbox.gameObject.SetActive(false);
        }
    }

    public void ActivateHitbox()
    {
        canDamage = true;

        foreach (var hitbox in hitboxes)
        {
            hitbox.gameObject.SetActive(true);
        }
    }

    public void DeactivateAll()
    {
        foreach (var hitbox in hitboxes)
        {
            hitbox.gameObject.SetActive(false);
        }
    }

    public void OnHitBoxTakeDamage()
    {
        canDamage = false;
        DeactivateAll();
    }
}
