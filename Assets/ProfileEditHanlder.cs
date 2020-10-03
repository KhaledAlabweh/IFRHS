using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProfileEditHanlder : MonoBehaviour
{
    public ReportManager manager;
    void Start()
    {
        GetComponent<Button>().onClick.AddListener(() => manager.editProfile(transform.parent.GetSiblingIndex()));
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
