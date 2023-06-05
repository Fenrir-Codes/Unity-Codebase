
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

    private float Stepinterval;
    private int currentLayer;

    [Header("The name of the layer you walk on")]
    [SerializeField] private string nameOfLayer = "";

    [Header("The pich of the sound effects")]
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
        if (Player.isGrounded)
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
            }
        }


        // Play the appropriate footstep sound
        switch (nameOfLayer)
        {
            case "Sand": // Sand layer
                PlaySandFootsteps();
                break;
            case "Gravel": // Gravel layer
                PlayGravelFootsteps();
                break;
            case "Stone": // Stone layer
                PlayStoneFootsteps();
                break;
            case "Dirt": // Stone layer
                PlayDirtFootsteps();
                break;
            default: 
                PlaydefaultSteps();
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
        return layerName;
    }
    #endregion

    #region Default footstep sound
    private void PlaydefaultSteps()
    {
        if (!audioSource.isPlaying)
        {
            var walkDefault = sandFootstepSound[Random.Range(0, sandFootstepSound.Length)];
            if (Player.isWalking && Stepinterval >= walkStepInterval)
            {
                audioSource.PlayOneShot(walkDefault);
                Stepinterval = 0f;
            }
            if (Player.isRunning && Stepinterval >= runStepInterval)
            {
                audioSource.PlayOneShot(walkDefault);
                Stepinterval = 0f;
            }
        }
    }
    #endregion

    #region SandFootsteps
    private void PlaySandFootsteps()
    {
        if (!audioSource.isPlaying)
        {
            var walkOnSandFloor = sandFootstepSound[Random.Range(0, sandFootstepSound.Length)];
            if (Player.isWalking && Stepinterval >= walkStepInterval)
            {
                audioSource.PlayOneShot(walkOnSandFloor);
                Stepinterval = 0f;
            }
            if (Player.isRunning && Stepinterval >= runStepInterval)
            {
                audioSource.PlayOneShot(walkOnSandFloor);
                Stepinterval = 0f;
            }
        }
    }
    #endregion

    #region Gravel Footsteps
    private void PlayGravelFootsteps()
    {
        if (!audioSource.isPlaying)
        {
            var walkOnGravel = gravelFootstepSound[Random.Range(0, gravelFootstepSound.Length)];
            if (Player.isWalking && Stepinterval >= walkStepInterval)
            {
                audioSource.PlayOneShot(walkOnGravel);
                Stepinterval = 0f;
            }
            if (Player.isRunning && Stepinterval >= runStepInterval)
            {
                audioSource.PlayOneShot(walkOnGravel);
                Stepinterval = 0f;
            }
        }
    }
    #endregion

    #region Stone Footsteps
    private void PlayStoneFootsteps()
    {
        if (!audioSource.isPlaying)
        {
            var walkOnStone = stoneFootstepSound[Random.Range(0, stoneFootstepSound.Length)];
            if (Player.isWalking && Stepinterval >= walkStepInterval)
            {
                audioSource.PlayOneShot(walkOnStone);
                Stepinterval = 0f;
            }
            if (Player.isRunning && Stepinterval >= runStepInterval)
            {
                audioSource.PlayOneShot(walkOnStone);
                Stepinterval = 0f;
            }
        }
    }
    #endregion

    #region Dirt Footsteps
    private void PlayDirtFootsteps()
    {
        if (!audioSource.isPlaying)
        {
            var walkOnDirt = dirtFootstepSound[Random.Range(0, dirtFootstepSound.Length)];
            if (Player.isWalking && Stepinterval >= walkStepInterval)
            {
                audioSource.PlayOneShot(walkOnDirt);
                Stepinterval = 0f;
            }
            if (Player.isRunning && Stepinterval >= runStepInterval)
            {
                audioSource.PlayOneShot(walkOnDirt);
                Stepinterval = 0f;
            }
        }
    }
    #endregion


}

