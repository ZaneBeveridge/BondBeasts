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


    public Button captureBut;
    public Button basicBut;
    public Button specialBut;

    public Button tag1But;
    public Button tag2But;
    public Button tag3But;
    public Image jumpBut;

    public BattleHealthBar enemyHealthBar;

    public GameObject enemyMultiMonsterSwitches;

    // WILL SET THE VISUALS OF CONTROLS AND WHAT THE BUTTONS LOOK LIKE ON SCREEN, ALL FUNCTIONALITY IN BATTLE WILL BE INSIDE THE BATTLE MANAGER



    public void DisableAllButCapture()
    {
        jumpHandler.Off();
        basicBut.interactable = false;
        specialBut.interactable = false;
        tag1But.interactable = false;
        tag2But.interactable = false;
        tag3But.interactable = false;
    }



    public void DisableControls()
    {
        jumpHandler.Off();
        captureBut.interactable = false;
        basicBut.interactable = false;
        specialBut.interactable = false;
        tag1But.interactable = false;
        tag2But.interactable = false;
        tag3But.interactable = false;
    }

    public void EnableControls()
    {
        jumpHandler.On();
        captureBut.interactable = true;
        basicBut.interactable = true;
        specialBut.interactable = true;
        tag1But.interactable = true;
        tag2But.interactable = true;
        tag3But.interactable = true;
    }

}
