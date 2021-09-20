using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialScreenManager : MonoBehaviour
{
    public Transform[] pages;
    public GameObject prevButton, NextButton;

    int currentIndex;

    public void NextPage()
    {
        pages[currentIndex].gameObject.SetActive(false);
        currentIndex++;
        pages[currentIndex].gameObject.SetActive(true);

        if(currentIndex == pages.Length - 1)
        {
            NextButton.SetActive(false);
        }
        prevButton.SetActive(true);
    }

    public void PreviousPage()
    {
        pages[currentIndex].gameObject.SetActive(false);
        currentIndex--;
        pages[currentIndex].gameObject.SetActive(true);

        if (currentIndex == 0)
        {
            prevButton.SetActive(false);
        }
        NextButton.SetActive(true);
    }
}
