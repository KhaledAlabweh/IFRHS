using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class FeedbackManager : MonoBehaviour
{
    public InputField text;
    void Start()
    {
        
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

        SceneManager.LoadScene(5);
    }

    public void submit()
    {
        FirebaseController.instance.submitFeedback(text.text,result => {
            if (result.IsCompleted)//bshyek 3la el callback eno kolo tmam aw l2
            {
                //finish submit
                UnityMainThreadDispatcher.Instance().Enqueue(() => {
                    SceneManager.LoadScene(2);
                });
            }
        }) ;
    }
}
