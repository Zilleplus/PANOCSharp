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
            builder.RegisterType<LipschitzEstimator>().AsImplementedInterfaces();
            builder.RegisterType<ProxLocationBuilder>().AsImplementedInterfaces();
            builder.RegisterType<ResidualCalculator>().AsImplementedInterfaces();
            builder.RegisterType<LBFGS>().SingleInstance().AsImplementedInterfaces();
            builder.RegisterType<ProximalGradientCalculator>().AsImplementedInterfaces();
            builder.RegisterType<ForwardBackwardEnvelopCalculator>().AsImplementedInterfaces();
        }
    }
}
