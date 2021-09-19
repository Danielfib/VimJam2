using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class HoverButtonAnimation : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField]
    private float sizeMultiplier, animationSpeed;

    Vector3 initSize, endSize;
    private void Start()
    {
        initSize = transform.localScale;
        endSize = initSize * sizeMultiplier;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        StopAllCoroutines();
        StartCoroutine(LerpUp());
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        StopAllCoroutines();
        StartCoroutine(LerpDown());
    }

    IEnumerator LerpUp()
    {
        float progress = 0;
        while (progress <= 1)
        {
            transform.localScale = Vector3.Lerp(initSize, endSize, progress);
            progress += Time.deltaTime * animationSpeed;
            yield return null;
        }
        transform.localScale = endSize;
    }

    IEnumerator LerpDown()
    {
        float progress = 0;
        while (progress <= 1)
        {
            transform.localScale = Vector3.Lerp(endSize, initSize, progress);
            progress += Time.deltaTime * animationSpeed;
            yield return null;
        }
        transform.localScale = initSize;

    }
}
