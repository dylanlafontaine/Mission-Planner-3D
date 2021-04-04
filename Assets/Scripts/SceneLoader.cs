using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneLoader : MonoBehaviour
{
    private bool vrSceneLoaded;
    private bool twoDSceneLoaded;
    public Button enableVRButton;
    public Image progressBar;
    public Image progressBarBackground;
    public Text progressText;
    public Text progressPercentage;
    AsyncOperation sceneToLoad;
    ///<summary>
    ///Loads the VR Scene from the 2D Scene
    ///</summary>
    public void LoadVRScene()
    {
        vrSceneLoaded = SceneManager.GetSceneByName("CitySimulator").isLoaded;
        twoDSceneLoaded = SceneManager.GetSceneByName("Initial World").isLoaded;

        if (!vrSceneLoaded)
        {
            enableVRButton.gameObject.SetActive(false);
            ShowLoadingBar();
            sceneToLoad = SceneManager.LoadSceneAsync("CitySimulator", LoadSceneMode.Additive);
            progressText.text = "Loading Mission Planner VR";
            StartCoroutine(LoadingScreen());
        }
        if (twoDSceneLoaded)
        {
            SceneManager.UnloadSceneAsync("Initial World");
        }
    }

    public void Load2DScene()
    {
        vrSceneLoaded = SceneManager.GetSceneByName("CitySimulator").isLoaded;
        twoDSceneLoaded = SceneManager.GetSceneByName("Initial World").isLoaded;
        if (!twoDSceneLoaded)
        {
            SceneManager.LoadSceneAsync("Initial World");
        }
        if (vrSceneLoaded)
        {
            SceneManager.UnloadSceneAsync("CitySimulator");
        }
    }

    public void ShowLoadingBar()
    {
        progressBar.gameObject.SetActive(true);
        progressBarBackground.gameObject.SetActive(true);
        progressText.gameObject.SetActive(true);
        progressPercentage.gameObject.SetActive(true);
    }

    public void HideLoadingBar()
    {
        progressBar.gameObject.SetActive(false);
        progressBarBackground.gameObject.SetActive(false);
        progressText.gameObject.SetActive(false);
        progressPercentage.gameObject.SetActive(false);
    }

    IEnumerator LoadingScreen()
    {
        float totalProgress = 0;
        while (!sceneToLoad.isDone)
        {
            totalProgress = Mathf.Clamp01(sceneToLoad.progress / .9f);
            progressBar.fillAmount = totalProgress;
            progressPercentage.text = Mathf.Round(totalProgress * 100) + "%";
            yield return null;
        }
        HideLoadingBar();
    }
}
