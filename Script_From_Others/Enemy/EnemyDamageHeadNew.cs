using UnityEngine;
using System.Collections;

[System.Serializable]
public partial class EnemyDamageHeadNew : MonoBehaviour
{
    public EnemyDamageNew enemyDamage;
    public void ApplyDamage(float damage)
    {
        this.enemyDamage.headshot = true;
    }

}