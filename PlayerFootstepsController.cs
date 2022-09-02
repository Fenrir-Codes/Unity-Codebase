using UnityEngine;

public class PlayerFootstepsController : MonoBehaviour
{
    PlayerMovementController Player;
    private AudioSource Audio;
    [Header("Player footstep aufio files")]
    [Tooltip("Audio clips")]
    //public AudioClip[] clip;
    public AudioClip stepsOnGravel;
    public AudioClip stepsOnStone;
    public AudioClip stepsOnSand;
    public AudioClip stepsOnWood;


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
                    Audio.clip = stepsOnGravel;
                    Audio.pitch = 1f;
                    Audio.Play();

                    if (Player.isRunning)
                    {
                        Audio.clip = stepsOnGravel;
                        Audio.pitch = 1.2f;
                        Audio.Play();
                    }

                }
                if (!Audio.isPlaying && Tag == "Stone")
                {

                    Audio.clip = stepsOnStone;
                    Audio.pitch = 0.8f;
                    Audio.Play();

                    if (Player.isRunning)
                    {
                        Audio.clip = stepsOnStone;
                        Audio.pitch = 1.1f;
                        Audio.Play();
                    }
                }
                if (!Audio.isPlaying && Tag == "Sand")
                {
                    Audio.clip = stepsOnSand;
                    Audio.pitch = 0.8f;
                    Audio.Play();

                    if (Player.isRunning)
                    {
                        Audio.clip = stepsOnSand;
                        Audio.pitch = 1.1f;
                        Audio.Play();
                    }
                }
                if (!Audio.isPlaying && Tag == "Wood")
                {
                    Audio.clip = stepsOnWood;
                    Audio.pitch = 0.8f;
                    Audio.Play();

                    if (Player.isRunning)
                    {
                        Audio.clip = stepsOnWood;
                        Audio.pitch = 1.1f;
                        Audio.Play();
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
