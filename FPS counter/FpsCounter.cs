using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class FpsCounter : MonoBehaviour
{
    [Header("FPS text display")]
    public TextMeshProUGUI displayFPS;

    public Color badColor;
    public Color averageColor;
    public Color goodColor;
    private float alphaValue = 1f;

    private float refreshTime = .5f;
    private float time;
    private int frameCount;
    private int frameRate;

    #region Start (calling the set alpha channel here)
    private void Start()
    {
        setTheAlphaChannel();
    }
    #endregion

    // Update is called once per frame
    void Update()
    {
        colorFrames();
        time += Time.unscaledDeltaTime;

        frameCount++;

        if (time >= refreshTime)
        {
            frameRate = Mathf.RoundToInt(frameCount / time);
            displayFPS.text = frameRate.ToString();
            time -= refreshTime;
            frameCount = 0;
        }
    }

    #region setting the alpha channel to 1f
    private void setTheAlphaChannel()
    {
        badColor.a = alphaValue;
        averageColor.a = alphaValue;
        goodColor.a = alphaValue;
    }
    #endregion

    #region coloring the frames according to frames/sec
    private void colorFrames()
    {
        if (frameRate <= 24)
        {
            displayFPS.color = badColor;
        }
        if (frameRate > 25 && frameRate <= 49)
        {
            displayFPS.color = averageColor;
        }
        if (frameRate >= 50)
        {
            displayFPS.color = goodColor;
        }

    }
    #endregion
}
