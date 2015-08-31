using Autofac;

namespace Entities.Model.AutoFackery
{
    public class PerScenarioModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.Register(c => new Iterator(0)).As<IIterator>();
        }
    }
}