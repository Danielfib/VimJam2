using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelsScreenManager : MonoBehaviour
{
    private Color noStarColor = new Color(0.1f, 0.1f, 0.1f, 1f);

    public void LoadLevel(int buildIndex)
    {
        GameManager.Instance.LoadLevel(buildIndex);
    }

    public void LoadLevelsStatus()
    {
        for (int i = 1; i < SceneManager.sceneCountInBuildSettings - 1; i++)
        {
            int stars = PlayerPrefs.GetInt(i.ToString());
            var starImages = transform.GetChild(i - 1).GetComponentsInChildren<Image>();

            starImages[1].color = stars > 0 ? Color.white : noStarColor;
            starImages[2].color = stars > 1 ? Color.white : noStarColor;
            starImages[3].color = stars > 2 ? Color.white : noStarColor;
        }
    }
}
