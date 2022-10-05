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
    
    public void SetRecord(string record)
    {
        dbRef.Child("users").Child(PlayerPrefs.GetString("Nickname")).Child("score").SetRawJsonValueAsync(record);
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
        if (countRegistrations < 3)
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
        var leaders = dbRef.Child("users").OrderByChild("score").LimitToLast(5).GetValueAsync();

        yield return new WaitUntil(predicate: () => leaders.IsCompleted);

        if (leaders.Exception != null)
        {
            Debug.LogError("Error: " + leaders.Exception);
        }
        else if (leaders.Result.Value == null)
        {
            Debug.LogError("Result.Value == null");
        }
        else
        {
            int num = 1;
            DataSnapshot snapshot = leaders.Result;
            foreach (var dataChildSnapshot in snapshot.Children.Reverse())
            {
                RewardsPeople.text += (num.ToString() + ". " + dataChildSnapshot.Child("name").Value.ToString() + "\t\t" + dataChildSnapshot.Child("score").Value.ToString() + "\n");
                ++num;
            }
        }

    }

    public void ShowTheMenu(bool isShow)
    {
        GetComponent<SpawnEnemies>().StartGame.gameObject.SetActive(isShow);
        GetComponent<SpawnEnemies>().NicknamePlayerText.gameObject.SetActive(isShow);
        GetComponent<SpawnEnemies>().GameOverText.gameObject.SetActive(isShow);
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
    public string score;
    public User(string name, string pass, string score = "0")
    {
        this.name = name;
        this.password = pass;
        this.score = score;
    }
}

