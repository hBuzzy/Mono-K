using TMPro;
using UnityEngine;

public class CookieBasketView : View<int>
{
    [SerializeField] private TMP_Text _cookiesCountText;
    [SerializeField] private CookieBasket _cookieBasket;

    private void OnEnable()
    {
        _cookieBasket.CookieAdded += Render;
    }

    private void OnDisable()
    {
        _cookieBasket.CookieAdded -= Render;
    }

    protected override void Render(int value)
    {
        _cookiesCountText.text = $"x{_cookieBasket.CookiesCount}";
    }
}