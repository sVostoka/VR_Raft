using UnityEngine.SceneManagement;

public class Conductor
{
    public enum Scenes
    {
        MainMenu = 0,
        GameScene = 1,
    }

    public static void ShowScene(Scenes scene, LoadSceneMode loadSceneMode = LoadSceneMode.Single)
    {
        var sceneId = (int)scene;
        SceneManager.LoadScene(sceneId, loadSceneMode);
    }
}
