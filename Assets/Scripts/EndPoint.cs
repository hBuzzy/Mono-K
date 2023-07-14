using UnityEngine;

public class EndPoint : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out Character character))
        {
            SceneSwitcher.Instance.NextLevel();
        }
    }
}
