using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ProfileManager : MonoBehaviour
{
   public Text name, age, performanece, diseases,comment;

    void Start()
    {
        UpdateUI();
    }
    void UpdateUI() //get vals from DB
    {
        name.text = FirebaseController.instance._patient.PatientName;

        age.text = FirebaseController.instance._patient.age.ToString();// (DateTime.Now.Year - int.Parse(string.Format("{0:yyyy}",DateTime.Parse(FirebaseController.instance._patient.birthday)))).ToString();
        //performanece.text = "100"; TODO 

        diseases.text = FirebaseController.instance._patient.disease;
        comment.text = FirebaseController.instance._patient.comment;

        GameStat g1_stat = new GameStat("","",0);
        GameStat g2_stat = new GameStat("","",0);
        GameStat g3_stat = new GameStat("","",0);
        int G1_lvl;
        int G2_lvl;
        int G3_lvl;

        FirebaseController.instance.getGameStat(result =>
        {
            if (result.IsCompleted)
            {
                G1_lvl = (int)result.Result.Child("Game1").ChildrenCount;
                if (G1_lvl != 0)
                    g1_stat = JsonUtility.FromJson<GameStat>(result.Result.Child("Game1").Child($"level{G1_lvl}").GetRawJsonValue()); // divide the result "Result" into games & its stats & diffeculties 
                else
                {
                    G1_lvl = 1;
                    g1_stat = new GameStat("", "", 0);
                }


                G2_lvl = (int)result.Result.Child("Game2").ChildrenCount;
                if (G2_lvl != 0)
                    g2_stat = JsonUtility.FromJson<GameStat>(result.Result.Child("Game2").Child($"level{G2_lvl}").GetRawJsonValue());
                else
                {
                    G2_lvl = 1;
                    g2_stat = new GameStat("", "", 0);
                }


                G3_lvl = (int)result.Result.Child("Game3").ChildrenCount;
                if (G3_lvl != 0)
                    g3_stat = JsonUtility.FromJson<GameStat>(result.Result.Child("Game3").Child($"level{G3_lvl}").GetRawJsonValue());
                else
                {
                    G3_lvl = 1;
                    g3_stat = new GameStat("", "", 0);
                }
            }
            UnityMainThreadDispatcher.Instance().Enqueue(() => { performanece.text = ((g1_stat.perfomance + g2_stat.perfomance + g3_stat.perfomance) / 3f).ToString(); });
        });
    }

    public void feedback()
    {
        SceneManager.LoadScene(10);
    }

    public void DrProfile()
    {
        SceneManager.LoadScene(13);

    }

    public void back()
    {
        SceneManager.LoadScene(4);
    }

    private void Update()
    {

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            back();
        }
    }
}


