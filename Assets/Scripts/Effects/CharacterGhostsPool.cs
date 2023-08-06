using System.Collections.Generic;
using UnityEngine;

public class CharacterGhostsPool : MonoBehaviour
{
    public static CharacterGhostsPool Instance { get; private set; }
    
    [SerializeField] private CharacterGhost _characterGhost;
    [SerializeField] private Character _character;
    
    [SerializeField] private int _startValue;
    [SerializeField] private int _extendValue;
    
    private readonly Queue<CharacterGhost> _ghosts = new Queue<CharacterGhost>();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        ExtendPool(_startValue);
    }

    public CharacterGhost GetGhost()
    {
        if (_ghosts.Count == 0)
        {
            ExtendPool(_extendValue);
        }
        
        return _ghosts.Dequeue();
    }
    
    public void Add(CharacterGhost characterGhost)
    {
        characterGhost.gameObject.SetActive(false);
        _ghosts.Enqueue(characterGhost);
    }

    private void ExtendPool(int amount)//TOdo: maybe creat only large wthout adding new?
    {
        for (int i = 0; i < amount; i++)
        {
            CharacterGhost characterGhostInstance = Instantiate(_characterGhost, transform, true);
            characterGhostInstance.Init(_character);
            
            Add(characterGhostInstance);
        }
    }
}