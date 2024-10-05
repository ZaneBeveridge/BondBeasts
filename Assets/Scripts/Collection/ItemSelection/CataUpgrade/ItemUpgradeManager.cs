using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ItemUpgradeManager : MonoBehaviour
{
    public GameObject obj;
    public GameObject loreObj;
    public RectTransform matArea;
    public GameObject closeButtonObject;
    public GameObject itemMatPrefab;



    public Image glitterIcon;
    public TextMeshProUGUI glitterOwned;
    public TextMeshProUGUI glitterNeeded;

    public CollectionManager GM;

    public ItemInspectManagerPopup itemInspectManager;

    private MonsterItemSO item;


    private GameObject usedCataObject;
    private List<GameObject> matObjects = new List<GameObject>();

    private int partySlot = 0;
    private int equipSlot = 0;
    private bool partyItemSelected = false;

    private bool hasItemsToUpgrade = true;
    public void Init(MonsterItemSO itm, GameManager g, bool piSelected, int pSlot, int eSlot)
    {
        obj.SetActive(true);
        loreObj.SetActive(false);
        item = itm;
        GM = g.collectionManager;
        itemInspectManager = g.itemInspectManagerPopup;

        int extraItemAmount = 0;

        if (piSelected)
        {
            partyItemSelected = piSelected;
            partySlot = pSlot;
            equipSlot = eSlot;
            extraItemAmount = 1;
        }

        //Set hasItemsToUpgrade to true if the player has the items in MonsterItemSO recipe 

        //GO through each item, spawn object and to init on upgradeMatSlot, with TotalOwned sent through / if not red, if true 

        int usedAmount = 0;
        bool usedHas = false;

        for (int i = 0; i < GM.itemsOwned.Count; i++)
        {
            if (GM.itemsOwned[i].item.id == item.recipe.usedCatalyst.item.id)
            {
                usedAmount = GM.itemsOwned[i].amount;
                break;
            }
        }

        if (usedAmount + extraItemAmount >= item.recipe.usedCatalyst.amount)
        {
            usedHas = true;
        }
        else
        {
            hasItemsToUpgrade = false;
            usedHas = false;
        }



        GameObject usedCata = Instantiate(itemMatPrefab, matArea);
        usedCata.GetComponent<ItemUpgradeMatSlot>().Init(item.recipe.usedCatalyst, usedAmount + extraItemAmount, usedHas);

        usedCataObject = usedCata;

        for (int i = 0; i < item.recipe.extraMats.Count; i++)
        {
            int matAmount = 0;
            bool matHas = false;

            for (int j = 0; j < GM.itemsOwned.Count; j++)
            {
                if (GM.itemsOwned[j].item.id == item.recipe.extraMats[i].item.id)
                {
                    matAmount = GM.itemsOwned[j].amount;
                    break;
                }
            }

            if (matAmount >= item.recipe.extraMats[i].amount)
            {
                matHas = true;
            }
            else
            {
                hasItemsToUpgrade = false;
                matHas = false;
            }


            GameObject mat = Instantiate(itemMatPrefab, matArea);
            mat.GetComponent<ItemUpgradeMatSlot>().Init(item.recipe.extraMats[i], matAmount, matHas);

            matObjects.Add(mat);
        }


        int glitterAmount = 0;

        for (int i = 0; i < GM.itemsOwned.Count; i++)
        {
            if (GM.itemsOwned[i].item.id == 1)
            {
                glitterAmount = GM.itemsOwned[i].amount;
                break;
            }
        }

        if (glitterAmount >= item.recipe.glitter.amount)
        {
            glitterNeeded.color = Color.green;
            glitterOwned.color = Color.green;
        }
        else
        {
            hasItemsToUpgrade = false;
            glitterNeeded.color = Color.red;
            glitterOwned.color = Color.red;
        }

        glitterNeeded.text = item.recipe.glitter.amount.ToString();
        glitterOwned.text = glitterAmount.ToString();



        if (hasItemsToUpgrade)
        {
            closeButtonObject.SetActive(true);
        }
        else
        {
            closeButtonObject.SetActive(false);
        }
    }

    private void DeleteItems()
    {
        Destroy(usedCataObject.gameObject);
        usedCataObject = null;

        for (int i = 0; i < matObjects.Count; i++)
        {
            Destroy(matObjects[i]);
        }

        matObjects = new List<GameObject>();
    }




    public void Close()
    {
        // Upgrade Item
        
        for (int i = 0; i < item.recipe.extraMats.Count; i++)
        {
            for (int j = 0; j < item.recipe.extraMats[i].amount; j++)
            {
                GM.RemoveItemFromStorage(item.recipe.extraMats[i].item);
            }
        }

        for (int i = 0; i < item.recipe.glitter.amount; i++)
        {
            GM.RemoveItemFromStorage(item.recipe.glitter.item);
        }

        //Remove and add item

        if (partyItemSelected)
        {
            GM.RemoveItemFromMonsterInParty(partySlot, equipSlot + 1);

            for (int i = 0; i < item.recipe.usedCatalyst.amount - 1; i++)
            {
                GM.RemoveItemFromStorage(item.recipe.usedCatalyst.item);
            }

            GM.AddItemToMonsterInParty(item.recipe.createdCatalyst.item, partySlot, equipSlot + 1);
        }
        else
        {
            for (int i = 0; i < item.recipe.usedCatalyst.amount; i++)
            {
                GM.RemoveItemFromStorage(item.recipe.usedCatalyst.item);
            }

            for (int i = 0; i < item.recipe.createdCatalyst.amount; i++)
            {
                GM.AddItemToStorage(item.recipe.createdCatalyst.item, 1);
            }
        }


        


        DeleteItems();
        
        obj.SetActive(false);
        loreObj.SetActive(true);

        itemInspectManager.CloseCurrentPanel();
        itemInspectManager.SpawnInspectPanel(item.recipe.createdCatalyst.item, GM.GM);

        GM.UpdateCollectionAll();
        //GM.UpdatePartyMonsters();
    }
}
