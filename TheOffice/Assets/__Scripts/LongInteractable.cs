using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LongInteractable : MonoBehaviour
{
    [SerializeField] GameObject interactablePopup;
    [SerializeField] float duration, stressRelief;
    [SerializeField] SpriteProgressBar pb;

    PlayerController contactingPlayer;
    bool isBeingInteracted = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            interactablePopup.SetActive(true);
            contactingPlayer = collision.GetComponent<PlayerController>();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            interactablePopup.SetActive(false);
            contactingPlayer = null;
        }
    }

    private void Update()
    {
        if(contactingPlayer != null && Input.GetKeyDown(KeyCode.E) && !isBeingInteracted)
        {
            Interacted();
        }
    }

    void Interacted()
    {
        isBeingInteracted = true;
        interactablePopup.SetActive(false);
        pb.gameObject.SetActive(true);
        StartCoroutine(ProgressCoroutine());
        //TODO: block player movement
    }

    IEnumerator ProgressCoroutine()
    {
        var progress = 0f;
        var speed = Time.fixedDeltaTime / duration;
        while(progress < 1)
        {
            yield return new WaitForFixedUpdate();
            pb.SetValue(progress);
            progress += speed;
        }
        FinishedInteracting();
    }

    void FinishedInteracting()
    {
        isBeingInteracted = false;
        pb.gameObject.SetActive(false);
        contactingPlayer.RelieveStress(stressRelief);
        //TODO: re-enable player input
    }

    public void Interrupt()
    {
        //TODO: interrupt when boss caught player playing
    }
}