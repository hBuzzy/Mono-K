using System;
using UnityEngine;

public class CharacterRespawner : MonoBehaviour
{
    [SerializeField] private CheckPoint _startPoint;
    
    //[SerializeField] private Transform[] _checkpoints;

    private CheckPoint _currentPoint;

    private void Awake()
    {
        _currentPoint = _startPoint;
    }

    private void Update()
    {
        
    }

    public void SetCheckPoint(CheckPoint checkPoint)
    {
        _currentPoint.Uncheck();
        _currentPoint = checkPoint;
    }

    public void MoveToCheckPoint(Character character)
    {
        character.transform.position = _currentPoint.RespawnPoint.position;
    }
}
