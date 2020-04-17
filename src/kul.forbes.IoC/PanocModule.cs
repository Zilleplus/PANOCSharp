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
            builder.RegisterType<LocationBuilder>();
            builder.RegisterType<LipschitzEstimator>().AsImplementedInterfaces();
            builder.RegisterType<ProxLocationBuilder>();
            builder.RegisterType<ProximalGradientCalculator>();
            builder.RegisterType<LBFGS>().SingleInstance().AsImplementedInterfaces();
            builder.RegisterType<Panoc>().AsImplementedInterfaces();
        }
    }
}
