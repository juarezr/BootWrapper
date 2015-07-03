namespace BootWrapper.Mvc.Formatter
{
    public static class NumberUtils
    {
        public static bool IsEven(this int number)
        {
            return number % 2 == 0;
        }

        public static bool IsOdd(this int number)
        {
            return !number.IsEven();
        }

        public static bool Between(this int number, int min, int max)
        {
            return (min <= number) && (number <= max);
        }

        public static bool NotBetween(this int number, int min, int max)
        {
            return (number < min) || (max < number);
        }

        public static bool In(this int number, params int[] values)
        {
            foreach (int x in values)
                if (number == x)
                    return true;
            return false;
        }

        public static bool NotIn(this int number, params int[] values)
        {
            return !number.In(values);
        }
    }
}
