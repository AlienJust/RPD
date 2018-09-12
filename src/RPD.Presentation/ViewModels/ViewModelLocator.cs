using System;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Messaging;
using RPD.DAL;
using Microsoft.Practices.Unity;
using RPD.Presentation.Contracts.Model.SelectionMasks;
using RPD.Presentation.Contracts.Repositories;
using RPD.Presentation.Contracts.ViewModels;
using RPD.Presentation.Contracts.ViewModels.DalViewModels;
using RPD.Presentation.Mocks;
using RPD.Presentation.Mocks.DAL;
using RPD.Presentation.Model.SelectionMasks;
using RPD.Presentation.Repositories;
using RPD.Presentation.Settings;
using RPD.Presentation.Utils.Classes;
using RPD.Presentation.ViewModels.AddDataViewModels;
using RPD.Presentation.ViewModels.AddDataViewModels.DesignTime;
using RPD.Presentation.ViewModels.DalViewModels;

namespace RPD.Presentation.ViewModels
{
	internal class ViewModelLocator: IDisposable

	{
		#region Cвойства, которые будут использоваться для инициализации разных ViewModel.

        // Загрузчик
        readonly ILoader _loader;
        
		#endregion

		readonly IUnityContainer _container;
	    private readonly bool _simpleMode;

	    public ViewModelLocator()
		{
            _container = new UnityContainer();

            _simpleMode = App.IsSimpleMode;

			if (ViewModelBase.IsInDesignModeStatic)
			{
				_container.RegisterType<IMainViewModel, MainViewModelDesign>(new ContainerControlledLifetimeManager());
                _container.RegisterType<IAddDataViewModel, AddDataDesignTime>();
			    _container.RegisterType<ICopyProgressViewModel, CopyProgressDesignTime>();
			    _container.RegisterType<IDeafultColorsViewModel, DeafultColorsDesignTime>();
			}
			else
			{
                _container.RegisterInstance(Messenger.Default);
                
                _container.RegisterType<IApplicationSettings, ApplicationSettings>(new ContainerControlledLifetimeManager());
                _container.RegisterType<ISelectionMasksStorage, SelectionMasksStorage>();
                _container.RegisterType<IUsbRemovableDrives, UsbRemovableDrives>();
                _container.RegisterType<IRepositoryViewModelFactory, RepositoryViewModelFactory>();
			    _container.RegisterType<ISciChartViewModelFactory, SciChartViewModelFactory>();
			    _container.RegisterType<IAboutProgramViewModel, AboutProgramViewModel>();
			    _container.RegisterType<IFtpServersRepository, FtpServersRepository>();
                _container.RegisterType<IDeviceInfoRepository, DeviceInfoRepository>();
                _container.RegisterType<IColorsStorage, ColorsStorage>(new ContainerControlledLifetimeManager());
			    _container.RegisterType<ILogIntegrityViewModel, LogIntegrityViewModel>();
			    _container.RegisterType<IDeviceNumberToPsnConfigurationRepository, DeviceNumberToPsnConfigurationRepository>();

                _container.RegisterType<IChangePsnConfigurationViewModel, ChangePsnConfigurationViewModel>();

			    var settings = _container.Resolve<IApplicationSettings>();

                if (settings.UseMock)
                    _loader = new LoaderMock();
                else
                    _loader = new Loader();

                _container.RegisterType<IAddDataViewModel, AddDataViewModel>();
                
                _container.RegisterInstance(_loader);

                _container.RegisterType<IMainViewModel, MainViewModel>(new ContainerControlledLifetimeManager()); 
                _container.RegisterType<SettingsViewModel>(); 
                
			    _container.RegisterType<ICopyProgressViewModel, CopyProgressViewModel>();
			    _container.RegisterType<ICopyProgressViewModel, ExportProgressViewModel>("ExportProgress");
                _container.RegisterType<IDeafultColorsViewModel, DefualtColorsViewModel>();
			}
		}

		public IMainViewModel Main
		{
			get
			{
                if (ViewModelBase.IsInDesignModeStatic)
                    return _container.Resolve<IMainViewModel>();

			    return _container.Resolve<IMainViewModel>(new ParameterOverride("fileName", 
                    _container.Resolve<IApplicationSettings>().ColorsStorageFullFilePath));
			}
		}

	    public IChangePsnConfigurationViewModel ChangePsnConfiguration 
        {
	        get { return _container.Resolve<IChangePsnConfigurationViewModel>(); }
	    }

	    public ILogIntegrityViewModel LogIntegrity 
        {
	        get { return _container.Resolve<ILogIntegrityViewModel>(); }
	    }

	    public IAddDataViewModel AddDataViewModel
        {
            get
            {
                var ftpDeviceInfoRepository = _container.Resolve<IDeviceInfoRepository>(new ParameterOverride("fileName",
                                            _container.Resolve<IApplicationSettings>().FtpDeviceInfoFullFilePath));

                var deviceToPsnConfigRepo = _container.Resolve<IDeviceNumberToPsnConfigurationRepository>(new ParameterOverride("fileName",
                                                _container.Resolve<IApplicationSettings>().DeviceNumberToPsnConfigFullFilePath));

                if (_simpleMode)
                    return new AddDataViewModel(_container.Resolve<IApplicationSettings>(), 
                        _container.Resolve<ILoader>(),
                        _container.Resolve<IUsbRemovableDrives>(), 
                        _container.Resolve<IFtpServersRepository>(),
                        ftpDeviceInfoRepository,
                        deviceToPsnConfigRepo,
                        true);

                return new AddDataViewModel(_container.Resolve<IApplicationSettings>(), 
                    _container.Resolve<ILoader>(),
                    _container.Resolve<IRepository>(),
                    _container.Resolve<IUsbRemovableDrives>(),
                    _container.Resolve<IFtpServersRepository>(),
                    ftpDeviceInfoRepository,
                    deviceToPsnConfigRepo,
                    false);
            }
        }

		public ICopyProgressViewModel CopyProgress
		{
			get
			{
                return _container.Resolve<ICopyProgressViewModel>();
			}
		}

	    public ICopyProgressViewModel ExportProgress
	    {
            get
            {
                return _container.Resolve<ICopyProgressViewModel>("ExportProgress");
            }
	    }

	    public IAboutProgramViewModel AboutProgram 
        { 
            get
            {
                return _container.Resolve<IAboutProgramViewModel>();
            }
	    }

        public SettingsViewModel Settings
		{
			get
			{
                return _container.Resolve<SettingsViewModel>();
			}
		}

	    public IDeafultColorsViewModel DefaultColors
	    {
	        get { return _container.Resolve<IDeafultColorsViewModel>(); }
	    }

	    public void Dispose()
	    {
	        _loader.Dispose();
            _container.Dispose();
	    }
	}
}