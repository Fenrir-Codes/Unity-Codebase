using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class EnemyAIController : MonoBehaviour
{
    [Header("Settings")]
    public float maxHealt = 100f;
    public float DamageToPlayer = 10f;
    public float Health = 0f;
    private float nextAttackTime = 1.4f;
    public int moneyOnDeath;
    public int killCounter = 0;
    [Space]
    [Header("Zombie scream audio clip")]
    public AudioClip[] attackScreams;
    public AudioClip[] screams;
    public AudioClip HitEffect;
    public AudioClip footStepsEffect;
    [Space]
    [Header("Speed of the zombies")]
    [Range(0, 100)] public float speed;
    [Range(0, 10)] public float turnSpeed = 10f;
    [Space]
    public int Deathanim = 0;
    [Space]
    Collider Collider;
    private NavMeshAgent Agent = null;
    private Transform Player;
    private AudioSource Audio;
    [Space]
    private float DistanceFromPlayer = 0f;
    private float Screaminterval;
    public bool isMoving;
    public bool isDead = false;
    public bool isAttacking = false;
    public bool canAttack;

    ZombieAnimationController animationController;
    PauseMenu PauseMenu;
    EndGameMenu EndGameMenu;
    MoneyDisplayController MoneyCtrl;

    // Start is called before the first frame update
    void Start()
    {
        InstantiatePrerequisites();
        playScreamOnce();

    }

    #region Instantiate prerequisites
    private void InstantiatePrerequisites()
    {
        animationController = GetComponent<ZombieAnimationController>();
        PauseMenu = GameObject.FindGameObjectWithTag("PauseMenu").GetComponentInChildren<PauseMenu>();
        EndGameMenu = GameObject.FindGameObjectWithTag("EndGameMenu").GetComponentInChildren<EndGameMenu>();
        MoneyCtrl = GameObject.FindGameObjectWithTag("MoneyDisplay").GetComponent<MoneyDisplayController>();

        canAttack = true;
        Collider = GetComponent<Collider>();
        Audio = GetComponent<AudioSource>();
        Agent = GetComponent<NavMeshAgent>();
        Health = maxHealt;
    }
    #endregion
    
    #region Update
    // Update is called once per frame
    void Update()
    {
        Screaminterval += Time.deltaTime;
        moneyOnDeath = Random.Range(30, 65);

        if (Agent != null)
        {
            MoveToTarget();

            DistanceFromPlayer = Vector3.Distance(transform.position, Player.position);

            if (DistanceFromPlayer > 4f && !isDead && Screaminterval >= 8f)
            {
                playScreamOnMoving();
            }
            if (DistanceFromPlayer <= 2.4f && !isDead && canAttack)
            {
                isMoving = false;
                Agent.isStopped = true;
                Agent.speed = 0f;
                AttackPlayer();
            }
            if (DistanceFromPlayer > 2.4f)
            {
                isMoving = true;
                Agent.isStopped = false;
                Agent.speed = speed;
                playFotstepEffect();
            }
            if (isDead)
            {
                canAttack = false;
                Agent.isStopped = true;
                Audio.Stop();
            }

        }
    }
    #endregion

    #region Move to target
    void MoveToTarget()
    {
        Player = GameObject.FindGameObjectWithTag("Player").transform;
        if (Player == null) 
        {
            Debug.Log("No Player character found!");
            Destroy(gameObject);
        }
        else
        {
            Agent.speed = speed;
            Agent.SetDestination(Player.position);
        }

    }
    #endregion

    #region footsteps effect audio
    void playFotstepEffect()
    {
        if (!Audio.isPlaying && !isDead && !PauseMenu.isPaused)
        {
            Audio.PlayOneShot(footStepsEffect);
        }
    }
    #endregion

    #region Play screan once
    void playScreamOnce()
    {
        var OnAwake = Audio.clip = screams[Random.Range(0, screams.Length)];
        Audio.PlayOneShot(OnAwake);
    }
    #endregion

    #region play  scream while moving 
    void playScreamOnMoving()
    {
        var randomAttackclip = Audio.clip = attackScreams[Random.Range(0, attackScreams.Length)];
        if (!Audio.isPlaying&& !PauseMenu.isPaused)
        {
            Audio.PlayOneShot(randomAttackclip);
            Screaminterval = 0.0f;
        }
    }
    #endregion

    #region play effect on attack (scream)
    void playAttackScreamOnAttack()
    {
        var randomAttackclip = Audio.clip = attackScreams[Random.Range(0, attackScreams.Length)];
        if (!Audio.isPlaying && !PauseMenu.isPaused)
        {
            Audio.PlayOneShot(randomAttackclip);

        }
    }
    #endregion

    #region play hit effect
    void playHitEffect()
    {
        if (!Audio.isPlaying)
        {
            Audio.PlayOneShot(HitEffect);
        }
    }
    #endregion

    #region Take damage script
    public void takeDamage(float amount)
    {
       // Debug.Log("Damage taken: " + amount);
        Health -= amount;
        if (amount == 100f)
        {
            EndGameMenu.HeadShotCounter += 1;
        }

        if (Health <= 0f)
        {
            isDead = true;
            gameObject.tag = "DeadEnemy";
            //disable nav agent, colliders, set tag to DeadEnemy
            disableOnDeath();
            // give money on zombie kill
            giveMoneyOnDeath();
            //Random number for play the one of the two animations on death
            Deathanim = Random.Range(0, 10);
            //killcount if tag switched to DeadEnemy
            countkills();
            //destroy zombie object
            Destroy(gameObject, 1.5f);
        }
    }
    #endregion

    #region Attack Player
    void AttackPlayer()
    {
        StartCoroutine(Attack());
    }

    #endregion

    #region Attack Ienumerator
    IEnumerator Attack()
    {
        if (canAttack && !PauseMenu.isPaused)
        {
            isMoving = false;
            Agent.isStopped = true;

            Vector3 Direction = Player.position - transform.position;
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(Direction), turnSpeed * Time.deltaTime);

            PlayerHealthManager player = GameObject.Find("Player").GetComponent<PlayerHealthManager>();

            canAttack = false;
            yield return new WaitForSeconds(1f);
            if (animationController.animator.GetCurrentAnimatorStateInfo(0).IsName("Zombie_Punching"))
            {
                isAttacking = true;
                playHitEffect();
                player.TakeDamage(DamageToPlayer);
            }
            yield return new WaitForSeconds(nextAttackTime);
            canAttack = true;
            isAttacking = false;
        }

    }
    #endregion

    #region disableColliderOnDeath
    void disableOnDeath()
    {
        isMoving = false;
        Collider[] colliders = transform.GetComponentsInChildren<Collider>();
        foreach (Collider col in colliders)
        {
            col.enabled = false;
        }
    }
    #endregion

    #region money on death to player
    void giveMoneyOnDeath()
    {
        MoneyCtrl.GiveMoney(moneyOnDeath);
    }
    #endregion

    #region killcount on tag Deadenemy
    void countkills()
    {
        if (gameObject.CompareTag("DeadEnemy"))
        {
            killCounter++;
            EndGameMenu.killCounter += killCounter;
        }
    }
    #endregion
}
