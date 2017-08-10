namespace OrderBookInterview
{
    public struct DiffS
    {
        public double Price;
        public double Value;
        public int Position;

        public DiffS(double price, double value, int position)
        {
            Price = price;
            Value = value;
            Position = position;
        }
    }
}
