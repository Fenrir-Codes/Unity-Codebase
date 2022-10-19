using UnityEngine;

public class ExplosiveBarrel : MonoBehaviour
{
    public GameObject Barrel;
    public GameObject Explosion;
    public float explosionDamage = 110f;
    private Collider Collider;
    [SerializeField] private float ExplosionRange = 6f;

    AudioSource can_Explosion;

    // Start is called before the first frame update
    void Start()
    {
        InitializeBarrel();
    }

    #region Initialize
    private void InitializeBarrel()
    {
        Barrel.SetActive(true);
        Explosion.SetActive(false);
        Collider = GetComponent<Collider>();
        Collider.enabled = true;
        can_Explosion = GetComponent<AudioSource>();
    }
    #endregion

    #region Explosion function
    public void Explode()
    {
        Barrel.SetActive(false);
        Collider.enabled = false;
        if (!can_Explosion.isPlaying)
        {
            can_Explosion.Play();
            Explosion.SetActive(true);
        }

        Collider[] enemies = GetComponentsInChildren<Collider>();

        if (enemies == null)
        {
            Debug.Log("No collider or enemy found in scene");

        }
        else
        {
            enemies = Physics.OverlapSphere(transform.position, ExplosionRange);
            foreach (var enemy in enemies)
            {
                if (enemy.GetComponentInParent<EnemyAIController>() != null)
                {
                    //Vector3 closestPoint = Collider.ClosestPointOnBounds(transform.position);
                    //float distance = Vector3.Distance(closestPoint, transform.position);
                    //float Damage = 1f - Mathf.Clamp01(distance / ExplosionRange);
                    //explosionDamage -= Damage * 10f;

                    enemy.GetComponentInParent<EnemyAIController>().takeDamage(explosionDamage);
                }
            }
        }
        Destroy(gameObject, 3f);

    }
    #endregion

    #region Update
    // Update is called once per frame
    void Update()
    {
        
    }
    #endregion

    //private void OnDrawGizmos()
    //{
    //    Gizmos.DrawSphere(transform.position, ExplosionRange);
    //}

}
