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
    public Animator friendlyAnim;
    public Animator friendlyAnimVariant;
    public Animator friendlyParentAnim;
    public Animator friendlyUIAnimBasic;
    public Animator friendlyUIAnimSpecial;
    public Animator healthBarShakeAnim;
    public Slider juiceRegenSlider;
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

    public List<bool> basicReady2 = new List<bool>();
    public List<bool> specialReady2 = new List<bool>();
    public List<bool> tagReady2 = new List<bool>();


    public List<float> basicC2 = new List<float>();
    public List<float> specialC2 = new List<float>();
    public List<float> tagC2 = new List<float>();

    public List<bool> basicReady3 = new List<bool>();
    public List<bool> specialReady3 = new List<bool>();
    public List<bool> tagReady3 = new List<bool>();


    public List<float> basicC3 = new List<float>();
    public List<float> specialC3 = new List<float>();
    public List<float> tagC3 = new List<float>();

    private int basicExtraCharges = 0;
    private int specialExtraCharges = 0;
    private int tagExtraCharges = 0;

    private bool regenOn = false;
    private float regenTimer = 0f;
    private float regenBaseTime = 1f;

    private bool regenCooldown = true;
    private float regenCooldownTime = 3f;
    private float regenCoolodownBaseTime = 3f;

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


            if (regenCooldown)
            {
                if (regenCooldownTime > 0)
                {
                    regenCooldownTime -= Time.deltaTime;
                }
                else if (regenCooldownTime <= 0)
                {
                    //REGEN
                    regenOn = true;
                    regenCooldown = false;
                }

                juiceRegenSlider.value = regenCooldownTime / regenCoolodownBaseTime;
            }

            if (regenOn)
            {
                if (regenTimer > 0)
                {
                    regenTimer -= Time.deltaTime;
                }
                else if (regenTimer <= 0)
                {

                    float regenAmount = 0f;
                    float itemPassives = friendlyBattleBuffManager.GetStatsFromItemsPassives(EffectedStat.Juice);
                    float buffSlots = friendlyBattleBuffManager.slotValues[2];
                    regenAmount = 0.1f * (juice + itemPassives + buffSlots);

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
                   
                    float regenAmount = 0.1f * (juice + friendlyBattleBuffManager.GetStatsFromItemsPassives(EffectedStat.Juice) + friendlyBattleBuffManager.slotValues[2]);

                    if (regenOn && GM.playerHP < 100)
                    {
                        healthBar.ChangeColor("Regen");
                    }
                    else
                    {
                        healthBar.ChangeColor("Normal");
                    }
                }
            }
        }
    }

    private void DoCooldowns()
    { // 1111111111
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
        // 22222222222
        for (int i = 0; i < basicReady2.Count; i++)
        {
            if (!basicReady2[i] && basicReady[i])
            {
                if (basicC2[i] > 0) { basicC2[i] -= Time.deltaTime; }
                else if (basicC2[i] <= 0) { basicReady2[i] = true; basicC2[i] = 0f; }
            }
        }

        for (int i = 0; i < specialReady2.Count; i++)
        {
            if (!specialReady2[i] && specialReady[i])
            {
                if (specialC2[i] > 0) { specialC2[i] -= Time.deltaTime; }
                else if (specialC2[i] <= 0) { specialReady2[i] = true; specialC2[i] = 0f; }
            }
        }

        for (int i = 0; i < tagReady2.Count; i++)
        {
            if (!tagReady2[i] && tagReady[i])
            {
                if (tagC2[i] > 0) { tagC2[i] -= Time.deltaTime; }
                else if (tagC2[i] <= 0) { tagReady2[i] = true; tagC2[i] = 0f; }
            }
        }
        // 33333333333333333
        for (int i = 0; i < basicReady3.Count; i++)
        {
            if (!basicReady3[i] && basicReady[i] && basicReady2[i])
            {
                if (basicC3[i] > 0) { basicC3[i] -= Time.deltaTime; }
                else if (basicC3[i] <= 0) { basicReady3[i] = true; basicC3[i] = 0f; }
            }
        }

        for (int i = 0; i < specialReady3.Count; i++)
        {
            if (!specialReady3[i] && specialReady[i] && specialReady2[i])
            {
                if (specialC3[i] > 0) { specialC3[i] -= Time.deltaTime; }
                else if (specialC3[i] <= 0) { specialReady3[i] = true; specialC3[i] = 0f; }
            }
        }

        for (int i = 0; i < tagReady3.Count; i++)
        {
            if (!tagReady3[i] && tagReady[i] && tagReady2[i])
            {
                if (tagC3[i] > 0) { tagC3[i] -= Time.deltaTime; }
                else if (tagC3[i] <= 0) { tagReady3[i] = true; tagC3[i] = 0f; }
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
            tagC[currentSlot] = cooldown - (cooldown * (0.008f * (spark + friendlyBattleBuffManager.slotValues[5] + friendlyBattleBuffManager.GetStatsFromItemsPassives(EffectedStat.Spark)))); //tagCooldown;
            tagReady[currentSlot] = false;
        }

        if (isStart)
        {
            regenCooldownTime = regenCoolodownBaseTime;
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

        float edgeAmount = edge + friendlyBattleBuffManager.slotValues[3] + friendlyBattleBuffManager.GetStatsFromItemsPassives(EffectedStat.Edge);
        float witsAmount = wits + friendlyBattleBuffManager.slotValues[4] + friendlyBattleBuffManager.GetStatsFromItemsPassives(EffectedStat.Wits);
        float sparkAmount = spark + friendlyBattleBuffManager.slotValues[5] + friendlyBattleBuffManager.GetStatsFromItemsPassives(EffectedStat.Spark);

        basicExtraCharges = 0;
        specialExtraCharges = 0;
        tagExtraCharges = 0;

        while (edgeAmount > 100)
        {
            edgeAmount -= 100f;
            basicExtraCharges++;
        }

        while (witsAmount > 100)
        {
            witsAmount -= 100f;
            specialExtraCharges++;
        }

        while (sparkAmount > 100)
        {
            sparkAmount -= 100f;
            tagExtraCharges++;
        }


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

    public void TakeDamage(int damage, bool effect, bool critical, int dotAmount, float dotTime, float stunnedTime, bool resetSpecial, float antiGravTime, bool echo, List<int> enemyStatBuffs, List<int> friendlyStatBuffs, Transform pos, FireProjectileEffectSO effectProjectile) // if effect cant be parried, guarded
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
            regenCooldownTime = regenCoolodownBaseTime;
            regenCooldown = true;

            regenOn = false;

            float flDmg = damage;

            if (!effect)
            {
                
                if (critical)
                {
                    flDmg = damage * 2;
                }
                else
                {
                    flDmg = damage;
                }
            }
            
            //Debug.Log("start dmg: " + flDmg);

            // (BASE + NATURE) * (ITEM + BUFF + PASSIVE)

            float gutsReal = 0f;
            float gutsAmount = 0f;

            float itemPassives = friendlyBattleBuffManager.GetStatsFromItemsPassives(EffectedStat.Guts);
            float buffSlots = friendlyBattleBuffManager.slotValues[1];
            gutsAmount = guts + itemPassives + buffSlots;


            //float gutsAmount = guts + (guts * ((friendlyBattleBuffManager.slotValues[1] + friendlyBattleBuffManager.GetStatsFromItemsPassives(EffectedStat.Guts)) / 100f));

            if (gutsAmount > 100 && !effect)
            {
                gutsReal = 100f;

                float gutsNegateAmount = 0f;

                if (gutsAmount > 200) // reflect chance
                {
                    gutsNegateAmount = 100f;
                    float gutsReflectAmount = gutsAmount - 200f;

                    float reflectRand = Random.Range(0f, 1f);
                    float reflectChance = 0.008f * gutsReflectAmount;

                    //Debug.Log("Reflect Try: Rand: " + reflectRand.ToString() + ", Chance: " + reflectChance.ToString() + ", Guts Reflect Amount: " + gutsReflectAmount.ToString());

                    if (reflectRand <= reflectChance) // reflect projectile and take no dmg
                    {
                        if (effectProjectile != null)
                        {
                            friendlyMoveController.DoProjectile(effectProjectile.projectilePrefab, effectProjectile.projectileDamage, effectProjectile.projectileSpeed, effectProjectile.lifetime, effectProjectile.collideWithAmountOfObjects, effectProjectile.criticalProjectile, effectProjectile);
                        }
                        else
                        {
                            Debug.LogError("Projectile Data is Null, But is needed for reflecting");
                        }


                        hitNumbers.SpawnPopup(PopupType.Reflected, pos, "", 0);
                        return;
                    }
                }
                else
                {
                    gutsNegateAmount = gutsAmount - 100f;
                }

                float rand = Random.Range(0f, 1f);
                float chance = ((0.008f * gutsNegateAmount));

                //Debug.Log("Negate Try: Rand: " + rand.ToString() + ", Chance: " + chance.ToString() + ", Guts Negate Amount: " + gutsNegateAmount.ToString());

                if (rand <= chance) // negate damage
                {
                    hitNumbers.SpawnPopup(PopupType.Negated, pos, "", 0);
                    return;
                }
            }
            else
            {
                gutsReal = gutsAmount;
            }

            // IF NO NEGATE OR REFLECT THIS WILL CONTINUE BUT OTHERWISE THIS CODE WON'T RUN

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


            float trueDamage = flDmg - (flDmg * (0.008f * gutsReal));


            int dmg = Mathf.RoundToInt(trueDamage);

            if (dmg < 1)
            {
                dmg = 1;
            }

            int protectedDamage = dmg - (int)flDmg;
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
        if (GM.playerHP < 100)
        {
            int realAmount = amount;
            if (amount + GM.playerHP > 100)
            {
                realAmount = (amount + (int)GM.playerHP) - 100;
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
            DoSpecial(0f, 100);
        }
        else
        {
            if (specialReady2[currentSlot] && specialExtraCharges > 0)
            {
                DoSpecial(100f, 200);
            }
            else
            {
                if (specialReady3[currentSlot] && specialExtraCharges > 1)
                {
                    DoSpecial(200f, 300);
                }
            }
        }

    }

    public void Attack() // Used Basic Attack
    {
        if (basicReady[currentSlot])
        {
            DoAttack(0f, 100);
        }
        else
        {
            if (basicReady2[currentSlot] && basicExtraCharges > 0)
            {
                DoAttack(100f, 200);
            }
            else
            {
                if (basicReady3[currentSlot] && basicExtraCharges > 1)
                {
                    DoAttack(200f, 300);
                }
            }
        }

    }
    private void DoSpecial(float sizeNum, int num)
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

        float valueAmount = 0f;
        float itemPassives = friendlyBattleBuffManager.GetStatsFromItemsPassives(EffectedStat.Wits);
        float buffSlots = friendlyBattleBuffManager.slotValues[4];

        valueAmount = wits + itemPassives + buffSlots;

        

        float valueReal = 0f;


        if (valueAmount > num)
        {
            valueReal = 100f;

        }
        else
        {
            valueReal = valueAmount - sizeNum;
        }

        float sCool = friendlyMonster.specialMove.baseCooldown - (friendlyMonster.specialMove.baseCooldown * (0.008f * valueReal));
        if (sCool < friendlyMonster.specialMove.minCooldown)
        {
            sCool = friendlyMonster.specialMove.minCooldown;
        }


        if (num == 100)
        {
            specialC[currentSlot] = sCool;
            specialReady[currentSlot] = false;
        }
        else if (num == 200)
        {
            specialC2[currentSlot] = sCool;
            specialReady2[currentSlot] = false;
        }
        else if (num == 300)
        {
            specialC3[currentSlot] = sCool;
            specialReady3[currentSlot] = false;
        }


    }
    private void DoAttack(float sizeNum, int num)
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

        float valueAmount = 0f;
        float itemPassives = friendlyBattleBuffManager.GetStatsFromItemsPassives(EffectedStat.Edge);
        float buffSlots = friendlyBattleBuffManager.slotValues[3];
        valueAmount = edge + itemPassives + buffSlots;

        float valueReal = 0f;


        if (valueAmount > num)
        {
            valueReal = 100f;

        }
        else
        {
            valueReal = valueAmount - sizeNum;
        }

        float bCool = friendlyMonster.basicMove.baseCooldown - (friendlyMonster.basicMove.baseCooldown * (0.008f * valueReal));
        if (bCool < friendlyMonster.basicMove.minCooldown)
        {
            bCool = friendlyMonster.basicMove.minCooldown;
        }


        if (num == 100)
        {
            basicC[currentSlot] = bCool;
            basicReady[currentSlot] = false;
        }
        else if (num == 200)
        {
            basicC2[currentSlot] = bCool;
            basicReady2[currentSlot] = false;
        }
        else if (num == 300)
        {
            basicC3[currentSlot] = bCool;
            basicReady3[currentSlot] = false;
        }


        invulnerable = true;
        invTime = 0.1f;


    }

    public void FireProjectile(GameObject prefab, float speed, int dmg, float lifeTime, int collideWithAmountOfObjects, bool criticalProjectile, FireProjectileEffectSO projEffect)
    {
        GameObject proj = Instantiate(prefab, firePoint.position, firePoint.rotation);
        projectiles.Add(proj);
        //Debug.Log("What?");
        if (friendlyBattleBuffManager.slotValues[11] > 0) // critical chance
        {
            int amount = friendlyBattleBuffManager.slotValues[11];

            if (amount >= 100) // crit higher than 100
            {
                proj.GetComponent<Projectile>().Init(speed, dmg, lifeTime, collideWithAmountOfObjects, true, "Enemy", GM.battleManager.enemyMonsterController.enemyBattleBuffManager, projEffect);
            }
            else
            {
                float random = Random.Range(1, 100);
                //Debug.Log(random);

                if (random <= amount)
                {
                    proj.GetComponent<Projectile>().Init(speed, dmg, lifeTime, collideWithAmountOfObjects, true, "Enemy", GM.battleManager.enemyMonsterController.enemyBattleBuffManager, projEffect);
                }
                else
                {
                    proj.GetComponent<Projectile>().Init(speed, dmg, lifeTime, collideWithAmountOfObjects, criticalProjectile, "Enemy", GM.battleManager.enemyMonsterController.enemyBattleBuffManager, projEffect);
                }
            }
        }
        else
        {
            proj.GetComponent<Projectile>().Init(speed, dmg, lifeTime, collideWithAmountOfObjects, criticalProjectile, "Enemy", GM.battleManager.enemyMonsterController.enemyBattleBuffManager, projEffect);
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
        for (int i = 0; i < basicReady2.Count; i++) { basicReady2[i] = true; }
        for (int i = 0; i < basicReady3.Count; i++) { basicReady3[i] = true; }

        for (int i = 0; i < specialReady.Count; i++) { specialReady[i] = true; }
        for (int i = 0; i < specialReady2.Count; i++) { specialReady2[i] = true; }
        for (int i = 0; i < specialReady3.Count; i++) { specialReady3[i] = true; }

        for (int i = 0; i < tagReady.Count; i++) { tagReady[i] = true; }
        for (int i = 0; i < tagReady2.Count; i++) { tagReady2[i] = true; }
        for (int i = 0; i < tagReady3.Count; i++) { tagReady3[i] = true; }



        for (int i = 0; i < basicC.Count; i++) { basicC[i] = 0; }
        for (int i = 0; i < basicC2.Count; i++) { basicC2[i] = 0; }
        for (int i = 0; i < basicC3.Count; i++) { basicC3[i] = 0; }

        for (int i = 0; i < specialC.Count; i++) { specialC[i] = 0; }
        for (int i = 0; i < specialC2.Count; i++) { specialC2[i] = 0; }
        for (int i = 0; i < specialC3.Count; i++) { specialC3[i] = 0; }

        for (int i = 0; i < tagC.Count; i++) { tagC[i] = 0; }
        for (int i = 0; i < tagC2.Count; i++) { tagC2[i] = 0; }
        for (int i = 0; i < tagC3.Count; i++) { tagC3[i] = 0; }


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
