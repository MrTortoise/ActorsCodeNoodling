using Autofac;

namespace Entities.Model.AutoFackery
{
    public class AutoFackeryState
    {
        public AutoFackeryState(ContainerBuilder builder)
        {
            Builder = builder;
        }

        public ContainerBuilder Builder { get; set; }
        public IContainer Container { get; set; }
        public ILifetimeScope Scope { get; set; }
    }
}