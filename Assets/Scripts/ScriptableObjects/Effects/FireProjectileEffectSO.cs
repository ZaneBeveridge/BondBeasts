using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "FireProjectileEffect", menuName = "SO/Effects/FireProjectile")]
public class FireProjectileEffectSO : EffectSO
{
    [Header("Projectile")]
    public GameObject projectilePrefab;
    public int projectileDamage;
    public float projectileSpeed;
    public float lifetime = 0f;
    public int collideWithAmountOfObjects = 0; //0 = no collide, 1 = first collide destroys both objects, 2 = first collide doesn't destroy self but second does
    public bool criticalProjectile;

    public void Awake()
    {
        effectType = EffectType.FireProjectile;
    }
}
