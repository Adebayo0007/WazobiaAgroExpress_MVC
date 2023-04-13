using Agro_Express.Dtos.Farmer;

namespace Agro_Express.Dtos.AllFarmers
{
    public class ActiveAndNonActiveFarmer
    {
        public IEnumerable<FarmerDto> ActiveFarmers{get; set; }
        public IEnumerable<FarmerDto> NonActiveFarmers{get; set; }
    }
}