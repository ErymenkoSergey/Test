using Test.LavaProject.Farm.Mechanica;
using Test.LavaProject.Farm.Mechanica.MainProcess.Game;
using Test.LavaProject.Farm.Mechanica_Spawner;
using Test.LavaProject.Farm.Mechanica_Spawner.Planting;
using Test.LavaProject.Farm.Mechanica_UI;
using Test.LavaProject.Farm.UI;

namespace Test.LavaProject.Farm.DI
{
    public class SettingGame
    {
        public GameProcess GameProcess;
        public Spawner Spawner;
        public UIManager UIManager;
        public UIPresentor UIPresentor;
        public CameraControl CameraControl;
        public PlantingSystem PlantingSystem;
    }
}