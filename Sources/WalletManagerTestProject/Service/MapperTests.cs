using System;
using System.Collections.Generic;
using System.IO;
using WalletManagerDTO;
using WalletManagerSite.Models;
using WalletManagerSite.Tools.Mapper;
using WalletManagerTestProject.Utils;
using Xunit;

namespace WalletManagerTestProject.Service
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
            Assert.NotNull(transactionViewModel);
            Assert.Equal(transactionDtoFaker.Amount, transactionViewModel.Amount);
            Assert.Equal(transactionDtoFaker.Category.Name, transactionViewModel.Category.Name);
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
            Action mapAction = () => _ = _mapper.MapToTransactionViewModel(transactionDtoFaker);

            // Assert
            Assert.Throws<ArgumentNullException>(mapAction);
        }

        [Fact]
        public void ShouldMapTransactionsDtoToTransactionsViewModel()
        {
            // Arrange
            var transactionsDtoFaker = FakerUtils.GetTransactionDtoFaker.Generate(2);

            // Act
            var transactionsViewModel = _mapper.MapToTransactionViewModels(transactionsDtoFaker);

            // Assert
            Assert.NotNull(transactionsDtoFaker);
            transactionsDtoFaker.ForEach(t =>
            {
                Assert.Contains(transactionsViewModel, tvm => tvm.Amount.Equals(t.Amount));
                Assert.Contains(transactionsViewModel, tvm => tvm.Category.Name.Equals(t.Category.Name));
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
            Action mapAction = () => _ = _mapper.MapToTransactionViewModels(transactionsDtoFaker);

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
            Assert.Equal(transactionDtoFaker.Category.Name, transactionChartViewModel.Category);
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
            transactionsDtoFaker.ForEach(t =>
            {
                Assert.Contains(transactionsViewModel, tvm => tvm.Amount.Equals(t.Amount));
                Assert.Contains(transactionsViewModel, tvm => tvm.Category.Equals(t.Category.Name));
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

        [Fact]
        public void ShouldMapTransactionViewModelToTransactionDto()
        {
            // Arrange
            var transactionViewModelFake = FakerUtils.GetTransactionViewModelFaker.Generate();

            // Act
            var transactionDto = _mapper.MapToTransactionDto(transactionViewModelFake);

            // Assert
            Assert.NotNull(transactionDto);
            Assert.Equal(transactionViewModelFake.Amount, transactionDto.Amount);
            Assert.Equal(transactionViewModelFake.Category.Name, transactionDto.Category.Name);
            Assert.Equal(transactionViewModelFake.Compte, transactionDto.Compte);
            Assert.Equal(transactionViewModelFake.Label, transactionDto.Label);
            Assert.Equal(transactionViewModelFake.OperationDate, transactionDto.OperationDate);
            Assert.Equal(transactionViewModelFake.Reference, transactionDto.Reference);
        }

        [Fact]
        public void ShouldThrowExceptionWhenMapToTransactionDtoWithEmptyTransactionViewModel()
        {
            // Arrange
            WalletManagerSite.Models.TransactionViewModel transactionViewModelFake = null;

            // Act
            Action mapAction = () => _ = _mapper.MapToTransactionDto(transactionViewModelFake);

            // Assert
            Assert.Throws<ArgumentNullException>(mapAction);
        }

        [Fact]
        public void ShouldMapTransactionViewModelListToTransactionsViewModel()
        {
            // Arrange
            var transactionViewModelListFake = FakerUtils.GetTransactionViewModelFaker.Generate(2);

            // Act
            var transactionsViewModel = _mapper.MapToTransactionsViewModel(transactionViewModelListFake);

            // Assert
            Assert.NotNull(transactionsViewModel);

            transactionViewModelListFake.ForEach(t =>
            {
                Assert.Contains(transactionsViewModel.Transactions, tvm => tvm.Amount.Equals(t.Amount));
                Assert.Contains(transactionsViewModel.Transactions, tvm => tvm.Category.Name.Equals(t.Category.Name));
                Assert.Contains(transactionsViewModel.Transactions, tvm => tvm.Compte.Equals(t.Compte));
                Assert.Contains(transactionsViewModel.Transactions, tvm => tvm.Label.Equals(t.Label));
                Assert.Contains(transactionsViewModel.Transactions, tvm => tvm.OperationDate.Equals(t.OperationDate));
                Assert.Contains(transactionsViewModel.Transactions, tvm => tvm.Reference.Equals(t.Reference));
            });
        }

        [Fact]
        public void ShouldThrowExceptionWhenMapToTransactionsViewModelWithEmptyTransactionViewModelList()
        {
            // Arrange
            List<WalletManagerSite.Models.TransactionViewModel> transactionViewModelListFake = null;

            // Act
            Action mapAction = () => _ = _mapper.MapToTransactionsViewModel(transactionViewModelListFake);

            // Assert
            Assert.Throws<ArgumentNullException>(mapAction);
        }

        [Fact]
        public void ShouldMapFileInfoToCsvFileViewModel()
        {
            // Arrange
            var fileInfoFake = FakerUtils.GetFileInfoFaker.Generate();

            // Act
            var csvFileViewModel = _mapper.MapToCsvFileViewModel(fileInfoFake);

            // Assert
            Assert.NotNull(csvFileViewModel);
            Assert.Equal(fileInfoFake.CreationTime, csvFileViewModel.CreatedDate);
            Assert.Equal(fileInfoFake.Name, csvFileViewModel.FileName);
            Assert.Equal(fileInfoFake.FullName, csvFileViewModel.FullPath);
            Assert.Equal(fileInfoFake.LastWriteTime, csvFileViewModel.UpdateDate);
        }

        [Fact]
        public void ShouldThrowExceptionWhenMapCsvFileViewModelWithEmptyFileInfo()
        {
            // Arrange
            FileInfo fileInfoFake = null;

            // Act
            Action mapAction = () => _ = _mapper.MapToCsvFileViewModel(fileInfoFake);

            // Assert
            Assert.Throws<ArgumentNullException>(mapAction);
        }

        #region MapToCategoryViewModel
        [Fact]
        public void ShouldMapCategoryToViewModel()
        {
            // Arrange
            var fakeCategory = FakerUtils.CategoryFaker.Generate();

            // Act
            var categoryViewModel = _mapper.MapToCategoryViewModel(fakeCategory);

            // Assert
            Assert.NotNull(categoryViewModel);
            Assert.Equal(fakeCategory.Name, categoryViewModel.CategoryName);
            Assert.Equal(fakeCategory.CreationDate, categoryViewModel.CreationDate);
        }

        [Fact]
        public void ShouldThrowExceptionWhendMapCategoryToViewModelWithEmptyCategory()
        {
            // Arrange
            Category fakeCategory = null;

            // Act
            Action mapAction = () => _ = _mapper.MapToCategoryViewModel(fakeCategory);

            // Assert
            Assert.Throws<ArgumentNullException>(mapAction);
        }
        #endregion
        
        #region MapToCategory
        [Fact]
        public void ShouldMapCategoryViewModelToCategory()
        {
            // Arrange
            var fakeCategoryViewModel = FakerUtils.CategoryViewModelFaker.Generate();

            // Act
            var category = _mapper.MapToCategory(fakeCategoryViewModel);

            // Assert
            Assert.NotNull(category);
            Assert.Equal(fakeCategoryViewModel.CategoryName, category.Name);
            Assert.Equal(fakeCategoryViewModel.CreationDate, category.CreationDate);
        }

        [Fact]
        public void ShouldThrowExceptionWhendMapCategoryViewModelToCategoryWithEmptyCategoryViewModel()
        {
            // Arrange
            CategoryViewModel fakeCategoryViewModel = null;

            // Act
            Action mapAction = () => _ = _mapper.MapToCategory(fakeCategoryViewModel);

            // Assert
            Assert.Throws<ArgumentNullException>(mapAction);
        }
        #endregion
    }
}
