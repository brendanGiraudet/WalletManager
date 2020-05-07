
namespace WalletManagerServices.Mapper
{
    public interface IMapper
    {
        WalletManagerSite.Models.TransactionViewModel MapToTransactionViewModel(WalletManagerDTO.Transaction transaction);
    }
}
