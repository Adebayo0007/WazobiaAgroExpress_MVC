namespace Agro_Express.Dtos.RequestedProduct
{
    public class OrderedAndPending
    {
        public IEnumerable<RequestedProductDto> OrderedProduct{get; set; }
        public IEnumerable<RequestedProductDto> PendingProduct{get; set; }
    }
}