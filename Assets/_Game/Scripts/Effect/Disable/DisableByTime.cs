using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisableByTime : DisableSystem
{
    [SerializeField] private float TimeDisable = 1.0f;
    private float Timer;

    private void OnEnable()
    {
        Timer = 0.0f;
    }

    private void Update()
    {
        UpdateStep();
    }

    private void UpdateStep()
    {
        Timer += Time.deltaTime;
        if(Timer >= TimeDisable)
        {
            DisableGameObject(gameObject);
        }
    }

    public void DisableObjectByTime(GameObject go, float time)
    {
        StartCoroutine(C_DisableObjectByTime(go, time));
    }

    private IEnumerator C_DisableObjectByTime(GameObject go, float time)
    {
        yield return new WaitForSeconds(time);
        DisableGameObject(go);
    }

    protected override void DisableGameObject(GameObject go)
    {
        base.DisableGameObject(go);
    }
}