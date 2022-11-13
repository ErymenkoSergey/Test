using Test.LavaProject.Farm.Mechanica.Score;
using Zenject;

namespace Test.LavaProject.Farm.DI
{
    public class DIInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<SettingGame>().AsSingle().NonLazy();
            Container.Bind<ScoreSystem>().AsSingle();
        }
    }
}