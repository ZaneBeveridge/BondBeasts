using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BattleUI : MonoBehaviour
{
    [Header("Game Manager")]
    public GameManager GM;
    public FriendlyMonsterController controller;
    [Header("Buttons")]
    public GameObject jumpButton;
   
    public GameObject captureButton;
    public ButtonTagSlot tagSprites1;
    public ButtonTagSlot tagSprites2;
    public ButtonTagSlot tagSprites3;

    public IconRotatorItem enemyTagSprites1;
    public IconRotatorItem enemyTagSprites2;
    public IconRotatorItem enemyTagSprites3;

    public JumpHandler jumpHandler;
    public CaptureButton captureBute;

    public Image captureBut;
    public Image basicBut;
    public Image specialBut;

    public Image tag1But;
    public Image tag2But;
    public Image tag3But;
    public Image jumpBut;

    public BattleHealthBar enemyHealthBar;

    public GameObject enemyMultiMonsterSwitches;

    // WILL SET THE VISUALS OF CONTROLS AND WHAT THE BUTTONS LOOK LIKE ON SCREEN, ALL FUNCTIONALITY IN BATTLE WILL BE INSIDE THE BATTLE MANAGER


    public void HideAllButtonsButCapture(bool state)
    {
        jumpButton.SetActive(!state);
        basicBut.gameObject.SetActive(!state);
        specialBut.gameObject.SetActive(!state);
    }


    public void DisableAllButCapture()
    {
        jumpHandler.Off();
        basicBut.color = new Color(1f,1f,1f,0.1f);
        specialBut.color = new Color(1f, 1f, 1f, 0.1f);
        tag1But.color = new Color(1f, 0.85f, 0.14f, 0.1f);
        tag2But.color = new Color(1f, 0.85f, 0.14f, 0.1f);
        tag3But.color = new Color(1f, 0.85f, 0.14f, 0.1f);
    }



    public void DisableControls()
    {
        jumpHandler.Off();
        captureBut.color = new Color(1f, 1f, 1f, 0.1f);
        basicBut.color = new Color(1f, 1f, 1f, 0.1f);
        specialBut.color = new Color(1f, 1f, 1f, 0.1f);
        tag1But.color = new Color(1f, 0.85f, 0.14f, 0.1f);
        tag2But.color = new Color(1f, 0.85f, 0.14f, 0.1f);
        tag3But.color = new Color(1f, 0.85f, 0.14f, 0.1f);
    }

    public void EnableControls()
    {
        jumpHandler.On();
        captureBut.color = new Color(1f, 1f, 1f, 1f);
        basicBut.color = new Color(1f, 1f, 1f, 1f);
        specialBut.color = new Color(1f, 1f, 1f, 1f);
        tag1But.color = new Color(1f, 0.85f, 0.14f, 1f);
        tag2But.color = new Color(1f, 0.85f, 0.14f, 1f);
        tag3But.color = new Color(1f, 0.85f, 0.14f, 1f);
    }

}
