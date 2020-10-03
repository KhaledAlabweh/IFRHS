using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;
using System.Linq;

public class LoginController : MonoBehaviour
{
    //sign up
    public InputField _name, Email, password;
    //log in 
    public InputField Login_username, Login_password,disease;

    public Dropdown day,month,year,doctor;
    List<Doctor> doctors = new List<Doctor>();
    [Header("Doctor sign up")]

    public InputField DocName;
    public InputField DocEmail;
    public InputField DocPassword;
    public InputField DocAddress;
    public InputField DocPhone;
    public GameObject ErrorPopup;
    void Start()
    {
        var _days = new List<string>(31);
        for (int i = 0; i < 31; i++)
        {
            _days.Insert(i, (i + 1).ToString());
        }
        day.AddOptions(_days);
        var _months = new List<string>(12);
        for (int i = 0; i < 12; i++)
        {
            _months.Insert(i, (i + 1).ToString());
        }
        month.AddOptions(_months);
        var _years = new List<string>(80);

        for (int i = 0; i < 75; i++)
        {
            _years.Insert(i, (i + 1950).ToString());
        }
        year.AddOptions(_years);
        getDoctors(); // b3be el doctor list men el db
    }
    int getValue(Dropdown list) // function to fetch the Birthdate from inpute lists
    {
        string date = list.options[list.value].text;
        print(date);//console
        return int.Parse(date);
    }

    string getDoctorID() // fetch dr name from doctors list
    {
        string name = doctor.options[doctor.value].text;
        return doctors.Single(doc => doc.doctorname == name).uid;
    }

    public void DocSignup() 
    {
        // 1st 2 are 4 auth ,, after that for db
        try
        {
            FirebaseController.instance.DocSignup(DocEmail.text, DocPassword.text, new Doctor("", DocName.text, DocEmail.text, DocPhone.text, DocAddress.text), task =>
            {
                if (task.IsCompleted) // call back is completed
                {

                    UnityMainThreadDispatcher.Instance().Enqueue(() => // el unity single threaded f had el script b5lena n9'l mashyeen bnfs el thread
                    {
                        SceneManager.LoadScene(11);
                    });
                }
            });
        }
        catch
        {
            ErrorPopup.SetActive(true);
        }
    }
    public void Patient_signup()
    {
        try
        {
            FirebaseController.instance.signup(Email.text, password.text, _name.text, new System.DateTime(getValue(year), getValue(month), getValue(day)), disease.text, getDoctorID(),task=> {
                if (task.IsCanceled || task.IsFaulted)
                {
                    UnityMainThreadDispatcher.Instance().Enqueue(() =>
                    {
                        Debug.LogError("sign up Error");
                        ErrorPopup.SetActive(true);
                        return;
                    });
                }
            }, callback =>
            {
                if(callback.IsCanceled || callback.IsFaulted)
                {
                    UnityMainThreadDispatcher.Instance().Enqueue(() =>
                    {
                        Debug.LogError("sign up Error");
                        ErrorPopup.SetActive(true);
                        return;
                    });
                }
                if (callback.IsCompleted)
                {

                    UnityMainThreadDispatcher.Instance().Enqueue(() =>
                    {
                        SceneManager.LoadScene(2);

                    });
                }
            });
        }
        catch
        {
            UnityMainThreadDispatcher.Instance().Enqueue(() =>
            {
                Debug.LogError("sign up Error");
                ErrorPopup.SetActive(true);
                return;
            });
        }
    }
    public void signin()
    {
        FirebaseController.instance.signin(Login_username.text, Login_password.text,callback => {
            if(callback.IsCanceled || callback.IsFaulted)
                    UnityMainThreadDispatcher.Instance().Enqueue(() =>
                    {
                        ErrorPopup.SetActive(true);
                    });
            if (Login_username.text.Contains("dr") ||
            Login_username.text.Contains("doctor"))
            {
                UnityMainThreadDispatcher.Instance().Enqueue(() =>
                {
                    SceneManager.LoadScene(11);
                });
            }
            else 
            {
                FirebaseController.instance.setPatient();
                UnityMainThreadDispatcher.Instance().Enqueue(() =>
                {

                    SceneManager.LoadScene(2);
                });
            }      
        });
    }

    void getDoctors()
    {
        //3shan y7o6 el drs bel list
        FirebaseController.instance.getDoctors(task => 
        {
            foreach (var item in task.Result.Children)
            {
                try
                {
                    doctors.Add(JsonUtility.FromJson<Doctor>(item.GetRawJsonValue())); // db return objs as json this coinvert it to c# objs
                    print(JsonUtility.FromJson<Doctor>(item.GetRawJsonValue()).doctorname);//console
                }
                catch (Exception e)
                {
                    Debug.LogError(e);
                }
            }
            
            UnityMainThreadDispatcher.Instance().Enqueue(() => //fill the dropdown list
            {
                List<string> doctorNames = new List<string>(doctors.Count);
                foreach (var item in doctors)
                {
                    doctorNames.Add(item.doctorname);
                }
                doctor.AddOptions(doctorNames);
            });
        });
    }
    void Update()
    {

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            back();
        }
    }

    public void exit()
    {
        Application.Quit();
    }

    public void back()
    {
        SceneManager.LoadScene(1);
    }
}
