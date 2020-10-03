using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    int Game;
    int level;//1,2,3
    DateTime startTime;
    public Text duration, performance;
    public GameObject finishPopup;
    public int nextExcersiceIndex;
    public List<int> bestTime;
    public GameObject pausePopup;
    void Start()
    {
        if(PlayerPrefs.GetInt("sounds",1) == 0)
        Camera.main.GetComponent<AudioListener>().enabled = false;
        level = PlayerPrefs.GetInt("level",1);
        Game= PlayerPrefs.GetInt("Game",1);
        if(level == 1)
        {
            Instructions.instance.next();
        }
        else
        {
            Instructions.instance.block();
        }
        startTime = DateTime.Now;
    }

    public void onFinish()
    {
        finishPopup.SetActive(true);
        duration.text = ((int)(DateTime.Now - startTime).TotalSeconds).ToString() + " seconds";
        int _performance =(int)Mathf.Clamp01((float)(bestTime[level - 1] / (DateTime.Now - startTime).TotalSeconds)) * 100; // clamp01 is to return a numbe between 0 & 1
        performance.text = _performance + "%";
        FirebaseController.instance.SaveGameStat(Game, level, new GameStat(startTime.ToString(), DateTime.Now.ToString(), _performance));
    }
    public void toMainmenu()
    {
        SceneManager.LoadScene(3);
    }
    public void nextLevel()
    {
        if (level >= 3)
        {
            PlayerPrefs.SetInt("level", 1);
            SceneManager.LoadScene(nextExcersiceIndex);
        }
        else
        {
            PlayerPrefs.SetInt("level", level + 1);
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex); // to reload the same scene
        }

    }
    void Update()
    {

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            back();
        }
    }

    public void back()
    {
        if (!pausePopup.activeInHierarchy)
        {
            pausePopup.SetActive(true);
            Time.timeScale = 0;

        }
        else
        {

            pausePopup.SetActive(false);
            Time.timeScale = 1;
        }
    }
    public void leave()
    {
        SceneManager.LoadScene(3);

    }
}