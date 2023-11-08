using UnityEngine;
using UnityEngine.Rendering.Universal;

public class CheckPoint : MonoBehaviour
{
    [Header("Light options")]
    [SerializeField] private Light2D _light;
    [SerializeField, Range(0f, 5f)] private float _checkedLightIntensity;
    [SerializeField, Range(0f, 5f)] private float _uncheckedLightIntensity;
    
    [Header("Respawn")]
    [SerializeField] private Transform _respawnPoint;
    
    [Header("Sounds")] 
    [SerializeField] private AudioSource _activationSound;
    
    private bool _isChecked;
    
    public Transform RespawnPoint => _respawnPoint;

    private void Start()
    { 
        SetLightIntensity();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (_isChecked)
            return;
        
        if (other.TryGetComponent(out Character character))
        {
            Check();
            CharacterRespawner.Instance.SetCheckPoint(this);
        }
    }

    public void Uncheck()
    {
        _isChecked = false;
        SetLightIntensity();
    }

    public void Check(bool isSoundRequired = true)
    {
        _isChecked = true; 
        SetLightIntensity();
        
        if (isSoundRequired)
            _activationSound.PlayOneShot(_activationSound.clip);
    }

    private void SetLightIntensity()
    {
        _light.intensity = _isChecked ? _checkedLightIntensity : _uncheckedLightIntensity;
    }
}
