using log4net;
using System;
using System.Collections;
using System.ComponentModel;
using System.IO;
using System.Windows.Input;

namespace ZeroEditorRedux
{
    internal class CreateWorldViewModel : ViewModelBase, INotifyDataErrorInfo
    {
        private static readonly ILog logger = LogManager.GetLogger(nameof(CreateWorldViewModel));

        #region INotifyDataErrorInfo

        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;

        public bool HasErrors => throw new NotImplementedException();

        public IEnumerable GetErrors(string propertyName)
        {
            throw new NotImplementedException();
        }

        #endregion INotifyDataErrorInfo

        #region Properties

        private string _worldName;

        private string WorldName
        {
            get { return _worldName; }
            set { SetProperty(ref _worldName, value); }
        }

        private string _worldAbbreviation;

        private string WorldAbbreviation
        {
            get { return _worldAbbreviation; }
            set { SetProperty(ref _worldAbbreviation, value); }
        }

        private string _worldDescription;

        private string WorldDescription
        {
            get { return _worldDescription; }
            set { SetProperty(ref _worldDescription, value); }
        }

        private bool _isSpaceMap;

        private bool IsSpaceMap
        {
            get { return _isSpaceMap; }
            set { SetProperty(ref _isSpaceMap, value); }
        }

        private bool _conquest;

        private bool Conquest
        {
            get { return _conquest; }
            set { SetProperty(ref _conquest, value); }
        }

        private bool _twoFlagCTF;

        private bool TwoFlagCTF
        {
            get { return _twoFlagCTF; }
            set { SetProperty(ref _twoFlagCTF, value); }
        }

        private bool _oneFlagCTF;

        private bool OneFlagCTF
        {
            get { return _oneFlagCTF; }
            set { SetProperty(ref _oneFlagCTF, value); }
        }

        private bool _heroAssault;

        private bool HeroAssault
        {
            get { return _heroAssault; }
            set { SetProperty(ref _heroAssault, value); }
        }

        #endregion Properties

        private ICommand CreateWorldCommand { get; }

        public CreateWorldViewModel()
        {
            CreateWorldCommand = new RelayCommand(new System.Action<object>(CreateWorld));
        }

        private void CreateWorld(object parameter)
        {
            ValidateInput();
            ValidateAndCopyDirectory();

            /*
             Copying "data" folder to "data_AAA" - this may, and indeed *will *, take a while...done.
             Creating new world for "AAA"...
             Creating munged data folder...done.
             Creating world data folders...done.
             Copying template \Effects folder...done.
             Copying template \MSH folder...done.
             Copying template \ODF folder...done.
             Copying template world files to new world folder...done.
             Copying template files for conquest mode...done.
             Rewriting LDX file...done.
             Rewriting world REQ file...done.
             Copying game - mode LUAs and LUA - REQs...done.
             Adjusting mission.req to suit the needs of your discerning personality...all set.
             Adjusting addme.lua in the same way, only moreso...all set, only moreso.
             Adding world name and description to english.cfg...done.
             Adding world name and description to french.cfg...done.
             Adding world name and description to german.cfg...done.
             Adding world name and description to italian.cfg...done.
             Adding world name and description to spanish.cfg...done.
             Adding world name and description to japanese.cfg...done.
             Adding world name and description to uk_english.cfg...done.
             World creation completed to complete completion.
             */
        }

        private void ValidateInput()
        {
            if (string.IsNullOrWhiteSpace(WorldName))
            {
                throw new ArgumentException("World Name cannot be empty");
            }

            if (string.IsNullOrWhiteSpace(WorldAbbreviation))
            {
                throw new ArgumentException("World Abbreviation cannot be empty");
            }

            if (string.IsNullOrWhiteSpace(WorldDescription))
            {
                throw new ArgumentException("World Description cannot be empty");
            }
        }

        private void ValidateAndCopyDirectory()
        {
            string modDir = Properties.Settings.Default.SWBF2_MODTOOLS;
            if (!Directory.Exists(modDir))
            {
                throw new DirectoryNotFoundException(string.Format("Cannot find the BF2_ModTools directory: '{0}'", modDir));
            }

            string newDir = Path.Combine(modDir, "data_" + WorldAbbreviation);
            if (Directory.Exists(newDir))
            {
                throw new IOException(string.Format("Directory '{0}' already exists!", newDir));
            }

            string dataDir = Path.Combine(modDir, "data");
            logger.Info("Copying 'data' directory to 'data_'" + WorldAbbreviation + "'...");
            new Microsoft.VisualBasic.Devices.Computer().FileSystem.CopyDirectory(dataDir, newDir);
        }
    }
}