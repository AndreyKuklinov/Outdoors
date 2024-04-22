namespace RedBjorn.Utils
{
    [System.Serializable]
    public class Comparator<T> where T : System.IComparable
    {
        public enum OperatorType
        {
            Equal,
            Greater,
            GreaterOrEqual,
            Less,
            LessOrEqual
        }

        public OperatorType Operator;
        public T Value;

        public bool IsMet(T v)
        {
            switch (Operator)
            {
                case OperatorType.Equal: return v.CompareTo(Value) == 0;
                case OperatorType.Greater: return v.CompareTo(Value) > 0;
                case OperatorType.GreaterOrEqual: return v.CompareTo(Value) >= 0;
                case OperatorType.Less: return v.CompareTo(Value) < 0;
                case OperatorType.LessOrEqual: return v.CompareTo(Value) <= 0;
                default: return false;
            }
        }
    }
}
