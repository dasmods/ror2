using BepInEx;
using RoR2.UI.MainMenu;
using RoR2.Networking;
using UnityEngine.SceneManagement;

namespace Quisk
{
    [BepInDependency("com.bepis.r2api")]
    [BepInPlugin("com.dasmods.quickstart", "Quisk", "1.1")]
    public class Quisk : BaseUnityPlugin
    {
        private bool loaded = false;

        private static void GoToTitleMenuScreen()
        {
            MainMenuController mainMenuController = FindObjectOfType<MainMenuController>();
            mainMenuController.SetDesiredMenuScreen(mainMenuController.titleMenuScreen);
        }

        private static void GoToSinglePlayerLobby()
        {
            GameNetworkManager gameNetworkManager = FindObjectOfType<GameNetworkManager>();
            GameNetworkManager.HostDescription.HostingParameters hostingParameters = new GameNetworkManager.HostDescription.HostingParameters
            {
                listen = false,
                maxPlayers = 4
            };
            gameNetworkManager.desiredHost = new GameNetworkManager.HostDescription(hostingParameters);
            SceneManager.LoadScene("lobby", LoadSceneMode.Single);
        }

        public void Update()
        {
            if (!loaded)
            {
                GoToTitleMenuScreen(); // skip initial loading screen
                GoToSinglePlayerLobby();
                loaded = true;
            }
        }
    }
}
