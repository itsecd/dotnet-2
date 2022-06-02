using UnityEngine;

public class OpponentCar : MonoBehaviour
{

    public RaceClient client;

    public Transform _transform;
    private void Awake()
    {
        var obj = GameObject.FindWithTag("CLIENT_CREATED");
        client = obj.GetComponent<RaceClient>();
    }

    private void Update()
    {
        if (client != null)
        {
            _transform.position = new Vector3(client.Position.X + 5.51f, client.Position.Y - 3.2f, _transform.position.z);
            _transform.rotation = new Quaternion(_transform.rotation.x, _transform.rotation.y, client.Rotate, _transform.rotation.w);
        }
    }
}
