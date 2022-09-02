using UnityEngine;

public class ColumnBreakScript : MonoBehaviour
{


    public GameObject unbrokenColumn;
    public GameObject brokenColumn;
    AudioSource debrisSFX;

    //this determines whether the column will be broken or unbroken at the at runtime
    public bool isBroken;


    void Start()
    {
        debrisSFX = GetComponent<AudioSource>();
        if (isBroken)
        {
            BreakColumn();
        }
        else
        {
            unbrokenColumn.SetActive(true);
            brokenColumn.SetActive(false);
        }
    }


    public void BreakColumn()
    {
        debrisSFX.Play();
        debrisSFX.pitch = Random.Range(0.3f, 1f);
        isBroken = true;
        unbrokenColumn.SetActive(false);
        brokenColumn.SetActive(true);
    }

}