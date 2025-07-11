using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class CutsceneController : MonoBehaviour
{
    public GameObject cutsceneGameobject;
    public Transform imageArea;
    public GameObject imagePrefab;

    public CutsceneTextController textController;

    

    private int frameIndex = 0;
    private CutsceneSO currentScene;

    private List<CutsceneImagePrefab> currentImages = new List<CutsceneImagePrefab>();

    public GameManager GM;


    //EXTRA
    private bool isPreStarter = false;


    private bool autoScene = false;
    private float autoSceneTimer = 0f;

    void Update()
    {
        if (autoScene)
        {
            if (autoSceneTimer > 0)
            {
                autoSceneTimer -= Time.deltaTime;
            }
            else if (autoSceneTimer <= 0)
            {
                NextFrame();
            }
        }
    }

    private void DoFrame(SceneFrame frame)
    {
        // remove images
        if (frame.imagesToRemove.Count > 0)
        {
            for (int i = 0; i < frame.imagesToRemove.Count; i++)
            {
                for (int j = 0; j < currentImages.Count; j++)
                {
                    if (currentImages[j].index == frame.imagesToRemove[i].imageID)
                    {
                        if (frame.imagesToRemove[i].fade)
                        {
                            currentImages[j].FadeOut();
                        }
                        else if (frame.imagesToRemove[i].shake)
                        {
                            currentImages[j].Shake(true);
                        }
                        else
                        {
                            currentImages[j].Hide();
                        }

                        currentImages.RemoveAt(j);
                    }
                }
            }
        }

        // add images

        if (frame.imagesToDisplay.Count > 0)
        {
            for (int i = 0; i < frame.imagesToDisplay.Count; i++)
            {
                GameObject img = Instantiate(imagePrefab, imageArea) as GameObject;
                CutsceneImagePrefab manager = img.GetComponent<CutsceneImagePrefab>();
                manager.index = frame.imagesToDisplay[i].imageID;
                manager.image.sprite = frame.imagesToDisplay[i].image;

                if (frame.imagesToDisplay[i].fade)
                {
                    manager.FadeIn();
                }
                else if (frame.imagesToDisplay[i].shake)
                {
                    manager.Shake(false);
                }
                else
                {
                    manager.Show();
                }

                currentImages.Add(manager);
            }
        }

        // remove text

        if (frame.removeText.removeText)
        { 
            if (frame.removeText.fade)
            {
                textController.FadeOut();
            }
            if (frame.removeText.shake)
            {
                textController.Shake();
            }
            else
            {
                textController.Hide();
            } 
        }
        else // add text
        {
            textController.Init(frame.displayText.dialogueText, frame.displayText.speakerText, frame.displayText.dialogueTextColour, frame.displayText.speakerTextColour, frame.displayText.backgroundColour);

            if (frame.displayText.fade)
            {
                textController.FadeIn();
            }
            if (frame.displayText.shake)
            {
                textController.Shake();
            }
            else
            {
                textController.Show();
            }
        }

        if (frame.automaticallyGoToNextScene)
        {
            autoSceneTimer = frame.timeUntilNextScene;
            autoScene = true;
        }


    }

    public void NextFrame()
    {
        autoScene = false;
        autoSceneTimer = 0f;

        if (frameIndex < currentScene.sceneFrames.Count - 1)
        {
            frameIndex++;
            DoFrame(currentScene.sceneFrames[frameIndex]);
        }
        else
        {
            FinishCutscene();
        }
        
    }

    public void PlayCutscene(CutsceneSO cutscene)
    {
        isPreStarter = false;
        GM.overworldUI.HideUI(true);
        cutsceneGameobject.SetActive(true);
        frameIndex = 0;
        currentScene = cutscene;

        DoFrame(currentScene.sceneFrames[frameIndex]);
    }

    public void FinishCutscene()
    {
        GM.overworldUI.HideUI(false);
        cutsceneGameobject.SetActive(false);

        if (isPreStarter)
        {
            GM.beastStarterMenuGUI.Init();
        }
    }


    // EXTRA STUFF


    public void PlayCutscenePreStarter(CutsceneSO cutscene)
    {
        isPreStarter = true;
        GM.overworldUI.HideUI(true);
        cutsceneGameobject.SetActive(true);
        frameIndex = 0;
        currentScene = cutscene;

        DoFrame(currentScene.sceneFrames[frameIndex]);
    }
}
