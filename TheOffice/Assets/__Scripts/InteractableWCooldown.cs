using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableWCooldown : MonoBehaviour
{
    [SerializeField] GameObject interactablePopup;
    [SerializeField] float cooldown, stressRelief;

    bool isPlayerInside = false, isOnCooldown = false;
    PlayerController contactingPlayer;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            interactablePopup.SetActive(true);
            isPlayerInside = true;
            contactingPlayer = collision.GetComponent<PlayerController>();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            interactablePopup.SetActive(false);
            isPlayerInside = false;
            contactingPlayer = null;
        }
    }

    private void Update()
    {
        if(!isOnCooldown && isPlayerInside && Input.GetKeyDown(KeyCode.E))
        {
            Interacted();
        }
    }

    private void Interacted()
    {
        StartCoroutine(StartCooldown());
        contactingPlayer?.RelieveStress(stressRelief);
    }

    private IEnumerator StartCooldown()
    {
        isOnCooldown = true;
        yield return new WaitForSeconds(cooldown);
        isOnCooldown = false;
    }
}
