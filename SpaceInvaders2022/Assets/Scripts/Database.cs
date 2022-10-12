using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using UnityEngine;
using UnityEngine.UI;
using Firebase.Database;
using JetBrains.Annotations;
using Firebase.Auth;

public class Database : MonoBehaviour
{
    private DatabaseReference dbRef;
    private FirebaseAuth auth;
    public Text NameAlreadyExist;
    public Text SuccessfulRegistration;
    public Text IncorrectLoginOrPassword;
    public GameObject UI_InputWindow;
    private int countRegistrations = 0;
    
    public Button ButtonRewards;
    public Button ButtonBackFromRewards;
    public GameObject RewardsImage;
    public Text RewardsPeople;

    // Start is called before the first frame update
    void Start()
    {
        if (PlayerPrefs.HasKey("Nickname"))
        {
            UI_InputWindow.SetActive(false);
        }
        else
        {
            UI_InputWindow.SetActive(true);
        }
        dbRef = FirebaseDatabase.DefaultInstance.RootReference;
        auth = FirebaseAuth.DefaultInstance;
    }

    public void Register(string nickname, string pass)
    {
        string json = JsonUtility.ToJson(new User(nickname, pass));
        dbRef.Child("users").Child(nickname).SetRawJsonValueAsync(json);
    }
    
    public void SetRecord(int record)
    {
        string json = JsonUtility.ToJson(new User(PlayerPrefs.GetString("Nickname"),
            PlayerPrefs.GetString("Password"), record));
        dbRef.Child("users").Child(PlayerPrefs.GetString("Nickname")).SetRawJsonValueAsync(json);
    }
    
    public IEnumerator Reg(string nickname, string password)
    {
        var user = dbRef.Child("users").Child(nickname).GetValueAsync();

        yield return new WaitUntil(predicate: () => user.IsCompleted);

        if (user.Exception != null)
        {
            Debug.Log(user.Exception);
        }
        else if (user.Result != null && user.Result.Exists)
        {
            DataSnapshot snapshot = user.Result;
            SuccessfulRegistration.gameObject.SetActive(false);
            NameAlreadyExist.gameObject.SetActive(true);
            IncorrectLoginOrPassword.gameObject.SetActive(false);
        }
        else
        {
            ++countRegistrations;
            Register(nickname, password);
            SuccessfulRegistration.gameObject.SetActive(true);
            NameAlreadyExist.gameObject.SetActive(false);
            IncorrectLoginOrPassword.gameObject.SetActive(false);
        }
    }
    
    public void ButtonRegister()
    {
        SuccessfulRegistration.gameObject.SetActive(false);
        NameAlreadyExist.gameObject.SetActive(false);
        IncorrectLoginOrPassword.gameObject.SetActive(false);
        if (countRegistrations < 5)
        {
            if (GetComponent<EnterNameAndPassword>().CorrectСharactersFlag &&
                !GetComponent<EnterNameAndPassword>().ShortPassword)
            {
                StartCoroutine(Reg(GetComponent<EnterNameAndPassword>().NicknamePlayer, 
                    GetComponent<EnterNameAndPassword>().PasswordPlayer));
            }
        }
    }
    
    public IEnumerator Log(string nickname, string password)
    {
        var user = dbRef.Child("users").Child(nickname).GetValueAsync();

        yield return new WaitUntil(predicate: () => user.IsCompleted);

        if (user.Exception != null)
        {
            Debug.Log(user.Exception);
        }
        else if (user.Result != null && user.Result.Exists)
        {
            DataSnapshot snapshot = user.Result;
            if (snapshot.Child("password").Value.ToString() == password)
            {
                PlayerPrefs.SetString("Nickname", nickname);
                PlayerPrefs.SetString("Password", password);
                PlayerPrefs.SetInt("Record", Convert.ToInt32(snapshot.Child("score").Value.ToString()));
                UI_InputWindow.SetActive(false);
                GetComponent<SpawnEnemies>().BeginSetup();
            }
            else
            {
                SuccessfulRegistration.gameObject.SetActive(false);
                NameAlreadyExist.gameObject.SetActive(false);
                IncorrectLoginOrPassword.gameObject.SetActive(true);
            }
        }
        else
        {
            SuccessfulRegistration.gameObject.SetActive(false);
            NameAlreadyExist.gameObject.SetActive(false);
            IncorrectLoginOrPassword.gameObject.SetActive(true);
        }
    }
    
    public void ButtonLogin()
    {
        SuccessfulRegistration.gameObject.SetActive(false);
        NameAlreadyExist.gameObject.SetActive(false);
        IncorrectLoginOrPassword.gameObject.SetActive(false);
        if (GetComponent<EnterNameAndPassword>().CorrectСharactersFlag &&
            !GetComponent<EnterNameAndPassword>().ShortPassword)
        {
            StartCoroutine(Log(GetComponent<EnterNameAndPassword>().NicknamePlayer,
                GetComponent<EnterNameAndPassword>().PasswordPlayer));
        }
    }

    public IEnumerator GetLeaders()
    {
        int countLeaders = 10;
        var leaders = dbRef.Child("users").OrderByChild("score").LimitToLast(countLeaders).GetValueAsync();

        yield return new WaitUntil(predicate: () => leaders.IsCompleted);

        if (leaders.Exception != null)
        {
            Debug.LogError("Error: " + leaders.Exception);
        }
        else if (leaders.Result != null && leaders.Result.Exists)
        {
            int num = 1;
            RewardsPeople.text = "";
            DataSnapshot snapshot = leaders.Result;
            foreach (var dataChildSnapshot in snapshot.Children.Reverse())
            {
                if (dataChildSnapshot.HasChild("name") && dataChildSnapshot.HasChild("password") && dataChildSnapshot.HasChild("score"))
                {
                    RewardsPeople.text += (num.ToString() + ". " + dataChildSnapshot.Child("name").Value.ToString() + "\t\t" + dataChildSnapshot.Child("score").Value.ToString() + "\n");
                    ++num;
                }
            }
        }
        else
        {
            Debug.LogError("Result.Value == null");
        }
    }

    public void ShowTheMenu(bool isShow)
    {
        GetComponent<SpawnEnemies>().StartGame.gameObject.SetActive(isShow);
        GetComponent<SpawnEnemies>().RecordText.gameObject.SetActive(isShow);
        GetComponent<SpawnEnemies>().NicknamePlayerText.gameObject.SetActive(isShow);
        GetComponent<SpawnEnemies>().GameOverText.gameObject.SetActive(false);
        if (GetComponent<Music>().MusicIsPlay)
        {
            GetComponent<Music>().buttonOnVolume.gameObject.SetActive(isShow);
        }
        else
        {
            GetComponent<Music>().buttonOffVolume.gameObject.SetActive(isShow);
        }
        GetComponent<Quit>().ButtonQuit.gameObject.SetActive(isShow);
        ButtonRewards.gameObject.SetActive(isShow);
    }
    public void ButtonRewardsOnClick()
    {
        ShowTheMenu(false);
        ButtonBackFromRewards.gameObject.SetActive(true);
        StartCoroutine(GetLeaders());
        RewardsImage.SetActive(true);
    }

    public void ButtonBackFromRewardsOnClick()
    {
        ShowTheMenu(true);
        ButtonBackFromRewards.gameObject.SetActive(false);
        RewardsImage.SetActive(false);
    }
}

public class User
{
    public string name;
    public string password;
    public int score;
    public User(string name, string pass, int score = 0)
    {
        this.name = name;
        this.password = pass;
        this.score = score;
    }
}

