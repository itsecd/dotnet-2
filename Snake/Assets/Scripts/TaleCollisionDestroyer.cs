using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TaleCollisionDestroyer : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D col)
    {
        RecursivePositionRepeater repeater = 
            col.GetComponent<RecursivePositionRepeater>();

        if (repeater != null)
        {
            Debug.LogError("Вы проиграли, очень жаль!");
            Destroy(transform.parent.gameObject);
        }
    }
}
