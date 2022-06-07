using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EatTaleIncrementer : MonoBehaviour
{
    [SerializeField]
    private RecursivePositionRepeater _tale;

    private ICaudateObject _caudate;
    void Start()
    {
        _caudate = GetComponent<ICaudateObject>();

        if (_caudate == null)
            Debug.LogError("Отстутствует IncrementObjectMover!");
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        var eat = col.GetComponent<IEatDestroyer>();
        if (eat != null)
        {
            var tale = Instantiate(_tale, transform.position, Quaternion.identity);
            _caudate.tale = tale;
            eat.Destroy();
        }
    }
}
