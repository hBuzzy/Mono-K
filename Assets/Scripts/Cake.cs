using System.Collections;
using UnityEngine;

public class Cake : MonoBehaviour
{
    [SerializeField] private AudioSource _takensound;
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
        _takensound.PlayOneShot(_takensound.clip);

        yield return new WaitForSeconds(_takensound.clip.length);
        
        gameObject.SetActive(false);
    }
}
