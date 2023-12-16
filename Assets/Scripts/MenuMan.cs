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
    public GameObject button;

    public string mainScene;


    public TextMeshProUGUI text;
    public Slider slider;
    public GameObject loadingScreen;

    private float time = 0f;

    public void LoadScene()
    {
        button.gameObject.SetActive(false);

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
            text.text = "Loading... " + (int)(operation.progress * 100f) + "%";
            
            float progress = Mathf.Clamp01(operation.progress / 0.9f);
            slider.value = progress;

            if (operation.progress >= 0.9f)
            {
                text.text = "Finished Loading";
                if (time > 2f)
                {
                    operation.allowSceneActivation = true;
                    loadingScreen.SetActive(false);
                }
                
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
