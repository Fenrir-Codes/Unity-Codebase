using UnityEngine;
using System.Collections;

[System.Serializable]
public partial class ObjectDamageNew : MonoBehaviour
{
    public float hitPoints;
    public ManagerScore scoreManager;
    public Rigidbody deadReplacement;
    public int pointsToAdd;
    void Start()
    {
        GameObject score = GameObject.FindWithTag("ScoreManager");
        scoreManager = score.GetComponent<ManagerScore>();
    }

    public void ApplyDamage(float damage)
    {
        if (hitPoints <= 0f)
        {
            return;
        }
        hitPoints = hitPoints - damage;
        scoreManager.DrawCrosshair();
        if (hitPoints <= 0f)
        {
            StartCoroutine(Detonate());
        }
    }

    public IEnumerator Detonate()
    {
        scoreManager.addScore(pointsToAdd);
        if (deadReplacement)
        {
            var dead = Instantiate(deadReplacement, transform.position, transform.rotation);
            dead.GetComponent<Rigidbody>().velocity = GetComponent<Rigidbody>().velocity;
            dead.angularVelocity = GetComponent<Rigidbody>().angularVelocity;
        }
        yield return new WaitForSeconds(0.3f);
        Destroy(gameObject);
    }

}