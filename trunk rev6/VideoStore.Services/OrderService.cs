using Common;
using VideoStore.Services.Interfaces;
using VideoStore.Business.Components.Interfaces;


namespace VideoStore.Services
{
    public class OrderService : IOrderService
    {

        private IOrderProvider OrderProvider
        {
            get
            {
                return ServiceFactory.GetService<IOrderProvider>();
            }
        }

        public void SubmitOrder(Business.Entities.Order pOrder)
        {
            OrderProvider.SubmitOrder(pOrder);
        }
    }
}
