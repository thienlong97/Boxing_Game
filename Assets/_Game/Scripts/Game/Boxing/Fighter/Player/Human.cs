using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AnimationInstancing;

public class Human : MonoBehaviour
{
    public CheeringType cheeringType;
    public enum CheeringType
    {
        Standing = 0,
        Sitting  = 1
    }

    [SerializeField] private AnimationInstancing.AnimationInstancing animationInstancing;
    private void Start()
    {
        animationInstancing.playSpeed = 0;
        float random = Random.Range(0.2f, 1.5f);
        Invoke(nameof(SetDefaultAnimation), random);
    }

    private void SetDefaultAnimation()
    {
        animationInstancing.playSpeed = 1;
        int _animationIndex = (int)cheeringType;
        animationInstancing.PlayAnimation(_animationIndex);
    }

}
