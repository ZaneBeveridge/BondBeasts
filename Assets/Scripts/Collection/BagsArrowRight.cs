using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BagsArrowRight : MonoBehaviour
{
    [HideInInspector]public GameManager GM;

    public void Press()
    {
        GM.collectionManager.PressRightStorage();
    }
}
