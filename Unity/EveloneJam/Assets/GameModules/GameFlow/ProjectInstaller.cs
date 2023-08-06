using Project.GameFlow;
using Project.Interaction;
using Zenject;

namespace Project.Game
{
    public class ProjectInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<GlobalMoney>().ToSelf().FromNew().AsSingle();
            Container.Bind<SkinStore>().ToSelf().FromNew().AsSingle();
            Container.Bind<SkinSettings>().ToSelf().FromNew().AsSingle();
        }
    }
}
