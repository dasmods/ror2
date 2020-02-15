using BepInEx;
using RoR2.UI.MainMenu;

namespace Quisk
{
    [BepInDependency("com.bepis.r2api")]
    [BepInPlugin("com.dasmods.quickstart", "Quisk", "1.0")]
    public class Quisk : BaseUnityPlugin
    {
        private bool loaded = false;

        private static void GotoTitleMenuScreen()
        {
            MainMenuController mainMenuController = FindObjectOfType<MainMenuController>();
            mainMenuController.SetDesiredMenuScreen(mainMenuController.titleMenuScreen);
        }

        public void Update()
        {
            if (!loaded)
            {
                GotoTitleMenuScreen();
                loaded = true;
            }
        }
    }
}
