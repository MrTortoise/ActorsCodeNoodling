namespace Entities.Model.AutoFackery
{
    public interface IIterator
    {
        int Value { get; }
        void Iterate();
    }
}