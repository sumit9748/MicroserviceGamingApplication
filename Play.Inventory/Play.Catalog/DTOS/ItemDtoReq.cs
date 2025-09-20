namespace Play.Catalog.DTOS
{
    public class ItemDtoReq
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public ItemDtoReq( string name, string description, decimal price)
        {
            Name = name;
            Description = description;
            Price = price;
        }
    }
}
