using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSwitcher : MonoBehaviour
{
    public static SceneSwitcher Instance;
    
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

    public void NextLevel()
    {
        SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void LoadScene(string sceneName)
    {
        // var scene = SceneManager.LoadSceneAsync(sceneName);
        // scene.allowSceneActivation = false;

        SceneManager.LoadSceneAsync(sceneName);
    }
}
