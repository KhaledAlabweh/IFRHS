using Leap.Unity.Interaction;
using UnityEngine;

public class BoxHandler : MonoBehaviour
{
    Rigidbody rb;
    public float distance;
    static int score = 0;
    InteractionBehaviour interaction;
    public int index;
    public int boxesRequired;
    public float maxSpeed;
    [SerializeField]bool flag = false;
    Vector3 movePos = Vector3.zero;
    public float moveSpeed =1;
    bool contact = false;
    bool grasped = false;
    AudioSource _audio;
   public AudioClip pickup, release;
    void Start()
    {

        score = 0;
        _audio = GetComponent<AudioSource>(); // 3shan y6l3 bel inspictur
        if (index > PlayerPrefs.GetInt("level", 1))
        {
            Destroy(gameObject);
        }

        switch (PlayerPrefs.GetInt("level", 1))
        {
            case 1:
                boxesRequired = 1;
                break;
            case 2:
                boxesRequired = 3;
                break;
            case 3:
                boxesRequired = 5;
                break;
            default:
                boxesRequired = 1;
                break;
        }
        interaction = GetComponent<InteractionBehaviour>();

        interaction.OnGraspBegin = OnGraspBegin;
        interaction.OnHoverBegin= onHover;
        rb = GetComponent<Rigidbody>();
    }
    void onHover()
    {
        if (!contact)
        {
            contact = true;
            Instructions.instance.next();
        }

    }
    

    void OnGraspBegin()
    {
        playPickup();
        if (!grasped)
        {
            grasped = true;
            Instructions.instance.next();
        }
        GetComponent<Collider>().isTrigger = false; // check if has touch ever befor
        rb.constraints = RigidbodyConstraints.None; // eza nshel el 8youd 3no

    }
    // Update is called once per frame
    void Update()
    {
        rb.velocity = Vector3.ClampMagnitude(rb.velocity, maxSpeed); // untifliker(3shjan ma y9'l y6er)

        if(movePos != Vector3.zero)
        {
            transform.position = Vector3.Lerp(transform.position, movePos, Time.deltaTime * moveSpeed); // move to the distenation(3shan lma atroko ynzl 3l box)
        }
    }
    
    private void OnMouseDrag()
    {
        
        var ray = Camera.main.ScreenPointToRay(Input.mousePosition); //for mouse movement 
        rb.MovePosition(ray.GetPoint(distance));
        
    }

    private void OnMouseUp()
    {
        rb.isKinematic = false; // disable cravity when holding
    }
    private void OnMouseDown()
    {
        rb.isKinematic = true;

    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.CompareTag("Finish"))
        {
            if(collision.name == "area") // if he get neatrest the box
            {
                Instructions.instance.next();
                return;
            }
            if (flag)
                return;
            else
                flag = true;
            movePos = collision.transform.position; // assign vale to move position
            score++; 
            if(score == boxesRequired)
            {
                GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>().onFinish();
                print("finish"); // console
            }
            playRelease(); 
            Destroy(interaction);
            Destroy(rb,1);
            Destroy(this,1);
        }
    }
    void playPickup()
    {
        _audio.clip = pickup;
        _audio.Play();
    }
    void playRelease()
    {
        _audio.clip = release;
        _audio.Play();
    }

}
