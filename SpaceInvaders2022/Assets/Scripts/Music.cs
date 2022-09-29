using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Music : MonoBehaviour
{
    public Button buttonOnVolume;
    public Button buttonOffVolume;
    public bool MusicIsPlay;
    // Start is called before the first frame update
    void Start()
    {
        if (PlayerPrefs.GetString("BackgroundMusic") != "no")
        {
            MusicIsPlay = true;
            buttonOnVolume.gameObject.SetActive(true);
            buttonOffVolume.gameObject.SetActive(false);
            gameObject.GetComponent<AudioSource>().Play();
        }
        else
        {
            MusicIsPlay = false;
            buttonOffVolume.gameObject.SetActive(true);
            buttonOnVolume.gameObject.SetActive(false);
        }
    }

    // Update is called once per frame
    public void OnVolume()
    {
        MusicIsPlay = false;
        PlayerPrefs.SetString("BackgroundMusic", "no");
        buttonOnVolume.gameObject.SetActive(false);
        buttonOffVolume.gameObject.SetActive(true);
        gameObject.GetComponent<AudioSource>().Stop();
    }

    public void OffVolume()
    {
        MusicIsPlay = true;
        PlayerPrefs.SetString("BackgroundMusic", "yes");
        buttonOffVolume.gameObject.SetActive(false);
        buttonOnVolume.gameObject.SetActive(true);
        gameObject.GetComponent<AudioSource>().Play();
    }
}
