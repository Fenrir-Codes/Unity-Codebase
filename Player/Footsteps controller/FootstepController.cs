
using UnityEngine;

public class FootstepController : MonoBehaviour
{
    Terrain terrain;
    private AudioSource audioSource;
    private PlayerMovementController Player;

    [Header("Audio clip arrays")]
    public AudioClip[] defaultFootstepSound;
    public AudioClip[] sandFootstepSound;
    public AudioClip[] gravelFootstepSound;
    public AudioClip[] dirtFootstepSound;
    public AudioClip[] stoneFootstepSound;
    public AudioClip[] concreteFootstepSound;
    public AudioClip[] metalFootstepSound;
    public AudioClip[] woodFootstepSound;
    public AudioClip[] woodenFloorFootstepSound;

    private float Stepinterval;
    private int currentLayer = 0;

    [Header("The name of the layer you walk on")]
    [SerializeField] private string nameOfLayer = "";

    [Header("The interval of the sound effects")]
    [Range(0, 3)] public float walkStepInterval = .5f;
    [Range(0, 3)] public float runStepInterval = .3f;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        Player = GetComponent<PlayerMovementController>();
    }

    #region Update
    private void Update()
    {
        Stepinterval += Time.deltaTime;

        // Check if the character is grounded
        if (Player.isGrounded && Player.isWalking || Player.isGrounded && Player.isRunning)
        {
            FootstepsSwitcher();
        }
    }
    #endregion

    #region Swithc footsteps
    private void FootstepsSwitcher()
    {

        RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.down, out hit, 1))
        {
            //Debug.DrawRay(transform.position, Vector3.down * 5, Color.red);
            // Check if the raycast hits a terrain object
            if (hit.collider.GetComponent<Terrain>() != null)
            {
                nameOfLayer = GetLayerName(getPosition(currentLayer));
            }
            else
            {

                if (hit.collider.CompareTag("Stone"))
                {
                    nameOfLayer = "Stone";
                }
                if (hit.collider.CompareTag("Concrete"))
                {
                    nameOfLayer = "Concrete";
                }
                if (hit.collider.CompareTag("Metal"))
                {
                    nameOfLayer = "Metal";
                }
                if (hit.collider.CompareTag("Wood"))
                {
                    nameOfLayer = "Wood";
                }
                if (hit.collider.CompareTag("WoodenFloor"))
                {
                    nameOfLayer = "WoodenFloor";
                }
            }
        }

        // Play the appropriate footstep sound
        switch (nameOfLayer)
        {
            case "Sand": // Sand layer
                PlaySteps(sandFootstepSound);
                break;
            case "Gravel": // Gravel layer
                PlaySteps(gravelFootstepSound);
                break;
            case "Stone": // Stone layer
                PlaySteps(stoneFootstepSound);
                break;
            case "Concrete": // Stone layer
                PlaySteps(concreteFootstepSound);
                break;
            case "Metal": // Stone layer
                PlaySteps(metalFootstepSound);
                break;
            case "Dirt": // Stone layer
                PlaySteps(dirtFootstepSound);
                break;
            case "Wood": // Stone layer
                PlaySteps(woodFootstepSound);
                break;
            case "WoodenFloor": // Stone layer
                PlaySteps(woodenFloorFootstepSound);
                break;
            default:
                PlaySteps(defaultFootstepSound);
                break;
                // Add more cases for additional terrain layers if needed
        }
    }
    #endregion

    #region getting the layer index of the textures
    private int getPosition(int layerIndex)
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.down, out hit, 1))
        {
            //Debug.DrawRay(transform.position, Vector3.down * 5, Color.red);
            // Check if the raycast hits a terrain object
            if (hit.collider.GetComponent<Terrain>() != null)
            {
                terrain = hit.collider.GetComponent<Terrain>();
                // Get the position on the terrain
                Vector3 playerPos = hit.point;

                // Get the terrain layer at the position
                TerrainData terrainData = terrain.terrainData;

                // The position of the player transfor - terrain transform gives exact position on the terrain
                Vector3 terrainPosition = playerPos - terrain.transform.position;
                Vector3 mapPosition = new Vector3(terrainPosition.x / terrain.terrainData.size.x, 0, terrainPosition.z / terrain.terrainData.size.z);

                //coordinates
                float xCoord = mapPosition.x * terrain.terrainData.alphamapWidth;
                float zCoord = mapPosition.z * terrain.terrainData.alphamapHeight;
                int posX = (int)xCoord;
                int posZ = (int)zCoord;

                //splatmap data
                float[,,] splatmapData = terrainData.GetAlphamaps(posX, posZ, 1, 1);
                float[] terrainLayers = new float[splatmapData.GetUpperBound(2) + 1];

                for (int i = 0; i < terrainLayers.Length; i++)
                {
                    terrainLayers[i] = splatmapData[0, 0, i];
                }

                // Check the terrain layer and play the corresponding footstep sound
                if (terrainLayers.Length > 0)
                {
                    float LayerValue = terrainLayers[0];
                    int indexOfLayer = 0;
                    for (int i = 0; i < terrainLayers.Length; i++)
                    {
                        if (terrainLayers[i] > LayerValue)
                        {
                            LayerValue = terrainLayers[i];
                            indexOfLayer = i;
                            layerIndex = indexOfLayer;
                        }
                    }
                    // Output the detected terrain layer to the console
                    // Debug.Log("Walking on terrain layer: " + currentLayer);}}
                }
            }
        }
        return layerIndex;


    }
    #endregion

    #region Getting the name of the layer
    private string GetLayerName(int layerIndex)
    {
        string layerName = terrain.terrainData.terrainLayers[layerIndex].name.ToString();
        //Debug.Log("index: " + layerIndex);
        return layerName;
    }
    #endregion

    #region GetrandomClip
    private AudioClip GetRandomClip(AudioClip[] clips)
    {
        return clips[Random.Range(0, clips.Length)];
    }
    #endregion

    #region play steaps methode
    private void PlaySteps(AudioClip[] clips)
    {
        if (!audioSource.isPlaying)
        {
            AudioClip clip = GetRandomClip(clips);

            if (Player.isWalking && Stepinterval >= walkStepInterval)
            {
                audioSource.PlayOneShot(clip);
                Stepinterval = 0f;
            }

            if (Player.isRunning && Stepinterval >= runStepInterval)
            {
                audioSource.PlayOneShot(clip);
                Stepinterval = 0f;
            }
        }
    }
    #endregion



}

