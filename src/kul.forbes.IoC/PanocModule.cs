using Autofac;
using kul.forbes.domain;
using kul.forbes.helpers.domain;
using kul.forbes.helpers.domain.Accelerators;

namespace kul.forbes.IoC
{
    public class PanocModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<LipschitzEstimator>();
            builder.RegisterType<LBFGS>()
                .SingleInstance();
            builder.RegisterType<ProximalGradient>()
                .SingleInstance();
            builder.RegisterType<ForwardBackwardEnvelop>();

            builder.RegisterType<ConfigPanoc>();
            builder.RegisterType<ProxBox>();
            builder.RegisterType<Function>();
        }
    }
}
