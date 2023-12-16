using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerInfoInterface : MonoBehaviour
{
    public TextMeshProUGUI rankText;
    public TextMeshProUGUI rankText2;

    public TextMeshProUGUI nodesTextNum;
    public TextMeshProUGUI monsTextNum;

    public GameManager manager;
    public void UpdateInfo()
    {
        int numCompleted = 0;
        for (int i = 0; i < manager.nodesCompleted.Count; i++)
        {
            if (manager.nodesCompleted[i])
            {
                numCompleted++;
            }
        }


        

        nodesTextNum.text = numCompleted.ToString() + "/" + manager.nodesCompleted.Count;


        int monCompleted = 0;
        for (int i = 0; i < manager.monsterSOData.Count; i++)
        {
            for (int j = 0; j < manager.playerData.mData.Count; j++)
            {
                if (manager.playerData.mData[j] == manager.monsterSOData[i].ID.ID)
                {
                    monCompleted++;
                    break;
                }
            }
        }




        monsTextNum.text = monCompleted.ToString();

        float num = (numCompleted * 5) + (monCompleted * 5);

        rankText.text = "Rank " + num;
        rankText2.text = "Rank " + num;
    }
}
