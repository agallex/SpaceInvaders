using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using Random = System.Random;

public class SpawnEnemies : MonoBehaviour
{
    public static int Score = 0;
    public static int Level = 0;
    public static int CountEnemy = 0;
    public static int is_Live = 0;
    
    public float StartLevelSpeedEnemy = 0f;
    private float DeltaLevelSpeedEnemy = 0.1f;
    private int EnemiesInARow = 7;
    private int EnemiesInTheColumn = 3;
    
    private Vector3 Direction = Vector3.left;
    private float _maxX = 2f;
    
    private int TouchingTheWalls = 0;
    
    public GameObject Enemy;
    public List<GameObject> Enemies = new List<GameObject>();
    public GameObject EnemiesBullet;
    private bool EnemiesHaveBullet = true;

    public Button StartGame;
    public Button Play;
    public Button Restart;
    public Button Pause;
    public Button Left;
    public Button Right;
    public Button Shoot;
    
    public TextMeshProUGUI ScoreText;
    public TextMeshProUGUI LevelText;

    public GameObject Player;

    private Coroutine CoroutineShoot;
    
    // Update is called once per frame

    void Update()
    {
        if (is_Live == 1)
        {
            if (Score > PlayerPrefs.GetInt("Record"))
            {
                PlayerPrefs.SetInt("Record", Score);
                //PlayerPrefs.Save();
            }

            if (CountEnemy == 0)
            {
                ++Level;
                StartLevelSpeedEnemy += DeltaLevelSpeedEnemy;
                SpawnEnemiesRectangle();
            }

            float speedEnemy = StartLevelSpeedEnemy + 5.0f / (CountEnemy + 1);
            foreach (var enemy in Enemies)
            {
                if (enemy != null)
                {
                    enemy.transform.Translate(Direction * speedEnemy * Time.deltaTime);
                }
            }

            if (EnemiesHaveBullet)
            {
                CoroutineShoot = StartCoroutine(EnemiesShooting());
            }

            if (TouchingTheWalls == 2)
            {
                TouchingTheWalls = 0;
                Vector3 downY = new Vector3(0, -0.25f, 0);
                foreach (var enemy in Enemies)
                {
                    if (enemy != null)
                    {
                        enemy.transform.position += downY;
                    }
                }
            }

            foreach (var enemy in Enemies)
            {
                if (enemy != null)
                {
                    if (enemy.transform.position.x <= -_maxX)
                    {
                        Direction = Vector3.right;
                        ++TouchingTheWalls;
                        break;
                    }

                    if (enemy.transform.position.x >= _maxX)
                    {
                        Direction = Vector3.left;
                        ++TouchingTheWalls;
                        break;
                    }
                }
            }
        }
        else if (is_Live == -1)
        {
            Lose();
        }
    }

    public void ButtonStartGame()
    {
        is_Live = 1;
        StartGame.gameObject.SetActive(false);
        Left.gameObject.SetActive(true);
        Right.gameObject.SetActive(true);
        Shoot.gameObject.SetActive(true);
        ScoreText.gameObject.SetActive(true);
        LevelText.gameObject.SetActive(true);
        Player.SetActive(true);
        Player.transform.position = new Vector3(0, -3, 0);
    }

    public void Lose()
    {
        foreach (var enemy in Enemies)
        {
            if (enemy != null)
            {
                Destroy(enemy);
            }
        }
        is_Live = 0;
        StopCoroutine(CoroutineShoot);
        StartGame.gameObject.SetActive(true);
        Left.gameObject.SetActive(false);
        Right.gameObject.SetActive(false);
        Shoot.gameObject.SetActive(false);
        ScoreText.gameObject.SetActive(false);
        LevelText.gameObject.SetActive(false);
        Player.SetActive(false);
    }
    
    void SpawnEnemiesRectangle()
    {
        float LeftX = -1.5f;
        float UpY = 3f;
        float delta = 0.5f;
        for (int i = 0; i < EnemiesInTheColumn; ++i)
        {
            for (int j = 0; j < EnemiesInARow; ++j)
            {
                Enemies.Add(Instantiate(Enemy, new Vector3(LeftX + j * delta, UpY - i * delta), Quaternion.identity));
            }
        }
        CountEnemy = 21;
    }


    int GetIndexEnemies()
    {
        List<int> indexEnemy = new List<int>() { 0, 0, 0, 0, 0, 0, 0};
        int index = 0;
        foreach (var enemy in Enemies)
        {
            if (enemy != null)
            {
                indexEnemy[index % EnemiesInARow] = index;
            }
            else
            {
                if (index < EnemiesInARow)
                {
                    indexEnemy[index] = -1;
                }
            }
            ++index;
        }
        List<int> NoEmptyIndex = new List<int>();
        foreach (var i in indexEnemy)
        {
            if (i != -1)
            {
                NoEmptyIndex.Add(i);
            }
        }
        Random rnd = new Random();
        return NoEmptyIndex[rnd.Next(0, NoEmptyIndex.Count)];
    }
    
    IEnumerator EnemiesShooting()
    {
        EnemiesHaveBullet = false;
        Random rnd = new Random();
        float Timeshot = 2f;
        if (Timeshot - Level * 0.01f > 0)
        {
            Timeshot -= Level * 0.01f;
        }
        yield return new WaitForSeconds(Timeshot + rnd.Next(0, 11) / (Level + 10.0f));
        int ind = GetIndexEnemies();
        Vector3 enemyPos = Enemies[ind].transform.position;
        Vector3 spawnPos = new Vector3(enemyPos.x, enemyPos.y - 0.3f, enemyPos.z);
        Instantiate(EnemiesBullet, spawnPos, transform.rotation);
        EnemiesHaveBullet = true;
    }
}