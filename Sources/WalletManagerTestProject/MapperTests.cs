using System;
using System.Collections.Generic;
using System.Linq;
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

        [Fact]
        public void ShouldMapTransactionsDtoToTransactionsViewModel()
        {
            // Arrange
            var transactionsDtoFaker = FakerUtils.GetTransactionDtoFaker.Generate(2);

            // Act
            var transactionsViewModel = _mapper.MapToTransactionsViewModel(transactionsDtoFaker);

            // Assert
            Assert.NotNull(transactionsDtoFaker);
            transactionsDtoFaker.ForEach( t => {
                Assert.Contains(transactionsViewModel, tvm => tvm.Amount.Equals(t.Amount));
                Assert.Contains(transactionsViewModel, tvm => tvm.Category.Equals(t.Category));
                Assert.Contains(transactionsViewModel, tvm => tvm.Compte.Equals(t.Compte));
                Assert.Contains(transactionsViewModel, tvm => tvm.Label.Equals(t.Label));
                Assert.Contains(transactionsViewModel, tvm => tvm.OperationDate.Equals(t.OperationDate));
                Assert.Contains(transactionsViewModel, tvm => tvm.Reference.Equals(t.Reference));
            });
        }

        [Fact]
        public void ShouldThrowExceptionWhenMapToTransactionsViewModelWithEmptyTransactionsDto()
        {
            // Arrange
            IEnumerable<WalletManagerDTO.Transaction> transactionsDtoFaker = null;

            // Act
            Action mapAction = () => _ = _mapper.MapToTransactionsViewModel(transactionsDtoFaker);

            // Assert
            Assert.Throws<ArgumentNullException>(mapAction);
        }

        [Fact]
        public void ShouldMapTransactionDtoToTransactionChartViewModel()
        {
            // Arrange
            var transactionDtoFaker = FakerUtils.GetTransactionDtoFaker.Generate();

            // Act
            var transactionChartViewModel = _mapper.MapToTransactionChartViewModel(transactionDtoFaker);

            // Assert
            Assert.NotNull(transactionDtoFaker);
            Assert.Equal(transactionDtoFaker.Amount, transactionChartViewModel.Amount);
            Assert.Equal(transactionDtoFaker.Category.ToString(), transactionChartViewModel.Category);
        }

        [Fact]
        public void ShouldThrowExceptionWhenMapToTransactionChartViewModelWithEmptyTransactionDto()
        {
            // Arrange
            WalletManagerDTO.Transaction transactionDtoFaker = null;

            // Act
            Action mapAction = () => _ = _mapper.MapToTransactionChartViewModel(transactionDtoFaker);

            // Assert
            Assert.Throws<ArgumentNullException>(mapAction);
        }

        [Fact]
        public void ShouldMapTransactionsDtoToTransactionsChartViewModel()
        {
            // Arrange
            var transactionsDtoFaker = FakerUtils.GetTransactionDtoFaker.Generate(2);

            // Act
            var transactionsViewModel = _mapper.MapToTransactionsChartViewModel(transactionsDtoFaker);

            // Assert
            Assert.NotNull(transactionsDtoFaker);
            transactionsDtoFaker.ForEach(t => {
                Assert.Contains(transactionsViewModel, tvm => tvm.Amount.Equals(t.Amount));
                Assert.Contains(transactionsViewModel, tvm => tvm.Category.Equals(t.Category.ToString()));
            });
        }

        [Fact]
        public void ShouldThrowExceptionWhenMapToTransactionsChartViewModelWithEmptyTransactionsDto()
        {
            // Arrange
            IEnumerable<WalletManagerDTO.Transaction> transactionsDtoFaker = null;

            // Act
            Action mapAction = () => _ = _mapper.MapToTransactionsChartViewModel(transactionsDtoFaker);

            // Assert
            Assert.Throws<ArgumentNullException>(mapAction);
        }
    }
}
