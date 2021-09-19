using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteProgressBar : MonoBehaviour
{
    [SerializeField]
    Transform left, right, progress;

    public void SetValue(float t)
    {
        progress.position = Vector3.Lerp(left.position, right.position, t);
    }

    public float GetValue()
    {
        return InverseLerp(left.position, right.position, progress.position);
    }

    public static float InverseLerp(Vector3 a, Vector3 b, Vector3 value)
    {
        Vector3 AB = b - a;
        Vector3 AV = value - a;
        return Vector3.Dot(AV, AB) / Vector3.Dot(AB, AB);
    }
}
