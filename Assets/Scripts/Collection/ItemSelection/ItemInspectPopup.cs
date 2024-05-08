using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ItemInspectPopup : MonoBehaviour
{
    public Image icon;
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI descText;

    public GameObject upgradeObject;

    public ItemUpgradeManager upgradeManager;

    private MonsterItemSO item;

    private GameManager GM;
    public void Init(MonsterItemSO i, GameManager g, bool simple)
    {
        GM = g;
        item = i;
        icon.sprite = i.icon;
        nameText.text = i.itemName;
        descText.text = i.desc;

        if (i.canBeUpgraded && !simple)
        {
            upgradeObject.SetActive(true);
        }
        else
        {
            upgradeObject.SetActive(false);
        }
    }


    public void Upgrade()
    {
        upgradeManager.Init(item, GM);
    }
}
