using UnityEngine;

public class Enemy : MonoBehaviour
{
    // Gegnerlevel der mit Slider eingestellt werden kann
    [Range(1, 3)] public int tier = 1;

    struct TierStats
    {
        public int maxHealth, contactDamage;
        public float patrolSpeed, patrolDistance, chaseSpeed, detectRange, damageCooldown;

        public TierStats(int hp, int dmg, float patrol, float dist, float chase, float range, float cd)
        {
            maxHealth = hp; contactDamage = dmg; patrolSpeed = patrol; patrolDistance = dist;
            chaseSpeed = chase; detectRange = range; damageCooldown = cd;
        }
    }
    
    //HP: Lepenspunkte des gegners
    //dmg: Schaden den der Gegner pro Hit macht
    //patrol: geschwindigkeit der hin- und her- "tracking" bewegung
    //dist: Entfernung die der Gegner von seinem Spawn weglaufen kann bevor er sich unmdreht
    //chase: Verfolgungsgeschwindigkeit nachdem der Gegner gesichtet wird
    //range: sichweite des Gegners für den Spieler
    //cd: abklingzeit für die Schläge eines Spielers
    
    static readonly TierStats[] tierTable = {
        new TierStats(20,  5, 2f, 1f, 1f,   0f,  1.5f),
        new TierStats(50, 13, 3f,   1.5f, 3f,   5f,  1f),
        new TierStats(90, 20, 4f, 2f, 5f,  10f,  0.5f),
    };

    // Chasespeed 0 heißt dass der Gegner einen nicht verfolgen kann d.h. statisch ist.
    TierStats stats;
    bool CanChase => stats.chaseSpeed > 0f;

    int health;
    // Merkt sich, mit welchem Tier stats zuletzt gefuellt wurde, damit eine
    // Slider-Aenderung im Play Mode auffaellt.
    int appliedTier = -1;
    float startX;
    int dir = 1;
    float nextHitTime;
    Transform player;
    SpriteRenderer sprite;
    EnemyHealthBar healthBar;

    // Optional im Inspector setzbar; sonst wird er in Start in der Szene gesucht.
    [SerializeField] CombatManager combatManager;

    // Fuer die Testbuttons im Inspector.
    public int CurrentHealth => health;
    public int MaxHealth => stats.maxHealth;

    // Awake, damit die Leiste noch nicht als Kind existiert wenn gesucht wird.
    void Awake()
    {
        sprite = GetComponentInChildren<SpriteRenderer>();

        // Schon hier, nicht erst in Start: sonst sind stats und health noch leer, wenn
        // ein anderes Skript in seinem eigenen Start bereits Schaden macht.
        ApplyTier();
    }

    void Start()
    {
        startX = transform.position.x;

        healthBar = GetComponent<EnemyHealthBar>();
        if (healthBar == null)
            healthBar = gameObject.AddComponent<EnemyHealthBar>();

        // Die Leiste kannte den Startwert bei ApplyTier noch nicht.
        SetHealth(health);

        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
        if (playerObject != null)
            player = playerObject.transform;

        if (combatManager == null)
            combatManager = FindFirstObjectByType<CombatManager>();
    }

    // Uebernimmt die Werte des eingestellten Tiers und setzt die HP auf voll.
    void ApplyTier()
    {
        // Auch zurueckschreiben: sonst bleibt ein per Skript gesetztes tier ausserhalb
        // 1..5 dauerhaft ungleich appliedTier und ApplyTier laeuft in jedem Frame.
        tier = Mathf.Clamp(tier, 1, tierTable.Length);
        appliedTier = tier;
        stats = tierTable[appliedTier - 1];
        health = stats.maxHealth;

        if (healthBar != null)
            healthBar.SetPercent(1f);
    }

    void Update()
    {
        // Slider im Play Mode gedreht: Werte sofort neu uebernehmen.
        if (tier != appliedTier)
            ApplyTier();

        // noch kein Spieler, nur eine patrol
        // Voller Abstand statt nur X: sonst sieht der Gegner den Spieler auch durch
        // eine Decke oder einen Boden hindurch, solange er senkrecht darueber steht.
        if (player != null && CanChase && InDetectRange())
        {
            dir = player.position.x > transform.position.x ? 1 : -1;
            Move(stats.chaseSpeed);
        }
        else
        {
            Move(stats.patrolSpeed);

            // Am Rand des Reviers festsetzen und im selben Schritt umdrehen. Getrennt
            // wuerde sich beides blockieren: geklemmt wird der Abstand nie groesser
            // als patrolDistance, also wuerde eine eigene Umkehr-Abfrage nie ausloesen.
            Vector3 p = transform.position;

            if (p.x - startX >= stats.patrolDistance)
            {
                p.x = startX + stats.patrolDistance;
                dir = -1;
            }
            else if (p.x - startX <= -stats.patrolDistance)
            {
                p.x = startX - stats.patrolDistance;
                dir = 1;
            }

            transform.position = p;
        }
    }

    bool InDetectRange()
    {
        Vector2 delta = player.position - transform.position;
        return delta.sqrMagnitude < stats.detectRange * stats.detectRange;
    }

    void Move(float speed)
    {
        // Space.World: sonst dreht ein negativer Scale oder eine Rotation die Richtung um.
        transform.Translate(Vector2.right * dir * speed * Time.deltaTime, Space.World);

        // Nur das Sprite spiegeln, nicht das ganze Objekt: sonst kippt die Leiste mit.
        if (sprite != null)
            sprite.flipX = dir < 0;
    }

    // Feind fügt Spieler Schaden hinzu
    void OnCollisionStay2D(Collision2D other)
    {
        if (Time.time < nextHitTime) return;
        if (!other.gameObject.CompareTag("Player")) return;

        // Die Spieler-HP liegen zentral im CombatManager, nicht auf dem Spielerobjekt:
        // beide Tierformen teilen sich denselben Lebensbalken.
        if (combatManager == null) return;

        combatManager.SubtractHp(stats.contactDamage);
        nextHitTime = Time.time + stats.damageCooldown;
    }

    // Spieler Angriff damage
    public void TakeDamage(int amount)
    {
        SetHealth(health - amount);

        if (health <= 0)
            Destroy(gameObject);
    }

    public void SetHealth(int value)
    {
        // stats ist bis zum ersten ApplyTier() leer. Ohne den Guard waere maxHealth 0
        // und der Prozentwert NaN, was die Leiste still kaputt skaliert.
        if (stats.maxHealth <= 0)
            return;

        health = Mathf.Clamp(value, 0, stats.maxHealth);

        if (healthBar != null)
            healthBar.SetPercent((float)health / stats.maxHealth);
    }
}