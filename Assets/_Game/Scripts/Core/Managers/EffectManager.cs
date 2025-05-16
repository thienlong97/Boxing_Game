using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EffectManager : GenericSingleton<EffectManager>
{
    public GameObject confettiParticle;

    private void OnEnable()
    {
        BoxingManager.onLevelCompleted += Confetti;
    }

    private void OnDisable()
    {
        BoxingManager.onLevelCompleted -= Confetti;
    }

    public void Confetti()
    {
        confettiParticle.SetActive(true);
    }

    public void SpawnDamageTextEffect(Vector3 pos,int damage)
    {
        GameObject obj = SpawnEffect(PoolManager.NameObject.Effect_HitText, pos);
        obj.GetComponent<DamageTextEffect>().Active(damage);
    }

    public GameObject SpawnEffect(PoolManager.NameObject nameObjectEffect, Vector3 pos)
    {
        GameObject effectGO = PoolManager.Instance.GetObject(nameObjectEffect) as GameObject;
        effectGO.transform.position = pos;
        effectGO.SetActive(true);
        return effectGO;
    }

    private List<ParticleSystem> GetParticleSystemList(GameObject _go)
    {
        List<ParticleSystem> listParticleSystem = new ();
        listParticleSystem.Add(_go.GetComponent<ParticleSystem>());

        for (int c = 0; c < _go.transform.childCount; c++)
        {
            listParticleSystem.Add(_go.transform.GetChild(c).GetComponent<ParticleSystem>());
        }
        return listParticleSystem;
    }
}
