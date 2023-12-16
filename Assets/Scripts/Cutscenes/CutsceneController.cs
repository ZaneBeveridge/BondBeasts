using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CutsceneController : MonoBehaviour
{
    public GameObject cutsceneGameobject;
    public Image cutsceneBackground;
    public Animator backgroundAnim;

    public TextMeshProUGUI nameOnScreen;
    public TextMeshProUGUI textOnScreen;
    public GameObject textObject;
    public GameObject continueBackgroundButton;

    private int frameIndex = 0;
    private CutsceneSO currentScene;



    private void DoFrame(SceneFrame frame)
    {
        if (frame.isAnim)
        {
            backgroundAnim.runtimeAnimatorController = frame.backgroundAnimation;
        }
        else
        {
            backgroundAnim.runtimeAnimatorController = null;
            cutsceneBackground.sprite = frame.backgroundImage;
        }

        if (frame.onScreenText != "")
        {
            textObject.SetActive(true);
            continueBackgroundButton.SetActive(false);
            textOnScreen.text = frame.onScreenText;
            nameOnScreen.text = frame.talker;
        }
        else
        {
            continueBackgroundButton.SetActive(true);
            textObject.SetActive(false);
        }
    }

    public void NextFrame()
    {
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
        cutsceneGameobject.SetActive(true);
        frameIndex = 0;
        currentScene = cutscene;

        DoFrame(currentScene.sceneFrames[frameIndex]);
    }

    public void FinishCutscene()
    {
        cutsceneGameobject.SetActive(false);
    }
}
