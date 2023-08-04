using UnityEngine;
using UnityEngine.SceneManagement;

public class LoaderManager : MonoBehaviour
{
    public void MainScene()
    {
        SceneManager.LoadScene("KartTestScene");
    }

    public void Menu()
    {
        SceneManager.LoadScene("MainMenu");
    }

}
