using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Car2D : MonoBehaviour
{
    public RaceClient Client;

    public GameObject Check1;
    public GameObject Check2;

    public GameControl GameControl;

    [SerializeField]
    Rigidbody2D rb;

    [HideInInspector]
    public float ThrottleInput = 0f;

    [HideInInspector]
    public float BrakeInput = 0f;

    [HideInInspector]
    public float SteerInput = 0f;

    [HideInInspector]
    public float Kmh = 0f;

    float ms = 0f;

    float maxVelMS = 45f / 3.6f;
    float maxRotAngle = 120f;
    float goodSpeed = 60f / 3.6f;
    float deltaRot = 60f;
    float rotAngle = 0f;

    [SerializeField]
    AnimationCurve accel;

    private bool _check1 = false;

    private void Awake()
    {
        var obj = GameObject.FindWithTag("CLIENT_CREATED");
        Client = obj.GetComponent<RaceClient>();
    }
    void MoveCar()
    {
        rb.velocity += ThrottleInput * (Vector2)transform.up * accel.Evaluate(ms / maxVelMS);
    }

    void RotateCar()
    {
        if (ms <= goodSpeed)
            rotAngle = maxRotAngle;
        else
            rotAngle = maxRotAngle - Mathf.Lerp(0f, deltaRot, (ms - goodSpeed) / (maxVelMS - goodSpeed));
        transform.Rotate(0f, 0f, rotAngle * 1.5f * SteerInput * Time.fixedDeltaTime);
    }

    void Brakes()
    {
        rb.velocity = Vector2.Lerp(rb.velocity, Vector2.zero, Time.deltaTime * BrakeInput);
    }

    private void FixedUpdate()
    {
        ms = rb.velocity.magnitude;
        Kmh = 3.6f * ms;
        if (ThrottleInput != 0f)
            MoveCar();
        if (SteerInput != 0f)
        {
            RotateCar();
            Vector2 oldDir = rb.velocity / ms;
            Vector2 newDir = (Vector2)transform.up + oldDir;
            newDir.Normalize();
            rb.velocity = ms * newDir;
        }
        if (BrakeInput != 0f)
            Brakes();

        Client.ChangePosition(this.transform.localPosition.x, this.transform.localPosition.y, this.transform.localRotation.z);
        if (Vector3.Distance(this.transform.localPosition, Check1.GetComponent<Transform>().localPosition) < 0.5f)
        {
            _check1 = true;
        }

        if (Vector3.Distance(this.transform.localPosition, Check2.GetComponent<Transform>().localPosition) < 0.5f && _check1)
        {
            Client.Win();
        }
    }

    private void OnGUI()
    {
        GUI.TextField(new Rect(10, 10, 120, 20), Kmh + " km/h");
    }
}
