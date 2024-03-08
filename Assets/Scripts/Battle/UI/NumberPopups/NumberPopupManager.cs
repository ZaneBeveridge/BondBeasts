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


    public Transform textParent;

    private List<GameObject> nums = new List<GameObject>();

    public void SpawnPopup(PopupType type, Transform transform, string txt, int eTxt)
    {
        Vector2 worldPos = transform.position;


        Vector2 viewportPos = Camera.main.WorldToScreenPoint(worldPos);

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
            case PopupType.Negated:
                textToSpawn = negatedTextPrefab;
                break;
            case PopupType.Reflected:
                textToSpawn = reflectedTextPrefab;
                break;
        }

        GameObject spawnedText = Instantiate(textToSpawn, new Vector3(viewportPos.x, viewportPos.y, 0f), Quaternion.identity, textParent);
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
    Negated,
    Reflected
}
