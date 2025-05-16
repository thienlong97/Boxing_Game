using TMPro;
using UnityEngine;
using DG.Tweening;

public class DamageTextEffect : MonoBehaviour
{
    [SerializeField] private TMP_Text damageText;
    [SerializeField] private float moveDistance = 1f;
    [SerializeField] private float duration = 1f;
    [SerializeField] private float startScale = 0.5f;
    [SerializeField] private float endScale = 1f;

    private void OnEnable()
    {
        Active(5);
    }

    public void Active(int damage)
    {
        gameObject.SetActive(true);
        damageText.text = damage.ToString();
        PlayEffect();
    }

    private void PlayEffect()
    {
        Color color = damageText.color;
        color.a = 1.0f;
        damageText.color = color;
        transform.localScale = Vector3.one * startScale;

        transform
            .DOScale(endScale, duration * 0.3f)
            .SetEase(Ease.OutElastic);

        float _target = transform.position.y + moveDistance;
        transform
            .DOMoveY(_target, duration)
            .SetEase(Ease.OutSine);

        damageText
            .DOFade(0, 0.5f)
            .OnComplete(() => gameObject.SetActive(false)).SetDelay(1.0f);
    }
}
