namespace Agro_Express.Dtos
{
    public class BaseResponse<T>
    {
        public bool IsSucess {get;set;}
        public string Message {get;set;}
        public T Data {get;set;}
        
    }
}