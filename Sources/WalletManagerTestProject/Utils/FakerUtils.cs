using Bogus;
using System.IO;

namespace WalletManagerTestProject.Utils
{
    public static class FakerUtils
    {
        public static Faker<WalletManagerDTO.Transaction> GetTransactionDtoFaker => new Faker<WalletManagerDTO.Transaction>()
            .RuleFor(field => field.Amount, faker => faker.Random.Decimal())
            .RuleFor(field => field.Category, faker => faker.Random.String2(2))
            .RuleFor(field => field.Compte, faker => faker.Random.String2(2))
            .RuleFor(field => field.Label, faker => faker.Random.String2(2))
            .RuleFor(field => field.OperationDate, faker => faker.Date.Past())
            .RuleFor(field => field.Reference, faker => faker.Random.String2(2));

        public static Faker<WalletManagerSite.Models.TransactionViewModel> GetTransactionViewModelFaker => new Faker<WalletManagerSite.Models.TransactionViewModel>()
            .RuleFor(field => field.Amount, faker => faker.Random.Decimal())
            .RuleFor(field => field.Category, faker => faker.Random.String2(2))
            .RuleFor(field => field.Compte, faker => faker.Random.String2(2))
            .RuleFor(field => field.Label, faker => faker.Random.String2(2))
            .RuleFor(field => field.OperationDate, faker => faker.Date.Past())
            .RuleFor(field => field.Reference, faker => faker.Random.String2(2));

        public static Faker<FileInfo> GetFileInfoFaker => new Faker<FileInfo>()
            .CustomInstantiator(faker => new FileInfo(Path.GetTempFileName()));
    }
}
