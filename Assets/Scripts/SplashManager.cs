using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class SplashManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        if (FirebaseController.instance.isSignedin()) //check for sign in
            Invoke("GoTomainMenu", 2);
        else
            Invoke("GoToLoginScreen", 2);
    }
    void GoTomainMenu() 
    {
        
        if(FirebaseController.instance.auth.CurrentUser.Email.Contains("dr") ||//check if dr || patient
            FirebaseController.instance.auth.CurrentUser.Email.Contains("doctor"))
        {

            SceneManager.LoadScene(11);
        }
        else {
            FirebaseController.instance.setPatient();
            SceneManager.LoadScene(2);
        }
        

    }
    void GoToLoginScreen()
    {
        SceneManager.LoadScene(1);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
