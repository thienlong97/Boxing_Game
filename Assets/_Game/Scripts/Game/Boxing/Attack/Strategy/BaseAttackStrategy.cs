using UnityEngine;

public abstract class BaseAttackStrategy : ScriptableObject, IAttackStrategy
{
    public GameObject hitVFX;
    public AudioClip attackSFX;
    public float damage;
    [HideInInspector] public int combo;

    public int GetTotalDamage() => (int)damage + combo;

    public abstract AttackType GetAttackType();

    public void Attack(Fighter fighter)
    {
        PlayAnimation(fighter);
        TriggerEffects(fighter);
        Debug.Log($"{GetAttackType()} Attack Executed!");
    }

    protected void PlayAnimation(Fighter fighter)
    {
        fighter.FighterAnimator.PlayAttack(GetAttackType());
    }

    protected void TriggerEffects(Fighter fighter)
    {
        if (hitVFX != null)
        {
            GameObject effect = Instantiate(hitVFX, fighter.transform.position, Quaternion.identity);
            Destroy(effect, 2f);
        }

        if (attackSFX != null)
        {
            AudioSource.PlayClipAtPoint(attackSFX, fighter.transform.position);
        }
    }
}
