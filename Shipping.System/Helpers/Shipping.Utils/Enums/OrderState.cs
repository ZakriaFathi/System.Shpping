using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shipping.Utils.Enums
{
    public enum OrderState
    {
        Pending = 1,
        InTheWarehouse,
        DeliveredToTheRepresentative,
        Delivered,
        Returning,
        ReturnInTheWarehouse,
        ReturnInCustomer,
    }
    public enum OrderStateEmployee
    {
        DeliveredToTheRepresentative = 1,
        ReturnInTheWarehouse,
        ReturnInCustomer,
    }  
    public enum OrderStateRepresentative
    {
        Delivered = 1,
        Returning,
    }
}
