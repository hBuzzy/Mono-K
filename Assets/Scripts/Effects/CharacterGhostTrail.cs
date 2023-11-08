using System.Collections;
using UnityEngine;

public class CharacterGhostTrail : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private CharacterGhostsPool _ghostsPool;
    
    [Header("Settings")]
    [SerializeField, Range(2, 10)] private int _ghostsCount;
    [SerializeField, Range(0f, 0.3f)] private float _duration;

    public void Show()
    {
        StartCoroutine(ShowCoroutine());
    }
    
    private IEnumerator ShowCoroutine()
    {
        float delayBetweenGhosts = _duration / (_ghostsCount - 1);
        
        _ghostsPool.GetGhost().Show();
        
        for (int i = 0; i < _ghostsCount - 1; i++)
        {
            yield return new WaitForSeconds(delayBetweenGhosts);
            _ghostsPool.GetGhost().Show();
        }
    }
}