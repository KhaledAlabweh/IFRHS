using UnityEngine;
using UnityEngine.SceneManagement;

public class DocMainmenuManager : MonoBehaviour
{
   public  void report()
    {
        SceneManager.LoadScene(12);
    }

    public void profile()
    {
        SceneManager.LoadScene(13);

    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}