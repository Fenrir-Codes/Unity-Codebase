using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class EnemyAIController : MonoBehaviour
{
    ZombieAnimationController enemyAnimationController;
    PauseMenu PauseMenu;
    EndGameMenu EndGameMenu;
    MoneyDisplayController MoneyCtrl;
    private NavMeshAgent agent;
    private AudioSource Audio;
    private float Screaminterval;
    private float Stepinterval;
    private int killCounter = 0;
    private float nextAttackTime = 1.4f;

    private Transform Player;
    [Header("Zombie maximum health")]
    public float maxHealt = 100f;
    private float Health = 0f;
    [Space]
    [Header("Speed of the zombies")]
    [Range(0, 50)] public float walkSpeed = 2.5f;
    [Range(0, 30)] public float turnSpeed = 10f;
    [Space]
    [Header("Distance from the player")]
    public float distance = 0.0f;
    [Header("Zombies damage to player")]
    [Range(0, 100)] public float DamageToPlayer = 10f;
    [Space]
    [Header("The amount of money the dead zombi gives on hit")]
    [Tooltip("Non lethal hit amount")]
    public int NonLethalHit = 10;
    [Tooltip("Lethal hit amount")]
    public int LethalHit = 50;
    [Space]
    [Header(" --- Booleans --- ")]
    public bool isMoving;
    public bool isStopped;
    public bool canAttack;
    public bool Attacking;
    public bool isDead = false;
    [Space]
    [Range(0, 3)] public float setStepInterval = .8f;
    [Header("Zombie scream audio clip")]
    public AudioClip ZombieHit;
    public AudioClip ZombieFootSteps;
    public AudioClip[] screamWhileMoving;
    public AudioClip[] Screams;
    [Space]
    [Header("Randomizing int for death animation")]
    public int Deathanim = 0;


    // Start is called before the first frame update
    void Start()
    {
        InitializeComponents(); //Initialize the required components
        screamAtSpawn(); // play the sream once on spawning the enemy in the scene
    }

    // Update is called once per frame
    void Update()
    {
        Screaminterval += Time.deltaTime;
        Stepinterval += Time.deltaTime;

        if (agent != null && Player != null)
        {
            distance = calculateDistance();
            //Debug.Log("Distance : " + distance);

            if (distance > 2f && !isDead)
            {
                moveToTarget();
            }
            if (distance <= 2f || isDead)
            {
                stopEnemy();
            }
            if (!isDead && canAttack)
            {
                StartCoroutine(Attack());    
            }
            if (isDead)
            {
                canAttack = false;
                isMoving = false;
                Attacking = false;
                agent.isStopped = true;
                Audio.Stop();
            }
        }
        else
        {
            Debug.Log("NavMeshAgent or Player transform not found!");
        }
    }

    #region Attack Co-Routine
    IEnumerator Attack()
    {
        if (canAttack && !isDead && !PauseMenu.isPaused && !EndGameMenu.isGameOver)
        {
            isMoving = false;
            agent.speed = 0.0f;
            agent.isStopped = true;

            Vector3 Direction = Player.localPosition - transform.localPosition;
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(Direction), turnSpeed * Time.deltaTime);

            PlayerHealthManager player = GameObject.Find("Player").GetComponent<PlayerHealthManager>();

            canAttack = false;
            yield return new WaitForSeconds(1f);
            if (enemyAnimationController.animator.GetCurrentAnimatorStateInfo(0).IsName("Zombie_Punching"))
            {
                Attacking = true;
                hitOnAttack_play();
                player.TakeDamage(DamageToPlayer);
            }
            yield return new WaitForSeconds(nextAttackTime);
            canAttack = true;
            Attacking = false;
        }

    }
    #endregion

    #region Take damage
    public void takeDamage(float amount)
    {
        Health -= amount;

        if (amount < 100f)
        {
            giveMoneyOnNonLethatHit();
        }
        if (amount == 100f)
        {
            isDead = true;
            EndGameMenu.HeadShotCounter += 1;
        }
        if (Health <= 0f)
        {
            Deathanim = Random.Range(0, 10);
            isDead = true;
            gameObject.tag = "DeadEnemy";
            disableCollidersOnDeath();
            giveMoneyOnDeath();
            countkills();
            Destroy(gameObject, 1.5f);
        }
    }
    #endregion

    #region footsteps effect
    void playFotsteps()
    {
        //play the aufio file while moving the enemy if not paused, not game over and enemy is alive
        if (!Audio.isPlaying && !isDead && !PauseMenu.isPaused && !EndGameMenu.isGameOver)
        {
            if (Stepinterval >= setStepInterval)
            {
                Audio.PlayOneShot(ZombieFootSteps);
                Stepinterval = 0f;
            }
        }
    }
    #endregion

    #region Moving the zombie towards Player and playing the screams while moving every 5 sec
    private void moveToTarget()
    {
        //Setting booleans
        agent.isStopped = false;
        isMoving = true;
        isStopped = false;
        //setting the speed and destination
        agent.speed = walkSpeed;
        agent.SetDestination(Player.localPosition);
        //play the audio's
        playFotsteps();
        playScreamOnMoving();

    }
    #endregion

    #region stopping the zombie
    private void stopEnemy()
    {
        //setting the booleans
        isMoving = false;
        isStopped = true;
        //setting the speed to 0,  and agent isStopped to true
        agent.speed = 0.0f;
        agent.isStopped = true;
    }
    #endregion

    #region Play scream at the spawn
    void screamAtSpawn()
    {
        // setting a random range for the audio files played on spawning the enemy
        var OnSpawn = Audio.clip = Screams[Random.Range(0, Screams.Length)];
        Audio.PlayOneShot(OnSpawn);
    }
    #endregion

    #region play scream while moving 
    void playScreamOnMoving()
    {
        //randomize the audio clip in the variable randomScream
        var randomScream = Audio.clip = screamWhileMoving[Random.Range(0, screamWhileMoving.Length)];
        //if the audio is not playing, not paused, not even game over and the intervar reaches 5 sec then play one of the audio
        if (!Audio.isPlaying && !PauseMenu.isPaused && !EndGameMenu.isGameOver && Screaminterval >= 5f)
        {
            Audio.PlayOneShot(randomScream); // play the file
            Screaminterval = 0.0f; //set the interval to zero again
        }
    }
    #endregion

    #region play hit effect on attacking the player
    void hitOnAttack_play()
    {
        if (!Audio.isPlaying && !PauseMenu.isPaused && !EndGameMenu.isGameOver)
        {
            Audio.PlayOneShot(ZombieHit);
        }
    }
    #endregion

    #region Disabling all the colliders on death 
    void disableCollidersOnDeath()
    {
        agent.isStopped = true;
        agent.speed = 0.0f;

        Collider[] colliders = transform.GetComponentsInChildren<Collider>();
        foreach (Collider col in colliders)
        {
            col.enabled = false;
        }
    }
    #endregion

    #region Give the money amount on Lethal hit
    void giveMoneyOnDeath()
    {
        MoneyCtrl.GiveMoney(LethalHit);
    }
    #endregion

    #region Give the money amount on non lethat hit
    private void giveMoneyOnNonLethatHit()
    {
        MoneyCtrl.GiveMoney(NonLethalHit);
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

    #region calculate distance from Player
    private float calculateDistance()
    {
        // returning the distance in float variable from the transform to player position
        return Vector3.Distance(transform.position, Player.localPosition);
    }
    #endregion

    #region Instantiate Components
    private void InitializeComponents()
    {
        Player = GameObject.Find("Player").transform;
        enemyAnimationController = GetComponent<ZombieAnimationController>();
        PauseMenu = GameObject.FindGameObjectWithTag("PauseMenu").GetComponentInChildren<PauseMenu>();
        EndGameMenu = GameObject.FindGameObjectWithTag("EndGameMenu").GetComponentInChildren<EndGameMenu>();
        MoneyCtrl = GameObject.FindGameObjectWithTag("MoneyDisplay").GetComponent<MoneyDisplayController>();

        Audio = GetComponent<AudioSource>();
        agent = GetComponent<NavMeshAgent>();
        Health = maxHealt;
        canAttack = true;
    }
    #endregion


}
