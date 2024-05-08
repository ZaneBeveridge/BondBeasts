using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public class MenuMan : MonoBehaviour
{
    public GameObject mainScreen;
    public GameObject loadingScreen;

    public string mainScene;


    public TextMeshProUGUI text;
    public Slider slider;
    

    private float time = 0f;

    public void LoadScene()
    {
        mainScreen.SetActive(false);
        loadingScreen.SetActive(true);
        StartCoroutine(LoadAsynchronously(mainScene));
    }


    IEnumerator LoadAsynchronously(string sceneName)
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName);
        operation.allowSceneActivation = false;

        while (!operation.isDone)
        {
            time = time + Time.deltaTime;
            text.text = "LOADING... " + (int)(operation.progress * 100f) + "%";
            
            float progress = Mathf.Clamp01(operation.progress / 0.9f);
            slider.value = progress;

            if (operation.progress >= 0.9f)
            {
                text.text = "FINALIZING BOND BEASTS...";
                operation.allowSceneActivation = true;
            }
            yield return null;
        }
    }



    public void ResetSaveAndCloseGame()
    {
        string[] filePaths = Directory.GetFiles(Application.persistentDataPath + "/Saves/");

        foreach (string item in filePaths)
        {
            File.Delete(item);
        }

        Application.Quit();
    }
}
