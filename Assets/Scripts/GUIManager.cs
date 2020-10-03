using UnityEngine;

public class GUIManager : MonoBehaviour
{
    public void ShowPanel(GameObject x)
    {
       x.SetActive(true);        
    }
    public void HidePanel(GameObject x)
    {
        x.SetActive(false);
    }

    //Scene Load,, Scene_Manager (to work with it i need space scene Managment)
 
    public void _Exit()
    {
        Application.Quit();
    }
}
