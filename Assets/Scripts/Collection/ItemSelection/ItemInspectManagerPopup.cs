using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemInspectManagerPopup : MonoBehaviour
{
    public GameObject panelPrefab;
    public Transform thisObjectTransform;

    private GameObject currentPanel;
    private bool timerOn = false;
    private float timer = 0f;
    public void Update()
    {
        if (timerOn)
        {
            if (timer > 0)
            {
                timer -= Time.deltaTime;
            }
            else if (timer <= 0)
            {
                CloseCurrentPanel();
                timerOn = false;
                timer = 0f;
            }
        }
    }

    public void SpawnInspectPanel(MonsterItemSO item, Transform loc, GameManager g)
    {
        CloseCurrentPanel();

        GameObject obj = Instantiate(panelPrefab, loc.position, Quaternion.identity, thisObjectTransform);
        obj.GetComponent<ItemInspectPopup>().Init(item,g);
        currentPanel = obj;

        //timerOn = true;
        //timer = 5f;
    }

    public void CloseCurrentPanel()
    {
        if (currentPanel != null)
        {
            Destroy(currentPanel);
            currentPanel = null;
        }
    }
}
