using Grpc.Core;
using System.Threading.Tasks;

namespace BlazorInsider.App.Services
{
    public class OrdersService: OrdersManager.OrdersManagerBase
    {
        public override Task<OrderReply> GetNewOrder(OrderRequest request, ServerCallContext context)
        {
            DatabaseService service = new DatabaseService();
            var order = service.GetPendingOrder();
            int orderId = order == null ? 0 : order.OrderId;
            OrderReply reply = new OrderReply
            {
                OrderId = orderId
            };

            return Task.FromResult(reply);
        }
    }
}
