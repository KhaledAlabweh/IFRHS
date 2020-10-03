using System;
using System.Collections.Generic;

public static class DB_Manger // holds static objs for patients or drs (classes of dr and pat)
{
    public static Doctor doctor;
    public static Patient patient;
    public static GameStat stat; 
}
[Serializable] // eza rf3na obj bdl json b7wel l7alo (bs lazem kolhom ykono adts)
public class Doctor // class1
{
    public string uid;
    public string doctorname,email;
    public string phone;
    public string address;
    public Doctor(string uid,string doctorname, string email,string phone,string address) // constructor
    {
        this.uid = uid; 
        this.doctorname = doctorname;
        this.email = email;
        this.phone = phone;
        this.address = address;
    }
    public Dictionary<string, string> ToDictionary() // convert this obj to dictionary (bez of using child update to saving data into db) (bel testing ejet a9'ef aktar men dr mra w7deh f hek ashal f e3tmdet hay el 6re8a) 
    {
        Dictionary<string, string> result = new Dictionary<string, string>();
        result["uid"] = uid;
        result["doctorname"] = doctorname;
        result["email"] = email;
        result["phone"] = phone;
        result["address"] = address;

        return result;
    }
}

[Serializable]
public class Patient //class2
{

    public string UID,  PatientName, Email, PatientImage,disease;
    public int age;
    public  string comment;
    public string DocID;
    public Patient(string uID, string patientName, string email, string patientImage,DateTime birthday,string disease,string DocID) // constructor
    {

        this.DocID = DocID;
        UID = uID;
        PatientName = patientName;
        Email = email;
        PatientImage = patientImage;
        this.age =DateTime.Now.Year - birthday.Year;
        this.disease = disease;
        comment = "NEW";
    }
}
public class LeapMotionSensor
{
    public string sensorID, sensorName;
}

public class GameStat //class3
{
    public string playing_time_S, playing_time_E;
    public int perfomance;

    public GameStat(string playing_time_S, string playing_time_E, int perfomance) //  constructor
    {
        this.playing_time_S = playing_time_S;
        this.playing_time_E = playing_time_E;
        this.perfomance = perfomance;
    }
}
public class Session  //class 4
{
    public string login, logout, playCount;

    public Session(string login, string logout, string playCount) // constructor
    {
        this.login = login;
        this.logout = logout;
        this.playCount = playCount;
    }
}


public class Feedback //class5
{
    public string content, feedbackID, title, patientName;
    public long submissionDate;
}



