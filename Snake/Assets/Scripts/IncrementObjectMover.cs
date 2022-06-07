using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IncrementObjectMover : MonoBehaviour, IObjectMover, ICaudateObject
{
    [SerializeField]
    private float _delay = 0.05f;

    [SerializeField]
    private RecursivePositionRepeater _tale;

    public float speed => _renderer.bounds.size.x / _delay;

    public IPositionRepeater tale
    {
        get => _tale;
        set
        {
            if ((IPositionRepeater)_tale != value && value is RecursivePositionRepeater)
            {
                var temp = _tale;
                _tale = (RecursivePositionRepeater)value;
                _tale.SetNextRepeater(temp);
            }
        }
    }

    public void MoveForward()
    {
        if (_renderer == null)
            return;

        Stop();
        _moveRoutine = StartCoroutine(MoveRoutine());
    }

    public void Stop()
    {
        if (_renderer == null)
            return;

        if (_moveRoutine != null)
            StopCoroutine(_moveRoutine);
    }

    public void Rotate(Quaternion quaternion)
    {
        if (_renderer == null)
            return;

        transform.rotation = quaternion;
    }

    private Coroutine _moveRoutine;
    private SpriteRenderer _renderer;

    private void Awake()
    {
        _renderer = GetComponent<SpriteRenderer>();
        if (_renderer == null)
            Debug.LogError("Для корректного использования IncrementObjectMover требуется SpriteRenderer!");
    }

    private IEnumerator MoveRoutine()
    {
        while (true)
        {
            var lastPosition = transform.position;

            transform.position += transform.right * _renderer.bounds.size.x;

            if (_tale != null)
                _tale.SetPosition(lastPosition);

            yield return new WaitForSecondsRealtime(_delay);
        }
    }
}

public interface IObjectMover
{
    float speed { get; }
    void MoveForward();

    void Stop();

    void Rotate(Quaternion quaternion);
}

public interface ICaudateObject
{
    IPositionRepeater tale { get; set; }
}
