using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class GhostTrailPool : MonoBehaviour
{
    static public GhostTrailPool Instance { get; private set; }
    
    [SerializeField] private GhostTrail _ghost;
    [SerializeField] private Character _character;
    [SerializeField] private int _startValue;
    [SerializeField] private int _extendValue;
    
    private readonly Queue<GhostTrail> _ghosts = new Queue<GhostTrail>();

    private void Start()
    {
        Instance = this;
        ExtendPool(_startValue);
    }

    public GhostTrail GetGhost()
    {
        if (_ghosts.Count == 0)
        {
            ExtendPool(_extendValue);
        }
        
        return _ghosts.Dequeue();
    }
    
    public void Add(GhostTrail ghost)
    {
        ghost.gameObject.SetActive(false);
        _ghosts.Enqueue(ghost);
    }

    private void ExtendPool(int amount)//TOdo: maybe creat only large wthout adding new?
    {
        for (int i = 0; i < amount; i++)
        {
            GhostTrail ghostInstance = Instantiate(_ghost, transform, true);
            ghostInstance.Init(_character, gameObject.layer);
            Add(ghostInstance);
        }
    }
}