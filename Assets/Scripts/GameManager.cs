using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    GameStat g1_stat;
    GameStat g2_stat;
    GameStat g3_stat; // class which take the game detailes
    int G1_lvl;
    int G2_lvl;
    int G3_lvl; // representes the diffeculties
    public Button Game1, Game2, Game3;
    [Header("Game1")]
    public Text G1_levelTitle; // references for the games UI
    public Text G1_sTime      ;
    public Text G1_endTime    ;
    public Text G1_Duration   ;
    public Text G1_Performance; 
    [Header("Game2")]
    public Text G2_levelTitle;
    public Text G2_sTime;
    public Text G2_endTime;
    public Text G2_Duration;
    public Text G2_Performance;
    [Header("Game3")]
    public Text G3_levelTitle;
    public Text G3_sTime;
    public Text G3_endTime;
    public Text G3_Duration;
    public Text G3_Performance;


    void Start()
    {
        FirebaseController.instance.getGameStat(result => 
        {

            if (result.IsCompleted)
            {
                if (!result.Result.Exists)
                {
                    UnityMainThreadDispatcher.Instance().Enqueue(() => {
                        print("no games played");
                        Game1.interactable = false;
                        Game2.interactable = false;
                        Game3.interactable = false;
                    });
                    
                }

                G1_lvl = (int)result.Result.Child("Game1").ChildrenCount;
                if (G1_lvl != 0)
                    g1_stat = JsonUtility.FromJson<GameStat>(result.Result.Child("Game1").Child($"level{G1_lvl}").GetRawJsonValue()); // divide the result "Result" into games & its stats & diffeculties 
                else if((bool)result.Result.Child("Game1").Value == true)
                {
                    
                    G1_lvl = 1;
                    g1_stat = new GameStat("", "", 0);
                }
                else
                {
                    G1_lvl = 1;
                    g1_stat = new GameStat("", "", 0);
                    print("game 1not played");
                    UnityMainThreadDispatcher.Instance().Enqueue(() =>
                    {
                        Game1.interactable = false;
                    });
                }


                G2_lvl = (int)result.Result.Child("Game2").ChildrenCount;
                if (G2_lvl != 0)
                    g2_stat = JsonUtility.FromJson<GameStat>(result.Result.Child("Game2").Child($"level{G2_lvl}").GetRawJsonValue());
                else if((bool)result.Result.Child("Game2").Value == true)
                {
                    G2_lvl = 1;
                    g2_stat = new GameStat("", "", 0);
                }else
                {
                    print("game 2not played");

                    G2_lvl = 1;
                    g2_stat = new GameStat("", "", 0);
                    UnityMainThreadDispatcher.Instance().Enqueue(() =>
                    {
                        Game2.interactable = false;
                    });
                }


                G3_lvl = (int)result.Result.Child("Game3").ChildrenCount;
                if (G3_lvl != 0)
                    g3_stat = JsonUtility.FromJson<GameStat>(result.Result.Child("Game3").Child($"level{G3_lvl}").GetRawJsonValue());
                else if ((bool)result.Result.Child("Game3").Value == true)
                {
                    G3_lvl = 1;
                    g3_stat = new GameStat("", "", 0);
                }else
                {
                    G3_lvl = 1;
                    g3_stat = new GameStat("", "", 0);
                    print("game 3not played");

                    UnityMainThreadDispatcher.Instance().Enqueue(() =>
                    {
                        Game3.interactable = false;
                    });
                }
            }
            UnityMainThreadDispatcher.Instance().Enqueue(() => { setUI(); });
        });
    }
    void setUI()
    {
        G1_levelTitle.text = "squeezing: level "+ G1_lvl.ToString();
        G1_sTime.text = g1_stat.playing_time_S;
        G1_endTime.text= g1_stat.playing_time_E;
        G1_Duration.text = calculateDuration(g1_stat.playing_time_S, g1_stat.playing_time_E);
        G1_Performance.text = g1_stat.perfomance.ToString();

        G2_levelTitle.text = "hold and release: level " + G2_lvl.ToString();
        G2_sTime.text = g2_stat.playing_time_S;
        G2_endTime.text = g2_stat.playing_time_E;
        G2_Performance.text = g2_stat.perfomance.ToString();
        G2_Duration.text = calculateDuration(g2_stat.playing_time_S, g2_stat.playing_time_E);

        G3_levelTitle.text = "Left and hold: level " + G3_lvl.ToString();
        G3_levelTitle.resizeTextForBestFit = true;
        G2_levelTitle.resizeTextForBestFit = true;
        G1_levelTitle.resizeTextForBestFit = true;
        G3_sTime.text = g3_stat.playing_time_S;
        G3_endTime.text = g3_stat.playing_time_E;
        G3_Performance.text = g3_stat.perfomance.ToString();
        G3_Duration.text = calculateDuration(g3_stat.playing_time_S, g3_stat.playing_time_E);

    }

    string calculateDuration(string start,string end)
    {
        if(start == "" || end == "")
        {
            print("no start or end Time");
            return "not played yet";
        }
        else
        {
            DateTime startTime;
            DateTime endTime;
            try
            {
                startTime = DateTime.Parse(start);
                endTime = DateTime.Parse(end);
                return (endTime - startTime).TotalSeconds.ToString() + "seconds";

            }
            catch
            {
                print("Error");
                return "not played yet";
            }
        }
    }
    public void LoadGame(int index)
    {
        int level, game;
        switch (index)
        {
            case 7:
                game = 1;
                level = G1_lvl;
                break;
            case 8:
                game = 2;
                level = G2_lvl;
                break;
            case 9:
                game = 3;
                level = G3_lvl;
                break;
            default:
                Debug.LogError("game index invalid");
                return;
        }
        PlayerPrefs.SetInt("level", level); // save data localy
        PlayerPrefs.SetInt("Game", game);// takes the game and level to reloade it in the next time 
        SceneManager.LoadScene(index); 

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

        SceneManager.LoadScene(2);
    }
}
