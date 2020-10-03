using System.Collections.Generic;

using UnityEngine;
using Firebase.Auth;
using Firebase.Database;
using Firebase;
using Firebase.Unity.Editor;
using System;
using System.Threading.Tasks;

public class FirebaseController : MonoBehaviour
{
    public static FirebaseController instance;// singlton
  [HideInInspector] public FirebaseAuth auth; // Sign
    DatabaseReference DB_Refernce; // for DB communication 
    public Patient _patient; // public obj for thyis patient 
    
    private void Awake()
    {
        instance = this; // ay eshe men bra hai el script btnadeh
    }
    void Start() 
    {
        DontDestroyOnLoad(gameObject); // 3shan ma tedmar el script lma nntgl ben el  scens  
        FirebaseApp.DefaultInstance.SetEditorDatabaseUrl("https://gpdb-dcaff.firebaseio.com/"); // path of DB

        auth = FirebaseAuth.DefaultInstance; // assign val for obj
        DB_Refernce = FirebaseDatabase.DefaultInstance.RootReference;

    }
    public void setPatient() 
    {
        if (_patient == null)
        {
            DB_Refernce.Child("Patients").Child(auth.CurrentUser.UserId).GetValueAsync().ContinueWith(task => {
                _patient = JsonUtility.FromJson<Patient>(task.Result.GetRawJsonValue());
            });
        }
    }
    public void signup(string email,string password,string name, DateTime dateTime,string disease,string doctorID,Action<Task> signupCallback, Action<Task> callback)
    {

        auth.CreateUserWithEmailAndPasswordAsync(email, password).ContinueWith(task => {
            signupCallback(task);
            if (task.IsCanceled)
            {
                signupCallback(task);
                Debug.LogError("CreateUserWithEmailAndPasswordAsync was canceled.");
                return;
            }
            if (task.IsFaulted)
            {
                signupCallback(task);
                Debug.LogError("CreateUserWithEmailAndPasswordAsync encountered an error: " + task.Exception);
                return;
            }
            if (task.IsCompleted)
            {
                Patient patient = new Patient(auth.CurrentUser.UserId, name, email, "", dateTime, disease, doctorID);
                _patient = patient; // assign with the new data 
                DB_Refernce.Child("Patients").Child(auth.CurrentUser.UserId).SetRawJsonValueAsync(JsonUtility.ToJson(patient)).ContinueWith(DBcallback => // save patient in db
                {
                    callback(DBcallback); // return callback
                DB_Refernce.Child("MyDoctor").Child(doctorID).Child(auth.CurrentUser.UserId).SetValueAsync(name); // save p_id in the selected dr
            });

                // Firebase user has been created.
                Firebase.Auth.FirebaseUser newUser = task.Result; //assign for auth reference with new user 
                Debug.LogFormat("Firebase user created successfully: {0} ({1})",
                    newUser.DisplayName, newUser.UserId); // unity console(debuging perpose)
            }
        });

    }
    public void DocSignup(string email, string password, Doctor doctor, Action<Task> callback)
    {
        auth.CreateUserWithEmailAndPasswordAsync(email, password).ContinueWith(task =>
        {
            if (task.IsCanceled)
            {

                Debug.LogError("CreateUserWithEmailAndPasswordAsync was canceled.");
                return;
            }
            if (task.IsFaulted)
            {
                Debug.LogError("CreateUserWithEmailAndPasswordAsync encountered an error: " + task.Exception);
                return;
            }
            doctor.uid = task.Result.UserId;

            Dictionary<string, object> childUpdates = new Dictionary<string, object>();
            childUpdates["/Doctors/" + doctor.uid] = doctor.ToDictionary();

            DB_Refernce.UpdateChildrenAsync(childUpdates).ContinueWith(tasks => {
                callback(tasks);
            });

            });
    }

    public void signin(string username,string password,Action<Task> result)
    {
        
        auth.SignInWithEmailAndPasswordAsync(username , password).ContinueWith(result);
    }

