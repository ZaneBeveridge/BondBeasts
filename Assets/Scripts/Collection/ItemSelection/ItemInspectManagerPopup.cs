using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemInspectManagerPopup : MonoBehaviour
{
    public GameObject panelPrefab;
    public Transform thisObjectTransform;
    public Transform panelTransform;

    //public GameObject closePrefab;

    public GameObject currentPanel;
    private GameObject currentClose;

    public void SpawnInspectPanel(MonsterItemSO item, GameManager g)// for when opening from stored item
    {
        CloseCurrentPanel();

        //GameObject closeObj = Instantiate(closePrefab, thisObjectTransform);
        //closeObj.GetComponent<ItemInspectCloseButton>().Init(g);
        //currentClose = closeObj;

        GameObject obj = Instantiate(panelPrefab, panelTransform);
        obj.GetComponent<ItemInspectPopup>().Init(item, g);
        currentPanel = obj;
    }

    public void SpawnInspectPanel(MonsterItemSO item, GameManager g, int partySlot, int equipSlot, TeamEquipSlot tSlot) // for when opening from equipped item
    {
        CloseCurrentPanel();

        //GameObject closeObj = Instantiate(closePrefab, thisObjectTransform);
        //closeObj.GetComponent<ItemInspectCloseButton>().Init(g);
        //currentClose = closeObj;

        GameObject obj = Instantiate(panelPrefab, panelTransform);
        obj.GetComponent<ItemInspectPopup>().Init(item, g, partySlot, equipSlot, tSlot);
        currentPanel = obj;
    }

    public void CloseCurrentPanel()
    {
        if (currentPanel != null)
        {
            Destroy(currentPanel);
            currentPanel = null;
        }

        if (currentClose != null)
        {
            Destroy(currentClose);
            currentClose = null;
        }
    }
}
