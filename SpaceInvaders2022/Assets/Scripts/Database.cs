using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Mime;
using UnityEngine;
using UnityEngine.UI;
using Firebase.Database;
using JetBrains.Annotations;
using UnityEditorInternal;
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
                IncorrectLoginOrPassword.gameObject.SetActive(false);
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
        StartCoroutine(Log(GetComponent<EnterNameAndPassword>().NicknamePlayer, 
            GetComponent<EnterNameAndPassword>().PasswordPlayer));
    }
}

public class User
{
    private string _name;
    public string password;
    public string score;
    public User(string name, string pass, string score = "0")
    {
        this._name = name;
        this.password = pass;
        this.score = score;
    }
}

