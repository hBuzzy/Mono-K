using UnityEngine;

public class MainMenuButton : MonoBehaviour
{
    public void LoadMainMenu()
    {
        SceneSwitcher.Instance.LoadScene(SceneSwitcher.Scenes.MainMenu);
    }
}