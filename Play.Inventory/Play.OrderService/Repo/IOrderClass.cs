using Play.OrderService.Entities;

namespace Play.OrderService.Repo
{
    public interface IOrderClass
    {
        Task CreateOrder(Guid userId);
        Task<UserOrder> GetUserOrder(Guid userId);
    }
}
