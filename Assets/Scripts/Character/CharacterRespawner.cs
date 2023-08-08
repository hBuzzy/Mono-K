using UnityEngine;

public class CharacterRespawner : MonoBehaviour
{
    public static CharacterRespawner Instance { get; private set; }
    
    [SerializeField] private CheckPoint _startPoint;
    [SerializeField] private Character _character;
    
    private CheckPoint _currentPoint;

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
        //MoveCharacterToCheckPoint();
        _currentPoint.Check(false);
    }

    public void SetCheckPoint(CheckPoint checkPoint)
    {
        _currentPoint.Uncheck();
        _currentPoint = checkPoint;
    }

    public void MoveCharacterToCheckPoint()
    {
        _character.SetPosition(_currentPoint.RespawnPoint.position);
    }
}
