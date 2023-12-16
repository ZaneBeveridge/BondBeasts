using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MonsterUIFirstPickOption : MonoBehaviour
{
    public string monsterName;

    [Header("References")]
    public Sprite selectedArt;
    public Sprite unselectedArt;
    public Image background;

    public TextMeshProUGUI monsterText;
    public Image dynamicImage;
    public Image staticImage;
    public Image variantImage;

    public List<MonsterSO> monsterDatas = new List<MonsterSO>();

    public Monster monster;

    public void Reroll()
    {
        int rand = Random.Range(0, monsterDatas.Count - 1);

        monster = new Monster(monsterName, 1, monsterDatas[rand]);

        monsterText.text = monsterName;
        dynamicImage.sprite = monster.dynamicSprite;
        staticImage.sprite = monster.staticSprite;
        variantImage.sprite = monster.variant.variantStillSprite;

        dynamicImage.color = monster.colour.colour;
    }

    public void Deselect()
    {
        background.sprite = unselectedArt;
    }

    public void Select()
    {
        background.sprite = selectedArt;
    }
}
