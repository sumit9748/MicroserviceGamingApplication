namespace Play.Catalog.DTOS
{
    public class ItemDTO
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public ItemDTO(Guid id,string name,string description,decimal price)
        {
            Id=id;
            Name=name;
            Description= description;
            Price = price;
        }

    }
}
