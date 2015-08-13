namespace Entities
{
    /// <summary>
    /// Represents a stack of resources
    /// </summary>
    public class ResourceStack
    {
        public ResourceStack(IResource resource, int quantity)
        {
            Resource = resource;
            Quantity = quantity;
        }

        public IResource Resource { get; }
        public int Quantity { get; }
    }
}