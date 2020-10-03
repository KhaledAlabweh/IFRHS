using Leap.Unity.Interaction;
using UnityEngine;

public class PickupBox : MonoBehaviour
{
    Rigidbody rb;
    public float speed;
    public float PickupScore;
    Vector3 mousePos;
    int requireHits;
    AudioSource audio;
    void Start()
    {
        audio = GetComponent<AudioSource>();
        GetComponent<InteractionBehaviour>().OnContactBegin = onContact;
        switch (PlayerPrefs.GetInt("level",1))
        {
            case 1:
                requireHits = 3;
                break;
            case 2:
                requireHits = 6;
                break;
            case 3:
                requireHits = 9;
                break;

            default:
                break;
        }
        rb = GetComponent<Rigidbody>();
        mousePos = Input.mousePosition;
    }
    void onContact()
    {
        Instructions.instance.next();
    }
    
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Finish"))
        {
            audio.Play();

            print("one"); // console
            PickupScore++;
            if(PickupScore ==requireHits)
            {
                GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>().onFinish();
                print("Finish");//console
            }
        }
    }
}