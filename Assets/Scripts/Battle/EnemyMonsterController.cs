using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class EnemyMonsterController : MonoBehaviour
{
    public float enemyHealth = 100;

    [Header("References")]
    public GameManager GM;
    public MonsterAIController aiController;
    public BattleHealthBar healthBar;
    public BattleCapBar capBar;
    public Animator enemyAnim;
    public Animator enemyAnimVariant;
    public Animator enemyParentAnim;
    public Animator healthBarShakeAnim;
    public Slider juiceRegenSlider;
    public SpriteRenderer enemyDynamicSprite;
    public TextMeshProUGUI enemyNameText;
    public TextMeshProUGUI enemyLevelText;
    public EnemyIconRotator enemyIconRotator;
    public NumberPopupManager hitNumbers;
    public Transform defaultHitNumbersLocation;
    public BattleBuffManager enemyBattleBuffManager;

    public StunManager stunManager;
    public FriendlyMonsterController fMonsterController;

    public MoveController enemyMoveController;
    //public ItemController enemyItemController;
    //public PassiveController enemyPassiveController;

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
    //public float jumpForce;
    public float jumpForce;
    private bool doLand;
    
    
    [Header("Monster")]
    public Monster currentMonster;
    public List<Monster> backupMonsters = new List<Monster>();
    public int currentSlot;

    [Header("Stats and Cooldowns")]

    public int oomph, guts, juice, edge, wits, spark;

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

    [HideInInspector] public List<GameObject> projectiles = new List<GameObject>();

    private bool tagOn = false;
    private float tagTimer = 0f;
    private int taggingSlot = 0;

    public MaskCutout maskCutout;

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

    private bool invulnerable = false;
    private float invTime = 0f;

    [HideInInspector] public bool stunned = false;

    private NodeType nodeType;
    private float accumulatedJuiceHealing = 0f;

    //public bool active = true;

    private bool isJumping;
    private float jumpTime;
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    void Update()
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

        isGrounded = Physics2D.OverlapCircle(feetPos.position, checkRadius, whatIsGround);

        if (isGrounded)
        {
            if (doLand)
            {
                StopJump();
                enemyParentAnim.SetTrigger("Land");
                doLand = false;
            }
        }
        else
        {
            if (!doLand)
            {
                enemyParentAnim.SetTrigger("Jump");
            }
            doLand = true;
        }

        DoCooldowns();

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
                regenCooldownTime = 0f;
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
                //REGEN
                float regenAmount = 0f;
                float itemPassives = enemyBattleBuffManager.GetStatsFromItemsPassives(EffectedStat.Juice);
                float buffSlots = enemyBattleBuffManager.slotValues[2];
                regenAmount = 0.1f * (juice + itemPassives + buffSlots);


                //Debug.Log("Regen Amount: " + regenAmount.ToString());
                int flooredRegen = Mathf.FloorToInt(regenAmount + accumulatedJuiceHealing);
                //Debug.Log("Floored: " + flooredRegen.ToString());
                float leftOvers = (regenAmount + accumulatedJuiceHealing) - flooredRegen;
                //Debug.Log("Left Overs: " + leftOvers.ToString());

                accumulatedJuiceHealing = leftOvers;

                if (flooredRegen >= 1 || flooredRegen <= -1)
                {
                    healthBar.SetHealth(enemyHealth + flooredRegen, false);
                    enemyHealth = healthBar.slider.value;
                }

                //capBar.SetCaptureLevel(GM.battleManager.enemyCapturePoints, enemyHealth);

                regenTimer = regenBaseTime;
            }
        }

        if (tagOn)
        {
            if (tagTimer > 0)
            {
                tagTimer -= Time.deltaTime;
                //transform.localPosition = Vector3.Lerp(transform.position, new Vector3(10f, -1f, 0f), tagTimer / 1f);
            }
            else if (tagTimer <= 0)
            {
                aiController.SetActive(true);
                tagOn = false;
                SpawnNewMonster(taggingSlot, false);
                //transform.localPosition = new Vector3(5f, -1f, 0f);
                tagTimer = 0f;
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
                if (enemyBattleBuffManager.slotValues[6] > 0)
                {
                    healthBar.ChangeColor("Dot");
                }
                else
                {
                    float regenAmount = 0.1f * (juice + enemyBattleBuffManager.GetStatsFromItemsPassives(EffectedStat.Juice) + enemyBattleBuffManager.slotValues[2]);

                    if (regenOn && enemyHealth < 100)
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


        if (isJumping)
        {
            if (jumpTime > 0)
            {
                JumpRepeat();
                jumpTime -= Time.deltaTime;
            }
            else
            {
                isJumping = false;

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

    

    public void SetupEnemy(List<MonsterSpawn> mons, NodeType type, int extraStats)
    {
        backupMonsters = new List<Monster>();

        for (int i = 0; i < mons.Count; i++)
        {
            backupMonsters.Add(new Monster(Random.Range(mons[i].minLevel, mons[i].maxLevel), mons[i].monster));
            GM.AddBeastToSeenIDs(mons[i].monster);
        }



        SetMonsterActive(0);

        aiController.SetBattleType(false);

        nodeType = type;


        if (extraStats > 0)
        {
            Targets targ = new Targets(false, false);

            enemyBattleBuffManager.AddBuff(EffectedStat.Oomph, extraStats * 8, targ);
            enemyBattleBuffManager.AddBuff(EffectedStat.Guts, extraStats * 8, targ);
            enemyBattleBuffManager.AddBuff(EffectedStat.Juice, extraStats * 8, targ);
            enemyBattleBuffManager.AddBuff(EffectedStat.Edge, extraStats * 8, targ);
            enemyBattleBuffManager.AddBuff(EffectedStat.Wits, extraStats * 8, targ);
            enemyBattleBuffManager.AddBuff(EffectedStat.Spark, extraStats * 8, targ);
        }

        enemyHealth = 100;
        healthBar.SetMaxHealth(100);
        healthBar.SetHealth(100, false);

        regenCooldownTime = regenCoolodownBaseTime;

        for (int i = 0; i < 3; i++)
        {
            if (backupMonsters.Count >= i + 1)
            {
                if (i == 0)
                {
                    GM.battleUI.enemyTagSprites1.tagSprites.UpdateArt(backupMonsters[0]);
                }
                else if (i == 1)
                {
                    GM.battleUI.enemyTagSprites2.tagSprites.UpdateArt(backupMonsters[1]);
                }
                else if (i == 2)
                {
                    GM.battleUI.enemyTagSprites3.tagSprites.UpdateArt(backupMonsters[2]);
                }
            }
        }

    }

    public void SetupEnemyPunk(List<Monster> mons, NodeType type, int extraStats, int punkMaxHealth)
    {
        backupMonsters = mons;

        for (int i = 0; i < mons.Count; i++)
        {
            GM.AddBeastToSeenIDs(mons[i].backupData);
        }

        SetMonsterActive(0);

        if (mons.Count > 1)
        {
            aiController.SetBattleType(true);
        }
        

        nodeType = type;


        if (extraStats > 0)
        {
            Targets targ = new Targets(false, false);

            enemyBattleBuffManager.AddBuff(EffectedStat.Oomph, extraStats * 8, targ);
            enemyBattleBuffManager.AddBuff(EffectedStat.Guts, extraStats * 8, targ);
            enemyBattleBuffManager.AddBuff(EffectedStat.Juice, extraStats * 8, targ);
            enemyBattleBuffManager.AddBuff(EffectedStat.Edge, extraStats * 8, targ);
            enemyBattleBuffManager.AddBuff(EffectedStat.Wits, extraStats * 8, targ);
            enemyBattleBuffManager.AddBuff(EffectedStat.Spark, extraStats * 8, targ);
        }

        enemyHealth = punkMaxHealth;
        healthBar.SetMaxHealth(punkMaxHealth);
        healthBar.SetHealth(punkMaxHealth, false);

        for (int i = 0; i < 3; i++)
        {
            if (backupMonsters.Count >= i + 1)
            {
                if (i == 0)
                {
                    GM.battleUI.enemyTagSprites1.tagSprites.UpdateArt(backupMonsters[0]);
                }
                else if (i == 1)
                {
                    GM.battleUI.enemyTagSprites2.tagSprites.UpdateArt(backupMonsters[1]);
                }
                else if (i == 2)
                {
                    GM.battleUI.enemyTagSprites3.tagSprites.UpdateArt(backupMonsters[2]);
                }
            }
        }

    }

    public void SetupEnemyBondBattle(Monster mon)
    {
        backupMonsters.Add(mon);
        GM.AddBeastToSeenIDs(mon.backupData);
        SetMonsterActive(0);
        aiController.SetBattleType(false);
        //nodeType = type;
        enemyHealth = 100;
        healthBar.SetMaxHealth(100);
        healthBar.SetHealth(100, false);

        for (int i = 0; i < 3; i++)
        {
            if (backupMonsters.Count >= i + 1)
            {
                if (i == 0)
                {
                    GM.battleUI.enemyTagSprites1.tagSprites.UpdateArt(backupMonsters[0]);
                }
                else if (i == 1)
                {
                    GM.battleUI.enemyTagSprites2.tagSprites.UpdateArt(backupMonsters[1]);
                }
                else if (i == 2)
                {
                    GM.battleUI.enemyTagSprites3.tagSprites.UpdateArt(backupMonsters[2]);
                }
            }
        }

        aiController.SetActive(false);
    }
    private void SpawnNewMonster(int slot, bool isStart)
    {
        float cooldown = 7f;

        //currentMonster = backupMonsters[0];

        if (!isStart)
        {
            tagC[currentSlot] = cooldown - (cooldown * (0.008f * (spark + enemyBattleBuffManager.slotValues[5] + enemyBattleBuffManager.GetStatsFromItemsPassives(EffectedStat.Spark)))); //tagCooldown;
            tagReady[currentSlot] = false;
        }

        if (isStart)
        {
            regenCooldownTime = regenCoolodownBaseTime;
            ActivateAI(true);
        }

        currentSlot = slot;
        List<Monster> pMons = new List<Monster>();

        for (int i = 0; i < backupMonsters.Count; i++)
        {
            if (i != slot)
            {
                pMons.Add(backupMonsters[i]);
            }
        }

        GM.battleManager.enemySlotSelected = slot + 1;

        currentMonster = backupMonsters[slot];



        

        //capBar.SetMaxCapturePoints(GM.battleManager.enemyCapturePoints, enemyHealth);
        //capBar.SetCaptureLevel(0f, enemyHealth);

        //SETS STATS VALUES OF THE CONTROLLER TO WHATEVERT IS SWITCHED TO
        float oomphA = 0, oomphB = 0;
        float gutsA = 0, gutsB = 0;
        float juiceA = 0, juiceB = 0;
        float edgeA = 0, edgeB = 0;
        float witsA = 0, witsB = 0;
        float sparkA = 0, sparkB = 0;


        if (currentMonster.level - 1 <= 20)
        {
            if (currentMonster.stats[0].value < 0)
            {
                oomphA = 0;
                oomphB = 0;
            }
            else
            {
                oomphA = (1 * (currentMonster.stats[0].value / 5)) * (currentMonster.level - 1);
                oomphB = 0;
            }

            if (currentMonster.stats[1].value < 0)
            {
                gutsA = 0;
                gutsB = 0;
            }
            else
            {
                gutsA = (1 * (currentMonster.stats[1].value / 5)) * (currentMonster.level - 1);
                gutsB = 0;
            }

            if (currentMonster.stats[2].value < 0)
            {
                juiceA = 0;
                juiceB = 0;
            }
            else
            {
                juiceA = (1 * (currentMonster.stats[2].value / 5)) * (currentMonster.level - 1);
                juiceB = 0;
            }

            if (currentMonster.stats[3].value < 0)
            {
                edgeA = 0;
                edgeB = 0;
            }
            else
            {
                edgeA = (1 * (currentMonster.stats[3].value / 5)) * (currentMonster.level - 1);
                edgeB = 0;
            }

            if (currentMonster.stats[4].value < 0)
            {
                witsA = 0;
                witsB = 0;
            }
            else
            {
                witsA = (1 * (currentMonster.stats[4].value / 5)) * (currentMonster.level - 1);
                witsB = 0;
            }

            if (currentMonster.stats[5].value < 0)
            {
                sparkA = 0;
                sparkB = 0;
            }
            else
            {
                sparkA = (1 * (currentMonster.stats[5].value / 5)) * (currentMonster.level - 1);
                sparkB = 0;
            }

        }
        else if (currentMonster.level - 1 > 20 && currentMonster.level - 1 <= 40)
        {

            if (currentMonster.stats[0].value < 0)
            {
                oomphA = 0;
                oomphB = 0;
            }
            else
            {
                oomphA = (1 * (currentMonster.stats[0].value / 5)) * 19;
                oomphB = (0.66f * (currentMonster.stats[0].value / 5)) * (currentMonster.level - 20);
            }

            if (currentMonster.stats[1].value < 0)
            {
                gutsA = 0;
                gutsB = 0;
            }
            else
            {
                gutsA = (1 * (currentMonster.stats[1].value / 5)) * 19;
                gutsB = (0.66f * (currentMonster.stats[1].value / 5)) * (currentMonster.level - 20);
            }

            if (currentMonster.stats[2].value < 0)
            {
                juiceA = 0;
                juiceB = 0;
            }
            else
            {
                juiceA = (1 * (currentMonster.stats[2].value / 5)) * 19;
                juiceB = (0.66f * (currentMonster.stats[2].value / 5)) * (currentMonster.level - 20);
            }

            if (currentMonster.stats[3].value < 0)
            {
                edgeA = 0;
                edgeB = 0;
            }
            else
            {
                edgeA = (1 * (currentMonster.stats[3].value / 5)) * 19;
                edgeB = (0.66f * (currentMonster.stats[3].value / 5)) * (currentMonster.level - 20);
            }

            if (currentMonster.stats[4].value < 0)
            {
                witsA = 0;
                witsB = 0;
            }
            else
            {
                witsA = (1 * (currentMonster.stats[4].value / 5)) * 19;
                witsB = (0.66f * (currentMonster.stats[4].value / 5)) * (currentMonster.level - 20);
            }

            if (currentMonster.stats[5].value < 0)
            {

                sparkA = 0;
                sparkB = 0;
            }
            else
            {
                sparkA = (1 * (currentMonster.stats[5].value / 5)) * 19;
                sparkB = (0.66f * (currentMonster.stats[5].value / 5)) * (currentMonster.level - 20);
            }
            

 

            

          

        }


        oomph = Mathf.FloorToInt((oomphA + oomphB + currentMonster.stats[0].value) * currentMonster.nature.addedStats[0].value);
        guts = Mathf.FloorToInt((gutsA + gutsB + currentMonster.stats[1].value) * currentMonster.nature.addedStats[1].value);
        juice = Mathf.FloorToInt((juiceA + juiceB + currentMonster.stats[2].value) * currentMonster.nature.addedStats[2].value);
        edge = Mathf.FloorToInt((edgeA + edgeB + currentMonster.stats[3].value) * currentMonster.nature.addedStats[3].value);
        wits = Mathf.FloorToInt((witsA + witsB + currentMonster.stats[4].value) * currentMonster.nature.addedStats[4].value);
        spark = Mathf.FloorToInt((sparkA + sparkB + currentMonster.stats[5].value) * currentMonster.nature.addedStats[5].value);

        float edgeAmount = edge + enemyBattleBuffManager.slotValues[3] + enemyBattleBuffManager.GetStatsFromItemsPassives(EffectedStat.Edge);
        float witsAmount = wits + enemyBattleBuffManager.slotValues[4] + enemyBattleBuffManager.GetStatsFromItemsPassives(EffectedStat.Wits);
        float sparkAmount = spark + enemyBattleBuffManager.slotValues[5] + enemyBattleBuffManager.GetStatsFromItemsPassives(EffectedStat.Spark);

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

        //oomph = currentMonster.stats[0].value + currentMonster.nature.addedStats[0].value;
        //guts = currentMonster.stats[1].value + currentMonster.nature.addedStats[1].value;
        //juice = currentMonster.stats[2].value + currentMonster.nature.addedStats[2].value;
        //edge = currentMonster.stats[3].value + currentMonster.nature.addedStats[3].value;
        //wits = currentMonster.stats[4].value + currentMonster.nature.addedStats[4].value;
        //spark = currentMonster.stats[5].value + currentMonster.nature.addedStats[5].value;


        //Debug.Log(friendlyMonster.name + pMons[0].name + pMons[1].name);
        //enemyIconRotator.SetMonsterRotator(currentMonster, backupMonsters); // HERE

        enemyAnim.runtimeAnimatorController = currentMonster.animator;
        enemyAnimVariant.runtimeAnimatorController = currentMonster.variant.variantAnimator;
        enemyDynamicSprite.color = currentMonster.colour.colour;
        enemyNameText.text = currentMonster.name;
        enemyLevelText.text = "Level " + currentMonster.level.ToString();

    }
    public void SetMonsterActive(int slot)
    {
        if (backupMonsters.Count <= 0) { return; }

        if (!tagReady[slot]) { return; }

        if (!GM.battleManager.isPlayingIntro)
        {
            aiController.SetActive(false);

            taggingSlot = slot;
            tagTimer = 0.3f;
            maskCutout.Play(tagTimer);
            //TriggerAction(TriggerType.tagOut);
            tagOn = true;
        }
        else
        {
            SpawnNewMonster(slot, true);
        }
        
    }

    public void ActivateAI(bool state)
    {
        //active = state;

        aiController.SetActive(state);
        transform.position = new Vector3(5f, -1f, 0f);
    }



    public void TakeDamage(int damage, bool effect, bool critical, int dotAmount, float dotTime, float stunnedTime, bool resetSpecial, float antiGravTime, bool echo, List<int> enemyStatBuffs, List<int> friendlyStatBuffs, Transform pos, FireProjectileEffectSO effectProjectile)
    {
        if (GM.battleManager.isLosing || GM.battleManager.isWinning) { return; }

        if (parryOn)
        {
            // NO damage but do guard effect
            if (perfectGuard == PerfectGuardEffects.CritAttack)
            {
                enemyBattleBuffManager.AddBuff(perfectValue, 9);
            }
            else if (perfectGuard == PerfectGuardEffects.TakingCrits)
            {
                enemyBattleBuffManager.AddBuff(perfectValue, 10);
            }
            else if (perfectGuard == PerfectGuardEffects.Stunned)
            {
                enemyBattleBuffManager.AddBuff(perfectValue, 7);
            }
            else if (perfectGuard == PerfectGuardEffects.Heal)
            {
                Heal((int)perfectValue);
            }
            else if (perfectGuard == PerfectGuardEffects.Oomph)
            {
                enemyBattleBuffManager.AddBuff(EffectedStat.Oomph, (int)perfectValue, perfectTargets);
            }
            else if (perfectGuard == PerfectGuardEffects.Guts)
            {
                enemyBattleBuffManager.AddBuff(EffectedStat.Guts, (int)perfectValue, perfectTargets);
            }
            else if (perfectGuard == PerfectGuardEffects.Juice)
            {
                enemyBattleBuffManager.AddBuff(EffectedStat.Juice, (int)perfectValue, perfectTargets);
            }
            else if (perfectGuard == PerfectGuardEffects.Edge)
            {
                enemyBattleBuffManager.AddBuff(EffectedStat.Edge, (int)perfectValue, perfectTargets);
            }
            else if (perfectGuard == PerfectGuardEffects.Wits)
            {
                enemyBattleBuffManager.AddBuff(EffectedStat.Wits, (int)perfectValue, perfectTargets);
            }
            else if (perfectGuard == PerfectGuardEffects.Spark)
            {
                enemyBattleBuffManager.AddBuff(EffectedStat.Spark, (int)perfectValue, perfectTargets);
            }
            else if (perfectGuard == PerfectGuardEffects.Projectile)
            {
                enemyMoveController.DoProjectile(perfectProjectile.projectilePrefab, perfectProjectile.projectileDamage, perfectProjectile.projectileSpeed, perfectProjectile.lifetime, perfectProjectile.collideWithAmountOfObjects, perfectProjectile.criticalProjectile, perfectProjectile);
            }

            Targets ts = new Targets(false, false);
            hitNumbers.SpawnPopup(PopupType.PerfectBlock, pos, "", 0); // perfectblock popup
            GuardOff();
        }    
        else if (effect || !guardOn  && !invulnerable)
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
            float gutsReal = 0f;

            float gutsAmount = 0f;
            float itemPassives = enemyBattleBuffManager.GetStatsFromItemsPassives(EffectedStat.Guts);
            float buffSlots = enemyBattleBuffManager.slotValues[1];
            gutsAmount = guts + itemPassives + buffSlots;
            


            if (gutsAmount > 100 && !effect)
            {
                gutsReal = 100f;

                float gutsNegateAmount = 0f;

                if (gutsAmount > 200)
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
                            enemyMoveController.DoProjectile(effectProjectile.projectilePrefab, effectProjectile.projectileDamage, effectProjectile.projectileSpeed, effectProjectile.lifetime, effectProjectile.collideWithAmountOfObjects, effectProjectile.criticalProjectile, effectProjectile);
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
                    enemyBattleBuffManager.AddBuff(dotAmount, dotTime);
                }

                if (stunnedTime > 0)
                {
                    enemyBattleBuffManager.AddBuff(stunnedTime, 3);
                }

                if (resetSpecial)
                {
                    GM.battleManager.friendlyMonsterController.specialReady[GM.battleManager.friendlyMonsterController.currentSlot] = true;
                    GM.battleManager.friendlyMonsterController.specialC[GM.battleManager.friendlyMonsterController.currentSlot] = 0f;
                }

                if (antiGravTime > 0)
                {
                    enemyBattleBuffManager.AddBuff(antiGravTime, 5);
                }

                if (echo)
                {
                    GM.battleManager.friendlyMonsterController.friendlyMoveController.DoProjectile(effectProjectile.projectilePrefab, effectProjectile.projectileDamage, effectProjectile.projectileSpeed, effectProjectile.lifetime, effectProjectile.collideWithAmountOfObjects, effectProjectile.criticalProjectile, effectProjectile);
                }

                for (int i = 0; i < enemyStatBuffs.Count; i++)
                {
                    Targets t = new Targets(false, false);

                    if (enemyStatBuffs[i] != 0)
                    {
                        switch (i)
                        {
                            case 0:
                                enemyBattleBuffManager.AddBuff(EffectedStat.Oomph, enemyStatBuffs[i], t);
                                break;
                            case 1:
                                enemyBattleBuffManager.AddBuff(EffectedStat.Guts, enemyStatBuffs[i], t);
                                break;
                            case 2:
                                enemyBattleBuffManager.AddBuff(EffectedStat.Juice, enemyStatBuffs[i], t);
                                break;
                            case 3:
                                enemyBattleBuffManager.AddBuff(EffectedStat.Edge, enemyStatBuffs[i], t);
                                break;
                            case 4:
                                enemyBattleBuffManager.AddBuff(EffectedStat.Wits, enemyStatBuffs[i], t);
                                break;
                            case 5:
                                enemyBattleBuffManager.AddBuff(EffectedStat.Spark, enemyStatBuffs[i], t);
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
                                GM.battleManager.friendlyMonsterController.friendlyBattleBuffManager.AddBuff(EffectedStat.Oomph, friendlyStatBuffs[i], t);
                                break;
                            case 1:
                                GM.battleManager.friendlyMonsterController.friendlyBattleBuffManager.AddBuff(EffectedStat.Guts, friendlyStatBuffs[i], t);
                                break;
                            case 2:
                                GM.battleManager.friendlyMonsterController.friendlyBattleBuffManager.AddBuff(EffectedStat.Juice, friendlyStatBuffs[i], t);
                                break;
                            case 3:
                                GM.battleManager.friendlyMonsterController.friendlyBattleBuffManager.AddBuff(EffectedStat.Edge, friendlyStatBuffs[i], t);
                                break;
                            case 4:
                                GM.battleManager.friendlyMonsterController.friendlyBattleBuffManager.AddBuff(EffectedStat.Wits, friendlyStatBuffs[i], t);
                                break;
                            case 5:
                                GM.battleManager.friendlyMonsterController.friendlyBattleBuffManager.AddBuff(EffectedStat.Spark, friendlyStatBuffs[i], t);
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

            //Debug.Log("Real Guts: " + gutsReal);
            //Debug.Log("True dmg: " + dmg);
            //Debug.Log("Protected dmg: " + protectedDamage);

            healthBar.SetHealth(enemyHealth - dmg, true);
            enemyHealth = healthBar.slider.value;

            enemyParentAnim.SetTrigger("Hit");
            


            if (enemyHealth <= 0)
            {
                GM.battleManager.GetXP();
                if (nodeType == NodeType.Survival)
                {
                    GM.battleManager.StartWinSurvival();
                }
                else
                {
                    GM.battleManager.StartWin();
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
                        GM.battleManager.cameraAnimator.SetInteger("Focus", 2);
                    }
                }

                
            }
        }
        else // STUPID LONG CODE FOR GUARD STUFF
        {
            Targets ts = new Targets(false, false);


            if (invulnerable)
            {
                hitNumbers.SpawnPopup(PopupType.Parry, pos, "", 0); // parry popup
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
        if (enemyHealth < 100)
        {
            int realAmount = amount;
            if (amount + enemyHealth > 100)
            {
                realAmount = (amount + (int)enemyHealth) - 100;
            }

            healthBar.SetHealth(enemyHealth + amount, false);
            enemyHealth = healthBar.slider.value;
            hitNumbers.SpawnPopup(PopupType.Heal, defaultHitNumbersLocation, realAmount.ToString(), 0); // heal popup
        }
    }

    public void Stun(bool state, float time)
    {
        if (state)
        {
            fMonsterController.TriggerAction(TriggerType.enemyStunned);

            stunned = true;
            ActivateAI(false);
            stunManager.Stun(time);
        }
        else
        {
            stunned = false;
            ActivateAI(true);
            stunManager.StopStun();
        }

    }

    public void Guard(PerfectGuardEffects perfectGuardEffect, float perfectGuardValue, FireProjectileEffectSO perfectGuardProjectile, Targets targs, float parryTime)
    {
        guardOn = true;
        guardRenderer.gameObject.SetActive(true);
        guardRenderer.sprite = yellowGuard;
        parryTimer = parryTime;
        parryOn = true;
        perfectGuard = perfectGuardEffect;
        perfectValue = perfectGuardValue;
        perfectProjectile = perfectGuardProjectile;
        perfectTargets = targs;
    }

    public void GuardOff()
    {
        //Debug.Log("false");
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


    public void Jump(float maxHold) // Jumps monster into air
    {
        if (!isGrounded) return;


        isJumping = true;
        jumpTime = maxHold;
        JumpRepeat();
    }

    private void JumpRepeat()
    {
        rb.velocity = Vector2.up * jumpForce;

        enemyAnim.SetBool("Jump", true);
        enemyAnimVariant.SetBool("Jump", true);
    }

    public void StopJump()
    {
        isJumping = false;


        enemyAnim.SetBool("Jump", false);
        enemyAnimVariant.SetBool("Jump", false);
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
        if (currentMonster.specialMove == null) return;

        if (currentMonster.specialMove.moveActions.Count > 0)
        {
            // use moves
            enemyMoveController.UseMove(currentMonster.specialMove);
        }


        enemyParentAnim.SetTrigger("Attack");
        enemyAnim.SetTrigger("Special");
        enemyAnimVariant.SetTrigger("Special");

        float valueAmount = 0f;
        float itemPassives = enemyBattleBuffManager.GetStatsFromItemsPassives(EffectedStat.Wits);
        float buffSlots = enemyBattleBuffManager.slotValues[4];

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

        float sCool = currentMonster.specialMove.baseCooldown - (currentMonster.specialMove.baseCooldown * (0.008f * valueReal));
        if (sCool < currentMonster.specialMove.minCooldown)
        {
            sCool = currentMonster.specialMove.minCooldown;
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
        if (currentMonster.basicMove == null) return;
        

        if (currentMonster.basicMove.moveActions.Count > 0)
        {
            enemyMoveController.UseMove(currentMonster.basicMove);
        }
        enemyParentAnim.SetTrigger("Attack");
        enemyAnim.SetTrigger("Basic");
        enemyAnimVariant.SetTrigger("Basic");

        float valueAmount = 0f;
        float itemPassives = enemyBattleBuffManager.GetStatsFromItemsPassives(EffectedStat.Edge);
        float buffSlots = enemyBattleBuffManager.slotValues[3];
        valueAmount = edge + itemPassives + buffSlots;

        //float valueAmount = edge + (edge * ((enemyBattleBuffManager.slotValues[3]) / 100));
        float valueReal = 0f;


        if (valueAmount > num)
        {
            valueReal = 100f;

        }
        else
        {
            valueReal = valueAmount - sizeNum;
        }


        float bCool = currentMonster.basicMove.baseCooldown - (currentMonster.basicMove.baseCooldown * (0.008f * valueReal));
        if (bCool < currentMonster.basicMove.minCooldown)
        {
            bCool = currentMonster.basicMove.minCooldown;
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

        if (enemyBattleBuffManager.slotValues[11] > 0)
        {
            int amount = enemyBattleBuffManager.slotValues[11];

            if (amount >= 100)
            {
                proj.GetComponent<Projectile>().Init(speed, dmg, lifeTime, collideWithAmountOfObjects, true, "Friend", GM.battleManager.friendlyMonsterController.friendlyBattleBuffManager, projEffect);
            }
            else
            {
                float random = Random.Range(1, 100);


                if (random <= amount)
                {
                    proj.GetComponent<Projectile>().Init(speed, dmg, lifeTime, collideWithAmountOfObjects, true, "Friend", GM.battleManager.friendlyMonsterController.friendlyBattleBuffManager, projEffect);
                }
                else
                {
                    proj.GetComponent<Projectile>().Init(speed, dmg, lifeTime, collideWithAmountOfObjects, criticalProjectile, "Friend", GM.battleManager.friendlyMonsterController.friendlyBattleBuffManager, projEffect);
                }
            }
        }
        else
        {
            proj.GetComponent<Projectile>().Init(speed, dmg, lifeTime, collideWithAmountOfObjects, criticalProjectile, "Friend", GM.battleManager.friendlyMonsterController.friendlyBattleBuffManager, projEffect);
        }


    }

    public void ClearCooldowns()
    {
        for (int i = 0; i < basicReady.Count; i++) { basicReady[i] = true; }
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
