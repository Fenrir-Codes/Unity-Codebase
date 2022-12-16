using UnityEngine;
using UnityEngine.AI;

public class ZombieAnimationController : MonoBehaviour
{
    private string currentAnimaton;
    EnemyAIController zombie;
    public Animator animator;

    const string Zombie_Walk = "zombie_chasewalk";
    const string Zombie_Run = "Zombie_Run";
    const string Zombie_Punching = "Zombie_Punching";
    const string Zombie_Dead = "Z_FallingBack";
    const string Zombie_Dead2 = "Z_FallingForward";



    // Start is called before the first frame update
    void Start()
    {
        zombie = GetComponent<EnemyAIController>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (zombie.isMoving && !zombie.isDead)
        {
            ChangeAnimationState(Zombie_Walk);
        }
        //else if (!zombie.isMoving && zombie.hyperRun && !zombie.isDead)
        //{
        //    ChangeAnimationState(Zombie_Run);
        //}
        else if (zombie.isDead)
        {
            if (zombie.Deathanim >= 0 && zombie.Deathanim <= 5)
            {
                ChangeAnimationState(Zombie_Dead);
            }
            else
            {
                ChangeAnimationState(Zombie_Dead2);
            }
        }
        else if (!zombie.isMoving && !zombie.isDead)
        {
            ChangeAnimationState(Zombie_Punching);

        }
        else
        {
            ChangeAnimationState(Zombie_Walk);
        }

    }

    private void ChangeAnimationState(string newAnimation)
    {
        if (currentAnimaton == newAnimation) return;

        animator.Play(newAnimation);
        currentAnimaton = newAnimation;
    }
}
