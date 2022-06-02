using UnityEngine;

public class OpponentCar : MonoBehaviour
{

    public RaceClient Client;

    public Transform Transform;
    private void Awake()
    {
        var obj = GameObject.FindWithTag("CLIENT_CREATED");
        Client = obj.GetComponent<RaceClient>();
    }

    private void Update()
    {
        if (Client != null)
        {
            Transform.position = new Vector3(Client.Position.X + 5.51f, Client.Position.Y - 3.2f, Transform.position.z);
            Transform.rotation = new Quaternion(Transform.rotation.x, Transform.rotation.y, Client.Rotate, Transform.rotation.w);
        }
    }
}
