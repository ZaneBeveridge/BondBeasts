using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Variant", menuName = "SO/Variant", order = 3)]
public class VariantSO : ScriptableObject
{
    [Header("Info")]
    public string variantName;
    public int parentId;
    public int id;

    [Header("Visuals")]
    public Sprite variantStillSprite;
    public AnimatorOverrideController variantAnimator;
}
