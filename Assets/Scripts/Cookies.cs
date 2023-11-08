using UnityEngine;

public class Cookies : MonoBehaviour
{
    private int _cookiesCount;

    public int CookiesCount => _cookiesCount;
    
    private void Start()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            var child = transform.GetChild(i);

            if (child.GetComponentInChildren(typeof(Cookie)) != null)
                _cookiesCount++;
        }
    }
}