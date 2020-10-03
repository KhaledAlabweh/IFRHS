using UnityEngine;
using UnityEngine.SceneManagement;
public class MainMenuController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
    if(Input.GetKeyDown(KeyCode.Escape))
        {
            onExit();
        }
    }
    public void gotoScene(int index) // loads the scens (Option || Play)
    {
        SceneManager.LoadScene(index);
    }

    public void onExit()
    {
        Application.Quit();
    }
}
