using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = System.Random;

[RequireComponent(typeof(AudioSource))]

public class GameMusic : MonoBehaviour
{
    [SerializeField] private List<AudioClip> _clips;

    private AudioSource _audioSource;
    
    private void Start()
    {
        _audioSource = GetComponent<AudioSource>();
        _audioSource.ignoreListenerPause = true;
        _audioSource.loop = false;
        
        _clips = _clips.OrderBy(clip => new Random().Next()).ToList();
            
        StartCoroutine(PlayMusic());
    }

    private IEnumerator PlayMusic()
    {
        for (int i = 0; i < _clips.Count; i++)
        {
            _audioSource.PlayOneShot(_clips[i]);

            while (_audioSource.isPlaying)
            {
                yield return null;
            }

            if (i == _clips.Count - 1)
                i = 0;
        }
    }
}