using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSwitcher : MonoBehaviour
{
    public static SceneSwitcher Instance { get; private set; }
    
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void LoadScene(Scenes scene)
    {
        SceneManager.LoadSceneAsync(scene.ToString());
    }

    public enum Scenes
    {
        MainMenu = 0,
        GameLevel = 1
    }
}
