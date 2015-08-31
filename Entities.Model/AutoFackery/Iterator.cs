namespace Entities.Model.AutoFackery
{
    public class Iterator : IIterator
    {
        private int _value;

        public Iterator(int value)
        {
            _value = value;
        }

        public int Value => _value;

        public void Iterate()
        {
            _value = _value + 1;
        }
    }
}