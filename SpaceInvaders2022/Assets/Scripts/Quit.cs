using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Quit : MonoBehaviour
{
    public Button ButtonQuit;
    public void QuitApplication()
    {
        Application.Quit();
    }
}
