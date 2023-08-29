using System.Collections;
using UnityEngine;

public class CharacterRespawner : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private Character _character;
    [SerializeField] private CheckPoint _startPoint;
    
    [Header("Death screen")]
    [SerializeField] private DeathScreen _deathScreen;
    [SerializeField, Range(0f, 1.5f)] private float _deathScreenDuration;
    
    private CheckPoint _currentPoint;
    
    public static CharacterRespawner Instance { get; private set; }

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
        _currentPoint = _startPoint;
        //TODO: MoveCharacterToCheckPoint();
        _currentPoint.Check(false);
    }

    public void SetCheckPoint(CheckPoint checkPoint)
    {
        _currentPoint.Uncheck();
        _currentPoint = checkPoint;
    }

    public IEnumerator RespawnCharacter()
    {
        yield return _deathScreen.Show();
        
        _character.SetPosition(_currentPoint.RespawnPoint.position);

        yield return new WaitForSeconds(_deathScreenDuration);

        yield return _deathScreen.Hide();
    }
}
