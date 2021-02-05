using Bogus;
using System.IO;
using WalletManagerDTO;
using WalletManagerSite.Models;

namespace WalletManagerTestProject.Utils
{
    public static class FakerUtils
    {
        private static Faker _faker = new Faker();
        public static string[] StringArrayFake => new string[] { _faker.Random.String2(2), _faker.Random.String2(2) };

        public static Faker<Response<string[]>> StringResponseFaker => new Faker<Response<string[]>>()
            .RuleFor(field => field.Content, StringArrayFake);
        public static Faker<Response<bool>> BoolResponseFaker => new Faker<Response<bool>>()
            .RuleFor(field => field.Content, true);

        public static Faker<Transaction> GetTransactionDtoFaker => new Faker<Transaction>()
            .RuleFor(field => field.Amount, faker => faker.Random.Decimal())
            .RuleFor(field => field.Category, faker => CategoryFaker)
            .RuleFor(field => field.Compte, faker => faker.Random.String2(2))
            .RuleFor(field => field.Label, faker => faker.Random.String2(2))
            .RuleFor(field => field.OperationDate, faker => faker.Date.Past())
            .RuleFor(field => field.Reference, faker => faker.Random.String2(2));

        public static Faker<WalletManagerSite.Models.TransactionViewModel> GetTransactionViewModelFaker => new Faker<WalletManagerSite.Models.TransactionViewModel>()
            .RuleFor(field => field.Amount, faker => faker.Random.Decimal())
            .RuleFor(field => field.Category, CategoryFaker)
            .RuleFor(field => field.Compte, faker => faker.Random.String2(2))
            .RuleFor(field => field.Label, faker => faker.Random.String2(2))
            .RuleFor(field => field.OperationDate, faker => faker.Date.Past())
            .RuleFor(field => field.Reference, faker => faker.Random.String2(2));

        public static Faker<FileInfo> GetFileInfoFaker => new Faker<FileInfo>()
            .CustomInstantiator(faker => new FileInfo(Path.GetTempFileName()));

        public static Faker<Category> CategoryFaker => new Faker<Category>()
            .RuleFor(field => field.Name, faker => faker.Random.String2(2))
            .RuleFor(field => field.CreationDate, faker => faker.Date.Past());
        
        public static Faker<CategoryViewModel> CategoryViewModelFaker => new Faker<CategoryViewModel>()
            .RuleFor(field => field.CategoryName, faker => faker.Random.String2(2))
            .RuleFor(field => field.CreationDate, faker => faker.Date.Past());
    }
}
