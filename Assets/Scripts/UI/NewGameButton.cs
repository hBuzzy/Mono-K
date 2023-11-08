using UnityEngine;

public class NewGameButton : MonoBehaviour
{
    public void LoadNewGame()
    {
        SceneSwitcher.Instance.LoadScene(SceneSwitcher.Scenes.GameLevel);
    }
}