    public void initData()
    {
        DB_Refernce.Child("MyDoctor").Child(auth.CurrentUser.UserId).GetValueAsync().ContinueWith(callback => {
            if (callback.IsCompleted)
            {
                string doctorID =(string) callback.Result.Value;
                DB_Refernce.Child("patients").Child(auth.CurrentUser.UserId).GetValueAsync().ContinueWith(result => {
                    DB_Manger.patient = JsonUtility.FromJson<Patient>(result.Result.GetRawJsonValue());
                });
                DB_Refernce.Child("Doctors").Child(doctorID).GetValueAsync().ContinueWith(docCallback => {
                    DB_Manger.doctor = JsonUtility.FromJson<Doctor>(callback.Result.GetRawJsonValue());
                });
            }
        });
    }

    public void getDoctors(Action<Task<DataSnapshot>> result)
    {
        DB_Refernce.Child("Doctors").GetValueAsync().ContinueWith(
            task =>
            {
                result(task);
            });
    }

    public void SaveGameStat(int gameIndex,int levelIndex,GameStat gameStat) //save last game data
    {// call right after the game finish
        DB_Refernce.Child("GameStat").Child(auth.CurrentUser.UserId).Child($"Game{gameIndex}").Child($"level{levelIndex}").SetRawJsonValueAsync(JsonUtility.ToJson(gameStat));
    }
    public void getGameStat(Action<Task<DataSnapshot>> result) //call right after loading Letsplay scene to get latest game stat data
    {
        DB_Refernce.Child("GameStat").Child(auth.CurrentUser.UserId).GetValueAsync().ContinueWith(Task => {
            result(Task);

        });
    }

    public void submitFeedback(string feedback, Action<Task> result) //toi save the feed back in the db
    {
        DB_Refernce.Child("feedback").Child(auth.CurrentUser.UserId).SetValueAsync(feedback).ContinueWith(task => {
            result(task);
        });
    }

    public bool isSignedin() //check if the user is signed in 
    {
        return auth.CurrentUser != null;
    }

    [ContextMenu("signout")]// to =sign out from inspictur
    public void signout()
    {
        auth = FirebaseAuth.DefaultInstance;
        auth.SignOut();
    }

    public void getMyPatientsID(Action<Task<DataSnapshot>> IDsCallback, Action<Task<DataSnapshot>> PatientsCallback, Action<Task<DataSnapshot>> gamesCallback) // take a loke of reference
    {
        DB_Refernce.Child("MyDoctor").Child(auth.CurrentUser.UserId).GetValueAsync().ContinueWith(task => {
            IDsCallback(task);
            DB_Refernce.Child("Patients").GetValueAsync().ContinueWith(Ptask => {
                PatientsCallback(Ptask); // p to avoid collesion bez of using 2 callbacks (task & Ptask)
                DB_Refernce.Child("GameStat").GetValueAsync().ContinueWith(Gtask => {
                    gamesCallback(Gtask);
                });
            });
        });
    }

    public void getDoctorProfile(Action<Task<DataSnapshot>> callback)
    {

        Debug.Log(string.IsNullOrEmpty(_patient.DocID) ? "doc" : "patient");
        DB_Refernce.Child("Doctors").Child(string.IsNullOrEmpty(_patient.DocID)?auth.CurrentUser.UserId : _patient.DocID).GetValueAsync().ContinueWith(task => {

            callback(task);
        
        });
    }

    public void editProfile(string id,string disease,string comment,Games games) // save new data
    {
        DB_Refernce.Child("Patients").Child(id).Child("disease").SetValueAsync(disease);
        DB_Refernce.Child("Patients").Child(id).Child("comment").SetValueAsync(comment);
        DB_Refernce.Child("GameStat").Child(id).Child("Game1").SetValueAsync(games.game1);
        DB_Refernce.Child("GameStat").Child(id).Child("Game2").SetValueAsync(games.game2);
        DB_Refernce.Child("GameStat").Child(id).Child("Game3").SetValueAsync(games.game3);
    }
}