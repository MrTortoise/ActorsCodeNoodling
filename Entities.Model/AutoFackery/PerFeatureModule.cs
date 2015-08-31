using Autofac;

namespace Entities.Model.AutoFackery
{
    public class PerFeatureModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.Register(c => new Iterator(0)).As<IIterator>().InstancePerLifetimeScope();
        }
    }
}