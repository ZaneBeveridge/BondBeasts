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
    [Header("Name")]
    public string sceneName;
    [Header("Image Display")]
    public List<DisplayImage> imagesToDisplay = new List<DisplayImage>();
    [Header("Image Removal")]
    public List<RemoveImage> imagesToRemove = new List<RemoveImage>();
    [Header("Text Display")]
    public DisplayText displayText;
    [Header("Text Removal")]
    public RemoveText removeText;
}

[System.Serializable]
public class DisplayImage
{
    public Sprite image;
    public int imageID;
    public bool fade = false;
    public bool shake = false;
}

[System.Serializable]
public class RemoveImage
{
    public int imageID;
    public bool fade = false;
    public bool shake = false;
}

[System.Serializable]
public class DisplayText
{
    public string dialogueText;
    public Color dialogueTextColour;
    public string speakerText;
    public Color speakerTextColour;
    public Color backgroundColour;
    public bool fade = false;
    public bool shake = false;
}

[System.Serializable]
public class RemoveText
{
    public bool removeText = false;
    public bool fade = false;
    public bool shake = false;
}
