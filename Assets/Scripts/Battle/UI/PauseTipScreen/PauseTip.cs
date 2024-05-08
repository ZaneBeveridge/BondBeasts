using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseTip : MonoBehaviour
{
    [Header("Parent References")]
    public PauseScreen pauseManager;
    public BattleManager bManager;
    public GameObject mainObject;
    public Image tipImage;
    public Sprite tipSelectedSprite;
    public Sprite tipUnselectedSprite;

    public virtual void OpenTip()
    {
        pauseManager.CloseAllTips();
        mainObject.SetActive(true);
        tipImage.sprite = tipSelectedSprite;
    }
    public virtual void CloseTip()
    {
        mainObject.SetActive(false);
        tipImage.sprite = tipUnselectedSprite;
    }
}
