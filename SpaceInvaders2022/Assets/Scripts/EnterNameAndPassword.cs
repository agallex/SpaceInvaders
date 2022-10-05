using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class EnterNameAndPassword : MonoBehaviour
{
    public Text IncorrectСharacters;
    public Text ShortPass;
    public bool CorrectСharactersFlag = false;
    public bool ShortPassword = true;
    private const string ValidСharacters = "_0123456789abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ";
    public string NicknamePlayer;
    public string PasswordPlayer;

    public void ChangedValueName(string value)
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
    public void EndValueName(string value)
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
                CorrectСharactersFlag = false;
                IncorrectСharacters.gameObject.SetActive(true);
                break;
            }
        }
        if (isCorrectSymbol && NicknamePlayer.Length > 0)
        {
            CorrectСharactersFlag = true;
        }
    }
    
    public void EndValuePassword(string value)
    {
        PasswordPlayer = value;
        if (PasswordPlayer.Length > 4)
        {
            ShortPass.gameObject.SetActive(false);
            ShortPassword = false;
        }
        else
        {
            ShortPass.gameObject.SetActive(true);
            ShortPassword = true;
        }
    }
}
