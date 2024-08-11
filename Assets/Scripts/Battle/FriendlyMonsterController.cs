using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class FriendlyMonsterController : MonoBehaviour
{
    [Header("References")]
    public GameManager GM;
    public BattleHealthBar healthBar;
    public GameObject parentMain;
    public Animator friendlyAnim;
    public Animator friendlyAnimVariant;
    public Animator friendlyParentAnim;
    public Animator friendlyUIAnimBasic;
    public Animator friendlyUIAnimSpecial;
    public Animator healthBarShakeAnim;
    public TextMeshProUGUI regenText;
    public SpriteRenderer friendlyDynamicSprite;
    public TextMeshProUGUI friendlyNameText;
    public TextMeshProUGUI friendlyLevelText;
    public FriendlyIconRotator friendlyIconRotator;
    public NumberPopupManager hitNumbers;
    public Transform defaultHitNumbersLocation;
    public BattleBuffManager friendlyBattleBuffManager;

    public StunManager stunManager;
    public UIStunManager uiStunManager;

    public MoveController friendlyMoveController;
    public ItemController friendlyItemController;
    public PassiveController friendlyPassiveController;


    [Header("Effects")]
    public SpriteRenderer guardRenderer;
    public Sprite blueGuard;
    public Sprite yellowGuard;
    public GameObject antiGravEffect;
    [Header("Controller")]
    private Rigidbody2D rb;
    public Transform firePoint;

    public bool isGrounded;
    public Transform feetPos;
    public float checkRadius;
    public LayerMask whatIsGround;
    public float jumpForce;

    private bool doLand;

    [Header("Monster")]
    public Monster friendlyMonster;
    private List<Monster> backupMonsters;
    public int currentSlot;


    public int oomph, guts, juice, edge, wits, spark;

    [HideInInspector] public int itemOomph, itemGuts, itemJuice, itemEdge, itemWits, itemSpark;

    public List<bool> basicReady = new List<bool>();
    public List<bool> specialReady = new List<bool>();
    public List<bool> tagReady = new List<bool>();

    public List<float> basicC = new List<float>();
    public List<float> specialC = new List<float>();
    public List<float> tagC = new List<float>();


    public bool regenOn = false;
    private float regenTimer = 0f;
    private float regenBaseTime = 1f;
    private float regenMulti = 0.25f;

    public List<float> inBattleTime = new List<float>();

    [HideInInspector] public List<GameObject> projectiles = new List<GameObject>();

    [HideInInspector] public bool alive = false;
    private bool tagOn = false;
    private float tagTimer = 0f;
    private int taggingSlot = 0;

    public bool guardOn = false;
    private float parryTimer = 0f;
    private bool parryOn = false;
    private PerfectGuardEffects perfectGuard;
    private float perfectValue;
    private FireProjectileEffectSO perfectProjectile;
    private Targets perfectTargets;
    //private bool critAttacks = false;
    //private bool takingCrits = false;
    private int critchance = 0;

    public MaskCutout maskCutout;


    private bool invulnerable = false;
    private float invTime = 0f;

    private bool stunned = false;

    private float accumulatedJuiceHealing = 0f;

    private void Start()
    {



        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if (invulnerable)
        {
            if (invTime > 0)
            {
                invTime -= Time.deltaTime;
            }
            else if (invTime <= 0)
            {
                invTime = 0f;
                invulnerable = false;
            }
        }

        if (alive)
        {
            if (currentSlot == 0)
            {
                inBattleTime[0] += Time.deltaTime;
            }
            else if (currentSlot == 1)
            {
                inBattleTime[1] += Time.deltaTime;
            }
            else if (currentSlot == 2)
            {
                inBattleTime[2] += Time.deltaTime;
            }


            if (regenOn)
            {
                if (regenTimer > 0)
                {
                    regenTimer -= Time.deltaTime;
                }
                else if (regenTimer <= 0)
                {
                    float juiceAmount = juice + friendlyBattleBuffManager.slotValues[2] + friendlyBattleBuffManager.GetStatsFromItemsPassives(EffectedStat.Juice);

                    float regenAmount = (regenMulti * (juiceAmount * (0.001f * 1000f)));
                    regenMulti += 0.25f;
                    regenText.text = "HPs x " + regenMulti.ToString("F2");
                    //Debug.Log("Regen Amount: " + regenAmount.ToString());

                    int flooredRegen = Mathf.FloorToInt(regenAmount + accumulatedJuiceHealing);
                    //Debug.Log("Floored: " + flooredRegen.ToString());

                    float leftOvers = (regenAmount + accumulatedJuiceHealing) - flooredRegen;
                    //Debug.Log("Left Overs: " + leftOvers.ToString());

                    accumulatedJuiceHealing = leftOvers;

                    if (flooredRegen >= 1 || flooredRegen <= -1)
                    {
                        healthBar.SetHealth(GM.playerHP + flooredRegen, false);
                        GM.playerHP = healthBar.slider.value;

                    }



                    regenTimer = regenBaseTime;
                }
            }


        }



        isGrounded = Physics2D.OverlapCircle(feetPos.position, checkRadius, whatIsGround);

        if (isGrounded)
        {
            if (doLand)
            {
                StopJump();
                friendlyParentAnim.SetTrigger("Land");
                doLand = false;
            }
        }
        else
        {
            if (!doLand)
            {
                friendlyParentAnim.SetTrigger("Jump");
            }
            doLand = true;
        }

        DoCooldowns();


        

        if (tagOn)
        {
            if (tagTimer > 0)
            {
                //Debug.Log("Hello");
                tagTimer -= Time.deltaTime;
            }
            else if (tagTimer <= 0)
            {
                GM.battleManager.ResumeControls();
                SpawnNewMonster(taggingSlot, false);
                tagTimer = 0f;
                tagOn = false;
                //transform.localPosition = new Vector3(-5f, -1f, 0f);
            }
        }

        if (parryOn)
        {
            if (parryTimer > 0)
            {
                parryTimer -= Time.deltaTime;
            }
            else if (parryTimer <= 0)
            {
                guardRenderer.sprite = blueGuard;
                parryOn = false;
                parryTimer = 0f;
            }
        }

        if (stunned)
        {
            healthBar.ChangeColor("Stun");
        }
        else
        {
            if (guardOn)
            {
                healthBar.ChangeColor("Guard");
            }
            else if (parryOn)
            {
                healthBar.ChangeColor("PerfectGuard");
            }
            else
            {
                if (friendlyBattleBuffManager.slotValues[6] > 0)
                {
                    healthBar.ChangeColor("Dot");
                }
                else
                {
                    healthBar.ChangeColor("Normal");
                }
            }
        }
    }

    private void DoCooldowns()
    {
        for (int i = 0; i < basicReady.Count; i++)
        {
            if (!basicReady[i])
            {
                if (basicC[i] > 0) { basicC[i] -= Time.deltaTime; }
                else if (basicC[i] <= 0) { basicReady[i] = true; basicC[i] = 0f; }
            }
        }

        for (int i = 0; i < specialReady.Count; i++)
        {
            if (!specialReady[i])
            {
                if (specialC[i] > 0) { specialC[i] -= Time.deltaTime; }
                else if (specialC[i] <= 0) { specialReady[i] = true; specialC[i] = 0f; }
            }
        }

        for (int i = 0; i < tagReady.Count; i++)
        {
            if (!tagReady[i])
            {
                if (tagC[i] > 0) { tagC[i] -= Time.deltaTime; }
                else if (tagC[i] <= 0) { tagReady[i] = true; tagC[i] = 0f; }
            }
        }
    }
    public void RefreshCooldowns()
    {
        for (int i = 0; i < basicReady.Count; i++)
        {
            basicReady[i] = true;
            specialReady[i] = true;
            tagReady[i] = true;

            basicC[i] = 0f;
            specialC[i] = 0f;
            tagC[i] = 0f;
        }
    }

    private void SpawnNewMonster(int slot, bool isStart)
    {
        float cooldown = 7f;

        if (!isStart)
        {
            float sparkAmount = spark + friendlyBattleBuffManager.slotValues[5] + friendlyBattleBuffManager.GetStatsFromItemsPassives(EffectedStat.Spark);
            if (sparkAmount < 0)
            {
                tagC[currentSlot] = cooldown - (cooldown * (0.008f * sparkAmount));
            }
            else
            {
                tagC[currentSlot] = 1f / ((1f / cooldown) * sparkAmount * 0.04f + (1f / cooldown));
            }


            tagReady[currentSlot] = false;
        }

        if (isStart)
        {
            alive = true;
        }
        


        currentSlot = slot;
        List<Monster> pMons = new List<Monster>();

        for (int i = 0; i < GM.collectionManager.partySlots.Count; i++)
        {
            if (i != slot)
            {
                if (GM.collectionManager.partySlots[i].storedMonsterObject != null)
                {
                    //Debug.Log("backupMon" + i + " " + GM.collectionManager.partySlots[i].storedMonsterObject.GetComponent<Slot>().storedMonster.name);
                    pMons.Add(GM.collectionManager.partySlots[i].storedMonsterObject.GetComponent<Slot>().storedMonster);
                }

            }
        }

        GM.battleManager.slotSelected = slot + 1;

        friendlyMonster = GM.collectionManager.partySlots[slot].storedMonsterObject.GetComponent<Slot>().storedMonster;


        //SETS STATS VALUES OF THE CONTROLLER TO WHATEVERT IS SWITCHED TO

        if (friendlyMonster.stats[0].value < 0)
        {
            if (friendlyMonster.nature.addedStats[0].value < 1) oomph = Mathf.RoundToInt(-(-friendlyMonster.stats[0].value * (friendlyMonster.nature.addedStats[0].value + 1)));
            else oomph = Mathf.RoundToInt(-(-friendlyMonster.stats[0].value * (friendlyMonster.nature.addedStats[0].value - 1)));
        }
        else
        {
            oomph = Mathf.RoundToInt(friendlyMonster.stats[0].value * friendlyMonster.nature.addedStats[0].value);
        }

        if (friendlyMonster.stats[1].value < 0)
        {
            if (friendlyMonster.nature.addedStats[1].value < 1) guts = Mathf.RoundToInt(-(-friendlyMonster.stats[1].value * (friendlyMonster.nature.addedStats[1].value + 1)));
            else guts = Mathf.RoundToInt(-(-friendlyMonster.stats[1].value * (friendlyMonster.nature.addedStats[1].value - 1)));
        }
        else
        {
            guts = Mathf.RoundToInt(friendlyMonster.stats[1].value * friendlyMonster.nature.addedStats[1].value);
        }

        if (friendlyMonster.stats[2].value < 0)
        {
            if (friendlyMonster.nature.addedStats[2].value < 1) juice = Mathf.RoundToInt(-(-friendlyMonster.stats[2].value * (friendlyMonster.nature.addedStats[2].value + 1)));
            else juice = Mathf.RoundToInt(-(-friendlyMonster.stats[2].value * (friendlyMonster.nature.addedStats[2].value - 1)));
        }
        else
        {
            juice = Mathf.RoundToInt(friendlyMonster.stats[2].value * friendlyMonster.nature.addedStats[2].value);
        }

        if (friendlyMonster.stats[3].value < 0)
        {
            if (friendlyMonster.nature.addedStats[3].value < 1) edge = Mathf.RoundToInt(-(-friendlyMonster.stats[3].value * (friendlyMonster.nature.addedStats[3].value + 1)));
            else edge = Mathf.RoundToInt(-(-friendlyMonster.stats[3].value * (friendlyMonster.nature.addedStats[3].value - 1)));
        }
        else
        {
            edge = Mathf.RoundToInt(friendlyMonster.stats[3].value * friendlyMonster.nature.addedStats[3].value);
        }

        if (friendlyMonster.stats[4].value < 0)
        {
            if (friendlyMonster.nature.addedStats[4].value < 1) wits = Mathf.RoundToInt(-(-friendlyMonster.stats[4].value * (friendlyMonster.nature.addedStats[4].value + 1)));
            else wits = Mathf.RoundToInt(-(-friendlyMonster.stats[4].value * (friendlyMonster.nature.addedStats[4].value - 1)));
        }
        else
        {
            wits = Mathf.RoundToInt(friendlyMonster.stats[4].value * friendlyMonster.nature.addedStats[4].value);
        }

        if (friendlyMonster.stats[5].value < 0)
        {
            if (friendlyMonster.nature.addedStats[5].value < 1) spark = Mathf.RoundToInt(-(-friendlyMonster.stats[5].value * (friendlyMonster.nature.addedStats[5].value + 1)));
            else spark = Mathf.RoundToInt(-(-friendlyMonster.stats[5].value * (friendlyMonster.nature.addedStats[5].value - 1)));
        }
        else
        {
            spark = Mathf.RoundToInt(friendlyMonster.stats[5].value * friendlyMonster.nature.addedStats[5].value);
        }


        //oomph = Mathf.RoundToInt(friendlyMonster.stats[0].value * friendlyMonster.nature.addedStats[0].value);
        //guts = Mathf.RoundToInt(friendlyMonster.stats[1].value * friendlyMonster.nature.addedStats[1].value);
        //juice = Mathf.RoundToInt(friendlyMonster.stats[2].value * friendlyMonster.nature.addedStats[2].value);
        //edge = Mathf.RoundToInt(friendlyMonster.stats[3].value * friendlyMonster.nature.addedStats[3].value);
        //wits = Mathf.RoundToInt(friendlyMonster.stats[4].value * friendlyMonster.nature.addedStats[4].value);
        //spark = Mathf.RoundToInt(friendlyMonster.stats[5].value * friendlyMonster.nature.addedStats[5].value);

    

        //tagC[currentSlot] = tagCooldown;
        //Debug.Log(friendlyMonster.name + pMons[0].name + pMons[1].name);
        backupMonsters = pMons;
        ///friendlyIconRotator.SetMonsterRotator(friendlyMonster, pMons); // HERE


        friendlyAnim.runtimeAnimatorController = friendlyMonster.animator;
        friendlyAnimVariant.runtimeAnimatorController = friendlyMonster.variant.variantAnimator;
        friendlyDynamicSprite.color = friendlyMonster.colour.colour;
        friendlyNameText.text = friendlyMonster.name;
        friendlyLevelText.text = "Level " + friendlyMonster.level.ToString();


        if (friendlyMonster.backupData.isHeavyJumper)
        {
            jumpForce = 10f;
        }
        else
        {
            jumpForce = 15f;
        }

        
        //OnTagInBuffs


        StartBuffs(isStart);

    }


    private void StartBuffs(bool st)
    {
        List<PassiveSO> passivesShared = new List<PassiveSO>();
        for (int i = 0; i < GM.collectionManager.partySlots.Count; i++)
        {
            if (GM.collectionManager.partySlots[i].storedMonsterObject != null)
            {
                if (GM.collectionManager.partySlots[i].storedMonsterObject.GetComponent<Slot>().storedMonster.passiveMove.shared)
                {
                    if(!passivesShared.Contains(GM.collectionManager.partySlots[i].storedMonsterObject.GetComponent<Slot>().storedMonster.passiveMove))
                    {
                        passivesShared.Add(GM.collectionManager.partySlots[i].storedMonsterObject.GetComponent<Slot>().storedMonster.passiveMove);
                    }
                }

            }
        }

        List<MonsterItemSO> itemsFriendly = new List<MonsterItemSO>();
        itemsFriendly.Add(friendlyMonster.item1);
        itemsFriendly.Add(friendlyMonster.item2);
        itemsFriendly.Add(friendlyMonster.item3);



        friendlyPassiveController.StartPassives(friendlyMonster.passiveMove, passivesShared);
        friendlyItemController.StartItems(itemsFriendly);

        if (!st)
        {
            TriggerAction(TriggerType.tagIn);
        }
    }


    public void SetFriendlyMonster(int slot)
    {
        if (GM.collectionManager.partySlots[slot].storedMonsterObject == null) return;

        if (!tagReady[slot]) { return; }

        
        if (!GM.battleManager.isPlayingIntro)
        {
            //Debug.Log("what?");
            GM.battleManager.PauseControls();

            invulnerable = true;
            invTime = 0.2f;

            taggingSlot = slot;
            tagTimer = 0.3f;
            maskCutout.Play(tagTimer);
            TriggerAction(TriggerType.tagOut);
            tagOn = true;
        }
        else
        {
            SpawnNewMonster(slot, true);
        }


    }

    public void SetEmpty(bool state)
    {
        if (state) // set empty for bond battle, only bond button active, no beast yet
        {
            parentMain.SetActive(false);
        }
        else
        {
            parentMain.SetActive(true);
        }
    }

    public void TakeDamage(int damage, int baseDamage, bool effect, bool critical, int dotAmount, float dotTime, float stunnedTime, bool resetSpecial, float antiGravTime, bool echo, List<int> enemyStatBuffs, List<int> friendlyStatBuffs, Transform pos, FireProjectileEffectSO effectProjectile) // if effect cant be parried, guarded
    {
        if (GM.battleManager.isWinning || GM.battleManager.isLosing) { return; }

        //USE DEFENCE HERE
        if (!effect)
        {
            TriggerAction(TriggerType.beingHit);
        }
        

        if (parryOn) // perfect guard
        {
            // NO damage but do guard effect
            //Debug.Log("YO");
            if (perfectGuard == PerfectGuardEffects.CritAttack)
            {
                friendlyBattleBuffManager.AddBuff(perfectValue, 9);
            }
            else if (perfectGuard == PerfectGuardEffects.TakingCrits)
            {
                friendlyBattleBuffManager.AddBuff(perfectValue, 10);
            }
            else if (perfectGuard == PerfectGuardEffects.Stunned)
            {
                friendlyBattleBuffManager.AddBuff(perfectValue, 7);
            }
            else if (perfectGuard == PerfectGuardEffects.Heal)
            {
                Heal((int)perfectValue);
            }
            else if (perfectGuard == PerfectGuardEffects.Oomph)
            {
                friendlyBattleBuffManager.AddBuff(EffectedStat.Oomph, (int)perfectValue, perfectTargets);
            }
            else if (perfectGuard == PerfectGuardEffects.Guts)
            {
                //Debug.Log("YO2");
                friendlyBattleBuffManager.AddBuff(EffectedStat.Guts, (int)perfectValue, perfectTargets);
            }
            else if (perfectGuard == PerfectGuardEffects.Juice)
            {
                friendlyBattleBuffManager.AddBuff(EffectedStat.Juice, (int)perfectValue, perfectTargets);
            }
            else if (perfectGuard == PerfectGuardEffects.Edge)
            {
                friendlyBattleBuffManager.AddBuff(EffectedStat.Edge, (int)perfectValue, perfectTargets);
            }
            else if (perfectGuard == PerfectGuardEffects.Wits)
            {
                friendlyBattleBuffManager.AddBuff(EffectedStat.Wits, (int)perfectValue, perfectTargets);
            }
            else if (perfectGuard == PerfectGuardEffects.Spark)
            {
                friendlyBattleBuffManager.AddBuff(EffectedStat.Spark, (int)perfectValue, perfectTargets);
            }
            else if (perfectGuard == PerfectGuardEffects.Projectile)
            {
                friendlyMoveController.DoProjectile(perfectProjectile.projectilePrefab, perfectProjectile.projectileDamage, perfectProjectile.projectileSpeed, perfectProjectile.lifetime, perfectProjectile.collideWithAmountOfObjects, perfectProjectile.criticalProjectile, perfectProjectile);
            }

            Targets targs = new Targets(false, false);
            hitNumbers.SpawnPopup(PopupType.PerfectBlock, pos, "", 0);// perfectblock popup
            GuardOff();

        }
        else if (effect || !guardOn && !invulnerable)
        {
            regenMulti = 0.25f;
            regenTimer = regenBaseTime;
            regenText.text = "HPs x " + regenMulti.ToString("F2");

            float floatDamage = damage;

            if (!effect)
            {
                
                if (critical)
                {
                    floatDamage = damage * 2;
                }
                else
                {
                    floatDamage = damage;
                }
            }
            
            //Debug.Log("start dmg: " + flDmg);

            // (BASE + NATURE) * (ITEM + BUFF + PASSIVE)

            float itemPassives = friendlyBattleBuffManager.GetStatsFromItemsPassives(EffectedStat.Guts);
            float buffSlots = friendlyBattleBuffManager.slotValues[1];
            float gutsAmount = guts + itemPassives + buffSlots;


            if (!effect)
            {
                if (dotAmount > 0 || dotTime > 0) // has dot
                {
                    friendlyBattleBuffManager.AddBuff(dotAmount, dotTime);
                }

                if (stunnedTime > 0)
                {
                    friendlyBattleBuffManager.AddBuff(stunnedTime, 3);
                }

                if (resetSpecial)
                {
                    GM.battleManager.enemyMonsterController.specialReady[GM.battleManager.enemyMonsterController.currentSlot] = true;
                    GM.battleManager.enemyMonsterController.specialC[GM.battleManager.enemyMonsterController.currentSlot] = 0f;
                }

                if (antiGravTime > 0)
                {
                    friendlyBattleBuffManager.AddBuff(antiGravTime, 5);
                }

                if (echo)
                {
                    GM.battleManager.enemyMonsterController.enemyMoveController.DoProjectile(effectProjectile.projectilePrefab, effectProjectile.projectileDamage, effectProjectile.projectileSpeed, effectProjectile.lifetime, effectProjectile.collideWithAmountOfObjects, effectProjectile.criticalProjectile, effectProjectile);
                }

                for (int i = 0; i < enemyStatBuffs.Count; i++)
                {
                    Targets t = new Targets(false, false);

                    if (enemyStatBuffs[i] != 0)
                    {
                        switch (i)
                        {
                            case 0:
                                friendlyBattleBuffManager.AddBuff(EffectedStat.Oomph, enemyStatBuffs[i], t);
                                break;
                            case 1:
                                friendlyBattleBuffManager.AddBuff(EffectedStat.Guts, enemyStatBuffs[i], t);
                                break;
                            case 2:
                                friendlyBattleBuffManager.AddBuff(EffectedStat.Juice, enemyStatBuffs[i], t);
                                break;
                            case 3:
                                friendlyBattleBuffManager.AddBuff(EffectedStat.Edge, enemyStatBuffs[i], t);
                                break;
                            case 4:
                                friendlyBattleBuffManager.AddBuff(EffectedStat.Wits, enemyStatBuffs[i], t);
                                break;
                            case 5:
                                friendlyBattleBuffManager.AddBuff(EffectedStat.Spark, enemyStatBuffs[i], t);
                                break;
                        }
                    }
                }

                for (int i = 0; i < friendlyStatBuffs.Count; i++)
                {
                    Targets t = new Targets(false, false);
                    if (friendlyStatBuffs[i] != 0)
                    {
                        switch (i)
                        {
                            case 0:
                                GM.battleManager.enemyMonsterController.enemyBattleBuffManager.AddBuff(EffectedStat.Oomph, friendlyStatBuffs[i], t);
                                break;
                            case 1:
                                GM.battleManager.enemyMonsterController.enemyBattleBuffManager.AddBuff(EffectedStat.Guts, friendlyStatBuffs[i], t);
                                break;
                            case 2:
                                GM.battleManager.enemyMonsterController.enemyBattleBuffManager.AddBuff(EffectedStat.Juice, friendlyStatBuffs[i], t);
                                break;
                            case 3:
                                GM.battleManager.enemyMonsterController.enemyBattleBuffManager.AddBuff(EffectedStat.Edge, friendlyStatBuffs[i], t);
                                break;
                            case 4:
                                GM.battleManager.enemyMonsterController.enemyBattleBuffManager.AddBuff(EffectedStat.Wits, friendlyStatBuffs[i], t);
                                break;
                            case 5:
                                GM.battleManager.enemyMonsterController.enemyBattleBuffManager.AddBuff(EffectedStat.Spark, friendlyStatBuffs[i], t);
                                break;
                        }
                    }
                }
            }

            float baseDmgFloat = baseDamage;

            float trueDamage = floatDamage - (gutsAmount * 0.04f * baseDmgFloat);

            if (trueDamage < 1 && baseDamage > 0)
            {
                trueDamage = 1f;
            }


            int dmg = Mathf.RoundToInt(trueDamage);
            int protectedDamage = dmg - (int)floatDamage;
            //Debug.Log("True dmg: " + dmg);

            if (GM.invulnerableDebug)
            {
                dmg = 0;
                protectedDamage = 0;
            }

            healthBar.SetHealth(GM.playerHP - dmg, true);
            GM.playerHP = (int)healthBar.slider.value;

            friendlyParentAnim.SetTrigger("Hit");
            


            if (GM.playerHP <= 0)
            {
                GM.battleManager.StartLoss();

                if (dmg > 0)
                {
                    if (effect)
                    {
                        hitNumbers.SpawnPopup(PopupType.Dot, pos, dmg.ToString(), protectedDamage); //dot popup
                        healthBarShakeAnim.SetTrigger("DotShake");
                    }
                    else
                    {
                        hitNumbers.SpawnPopup(PopupType.Damage, pos, dmg.ToString(), protectedDamage); //damage popup
                        healthBarShakeAnim.SetTrigger("Shake");
                    }
                }
            }
            else
            {
                if (dmg > 0)
                {
                    if (effect)
                    {
                        hitNumbers.SpawnPopup(PopupType.Dot, pos, dmg.ToString(), protectedDamage); //dot popup
                        healthBarShakeAnim.SetTrigger("DotShake");
                    }
                    else
                    {
                        hitNumbers.SpawnPopup(PopupType.Damage, pos, dmg.ToString(), protectedDamage); //damage popup
                        healthBarShakeAnim.SetTrigger("Shake");
                        GM.battleManager.cameraAnimator.SetInteger("Focus", 3);
                    }
                }

            }
        }
        else
        {
            Targets targs = new Targets(false, false);

            if (invulnerable)
            {
                hitNumbers.SpawnPopup(PopupType.Parry, pos, "", 0); // parry popup
                GM.battleManager.cameraAnimator.SetTrigger("Shake");
            }
            else
            {
                hitNumbers.SpawnPopup(PopupType.Block, pos, "", 0); // block popup
            }



            GuardOff();
        }

    }

    public void Heal(int amount)
    {
        //Debug.Log("here");
        if (GM.playerHP < 1000)
        {
            int realAmount = amount;
            if (amount + GM.playerHP > 1000)
            {
                realAmount = (amount + (int)GM.playerHP) - 1000;
            }

            healthBar.SetHealth(GM.playerHP + amount, false);
            GM.playerHP = healthBar.slider.value;
            hitNumbers.SpawnPopup(PopupType.Heal, defaultHitNumbersLocation, realAmount.ToString(), 0); // heal popup
        }

    }

    public void Stun(bool state, float time)
    {
        if (state)
        {
            stunned = true;
            GM.battleManager.PauseControls();
            uiStunManager.Stun(time);
            stunManager.Stun(time);
            GM.battleManager.captureButton.StopCharging();
        }
        else
        {
            stunned = false;
            GM.battleManager.ResumeControls();
            uiStunManager.StopStun();
            stunManager.StopStun();
        }

    }

    public void Guard(PerfectGuardEffects perfectGuardEffect, float perfectGuardValue, FireProjectileEffectSO perfectGuardProjectile, Targets targs, float parryTime)
    {
        guardOn = true;
        guardRenderer.gameObject.SetActive(true);
        guardRenderer.sprite = yellowGuard;
        parryTimer = parryTime; // 0.25f
        parryOn = true;

        perfectGuard = perfectGuardEffect;
        perfectValue = perfectGuardValue;
        perfectProjectile = perfectGuardProjectile;
        perfectTargets = targs;
    }

    public void GuardOff()
    {
        guardOn = false;
        guardRenderer.gameObject.SetActive(false);
        parryTimer = 0;
        parryOn = false;
        perfectGuard = PerfectGuardEffects.None;
        perfectValue = 0f;
        perfectProjectile = null;
        perfectTargets = new Targets(false, false);
    }

    public void SetLowGravity(float grav, float jumpF)
    {
        rb.gravityScale = grav;
        jumpForce = jumpF;

        if (rb.gravityScale <= 1)
        {
            antiGravEffect.SetActive(true);
        }
        else
        {
            antiGravEffect.SetActive(false);
        }
    }
    public void SetCritChance(int amount)
    {
        critchance = amount;
    }


    public void Jump() // Jumps monster into air
    {
        rb.velocity = Vector2.up * jumpForce;

        friendlyAnim.SetBool("Jump", true);
        friendlyAnimVariant.SetBool("Jump", true);
    }

    public void StopJump()
    {
        friendlyAnim.SetBool("Jump", false);
        friendlyAnimVariant.SetBool("Jump", false);
    }

    public void Special() // Uses Special Attack
    {
        if (specialReady[currentSlot])
        {
            DoSpecial();
        }
    }

    public void Attack() // Used Basic Attack
    {
        if (basicReady[currentSlot])
        {
            DoAttack();
        }
    }
    private void DoSpecial()
    {
        //if (friendlyMonster.specialMove == null) return;

        if (friendlyMonster.specialMove.moveActions.Count > 0)
        {
            // use moves
            friendlyMoveController.UseMove(friendlyMonster.specialMove);
        }


        friendlyParentAnim.SetTrigger("Attack");
        friendlyAnim.SetTrigger("Special");
        friendlyAnimVariant.SetTrigger("Special");

        friendlyUIAnimSpecial.SetTrigger("Pressed");

        TriggerAction(TriggerType.useSpecial);

        float witsAmount = wits + friendlyBattleBuffManager.slotValues[4] + friendlyBattleBuffManager.GetStatsFromItemsPassives(EffectedStat.Wits);


        float baseCD = friendlyMonster.specialMove.baseCooldown;
        float sCool = 0f;
        if (witsAmount < 0)
        {
            sCool = baseCD - (baseCD * (0.008f * witsAmount));
        }
        else
        {
            sCool = 1f / ((1f / baseCD) * witsAmount * 0.04f + (1f / baseCD));
        }

        if (sCool < friendlyMonster.specialMove.minCooldown)
        {
            sCool = friendlyMonster.specialMove.minCooldown;
        }


        specialC[currentSlot] = sCool;
        specialReady[currentSlot] = false;
    }
    private void DoAttack()
    {
        //if (friendlyMonster.basicMove == null) return;

        if (friendlyMonster.basicMove.moveActions.Count > 0)
        {
            friendlyMoveController.UseMove(friendlyMonster.basicMove);
        }

        friendlyUIAnimBasic.SetTrigger("Pressed");
        friendlyParentAnim.SetTrigger("Attack");
        friendlyAnim.SetTrigger("Basic");
        friendlyAnimVariant.SetTrigger("Basic");

        TriggerAction(TriggerType.useBasic);

        float edgeAmount = edge + friendlyBattleBuffManager.slotValues[3] + friendlyBattleBuffManager.GetStatsFromItemsPassives(EffectedStat.Edge);

        float baseCD = friendlyMonster.basicMove.baseCooldown;
        float bCool = 0f;

        if (edgeAmount < 0)
        {
            bCool = baseCD - (baseCD * (0.008f * edgeAmount));
        }
        else
        {
            bCool = 1f / ((1f / baseCD) * edgeAmount * 0.04f + (1f / baseCD));
        }


        if (bCool < friendlyMonster.basicMove.minCooldown)
        {
            bCool = friendlyMonster.basicMove.minCooldown;
        }


        basicC[currentSlot] = bCool;
        basicReady[currentSlot] = false;

    }

    public void FireProjectile(GameObject prefab, float speed, int dmg, int baseDmg, float lifeTime, int collideWithAmountOfObjects, bool criticalProjectile, FireProjectileEffectSO projEffect)
    {
        GameObject proj = Instantiate(prefab, firePoint.position, firePoint.rotation);
        projectiles.Add(proj);
        //Debug.Log("What?");
        if (friendlyBattleBuffManager.slotValues[11] > 0) // critical chance
        {
            int amount = friendlyBattleBuffManager.slotValues[11];

            if (amount >= 1000) // crit higher than 100
            {
                proj.GetComponent<Projectile>().Init(speed, dmg, baseDmg, lifeTime, collideWithAmountOfObjects, true, "Enemy", GM.battleManager.enemyMonsterController.enemyBattleBuffManager, projEffect);
            }
            else
            {
                float random = Random.Range(1, 1000);
                //Debug.Log(random);

                if (random <= amount)
                {
                    proj.GetComponent<Projectile>().Init(speed, dmg, baseDmg, lifeTime, collideWithAmountOfObjects, true, "Enemy", GM.battleManager.enemyMonsterController.enemyBattleBuffManager, projEffect);
                }
                else
                {
                    proj.GetComponent<Projectile>().Init(speed, dmg, baseDmg, lifeTime, collideWithAmountOfObjects, criticalProjectile, "Enemy", GM.battleManager.enemyMonsterController.enemyBattleBuffManager, projEffect);
                }
            }
        }
        else
        {
            proj.GetComponent<Projectile>().Init(speed, dmg, baseDmg, lifeTime, collideWithAmountOfObjects, criticalProjectile, "Enemy", GM.battleManager.enemyMonsterController.enemyBattleBuffManager, projEffect);
        }

    }

    public void TriggerAction(TriggerType triggerType)
    {
        //Debug.Log("Trigger: " + triggerType);
        friendlyPassiveController.PassiveTrigger(triggerType);
        friendlyItemController.ItemTrigger(triggerType);
    }

    public void ClearCooldowns()
    {
        for (int i = 0; i < basicReady.Count; i++) { basicReady[i] = true ; }
        for (int i = 0; i < specialReady.Count; i++) { specialReady[i] = true; }
        for (int i = 0; i < tagReady.Count; i++) { tagReady[i] = true; }
        for (int i = 0; i < basicC.Count; i++) { basicC[i] = 0; }
        for (int i = 0; i < specialC.Count; i++) { specialC[i] = 0; }
        for (int i = 0; i < tagC.Count; i++) { tagC[i] = 0; }
    }

    
}


public enum TriggerType
{
    beingHit, // self being hit
    tagOut,
    tagIn,
    crit,
    enemyStunned,
    useBasic, // self use 
    useSpecial,
    enemyHitBasic, // enemy hit
    enemyHitSpecial,
    enemyHitInAir
}
