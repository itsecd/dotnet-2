using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EatPositionChanger : MonoBehaviour, IEatDestroyer
{
    [SerializeField]
    private Transform leftUpBound;

    [SerializeField]
    private Transform rightDownBound;

    public void Destroy()
    {
        GeneratePosition();
    }

    private const int tryCounts = 1000;

    private void GeneratePosition()
    {
        for (int i = 0; i < tryCounts; i++)
        {
            var newPos = new Vector3(Random.Range((int)leftUpBound.position.x,
                (int)rightDownBound.position.x),
                Random.Range((int)leftUpBound.position.y,
                (int)rightDownBound.position.y));

            if (!IsValidPosition(newPos))
                continue;

            transform.position = newPos;
            break;
        }
    }

    private bool IsValidPosition(Vector3 pos)
    {
        var direction = Camera.main.transform.position - pos;
        if (Physics.Raycast(Camera.main.transform.position, direction, 100))
            return false;

        return true;
    }

}

public interface IEatDestroyer
{
    void Destroy();
}
