using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemInspectManagerPopup : MonoBehaviour
{
    public GameObject panelPrefab;
    public Transform thisObjectTransform;

    public GameObject closePrefab;

    private GameObject currentPanel;
    private GameObject currentClose;

    public void SpawnInspectPanel(MonsterItemSO item, Transform loc, GameManager g, bool simple)
    {
        CloseCurrentPanel();

        GameObject closeObj = Instantiate(closePrefab, thisObjectTransform);
        closeObj.GetComponent<ItemInspectCloseButton>().Init(g);
        currentClose = closeObj;

        GameObject obj = Instantiate(panelPrefab, loc.position, Quaternion.identity, thisObjectTransform);
        obj.GetComponent<ItemInspectPopup>().Init(item,g, simple);
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
