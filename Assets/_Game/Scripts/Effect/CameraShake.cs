using UnityEngine;
using DG.Tweening;

public class CameraShake : MonoBehaviour
{
    private Vector3 originalPosition;

    private void Awake()
    {
        originalPosition = transform.localPosition;
    }

    private void OnEnable()
    {
        BoxingManager.onAnyFighterTakeDamage += Shake;
    }

    private void OnDisable()
    {
        BoxingManager.onAnyFighterTakeDamage -= Shake;
    }

    public void Shake()
    {
        transform.DOKill();
        transform.localPosition = originalPosition;

        float duration = 0.16f;
        float strength = 0.08f;
        int vibrato = 8;
        transform.DOShakePosition(duration, strength, vibrato)
                 .OnComplete(() => transform.localPosition = originalPosition);
    }
}
