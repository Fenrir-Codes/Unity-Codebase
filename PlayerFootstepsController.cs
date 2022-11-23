using UnityEngine;

public class PlayerFootstepsController : MonoBehaviour
{
    PlayerMovementController Player;
    private AudioSource Audio;
    [Header("Player footstep aufio files")]
    [Tooltip("Audio clips")]
    //public AudioClip[] clip;
    public AudioClip[] stepsOnGravel;
    public AudioClip[] stepsOnStone;
    public AudioClip[] stepsOnWood;
    public AudioClip[] stepsOnMetal;


    private string Tag;


    // Start is called before the first frame update
    void Start()
    {
        Player = GetComponent<PlayerMovementController>();
        Audio = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        playEffects();
    }

    void playEffects()
    {
        //Audio.pitch = Random.Range(0.4f, 1.5f);

        if (Player.isGrounded && Player.isWalking || Player.isGrounded && Player.isRunning)
        {
            RaycastHit hit;
            if (Physics.Raycast(transform.position, Vector3.down, out hit))
            {
                Tag = hit.collider.tag;

                if (!Audio.isPlaying && Tag == "Gravel")
                {
                    Audio.pitch = 1f;
                    var walkOnGravelFloor = stepsOnGravel[Random.Range(0, stepsOnGravel.Length)];
                    Audio.PlayOneShot(walkOnGravelFloor);

                    if (Player.isRunning)
                    {
                        Audio.pitch = 1.2f;
                        Audio.PlayOneShot(walkOnGravelFloor);
                    }

                }
                if (!Audio.isPlaying && Tag == "Stone")
                {
                    Audio.pitch = 0.8f;
                    var walkOnStoneFloor = stepsOnStone[Random.Range(0, stepsOnStone.Length)];
                    Audio.PlayOneShot(walkOnStoneFloor);

                    if (Player.isRunning)
                    {
                        Audio.pitch = 1.1f;
                        Audio.PlayOneShot(walkOnStoneFloor);
                    }
                }
                if (!Audio.isPlaying && Tag == "Metal")
                {
                    Audio.pitch = 0.8f;
                    var walkOnMetalFloor = stepsOnMetal[Random.Range(0, stepsOnMetal.Length)];
                    Audio.PlayOneShot(walkOnMetalFloor);

                    if (Player.isRunning)
                    {
                        Audio.pitch = 1.1f;
                        Audio.PlayOneShot(walkOnMetalFloor);

                    }
                }
                if (!Audio.isPlaying && Tag == "Wood")
                {
                    Audio.pitch = 0.8f;
                    var walkOnWoodFloor = stepsOnWood[Random.Range(0, stepsOnWood.Length)];
                    Audio.PlayOneShot(walkOnWoodFloor);

                    if (Player.isRunning)
                    {
                        Audio.pitch = 1.1f;
                        Audio.PlayOneShot(walkOnWoodFloor);
                    }
                }

            }
        }
        else
        {
            Audio.Stop();
        }
    }
}
