using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DisplayingPointsOnTheScreen : MonoBehaviour
{
    public TextMeshProUGUI RecordText;
    public TextMeshProUGUI ScoreText;
    public TextMeshProUGUI LevelText;

    // Update is called once per frame
    void Update()
    {
        RecordText.text = "Record: " + PlayerPrefs.GetInt("Record");
        ScoreText.text = "Score: " + SpawnEnemies.Score.ToString();
        LevelText.text = "Level: " + SpawnEnemies.Level.ToString();
    }
}
