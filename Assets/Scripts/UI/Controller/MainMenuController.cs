using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace TowerDeffence.UI.Controller
{
    public class MainMenuController : MonoBehaviour
    {
        public void LoadScene(string sceneName)
        {
            SceneManager.LoadScene(sceneName);
        }

        public void Quit()
        {
            Application.Quit();
        }
    }
}
