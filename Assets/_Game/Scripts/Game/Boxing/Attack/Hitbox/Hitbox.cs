
using System;
using UnityEngine;

public class Hitbox : MonoBehaviour
{
    private Fighter owner;
    private AttackHitbox attackHitbox;
    private Action onTakeDamage;

    public void Initialize(Fighter fighter,AttackHitbox attackHitbox,Action callBack)
    {
        owner = fighter;
        this.attackHitbox = attackHitbox;
        onTakeDamage = callBack;
    }

    public void Active(bool _isActive)
    {
        gameObject.SetActive(_isActive);
    }

    public void EffectHit()
    {
        EffectManager.Instance.SpawnEffect(PoolManager.NameObject.Effect_Hit, transform.position);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!attackHitbox.canDamage) return;
        if (other.TryGetComponent(out IDamageable target))
        {
            EffectHit();
            BaseAttackStrategy attack = owner.CurrentAttackStrategy as BaseAttackStrategy;
            target.TakeDamage(attack.GetTotalDamage(), attack);
            onTakeDamage?.Invoke();
        }
    }
}