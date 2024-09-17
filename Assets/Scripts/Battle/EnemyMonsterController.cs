using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class EnemyMonsterController : MonoBehaviour
{
    public float enemyHealth = 1000;

    [Header("References")]
    public GameManager GM;
    public MonsterAIController aiController;
    public BattleHealthBar healthBar;
    public BattleCapBar capBar;
    public Animator enemyAnim;
    public Animator enemyAnimVariant;
    public Animator enemyParentAnim;
    public Animator healthBarShakeAnim;
    public TextMeshProUGUI regenText;
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
    public ItemController enemyItemController;
    public PassiveController enemyPassiveController;

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


    public bool regenOn = false;
    private float regenTimer = 0f;
    private float regenBaseTime = 1f;
    private float regenMulti = 0.25f;

    [HideInInspector] public List<GameObject> projectiles = new List<GameObject>();

    //private bool tagOn = false;
    //private float tagTimer = 0f;
    //private int taggingSlot = 0;

    //public MaskCutout maskCutout;

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

    private List<MonsterItemSO> customPunkItems = new List<MonsterItemSO>();
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

        if (regenOn)
        {
            if (regenTimer > 0)
            {
                regenTimer -= Time.deltaTime;
            }
            else if (regenTimer <= 0)
            {
                float juiceAmount = juice + enemyBattleBuffManager.slotValues[2] + enemyBattleBuffManager.GetStatsFromItemsPassives(EffectedStat.Juice);

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
                    healthBar.SetHealth(enemyHealth + flooredRegen, false);
                    enemyHealth = healthBar.slider.value;
                }

                //capBar.SetCaptureLevel(GM.battleManager.enemyCapturePoints, enemyHealth);

                regenTimer = regenBaseTime;
            }
        }
        /*

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
        */

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
                    healthBar.ChangeColor("Normal");
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

            enemyBattleBuffManager.AddBuff(EffectedStat.Oomph, extraStats * 2, targ);
            enemyBattleBuffManager.AddBuff(EffectedStat.Guts, extraStats * 2, targ);
            enemyBattleBuffManager.AddBuff(EffectedStat.Juice, extraStats * 2, targ);
            enemyBattleBuffManager.AddBuff(EffectedStat.Edge, extraStats * 2, targ);
            enemyBattleBuffManager.AddBuff(EffectedStat.Wits, extraStats * 2, targ);
            enemyBattleBuffManager.AddBuff(EffectedStat.Spark, extraStats * 2, targ);
        }

        enemyHealth = 1000;
        healthBar.SetMaxHealth(1000);
        healthBar.SetHealth(1000, false);


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
        customPunkItems = null;
        customPunkItems = new List<MonsterItemSO>();

    }

    public void SetupEnemyPunk(List<Monster> mons, NodeType type, int extraStats, int punkMaxHealth, List<MonsterItemSO> customItems)
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

        customPunkItems = customItems;

    }

    public void SetupEnemyBondBattle(Monster mon)
    {
        backupMonsters.Add(mon);
        GM.AddBeastToSeenIDs(mon.backupData);
        SetMonsterActive(0);
        aiController.SetBattleType(false);
        //nodeType = type;
        enemyHealth = 1000;
        healthBar.SetMaxHealth(1000);
        healthBar.SetHealth(1000, false);

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
        customPunkItems = null;
        customPunkItems = new List<MonsterItemSO>();

        aiController.SetActive(false);
    }
    private void SpawnNewMonster(int slot, bool isStart)
    {
        float cooldown = 7f;

        //currentMonster = backupMonsters[0];

        if (!isStart)
        {
            float sparkAmount = spark + enemyBattleBuffManager.slotValues[5] + enemyBattleBuffManager.GetStatsFromItemsPassives(EffectedStat.Spark);
            
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


        if (currentMonster.backupData.isHeavyJumper)
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
        for (int i = 0; i < backupMonsters.Count; i++)
        {
            if (backupMonsters[i].passiveMove.shared)
            {
                if (!passivesShared.Contains(backupMonsters[i].passiveMove))
                {
                    passivesShared.Add(backupMonsters[i].passiveMove);
                }
            }
        }

        List<MonsterItemSO> itemsEnemy = new List<MonsterItemSO>();
        itemsEnemy.Add(currentMonster.item1);
        itemsEnemy.Add(currentMonster.item2);
        itemsEnemy.Add(currentMonster.item3);

        if (customPunkItems.Count > 0)
        {
            for (int i = 0; i < customPunkItems.Count; i++)
            {
                itemsEnemy.Add(customPunkItems[i]);
            }
        }



        enemyPassiveController.StartPassives(currentMonster.passiveMove, passivesShared);
        enemyItemController.StartItems(itemsEnemy, currentSlot);

        if (!st)
        {
            TriggerAction(TriggerType.tagIn);
        }
    }

    public void SetMonsterActive(int slot)
    {
        if (backupMonsters.Count <= 0) { return; }

        if (!tagReady[slot]) { return; }

        if (!GM.battleManager.isPlayingIntro)
        {

            invulnerable = true;
            invTime = 0.2f;
            TriggerAction(TriggerType.tagOut);
            SpawnNewMonster(slot, false);
            /*
            aiController.SetActive(false);
            taggingSlot = slot;
            tagTimer = 0.3f;
            maskCutout.Play(tagTimer);
            tagOn = true;
            */
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



    public void TakeDamage(int damage, int baseDamage, bool effect, bool critical, bool echo,Transform pos, FireProjectileEffectSO effectProjectile, List<EffectSO> effectsToTriggerOnFriendly, List<EffectSO> effectsToTriggerOnEnemy)
    {
        if (GM.battleManager.isLosing || GM.battleManager.isWinning) { return; }

        if (!effect)
        {
            TriggerAction(TriggerType.beingHit);
        }

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
            //GuardOff();
        }    
        else if (effect || !guardOn  && !invulnerable)
        {
            regenMulti = 0.25f;
            regenTimer = regenBaseTime;
            regenText.text = "HPs x " + regenMulti.ToString("F2");


            //Debug.Log(damage);
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
            float itemPassives = enemyBattleBuffManager.GetStatsFromItemsPassives(EffectedStat.Guts);
            float buffSlots = enemyBattleBuffManager.slotValues[1];
            float gutsAmount = guts + itemPassives + buffSlots;
            

            if (!effect)
            {
                for (int i = 0; i < effectsToTriggerOnFriendly.Count; i++) // Trigger these on friendlyMonsterController
                {
                    Targets tar = new Targets(false, true);
                    enemyMoveController.UseEffect(effectsToTriggerOnFriendly[i], tar);
                }

                for (int i = 0; i < effectsToTriggerOnEnemy.Count; i++) // Trigger these on the enemyMonsterController (this)
                {
                    Targets tar = new Targets(true, false);
                    enemyMoveController.UseEffect(effectsToTriggerOnEnemy[i], tar);
                }

            }
            float baseDmgFloat = baseDamage;

            float trueDamage = floatDamage - (gutsAmount * 0.04f * baseDmgFloat);

            if (trueDamage < 1 && baseDamage > 0)
            {
                trueDamage = 1f;
            }
            //Debug.Log(trueDamage);
            //Debug.Log(floatDamage);

            int dmg = Mathf.RoundToInt(trueDamage);
            int protectedDamage = dmg - Mathf.RoundToInt(floatDamage);

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

            //GuardOff();
        }
    }

    public void SetHealth(int amount)
    {
        healthBar.SetHealth(amount, false);
        enemyHealth = healthBar.slider.value;
    }

    public void Heal(int amount)
    {
        if (enemyHealth < 1000)
        {
            int realAmount = amount;
            if (amount + enemyHealth > 1000)
            {
                realAmount = (amount + (int)enemyHealth) - 1000;
            }

            healthBar.SetHealth(enemyHealth + realAmount, false);
            enemyHealth = healthBar.slider.value;
            hitNumbers.SpawnPopup(PopupType.Heal, defaultHitNumbersLocation, realAmount.ToString(), 0); // heal popup
        }
        else
        {
            hitNumbers.SpawnPopup(PopupType.Heal, defaultHitNumbersLocation, "0", 0); // heal popup
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
        if (currentMonster.specialMove == null) return;

        if (currentMonster.specialMove.moveActions.Count > 0)
        {
            // use moves
            enemyMoveController.UseMove(currentMonster.specialMove);
        }


        enemyParentAnim.SetTrigger("Attack");
        enemyAnim.SetTrigger("Special");
        enemyAnimVariant.SetTrigger("Special");

        TriggerAction(TriggerType.useSpecial);

        float witsAmount = wits + enemyBattleBuffManager.slotValues[4] + enemyBattleBuffManager.GetStatsFromItemsPassives(EffectedStat.Wits);

        float baseCD = currentMonster.specialMove.baseCooldown;
        float sCool = 0f;
        if (witsAmount < 0 )
        {
            sCool = baseCD - (baseCD * (0.008f * witsAmount));
        }
        else
        {
            sCool = 1f / ((1f / baseCD) * witsAmount * 0.04f + (1f / baseCD));
        }

        if (sCool < currentMonster.specialMove.minCooldown)
        {
            sCool = currentMonster.specialMove.minCooldown;
        }

        specialC[currentSlot] = sCool;
        specialReady[currentSlot] = false;


    }
    private void DoAttack()
    {
        if (currentMonster.basicMove == null) return;
        

        if (currentMonster.basicMove.moveActions.Count > 0)
        {
            enemyMoveController.UseMove(currentMonster.basicMove);
        }
        enemyParentAnim.SetTrigger("Attack");
        enemyAnim.SetTrigger("Basic");
        enemyAnimVariant.SetTrigger("Basic");

        TriggerAction(TriggerType.useBasic);

        float edgeAmount = edge + enemyBattleBuffManager.slotValues[3] + enemyBattleBuffManager.GetStatsFromItemsPassives(EffectedStat.Edge);


        float baseCD = currentMonster.basicMove.baseCooldown;
        float bCool = 0f;

        if (edgeAmount < 0)
        {
            bCool = baseCD - (baseCD * (0.008f * edgeAmount));
        }
        else
        {
            bCool = 1f / ((1f / baseCD) * edgeAmount * 0.04f + (1f / baseCD));
        }


        if (bCool < currentMonster.basicMove.minCooldown)
        {
            bCool = currentMonster.basicMove.minCooldown;
        }

        basicC[currentSlot] = bCool;
        basicReady[currentSlot] = false;




    }

    public void FireProjectile(GameObject prefab, float speed, int dmg, int baseDmg, float lifeTime, int collideWithAmountOfObjects, bool criticalProjectile, FireProjectileEffectSO projEffect)
    {
        GameObject proj = Instantiate(prefab, firePoint.position, firePoint.rotation);
        projectiles.Add(proj);

        if (enemyBattleBuffManager.slotValues[11] > 0)
        {
            int amount = enemyBattleBuffManager.slotValues[11];

            if (amount >= 1000)
            {
                proj.GetComponent<Projectile>().Init(speed, dmg, baseDmg, lifeTime, collideWithAmountOfObjects, true, "Friend", GM.battleManager.friendlyMonsterController.friendlyBattleBuffManager, projEffect);
            }
            else
            {
                float random = Random.Range(1, 1000);


                if (random <= amount)
                {
                    proj.GetComponent<Projectile>().Init(speed, dmg, baseDmg, lifeTime, collideWithAmountOfObjects, true, "Friend", GM.battleManager.friendlyMonsterController.friendlyBattleBuffManager, projEffect);
                }
                else
                {
                    proj.GetComponent<Projectile>().Init(speed, dmg, baseDmg, lifeTime, collideWithAmountOfObjects, criticalProjectile, "Friend", GM.battleManager.friendlyMonsterController.friendlyBattleBuffManager, projEffect);
                }
            }
        }
        else
        {
            proj.GetComponent<Projectile>().Init(speed, dmg, baseDmg, lifeTime, collideWithAmountOfObjects, criticalProjectile, "Friend", GM.battleManager.friendlyMonsterController.friendlyBattleBuffManager, projEffect);
        }


    }

    public void TriggerAction(TriggerType triggerType)
    {
        //Debug.Log("Trigger: " + triggerType);
        enemyPassiveController.PassiveTrigger(triggerType);
        enemyItemController.ItemTrigger(triggerType);
    }

    public void ClearCooldowns()
    {
        for (int i = 0; i < basicReady.Count; i++) { basicReady[i] = true; }
        for (int i = 0; i < specialReady.Count; i++) { specialReady[i] = true; }
        for (int i = 0; i < tagReady.Count; i++) { tagReady[i] = true; }
        for (int i = 0; i < basicC.Count; i++) { basicC[i] = 0; }
        for (int i = 0; i < specialC.Count; i++) { specialC[i] = 0; }
        for (int i = 0; i < tagC.Count; i++) { tagC[i] = 0; }
    }
}
