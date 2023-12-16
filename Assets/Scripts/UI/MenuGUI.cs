using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuGUI : MonoBehaviour
{
    public GameManager GM;

    public CreateProfile createProfile;
    public void OpenNewProfileMenu()
    {
        //Set gameobject to true
        createProfile.Open();
    }

    
}
