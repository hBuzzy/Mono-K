using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SoundSettings : MonoBehaviour
{
    [SerializeField] private AudioMixer _mixer;
    
    [Header("Volume sliders")]
    [SerializeField] private Slider _effectsSlider;
    [SerializeField] private Slider _musicSlider;
    [SerializeField] private Slider _ambientSlider;

    private const string MusicVolume = nameof(MusicVolume);
    private const string EffectsVolume = nameof(EffectsVolume);
    private const string AmbientVolume = nameof(AmbientVolume);

    private const float MinVolume = -80f;
    private const float MaxVolume = 0f;

    private static SoundSettings _instance;

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void OnEnable()
    {
        _musicSlider.onValueChanged.AddListener(OnMusicChanged);
        _ambientSlider.onValueChanged.AddListener(OnAmbientChanged);
        _effectsSlider.onValueChanged.AddListener(OnEffectsChanged);

        GamePause.Instance.PauseChanged += OnGamePauseChanged;
    }

    private void OnDisable()
    {
        _musicSlider.onValueChanged.RemoveListener(OnMusicChanged);
        _ambientSlider.onValueChanged.RemoveListener(OnAmbientChanged);
        _effectsSlider.onValueChanged.RemoveListener(OnEffectsChanged);

        GamePause.Instance.PauseChanged -= OnGamePauseChanged;
    }

    private void SetVolume(string volumeName, float volume)
    {
        _mixer.SetFloat(volumeName, Mathf.Lerp(MinVolume, MaxVolume, volume));
    }

    private void OnMusicChanged(float volume)
    {
        SetVolume(MusicVolume, volume);
    }

    private void OnAmbientChanged(float volume)
    {
        SetVolume(AmbientVolume, volume);
    }

    private void OnEffectsChanged(float volume)
    {
        SetVolume(EffectsVolume, volume);
    }

    private void OnGamePauseChanged(bool isPaused)
    {
        AudioListener.pause = isPaused;
    }
}
