using Agro_Express.Dtos.Buyer;

namespace Agro_Express.Dtos.AllBuyers
{
    public class ActiveAndNonActiveBuyer
    {
        public IEnumerable<BuyerDto> ActiveBuyers{get; set; }
        public IEnumerable<BuyerDto> NonActiveBuyers{get; set; }
    }
}