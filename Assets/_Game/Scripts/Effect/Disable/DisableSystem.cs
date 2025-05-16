using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisableSystem : MonoBehaviour
{
    protected virtual void DisableGameObject(GameObject go)
    {
        go.SetActive(false);
    }
}
