using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class OptionsManager : MonoBehaviour
{
    public Image sound; // image button
    public Sprite soundOn, soundOff;
    void Start()
    {
        refreshUI();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void refreshUI()
    {
        bool sounds = PlayerPrefs.GetInt("sounds", 1) == 1; //playerprefs takes only numbers and string so i use numbers as boolean
        sound.sprite = sounds ? soundOn : soundOff; // change the pic with on and of with the behavior

    }
    public void toggleSounds() // sounds behavior (on/off)
    {
        bool sounds = PlayerPrefs.GetInt("sounds", 1) == 1;
        PlayerPrefs.SetInt("sounds", sounds ? 0 : 1); //chane on voice
        sound.sprite = !sounds ? soundOn : soundOff; // change on pic
        print(!sounds); // console

    }

    public void loadScene(int index)
    {
        SceneManager.LoadScene(index);
    }

    public void resetProgress() // reset default
    {
        PlayerPrefs.DeleteAll();
    }


    public void signout()
    {
        FirebaseController.instance.signout();//Firebase controller
        SceneManager.LoadScene(0);
    }
}
