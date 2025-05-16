using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public abstract class Fighter : MonoBehaviour , IDamageable
{

    private float health;
    public float Health => health;

    private bool isDie;

    [Header("Components")]
    [SerializeField] private Animator animator;
    [SerializeField] private AttackHitbox attackHitbox;
    public AttackHitbox AttackHitbox => attackHitbox;

    private FighterAnimator fighterAnimator;
    public FighterAnimator FighterAnimator => fighterAnimator;

    private Fighter fighterTarget;
    private TeamData teamData;
    private FighterType fighterType;

    protected IState currentState;

    public IAttackStrategy CurrentAttackStrategy;


    protected virtual void Awake()
    {
        fighterAnimator = new FighterAnimator(animator);
        attackHitbox?.Initialize(this);
    }

    protected virtual void Update()
    {
        currentState?.OnUpdate();
    }

    protected virtual void Refresh()
    {
        health = 100;
        isDie = false;
    }

    public virtual void ActiveFighter(Transform _transform,TeamData _teamData,FighterType _fighterType)
    {
        teamData = _teamData;
        fighterType = _fighterType;
        Refresh();
        transform.position = _transform.position;
        transform.rotation = _transform.rotation;
        gameObject.SetActive(true);
    }

    public virtual void HideFighter()
    {
        gameObject.SetActive(false);
    }

    public void ChangeState(IState newState)
    {
        if (isDie) return;
        if (currentState != null)
            currentState.OnExit();

        currentState = newState;
        currentState.OnEnter(this);
    }

    public void MoveLookTarget()
    {
        var values = Enum.GetValues(typeof(FighterType)).Cast<FighterType>().ToList();
        values.Remove(fighterType);
        var randomValue = values[UnityEngine.Random.Range(0, values.Count)];
        if (!fighterTarget || !fighterTarget.gameObject.activeSelf) 
            fighterTarget = BoxingManager.Instance.GetClosestFighterTarget(randomValue,transform.position);
        if (!fighterTarget) return;

        float _spacing = 0.6f;
        float _moveSpeed = 2.0f;
        float _lookSpeed = 10.0f;
        Vector3 _direction = (fighterTarget.transform.position - transform.position).normalized;
        Vector3 _target = fighterTarget.transform.position - _direction * _spacing;
        transform.position = Vector3.MoveTowards(transform.position, _target, _moveSpeed * Time.deltaTime);
        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(_direction), Time.deltaTime * _lookSpeed);
    }

    public void SetAttackStrategy(AttackType type,int combo)
    {
        CurrentAttackStrategy = AttackSO.Instance.GetAttackByType(type);
        BaseAttackStrategy baseAttackStrategy = CurrentAttackStrategy as BaseAttackStrategy;
        baseAttackStrategy.combo = combo;
    }

    public void Attack()
    {
        CurrentAttackStrategy?.Attack(this);
    }

    public virtual void TakeDamage(float damage,BaseAttackStrategy attackStrategy)
    {
        float _randomDamage = UnityEngine.Random.Range(-2, 3);
        damage += _randomDamage;
        if (Health <= 0) return;
        health -= damage;
        EffectManager.Instance.SpawnDamageTextEffect(transform.position+ Vector3.up * 1.1f,(int)damage);
        if (Health < 0) health = 0;
        ChangeStateOnDamage(attackStrategy.GetAttackType());
        BoxingManager.onAnyFighterTakeDamage?.Invoke();
    }

    public virtual void ApplyHealth(int _health)
    {
        health += _health;
        if(Health > 100) health = 100;
        EffectManager.Instance.SpawnEffect(PoolManager.NameObject.Effect_Health,transform.position);
        BoxingManager.onAnyFighterApplyHealth?.Invoke();
    }

    private void ChangeStateOnDamage(AttackType attackType)
    {
        if (Health <= 0)
        {
            BoxingManager.onAnyFighterDie.Invoke(this);
            ChangeState(new DieState());
            isDie = true;
        }
        else
            ChangeState(new HitState(attackType));
    }
}

