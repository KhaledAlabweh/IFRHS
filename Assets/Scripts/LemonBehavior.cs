using Leap.Unity.Interaction;
using UnityEngine;

public class LemonBehavior : MonoBehaviour
{
    public ParticleSystem particle;
    [SerializeField] Transform liquid;
    bool isHolding;
    [SerializeField] float speed;
    public float amount = 0;
    public float  MaxAmount = 0.4f;
    bool isFinished = false;
    [SerializeField] InteractionBehaviour interaction;
    Quaternion initRot; //3shan t-thbit el limoneh ma tlf m3 el eed
    bool contact = false;
    AudioSource audio;
    void Start()
    {
        audio = GetComponent<AudioSource>();
        speed /= PlayerPrefs.GetInt("level", 1); //bgsim sor3t el te3bayeh 3la el level 3shan tzed el 93obeh
        
        initRot = transform.rotation;
        interaction = GetComponent<InteractionBehaviour>();
        interaction.OnGraspBegin = OnMouseDown;
        interaction.OnHoverBegin = onHoverBegin;
        interaction.OnHoverEnd = onHoverEnd;
        interaction.OnContactEnd = onHoverEnd;
        interaction.OnGraspEnd = OnMouseUp;
    }
    void onHoverBegin()
    {
        if (!contact)
        {
            contact = true;
            Instructions.instance.next();
        }
        transform.localScale = Vector3.one * 1.2f;//kberha 1.2

    }
    void onHoverEnd()
    {
        transform.localScale = Vector3.one; //rj3ha zy mhe

    }
    void Update()
    {

        transform.rotation = initRot; // initiat rotation
        if (interaction.isGrasped)
            print("is grasped"); //console
        if (amount < MaxAmount)
        {
            if (isHolding)
            {

                //fill the glass
                amount += Time.deltaTime * speed;
                liquid.localScale += Vector3.up * (speed * Time.deltaTime);
                liquid.localPosition += Vector3.up * (speed * Time.deltaTime);
            }
        }
        else
        {
            if (!isFinished)
            {
                onLevelFinished();
            }
        }

    }

    private void OnMouseDown()
    {
        audio.Play();
        Instructions.instance.next();
        particle.Play();
        isHolding = true;
    }

    private void OnMouseUp()
    {
        audio.Stop();
        particle.Stop();
        isHolding = false;

    }
    void onLevelFinished()
    {
        isFinished = true;
        particle.Stop();

        // finish level
        GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>().onFinish();

    }
}
