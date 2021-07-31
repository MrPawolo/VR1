using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HighScoreManager : MonoBehaviour
{
    static float actScore;
    float prevActScore;
    public TMP_Text[] actHighScoreDisplays;
    public TMP_Text highScoreDisplay;

    const string HIGH_SCORE = "HIGH_SCORE";

    private void OnEnable()
    {
        highScoreDisplay.text = PlayerPrefs.GetFloat(HIGH_SCORE).ToString();
        ForEachActScoreDisplay(0);
    }
    public static void AddToHighScore(float val)
    {
        actScore += val;
    }
    private void FixedUpdate()
    {
        if(prevActScore != actScore)
        {
            ForEachActScoreDisplay(actScore);
        }
    }
    void ForEachActScoreDisplay(float val)
    {
        foreach (TMP_Text text in actHighScoreDisplays)
        {
            text.text = val.ToString();
        }
    }
    private void OnDisable()
    {
        float lastHighScore = PlayerPrefsHandler.GetFloat(HIGH_SCORE);
        if (lastHighScore < actScore)
        {
            PlayerPrefsHandler.SetFloat(HIGH_SCORE, actScore);
        }
        actScore = 0;
    }
}
