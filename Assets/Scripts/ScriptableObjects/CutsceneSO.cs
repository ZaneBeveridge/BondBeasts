using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Cutscene", menuName = "SO/Cutscene")]
public class CutsceneSO : ScriptableObject
{
    public string cutsceneName;

    public List<SceneFrame> sceneFrames = new List<SceneFrame>();
}


[System.Serializable]
public class SceneFrame
{
    public string name;
    public string talker;
    public bool isAnim = false;
    public RuntimeAnimatorController backgroundAnimation;
    public Sprite backgroundImage;
    public string onScreenText;

}