using TMPro;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.HighDefinition;
using UnityEngine.UI;

public class DayNightController : MonoBehaviour
{
    public Slider DayNightSlider;
    [Header("Time of day slider")]
    [Range(0, 24)] public float timeOfDay;
    public float orbitSpeed = 0.0f;

    public int hour;
    public int minute;
    public TextMeshProUGUI timeString;

    [Header("Sun and Moon Light Transform")]
    [SerializeField] private Light Sun;
    [SerializeField] private Light Moon;
    [Header("Sky and fog volume")]
    public Volume SkyVolume;
    public AnimationCurve starCurve;

    public bool isNight;
    private PhysicallyBasedSky Sky;
    // Start is called before the first frame update
    void Start()
    {
        SkyVolume.profile.TryGet(out Sky);
    }

    // Update is called once per frame
    void Update()
    {
        if (DayNightSlider != null)
        {
            timeOfDay = DayNightSlider.value;
        }

        timeOfDay += Time.deltaTime * orbitSpeed; //increases timeOfDay by the desired amount every frame

        if (timeOfDay > 24)
        {
            timeOfDay = 0;
        }
        UpdateTime();

    }

    #region OnValidate, this is for scene view if we want to change daytime
    private void OnValidate()
    {
        UpdateTime();
    }
    #endregion

    #region UpdateTime
    public void UpdateTime()
    {

        float alpha = timeOfDay / 24.0f; //get how much time has passed
        float sunRotation = Mathf.Lerp(-90, 270, alpha); //rotate the sun somewhere between -90 and 270 degrees by how much time has passed
        float moonRotation = sunRotation - 180;

        Sun.transform.rotation = Quaternion.Euler(sunRotation, -150.0f, 0); //rotate the sun
        Moon.transform.rotation = Quaternion.Euler(moonRotation, -150.0f, 0); //rotate the moon

        SkyVolume.profile.TryGet(out Sky);
        Sky.spaceEmissionMultiplier.value = starCurve.Evaluate(alpha) * 20f;

        hour = Mathf.FloorToInt(timeOfDay); //round timeOfDay down to the nearest int
        minute = Mathf.FloorToInt((timeOfDay * 60f) % 60); //round minute down to the nearest int.

        timeString.text = "Time of day : " + hour.ToString("00") + ":" + minute.ToString("00");

        CheckNightDayTransition();
    }
    #endregion

    #region check if it is day or night
    private void CheckNightDayTransition()
    {
        if (isNight)
        {
            if (Moon.transform.rotation.eulerAngles.x > 180)
            {
                StartDay();
            }
        }
        else
        {
            if (Sun.transform.rotation.eulerAngles.x > 180)
            {
                StartNight();
            }
        }
    }
    #endregion

    #region start day , start night functions
    private void StartDay()
    {
        isNight = false;
        Sun.shadows = LightShadows.Soft;
        Moon.shadows = LightShadows.None;
    }

    private void StartNight()
    {
        isNight = true;
        Sun.shadows = LightShadows.None;
        Moon.shadows = LightShadows.Soft;
    }
    #endregion
}
