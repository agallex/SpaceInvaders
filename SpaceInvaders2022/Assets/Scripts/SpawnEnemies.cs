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
    public GameObject MainCamera;
    
    public static int Score = 0;
    public static int Level = 0;
    public static int CountEnemy = 0;
    public static int is_Live = 0;
    
    public float StartLevelSpeedEnemy = 0f;
    private float DeltaLevelSpeedEnemy = 0.1f;
    private int EnemiesInARow = 7;
    private int EnemiesInTheColumn = 3;
    
    private Vector3 Direction = Vector3.left;
    private bool CurrentWallTouchLeft = true;
    private float _maxX = 2f;
    
    private int TouchingTheWalls = 0;
    
    public GameObject Enemy;
    public List<GameObject> Enemies = new List<GameObject>();
    public GameObject EnemiesBullet;
    private bool EnemiesHaveBullet = true;

    public Button StartGame;
    public Button Continue;
    public Button Restart;
    public Button Pause;
    public Button Left;
    public Button Right;
    public Button Shoot;

    public TextMeshProUGUI ScoreText;
    public TextMeshProUGUI LevelText;
    public TextMeshProUGUI RecordText;
    public TextMeshProUGUI GameOverText;
    public Text NicknamePlayerText;

    public GameObject Player;

    private Coroutine CoroutineShoot;
    private Coroutine CoroutineGameOverText;

    public bool isPause = false;
    public bool isRestart = false;

    public void BeginSetup()
    {
        if (PlayerPrefs.HasKey("Nickname"))
        {
            if (MainCamera.GetComponent<Music>().MusicIsPlay)
            {
                MainCamera.GetComponent<Music>().buttonOnVolume.gameObject.SetActive(true);
            }
            else
            {
                MainCamera.GetComponent<Music>().buttonOffVolume.gameObject.SetActive(true);
            }
            MainCamera.GetComponent<Quit>().ButtonQuit.gameObject.SetActive(true);
            StartGame.gameObject.SetActive(true);
            RecordText.gameObject.SetActive(true);
            NicknamePlayerText.text = PlayerPrefs.GetString("Nickname");
            NicknamePlayerText.gameObject.SetActive(true);
        }
    }
    void Start()
    {
        BeginSetup();
    }

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

            float speedEnemy = StartLevelSpeedEnemy + 5.0f / (CountEnemy + 1.5f);
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

            foreach (var enemy in Enemies)
            {
                if (enemy != null)
                {
                    if (enemy.transform.position.x <= -_maxX && CurrentWallTouchLeft)
                    {
                        CurrentWallTouchLeft = false;
                        Direction = Vector3.right;
                        ++TouchingTheWalls;
                        break;
                    }

                    if (enemy.transform.position.x >= _maxX && !CurrentWallTouchLeft)
                    {
                        CurrentWallTouchLeft = true;
                        Direction = Vector3.left;
                        ++TouchingTheWalls;
                        break;
                    }
                }
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
            
        }
        else if (is_Live == -1)
        {
            Lose();
        }
    }

    public void ButtonStartGame()
    {
        is_Live = 1;
        NicknamePlayerText.gameObject.SetActive(false);
        StartGame.gameObject.SetActive(false);
        Pause.gameObject.SetActive(true);
        Left.gameObject.SetActive(true);
        Right.gameObject.SetActive(true);
        Shoot.gameObject.SetActive(true);
        ScoreText.gameObject.SetActive(true);
        LevelText.gameObject.SetActive(true);
        MainCamera.GetComponent<Quit>().ButtonQuit.gameObject.SetActive(false);
        Player.SetActive(true);
        Player.transform.position = new Vector3(0, -3, 0);
        if (MainCamera.GetComponent<Music>().MusicIsPlay)
        {
            MainCamera.GetComponent<Music>().buttonOnVolume.gameObject.SetActive(false);
        }
        else
        {
            MainCamera.GetComponent<Music>().buttonOffVolume.gameObject.SetActive(false);
        }

        if (CoroutineGameOverText != null)
        {
            StopCoroutine(CoroutineGameOverText);
        }
        GameOverText.gameObject.SetActive(false);
    }

    public void ButtonPause()
    {
        Time.timeScale = 0;
        isPause = true;
        Pause.gameObject.SetActive(false);
        Continue.gameObject.SetActive(true);
        Restart.gameObject.SetActive(true);
        if (MainCamera.GetComponent<Music>().MusicIsPlay)
        {
            MainCamera.GetComponent<Music>().buttonOnVolume.gameObject.SetActive(true);
        }
        else
        {
            MainCamera.GetComponent<Music>().buttonOffVolume.gameObject.SetActive(true);
        }
    }

    public void ButtonContinue()
    {
        Time.timeScale = 1;
        isPause = false;
        Pause.gameObject.SetActive(true);
        Continue.gameObject.SetActive(false);
        Restart.gameObject.SetActive(false);
        if (MainCamera.GetComponent<Music>().MusicIsPlay)
        {
            MainCamera.GetComponent<Music>().buttonOnVolume.gameObject.SetActive(false);
        }
        else
        {
            MainCamera.GetComponent<Music>().buttonOffVolume.gameObject.SetActive(false);
        }
    }

    public void ButtonRestart()
    {
        foreach (var enemy in Enemies)
        {
            if (enemy != null)
            {
                Destroy(enemy);
            }
        }

        GameObject[] playerBullets = GameObject.FindGameObjectsWithTag("PlayerBullet");
        for (int i = 0; i < playerBullets.Length; ++i)
        {
            Destroy(playerBullets[i]);
        }
        GameObject[] enemyBullets = GameObject.FindGameObjectsWithTag("EnemyBullet");
        for (int i = 0; i < enemyBullets.Length; ++i)
        {
            Destroy(enemyBullets[i]);
        }
        Level = 0;
        Score = 0;
        CountEnemy = 0;
        is_Live = 1;
        StartLevelSpeedEnemy = 0f;
        TouchingTheWalls = 0;
        StopCoroutine(CoroutineShoot);
        EnemiesHaveBullet = true;
        Time.timeScale = 1;
        isPause = false;
        isRestart = true;
        Pause.gameObject.SetActive(true);
        Continue.gameObject.SetActive(false);
        Restart.gameObject.SetActive(false);
        Player.transform.position = new Vector3(0, -3, 0);
        if (MainCamera.GetComponent<Music>().MusicIsPlay)
        {
            MainCamera.GetComponent<Music>().buttonOnVolume.gameObject.SetActive(false);
        }
        else
        {
            MainCamera.GetComponent<Music>().buttonOffVolume.gameObject.SetActive(false);
        }
    }
    
    public void Lose()
    {
        NicknamePlayerText.gameObject.SetActive(true);
        CoroutineGameOverText = StartCoroutine(ShowGameOverText());
        Pause.gameObject.SetActive(false);
        foreach (var enemy in Enemies)
        {
            if (enemy != null)
            {
                Destroy(enemy);
            }
        }
        GameObject[] playerBullets = GameObject.FindGameObjectsWithTag("PlayerBullet");
        for (int i = 0; i < playerBullets.Length; ++i)
        {
            Destroy(playerBullets[i]);
        }
        GameObject[] enemyBullets = GameObject.FindGameObjectsWithTag("EnemyBullet");
        for (int i = 0; i < enemyBullets.Length; ++i)
        {
            Destroy(enemyBullets[i]);
        }
        Level = 0;
        Score = 0;
        CountEnemy = 0;
        is_Live = 0;
        StartLevelSpeedEnemy = 0f;
        TouchingTheWalls = 0;
        StopCoroutine(CoroutineShoot);
        EnemiesHaveBullet = true;
        StartGame.gameObject.SetActive(true);
        Left.gameObject.SetActive(false);
        Right.gameObject.SetActive(false);
        Shoot.gameObject.SetActive(false);
        ScoreText.gameObject.SetActive(false);
        LevelText.gameObject.SetActive(false);
        MainCamera.GetComponent<Quit>().ButtonQuit.gameObject.SetActive(true);
        Player.SetActive(false);
        Player.GetComponent<Shoot>().HaveBullet = true;
        Player.GetComponent<MovePlayer>().left = false;
        Player.GetComponent<MovePlayer>().right = false;
        Player.GetComponent<Shoot>().ShootButtonEnter = false;
        if (MainCamera.GetComponent<Music>().MusicIsPlay)
        {
            MainCamera.GetComponent<Music>().buttonOnVolume.gameObject.SetActive(true);
        }
        else
        {
            MainCamera.GetComponent<Music>().buttonOffVolume.gameObject.SetActive(true);
        }
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

    IEnumerator ShowGameOverText()
    {
        GameOverText.gameObject.SetActive(true);
        var color = GameOverText.faceColor;
        color.a = 255;
        GameOverText.faceColor = color;
        yield return new WaitForSeconds(1f);
        for (byte i = 255; i > 0; --i)
        {
            color.a = i;
            GameOverText.faceColor = color;
            yield return new WaitForSeconds(0.01f);
        }
        GameOverText.gameObject.SetActive(false);
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