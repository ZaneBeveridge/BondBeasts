using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerInfoInterface : MonoBehaviour
{
    public TextMeshProUGUI thirdText;


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

        int monsSeen = 0;

        for (int i = 0; i < manager.monsterSOData.Count; i++)
        {
            for (int j = 0; j < manager.playerData.numOfBeastsSeenID.Count; j++)
            {
                if (manager.playerData.numOfBeastsSeenID[j] == manager.monsterSOData[i].ID.ID)
                {
                    monsSeen++;
                    break;
                }
            }
        }

        thirdText.text = "I've seen " + monsSeen.ToString() + " different species of Bond Beasts,\n and bonded with " + monCompleted.ToString() + " of them.";
    }
}
