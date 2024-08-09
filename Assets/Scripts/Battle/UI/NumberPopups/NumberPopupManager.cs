using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NumberPopupManager : MonoBehaviour
{
    public GameObject damageTextPrefab;
    public GameObject healTextPrefab;
    public GameObject dotTextPrefab;
    public GameObject blockTextPrefab;
    public GameObject perfectblockTextPrefab;
    public GameObject parryTextPrefab;
    public GameObject airHitPrefab;
    public GameObject negatedTextPrefab;
    public GameObject reflectedTextPrefab;

    public Camera uiCam;
    public Camera worldCam;
    public Canvas thisCanvas;

    public Transform textParent;

    private List<GameObject> nums = new List<GameObject>();

    public void SpawnPopup(PopupType type, Transform tran, string txt, int eTxt)
    {
        var screen = worldCam.WorldToScreenPoint(tran.position);
        screen.z = (thisCanvas.transform.position - uiCam.transform.position).magnitude;
        var position = uiCam.ScreenToWorldPoint(screen);
        Vector3 viewportPos = position; // element is the Text show in the UI.



        //var worldPos = cam.WorldToScreenPoint(tran.position);
        //worldPos.z = (thisCanvas.transform.position - cam.transform.position).magnitude;
        //Vector2 viewportPos = Camera.main.ScreenToWorldPoint(worldPos);

        //Vector3 viewportPos = cam.WorldToScreenPoint(worldPos);

        GameObject textToSpawn = damageTextPrefab;

        switch (type) // pick which prefab to use
        {
            case PopupType.Damage:
                textToSpawn = damageTextPrefab;
                break;
            case PopupType.Heal:
                textToSpawn = healTextPrefab;
                break;
            case PopupType.Dot:
                textToSpawn = dotTextPrefab;
                break;
            case PopupType.Block:
                textToSpawn = blockTextPrefab;
                break;
            case PopupType.PerfectBlock:
                textToSpawn = perfectblockTextPrefab;
                break;
            case PopupType.Parry:
                textToSpawn = parryTextPrefab;
                break;
            case PopupType.AirHit:
                textToSpawn = airHitPrefab;
                break;
        }

        GameObject spawnedText = Instantiate(textToSpawn, viewportPos, Quaternion.identity, textParent);
        spawnedText.GetComponent<NumberPopup>().Init(txt, eTxt);

        nums.Add(spawnedText);
    }

    public void End()
    {
        for (int i = 0; i < nums.Count; i++)
        {
            Destroy(nums[i]);
        }

        nums = new List<GameObject>();
    }
}



public enum PopupType
{
    Damage,
    Heal,
    Dot,
    Block,
    PerfectBlock,
    Parry,
    AirHit,
}
