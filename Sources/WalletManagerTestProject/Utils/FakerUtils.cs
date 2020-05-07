using Bogus;

namespace WalletManagerTestProject.Utils
{
    public static class FakerUtils
    {
        public static Faker<WalletManagerDTO.Transaction> GetTransactionDtoFaker => new Faker<WalletManagerDTO.Transaction>()
            .RuleFor(field => field.Amount, faker => faker.Random.Double())
            .RuleFor(field => field.Category, faker => faker.Random.Enum<WalletManagerDTO.Enumerations.TransactionCategory>())
            .RuleFor(field => field.Compte, faker => faker.Random.String2(2))
            .RuleFor(field => field.Label, faker => faker.Random.String2(2))
            .RuleFor(field => field.OperationDate, faker => faker.Date.Past())
            .RuleFor(field => field.Reference, faker => faker.Random.String2(2));
    }
}
