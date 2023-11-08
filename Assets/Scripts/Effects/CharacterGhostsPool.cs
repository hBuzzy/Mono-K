using System.Collections.Generic;
using UnityEngine;

public class CharacterGhostsPool : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private CharacterGhost _characterGhost;
    [SerializeField] private Character _character;
    [SerializeField] private SpriteRenderer _characterSpriteRenderer;
    
    [Header("Settings")]
    [SerializeField] private int _startValue;
    
    private readonly Queue<CharacterGhost> _ghosts = new ();

    private void Start()
    {
        ExtendPool(_startValue);
    }

    public CharacterGhost GetGhost()
    {
        return _ghosts.Dequeue();
    }

    public void Add(CharacterGhost characterGhost)
    {
        _ghosts.Enqueue(characterGhost);
        characterGhost.gameObject.SetActive(false);
    }

    private void ExtendPool(int amount)
    {
        for (int i = 0; i < amount; i++)
        {
            CharacterGhost characterGhostInstance = Instantiate(_characterGhost, transform);
            characterGhostInstance.Init(_character, _characterSpriteRenderer, this);
            
            Add(characterGhostInstance);
        }
    }
}