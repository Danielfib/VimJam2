using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeactivateOutsideWindows : MonoBehaviour
{
    void Start()
    {
#if !UNITY_STANDALONE_WIN
        gameObject.SetActive(false);
#endif
    }
}
