using PayStack.Net;
namespace Agro_Express.PayStack
{
    public class PayStackApiClass
    {
        public ITransactionsApi _transactionApi;
        public PayStackApiClass(ITransactionsApi transactionApi)
        {
            _transactionApi = transactionApi;
        }
        public void Payment(string email,int amountinKobo)
        {
            // var response = _transactionApi.Initialize("tg@gmail.com",2,null,false,"NGN",null);
            // if(response.Status)
            // {
            //     response.Data.
            // }
            var _api = new PayStackApi("sk_test_1dacd4891d686e4c9f616a96a1e868138cb9d067");
            var result = _api.Post<ApiResponse<dynamic>, dynamic>("transaction/initialize", new {
                amount = 5000000,  // i.e 50,000.00 NGN
                email = "johnwilson5864@gmail.com",
                currency = "NGN",
                reference = "",
            });
            if(result.Status)
            {
                result.Data.Authorization_url();
            }
            else{
                var message = result.Message;
            }
        }
    }
}