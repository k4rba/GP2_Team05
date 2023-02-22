using UnityEngine;
using UnityEngine.SceneManagement;

namespace Andreas.Scripts
{
    public class MenuManager : MonoBehaviour
    {
        public void StartGame()
        {
            SceneManager.LoadScene("MainScene");
        }

        public void QuitGame()
        {
            Application.Quit();
        }
    }
}
