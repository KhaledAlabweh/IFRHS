using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class DocProfileManger : MonoBehaviour
{
    public Text name, email, phone, address;
    void Start()
    {

        FirebaseController.instance.getDoctorProfile(task =>  // fetch the doc info and display it in the scene 
        {
            if (task.IsCompleted)
            {
                var doc = JsonUtility.FromJson<Doctor>(task.Result.GetRawJsonValue());
                UnityMainThreadDispatcher.Instance().Enqueue(() => {
                    name.text = doc.doctorname;
                    email.text = doc.email;
                    phone.text = doc.phone;
                    address.text = doc.address;          
                });
            }
        });
        
    }
    void Update()
    {
        
    }

    public void signout()
    {
        FirebaseController.instance.signout();//Firebase controller
        SceneManager.LoadScene(0);
    }


    
    public void Back()
    {
        SceneManager.LoadScene(11);
    }
}
