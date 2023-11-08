using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

public class DeathsView : View<int>
{ 
    [SerializeField] private TMP_Text _deathsCountText;
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
        _deathsCountText.text = $"x{DataConvector.NumberToString(deathsNumber)}";
    }
}