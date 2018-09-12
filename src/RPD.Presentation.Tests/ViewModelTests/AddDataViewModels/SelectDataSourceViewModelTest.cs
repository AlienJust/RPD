using NUnit.Framework;
using RPD.Presentation.Messages;
using RPD.Presentation.ViewModels.AddDataViewModels;

namespace RPD.Presentation.Tests.ViewModelTests.AddDataViewModels
{
    [TestFixture]
    class SelectDataSourceViewModelTest
    {
        private ISelectDataSourceViewModel _viewModel;

        [Test]
        public void ReadFromFlashCommand_NavigatesToSelectDevicePage()
        {
            bool correct = false;

            _viewModel = new SelectDataSourceViewModel(m => correct = m == AddDataWindowMessages.SelectUsbDevicePage, true);
            _viewModel.ReadFromFlashCommand.Execute(null);

            Assert.True(correct);
        }

        [Test]
        public void ReadFromImageCommand_NavigatesToSelectImagePage()
        {
            bool correct = false;

            _viewModel = new SelectDataSourceViewModel(m => correct = m == AddDataWindowMessages.SelectZippedRepositoryPage, true);
            _viewModel.ReadFromImageCommand.Execute(null);

            Assert.True(correct);
        }

        [Test]
        public void ReadFromFolderCommand_NavigatesSelectFolderPage()
        {
            bool correct = false;

            _viewModel = new SelectDataSourceViewModel(m => correct = m == AddDataWindowMessages.SelectFolderPage, true);
            _viewModel.ReadFromFolderCommand.Execute(null);

            Assert.True(correct);
        }
    }
}
