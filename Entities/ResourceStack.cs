using System;

namespace Entities
{
   /// <summary>
   ///    Represents a stack of resources
   /// </summary>
   public class ResourceStack : IEquatable<ResourceStack>
   {
      public ResourceStack(IResource resource, int quantity)
      {
         if (resource == null) throw new ArgumentNullException(nameof(resource));
         Resource = resource;
         Quantity = quantity;
      }

      public IResource Resource { get; }
      public int Quantity { get; }

      public bool Equals(ResourceStack other)
      {
         if (ReferenceEquals(null, other)) return false;
         if (ReferenceEquals(this, other)) return true;
         return Resource.Equals(other.Resource) && Quantity == other.Quantity;
      }

      public override int GetHashCode()
      {
         unchecked
         {
            return (Resource.GetHashCode()*397) ^ Quantity;
         }
      }

      public static bool operator ==(ResourceStack left, ResourceStack right)
      {
         return Equals(left, right);
      }

      public static bool operator !=(ResourceStack left, ResourceStack right)
      {
         return !Equals(left, right);
      }

      public override bool Equals(object obj)
      {
         if (ReferenceEquals(null, obj)) return false;
         if (ReferenceEquals(this, obj)) return true;
         if (obj.GetType() != GetType()) return false;
         return Equals((ResourceStack) obj);
      }
   }
}