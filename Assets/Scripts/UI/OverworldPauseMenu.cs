using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OverworldPauseMenu : MonoBehaviour
{

    
    public void PressDonateButton()
    {
        Application.OpenURL("https://www.ko-fi.com/guildstudios");
    }

    public void OpenInstagramLink()
    {
        Application.OpenURL("https://www.instagram.com/guild_games?igsh=dHl1NHU0MjVheDY5");
    }

    public void OpenFacebookLink()
    {
        Application.OpenURL("https://www.facebook.com/profile.php?id=61557533060350");
    }

}
