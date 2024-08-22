using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shipping.Utils.Enums
{
    public enum OrderState
    {
        Canceled = 0,
        New = 1,
        Pending,
        InTheWarehouse,
        DeliveredToTheRepresentative,
        Delivered
    }
    public enum OrderStateEmployee
    {
        InTheWarehouse,
        DeliveredToTheRepresentative,
    }
}
