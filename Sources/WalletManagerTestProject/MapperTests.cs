using System;
using WalletManagerServices.Mapper;
using WalletManagerTestProject.Utils;
using Xunit;

namespace WalletManagerTestProject
{
    public class MapperTests
    {
        readonly IMapper _mapper;

        public MapperTests()
        {
            _mapper = new Mapper();
        }

        [Fact]
        public void ShouldMapTransactionDtoToTransactionViewModel()
        {
            // Arrange
            var transactionDtoFaker = FakerUtils.GetTransactionDtoFaker.Generate();

            // Act
            var transactionViewModel = _mapper.MapToTransactionViewModel(transactionDtoFaker);

            // Assert
            Assert.NotNull(transactionDtoFaker);
            Assert.Equal(transactionDtoFaker.Amount, transactionViewModel.Amount);
            Assert.Equal(transactionDtoFaker.Category, transactionViewModel.Category);
            Assert.Equal(transactionDtoFaker.Compte, transactionViewModel.Compte);
            Assert.Equal(transactionDtoFaker.Label, transactionViewModel.Label);
            Assert.Equal(transactionDtoFaker.OperationDate, transactionViewModel.OperationDate);
            Assert.Equal(transactionDtoFaker.Reference, transactionViewModel.Reference);
        }

        [Fact]
        public void ShouldThrowExceptionWhenMapToTransactionViewModelWithEmptyTransactionDto()
        {
            // Arrange
            WalletManagerDTO.Transaction transactionDtoFaker = null;

            // Act
            Action mapAction =  () => _ = _mapper.MapToTransactionViewModel(transactionDtoFaker);

            // Assert
            Assert.Throws<ArgumentNullException>(mapAction);
        }
    }
}
