using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.EventSystems;
public struct Games
{
    public bool game1, game2, game3;

    public Games(bool game1, bool game2, bool game3)
    {
        this.game1 = game1;
        this.game2 = game2;
        this.game3 = game3;
    }
}
public class ReportManager : MonoBehaviour
{
    
    Patient CurrentEditPatient;
    public GameObject ListItem;
    public GameObject contentParent;
    List<Patient> allPatients = new List<Patient>();
    Dictionary<string, Games> avaliableGames = new Dictionary<string, Games>();

    List<string> IDs= new List<string>();

    public InputField _name, ageFrom, ageTo, _disease;

    [Header("profile")]
    public Text p_name, p_age, p_email; 
    public InputField p_Disease, p_comment;
    public Toggle game1;
    public Toggle game2;
    public Toggle game3;
    public GameObject Editpopup;


    void Start()
    {
        getAllPatients();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void getAllPatients()
    {
        FirebaseController.instance.getMyPatientsID(ID =>
        {
            foreach (var item in ID.Result.Children)
            {
                if (item == null)
                {
                    Debug.Log("nix");
                    print("nix");
                }
                else
                    IDs.Add(item.Key); // Query that get my patients id from my doctor node
            }
        },patient=> 
        {
            foreach (var item in patient.Result.Children) //Query that get all patients from patients node
            {
                try
                {
                    var _patient = JsonUtility.FromJson<Patient>(item.GetRawJsonValue());
                    if (IDs.Contains(_patient.UID)) // Query that finds my patients by checking if am a supervisor of this patient 
                    {
                        allPatients.Add(_patient); // put it in  the list
                    }
                }
                catch(Exception e)
                {
                    Debug.LogError(e);
                }
            }


            UnityMainThreadDispatcher.Instance().Enqueue(() =>
            {
                updateList(allPatients);

            });
        },games=> {
            foreach (var item in IDs)
            {
                print(item);
                if (!games.Result.Child(item).Exists ||
                !games.Result.Child(item).Child("Game1").Exists ||
                !games.Result.Child(item).Child("Game2").Exists ||
                !games.Result.Child(item).Child("Game3").Exists)
                    avaliableGames[item] = new Games(false, false, false);
                else
                avaliableGames[item] = new Games(
                    (bool)games.Result.Child(item).Child("Game1").Value == true,
                    (bool)games.Result.Child(item).Child("Game2").Value == true,
                    (bool)games.Result.Child(item).Child("Game3").Value == true);
            }
        });
    }


    void updateList(List<Patient> list)
    {
        foreach (var temp in list) //To init the list
        {
            try
            {
                GameObject item = Instantiate(ListItem, contentParent.transform); //game obj 
                item.transform.Find("Name").GetComponent<Text>().text = temp.PatientName;
                item.transform.Find("age").GetComponent<Text>().text = temp.age.ToString();
                item.transform.Find("disease").GetComponent<Text>().text = temp.disease;
                
                item.GetComponentInChildren<ProfileEditHanlder>().manager = this;
            }
            catch (Exception e)
            {
                Debug.LogError(e);
            }
        }
    }

    public void filter()
    {
        List<Patient> filtered = allPatients.Where(patient => // aggregate fun that fillters the patients list according to a set of constraints
        patient.PatientName.ToLower().Contains(_name.text.ToLower()) &&
        patient.age >= int.Parse(ageFrom.text) &&
        patient.age <= int.Parse(ageTo.text) &&
        patient.disease.ToLower().Contains(_disease.text.ToLower())

        ).ToList();
        foreach (Transform item in contentParent.transform)
        {
            Destroy(item.gameObject);
        }
        updateList(filtered);

    }

    public void editProfile(int index) {
        CurrentEditPatient =allPatients[index];
        p_name.text = CurrentEditPatient.PatientName;
        p_age.text = CurrentEditPatient.age.ToString();
        p_email.text = CurrentEditPatient.Email;
        p_Disease.text = CurrentEditPatient.disease;
        p_comment.text = CurrentEditPatient.comment;
        game1.isOn = avaliableGames[CurrentEditPatient.UID].game1;
        game2.isOn = avaliableGames[CurrentEditPatient.UID].game2;
        game3.isOn = avaliableGames[CurrentEditPatient.UID].game3;
        Editpopup.SetActive(true); 
    }


    public void save() // CALLED  FROM SCHENE DIRECTLY TO SAVE THE NEW DATA
    {
        FirebaseController.instance.editProfile(CurrentEditPatient.UID, p_Disease.text, p_comment.text,
            new Games(game1.isOn, game2.isOn, game3.isOn));
        avaliableGames[CurrentEditPatient.UID] = new Games(game1.isOn, game2.isOn, game3.isOn);
        CurrentEditPatient.disease = p_Disease.text;
        CurrentEditPatient.comment = p_comment.text;
    }

    public void back()
    {        
        SceneManager.LoadScene(11);
    }
   
}
