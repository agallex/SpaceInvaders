using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class EnterNicknamePlayer : MonoBehaviour
{
    public GameObject UI_InputWindow;
    public InputField EnterNicknameText;
    public Button ButtonOk;
    public Text NameAlreadyExist;
    public Text IncorrectСharacters;
    private bool IncorrectСharactersFlag = false;
    private const string ValidСharacters = "_0123456789abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ";
    private string NicknamePlayer;
    
    void Start()
    {
        if (PlayerPrefs.HasKey("Nickname"))
        {
            UI_InputWindow.SetActive(false);
        }
    }
    
    public void ChangedValue(string value)
    {
        NicknamePlayer = value;
        bool isCorrectSymbol = false;
        foreach (var ValidSymbol in ValidСharacters)
        {
            if (NicknamePlayer.EndsWith(ValidSymbol))
            {
                isCorrectSymbol = true;
                break;
            }
        }
        if (!isCorrectSymbol)
        {
            IncorrectСharacters.gameObject.SetActive(true);
        }
        else
        {
            IncorrectСharacters.gameObject.SetActive(false);
        }
    }
    public void EndValue(string value)
    {
        NicknamePlayer = value;
        bool isCorrectSymbol = false;
        foreach (var NicknameSymbol in NicknamePlayer)
        {
            isCorrectSymbol = false;
            foreach (var ValidSymbol in ValidСharacters)
            {
                if (NicknameSymbol.Equals(ValidSymbol))
                {
                    isCorrectSymbol = true;
                    break;
                }
            }
            if (!isCorrectSymbol)
            {
                IncorrectСharactersFlag = false;
                IncorrectСharacters.gameObject.SetActive(true);
                break;
            }
        }
        if (isCorrectSymbol && NicknamePlayer.Length > 0)
        {
            IncorrectСharactersFlag = true;
        }
    }

    public void ButtonOkClick()
    {
        if (IncorrectСharactersFlag)
        {
            PlayerPrefs.SetString("Nickname", NicknamePlayer);
            UI_InputWindow.SetActive(false);
            GameObject.Find("Main Camera").GetComponent<SpawnEnemies>().BeginSetup();
        }
    }
    
}
