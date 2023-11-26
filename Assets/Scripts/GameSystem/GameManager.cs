using System;
using UnityEngine;
using UnityEngine.Serialization;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private int _killCounter;

    [SerializeField]
    private float _elapsedTime;

    private void Start()
    {
        _killCounter = 0;
        _elapsedTime = 0f;
    }

    private void Update()
    {
        _elapsedTime += Time.deltaTime;
    }

    public void AddKill()
    {
        _killCounter++;
    }
}
