namespace WalletManagerDTO
{
    public class Response<T>
    {
        public bool HasError => string.IsNullOrEmpty(ErrorsMessage);
        public string ErrorsMessage { get; set; }
        public T Content { get; set; }
    }
}
