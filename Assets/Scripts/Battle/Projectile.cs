using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    private int damage;
    private int baseDamage;

    public bool isBasic;

    public GameObject onFireParticle;
    public GameObject onHitParticle;

    public Rigidbody2D rb;
    public SpriteRenderer sprite;

    public string opposition;


    public List<int> enemyStatsBuff = new List<int>();
    public List<int> friendlyStatsBuff = new List<int>();


    public float antiGravTime = 0f;
    public float critAttackTime = 0f;
    public float takingCritsTime = 0f;
    public float stunnedTime = 0f;

    public int dotAmount = 0;
    public float dotTime = 0f;

    public bool resetSpecialCooldownOnHit = false;
    public bool triggersBasicORSpecial = false;



    private bool life = false;
    private float lifeTimeTimer = 0f;

    private int amountOfObjectsToCollideWith = 0; // 0 means it will ignore other projectiles, 1 will break 1 and explode, 2 will break 1 and then the second its breaks dies with it
    private bool criticalProj = false;

    private FireProjectileEffectSO projectileEffect;

    private BattleBuffManager manager;

    void Update()
    {
        if (life)
        {
            if (lifeTimeTimer > 0)
            {
                lifeTimeTimer -= Time.deltaTime;
            }
            else if (lifeTimeTimer <= 0)
            {
                lifeTimeTimer = 0f;
                life = false;

                Destroy(gameObject);
            }
        }
    }

    public void Init(float spd, int dmg, int bDmg, float lifeTime, int collideWithAmountOfObjects, bool criticalProjectile, string op, BattleBuffManager man, FireProjectileEffectSO projEffect)
    {
        if (onFireParticle != null)
        {
            Instantiate(onFireParticle, transform.position, transform.rotation);
        }
        

        manager = man;
        projectileEffect = projEffect;
        if (op == "Enemy") // opposition is
        {
            sprite.flipX = false;
            rb.velocity = transform.right * spd;
        }
        else if (op == "Friend") // opposition is
        {
            sprite.flipX = true;
            rb.velocity = -transform.right * spd;
        }

        
        damage = dmg;
        baseDamage = bDmg;

        amountOfObjectsToCollideWith = collideWithAmountOfObjects;
        criticalProj = criticalProjectile;



        opposition = op;

        if (lifeTime > 0)
        {
            lifeTimeTimer = lifeTime;
            life = true;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (opposition == "Enemy")
        {
            EnemyMonsterController enemy = collision.GetComponent<EnemyMonsterController>();
            if (enemy != null)
            {

                if (criticalProj) // has crit
                {
                    enemy.fMonsterController.TriggerAction(TriggerType.crit);

                    if (enemy.transform.position.y > -1.1)
                    {
                        enemy.TakeDamage(damage + (int)(damage * 0.5f), baseDamage, false, true, dotAmount, dotTime, stunnedTime, resetSpecialCooldownOnHit, antiGravTime, projectileEffect.echo, enemyStatsBuff, friendlyStatsBuff, this.transform, projectileEffect);
                        enemy.hitNumbers.SpawnPopup(PopupType.AirHit, this.transform, "", 0);
                        enemy.fMonsterController.TriggerAction(TriggerType.enemyHitInAir);
                    }
                    else
                    {
                        enemy.TakeDamage(damage, baseDamage, false, true, dotAmount, dotTime, stunnedTime, resetSpecialCooldownOnHit, antiGravTime, projectileEffect.echo, enemyStatsBuff, friendlyStatsBuff, this.transform, projectileEffect);
                    }
                }
                else
                {
                    if (enemy.transform.position.y > -1.1)
                    {
                        enemy.TakeDamage(damage + (int)(damage * 0.5f), baseDamage, false, false, dotAmount, dotTime, stunnedTime, resetSpecialCooldownOnHit, antiGravTime, projectileEffect.echo, enemyStatsBuff, friendlyStatsBuff, this.transform, projectileEffect);
                        enemy.hitNumbers.SpawnPopup(PopupType.AirHit, this.transform, "", 0);
                        enemy.fMonsterController.TriggerAction(TriggerType.enemyHitInAir);
                    }
                    else
                    {
                        enemy.TakeDamage(damage, baseDamage, false, false, dotAmount, dotTime, stunnedTime, resetSpecialCooldownOnHit, antiGravTime, projectileEffect.echo, enemyStatsBuff, friendlyStatsBuff, this.transform, projectileEffect);
                    }
                }


                if (triggersBasicORSpecial)
                {
                    if (isBasic)
                    {
                        enemy.fMonsterController.TriggerAction(TriggerType.enemyHitBasic);
                    }
                    else
                    {
                        enemy.fMonsterController.TriggerAction(TriggerType.enemyHitSpecial);
                    }
                }

                if (onHitParticle != null)
                {
                    Instantiate(onHitParticle, transform.position, transform.rotation);
                }

                Destroy(gameObject);
            }
        }

        if (opposition == "Friend")
        {
            FriendlyMonsterController friend = collision.GetComponent<FriendlyMonsterController>();
            if (friend != null)
            {
                

                

                

                if (criticalProj)
                {
                    if (friend.transform.position.y > -1.1)
                    {
                        friend.TakeDamage(damage + (int)(damage * 0.5f), baseDamage, false, true, dotAmount, dotTime, stunnedTime, resetSpecialCooldownOnHit, antiGravTime, projectileEffect.echo, enemyStatsBuff, friendlyStatsBuff, this.transform, projectileEffect);
                        friend.hitNumbers.SpawnPopup(PopupType.AirHit, this.transform, "", 0);
                        //friend.TriggerAction(TriggerType.enemyHitInAir);
                    }
                    else
                    {
                        friend.TakeDamage(damage, baseDamage, false, true, dotAmount, dotTime, stunnedTime, resetSpecialCooldownOnHit, antiGravTime, projectileEffect.echo, enemyStatsBuff, friendlyStatsBuff, this.transform, projectileEffect);
                    }
                }
                else
                {
                    if (friend.transform.position.y > -1.1)
                    {
                        friend.TakeDamage(damage + (int)(damage * 0.5f), baseDamage, false, false, dotAmount, dotTime, stunnedTime, resetSpecialCooldownOnHit, antiGravTime, projectileEffect.echo, enemyStatsBuff, friendlyStatsBuff, this.transform, projectileEffect);
                        friend.hitNumbers.SpawnPopup(PopupType.AirHit, this.transform, "", 0);
                        //friend.fMonsterController.TriggerAction(TriggerType.enemyHitInAir);
                    }
                    else
                    {
                        friend.TakeDamage(damage, baseDamage, false, false, dotAmount, dotTime, stunnedTime, resetSpecialCooldownOnHit, antiGravTime, projectileEffect.echo, enemyStatsBuff, friendlyStatsBuff, this.transform, projectileEffect);
                    }
                }

                if (onHitParticle != null)
                {
                    Instantiate(onHitParticle, transform.position, transform.rotation);
                }

                Destroy(gameObject);
            }
        }

        
        
        if (collision.tag == "Border")
        {
            Destroy(gameObject);
        }
    }
}

