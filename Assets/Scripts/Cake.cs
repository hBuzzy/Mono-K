using System.Collections;
using UnityEngine;

public class Cake : MonoBehaviour
{
    [SerializeField] private AudioSource _takeSound;
    [SerializeField] private Transform _cakeImage;
    [SerializeField, Range(0f, 0.5f)] private float _waitBeforeDisappear;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.TryGetComponent(out PieBasket pieBasket))
        {
            pieBasket.AddPie();
            StartCoroutine(Take());
        }
    }

    private IEnumerator Take()
    {
        _takeSound.PlayOneShot(_takeSound.clip);

        yield return new WaitForSeconds(_waitBeforeDisappear);
        
        _cakeImage.gameObject.SetActive(false);

        yield return new WaitForSeconds(_takeSound.clip.length);

        gameObject.SetActive(false);
    }
}
