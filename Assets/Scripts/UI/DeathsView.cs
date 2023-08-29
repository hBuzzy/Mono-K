using TMPro;
using UnityEngine;

public class DeathsView : View<int>
{
    [SerializeField] private TMP_Text _text;
    [SerializeField] private CharacterDeath _characterDeath;

    private void OnEnable()
    {
        _characterDeath.DeathsCountChanged += Render;
    }

    private void OnDisable()
    {
        _characterDeath.DeathsCountChanged -= Render;
    }

    protected override void Render(int deathsNumber)
    {
        _text.text = $"x{DataConvector.NumberToString(deathsNumber)}";
    }
}