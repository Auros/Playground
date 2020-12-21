using IPA;
using SiraUtil;
using UnityEngine;
using SiraUtil.Zenject;
using Playground.Installers;
using IPALogger = IPA.Logging.Logger;

namespace Playground
{
    [Plugin(RuntimeOptions.DynamicInit)]
    public class Plugin
    {
        [Init]
        public Plugin(IPALogger log, Zenjector zenjector)
        {
            zenjector
                .On<PCAppInit>()
                .Pseudo(Container => Container.BindLoggerAsSiraLogger(log));

            zenjector
                .OnMenu<PlaygroundMenuInstaller>()
                .Expose<BloomFogEnvironment>()
                .Mutate<BloomFogEnvironment>((ctx, _) =>
                {
                    var mat = ctx.GetInjected<MaterialPropertyBlockController>(bc => bc.name == "Note (16)" && bc.transform.parent.name == "LevitatingNote");
                    ctx.Container.Bind<GameObject>().WithId("playground.block.prefab").FromInstance(mat.gameObject).AsSingle();
                });
        }

        [OnEnable, OnDisable]
        public void OnState() { /* State Change */ }
    }
}