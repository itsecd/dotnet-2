using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecursivePositionRepeater : MonoBehaviour, IRecursivePositionRepeater, ICaudateObject
{
    public void SetNextRepeater(IPositionRepeater repeater)
    {
        if (repeater == null || repeater == _nextRepeater)
            return;

        _nextRepeater = repeater;
    }

    public void SetPosition(Vector3 position)
    {
        var lastPosition = transform.position;

        transform.position = position;

        if (_nextRepeater != null)
            _nextRepeater.SetPosition(lastPosition);
    }

    private IPositionRepeater _nextRepeater;

    [SerializeField]
    private RecursivePositionRepeater _tale;

    public IPositionRepeater tale { get => _nextRepeater; set => _nextRepeater = value; }

    private void Start()
    {
        if (_tale != null)
            SetNextRepeater(_tale);
    }
}

public interface IPositionRepeater
{
    void SetPosition(Vector3 position);
}

public interface IRecursivePositionRepeater : IPositionRepeater
{
    void SetNextRepeater(IPositionRepeater repeater);
}

