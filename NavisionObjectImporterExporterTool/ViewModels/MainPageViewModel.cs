using System;
using System.Windows;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Collections.ObjectModel;
using System.IO;
using NavisionObjectImporterExporterTool.Services;
using NavisionObjectImporterExporterTool.Models;
using NavisionObjectImporterExporterTool.Enum;
// Toolkit namespace
using SimpleMvvmToolkit;

// Toolkit extension methods
using SimpleMvvmToolkit.ModelExtensions;

namespace NavisionObjectImporterExporterTool
{
    /// <summary>
    /// This class contains properties that a View can data bind to.
    /// <para>
    /// Use the <strong>mvvmprop</strong> snippet to add bindable properties to this ViewModel.
    /// </para>
    /// </summary>
    public class MainPageViewModel : ViewModelBase<MainPageViewModel>
    {
        #region Initialization and Cleanup

        INavObjectService _ObjectService;
        IConnectionsService _ConnectionService;
        ICommandService _CommandService;
        IScriptService _Scriptservice;
          
        public MainPageViewModel(
            IConnectionsService connservice,
            INavObjectService objservice,
            ICommandService commandservice,
            IScriptService Scriptservice)
        {
            _ObjectService = objservice;
            _ConnectionService = connservice;
            _CommandService = commandservice;
            _Scriptservice = Scriptservice;

            Init();
        }

        private void Init()
        {
            GeneratedCommands = new List<string>();

            DestinationConnectionList = new List<SelectedConnectionModel>();

            ObjectTypeList = _ObjectService.GetTypeList();

            ConnectionList = _ConnectionService.GetConectionsList();

            List<SelectedConnectionModel> list = new List<SelectedConnectionModel>(); 

            foreach (var conn in ConnectionList)
            {
                list.Add(new SelectedConnectionModel { IsChecked = false, Connection = conn });
            }

            DestinationConnectionList = list;

            ObjectID = "";

            BackupPath = NavisionObjectImporterExporterTool.Properties.Settings.Default.BackupPath;

            WorkingPath = NavisionObjectImporterExporterTool.Properties.Settings.Default.WorkingPath;

            AllowExecute = NavisionObjectImporterExporterTool.Properties.Settings.Default.AllowExecute;

            DestinationBackup = true;

            PauseBeforeImport = true;

            UseTimestamp = true;

            RefreshCanexecute();
        }

        #endregion

        #region Notifications

        // TODO: Add events to notify the view or obtain data from the view
        public event EventHandler<NotificationEventArgs<Exception>> ErrorNotice;

        #endregion

        #region Properties visible in view

        // Add properties using the mvvmprop code snippet

        private string bannerText = "Importador | Exportador multiple de objetos Navision";
        public string BannerText
        {
            get
            {
                return bannerText;
            }
            set
            {
                bannerText = value;
                NotifyPropertyChanged(m => m.BannerText);
            }
        }

        private List<NavisionObjectType> _objecttypelist;
        public List<NavisionObjectType> ObjectTypeList
        {
            get { return _objecttypelist; }
            set
            {
                _objecttypelist = value;
                NotifyPropertyChanged(m => m.ObjectTypeList);
            }
        }
        
        private NavisionObjectType _selectedobjecttypelist;
        public NavisionObjectType SelectedObjectTypeList
        {
            get { return _selectedobjecttypelist; }
            set
            {
                _selectedobjecttypelist = value;
                
                LoadObjects();

                GenerateCommand();

                RefreshCanexecute();

                NotifyPropertyChanged(m => m.SelectedObjectTypeList);
            }
        }

        private List<NavisionConnection> _connectionlist;
        public List<NavisionConnection> ConnectionList
        {
            get { return _connectionlist; }
            set
            {
                _connectionlist = value;
                NotifyPropertyChanged(m => m.ConnectionList);
            }
        }
        
        private NavisionConnection _selectedconnection;
        public NavisionConnection SelectedConnection
        {
            get { return _selectedconnection; }
            set
            {
                _selectedconnection = value;
                DestinationConnectionList.Clear();
                if (value != null)
                {
                    List<SelectedConnectionModel> list = new List<SelectedConnectionModel>();

                    foreach (var conn in ConnectionList)
                    {
                        if ((SelectedConnection != null) && (conn != SelectedConnection))
                        {
                            list.Add(new SelectedConnectionModel { IsChecked = false, Connection = conn });
                        }
                    }

                    DestinationConnectionList = list;
                }
                
                LoadObjects();

                GenerateCommand();

                RefreshCanexecute();

                NotifyPropertyChanged(m => m.SelectedConnection);
            }
        }

        private List<NavisionObject> _objectlist;
        public List<NavisionObject> ObjectList
        {
            get { return _objectlist; }
            set
            {
                _objectlist = value;
                NotifyPropertyChanged(m => m.ObjectList);
            }
        }
        
        private NavisionObject _selectedobjectlist;
        public NavisionObject SelectedObjectList
        {
            get { return _selectedobjectlist; }
            set
            {
                _selectedobjectlist = value;
                RefreshCanexecute();

                if (value == null) { ObjectID = ""; } else { ObjectID = value.ObjectId.ToString(); }

                GenerateCommand();

                RefreshCanexecute();

                NotifyPropertyChanged(m => m.SelectedObjectList);
            }
        }

        private string _objectid;
        public string ObjectID
        {
            get { return _objectid; }
            set
            {
                _objectid = value;
                RefreshCanexecute();
                NotifyPropertyChanged(m => m.ObjectID);
            }
        }

        private List<SelectedConnectionModel> _destinationconnectionlist;
        public List<SelectedConnectionModel> DestinationConnectionList
        {
            get { return _destinationconnectionlist; }
            set
            {
                _destinationconnectionlist = value;
                
                GenerateCommand();

                RefreshCanexecute();

                NotifyPropertyChanged(m => m.DestinationConnectionList);
            }
        }

        private string _exportcommand;
        public string ExportCommand
        {
            get
            {
                return _exportcommand;
            }
            set
            {
                _exportcommand = value;
                NotifyPropertyChanged(m => m.ExportCommand);
            }
        }

        private List<string> _generatedcommands;
        public List<string> GeneratedCommands
        {
            get
            {
                return _generatedcommands;
            }
            set
            {
                _generatedcommands = value;
                ExportCommand = String.Join(Environment.NewLine, _generatedcommands.ToArray());
                NotifyPropertyChanged(m => m.GeneratedCommands);
            }
        }

        #region Options
            private bool AllowExecute { get; set; }    

            private string _backuppath;
            public string BackupPath
            {
                get
                {
                    return _backuppath;
                }
                set
                {
                    _backuppath = value;                    
                    GenerateCommand();
                    RefreshCanexecute();
                    NotifyPropertyChanged(m => m.BackupPath);
                }
            }

            private string _workingpath;
            public string WorkingPath
            {
                get
                {
                    return _workingpath;
                }
                set
                {
                    _workingpath = value;                    
                    GenerateCommand();
                    RefreshCanexecute();
                    NotifyPropertyChanged(m => m.WorkingPath);
                }
            }

            private bool _usetimestamp;
            public bool UseTimestamp
            {
                get
                {
                    return _usetimestamp;
                }
                set
                {
                    _usetimestamp = value;                    
                    GenerateCommand();
                    RefreshCanexecute();
                    NotifyPropertyChanged(m => m.UseTimestamp);
                }
            }

            private bool _exportfob;
            public bool ExportFob
            {
                get
                {
                    return _exportfob;
                }
                set
                {
                    _exportfob = value;                    
                    GenerateCommand();
                    RefreshCanexecute();
                    NotifyPropertyChanged(m => m.ExportFob);
                }
            }

            private bool _exporttxt;
            public bool ExportTxt
            {
                get
                {
                    return _exporttxt;
                }
                set
                {
                    _exporttxt = value;                    
                    GenerateCommand();
                    RefreshCanexecute();
                    NotifyPropertyChanged(m => m.ExportTxt);
                }
            }

            private bool _destinationbackup;
            public bool DestinationBackup
            {
                get
                {
                    return _destinationbackup;
                }
                set
                {
                    _destinationbackup = value;
                    if (value)
                    {
                        ExportTxt = true;
                        ExportFob = true;
                    }
                    else
                    {
                        ExportTxt = false;
                        ExportFob = false;
                    }
                    GenerateCommand();
                    RefreshCanexecute();                    
                    NotifyPropertyChanged(m => m.DestinationBackup);
                }
            }

            private bool _pausebeforeimport;
            public bool PauseBeforeImport
            {
                get
                {
                    return _pausebeforeimport;
                }
                set
                {
                    _pausebeforeimport = value;                    
                    GenerateCommand();
                    RefreshCanexecute();
                    NotifyPropertyChanged(m => m.PauseBeforeImport);
                }
            }
        #endregion

        #endregion

        #region CanExecute_evaluation_functions

        public bool CanGenerateCommand()
        {
            //if (SelectedObjectTypeList == null)
            //    return false;

            //if (ObjectID.Trim() == string.Empty)
            //    return false;

            //if ((DestinationBackup) & (BackupPath.Trim() == string.Empty))
            //    return false;

            //if ((WorkingPath.Trim() == string.Empty))
            //    return false;

            //if (SelectedConnection == null)
            //    return false;

            //if ((DestinationConnectionList == null) || (DestinationConnectionList.Where(x=>x.IsChecked==true).ToList().Count == 0))
            //    return false;

            return true;
        }

        public bool CanSaveBAT()
        {
            if (!CanGenerateCommand())
                return false;

            if ((GeneratedCommands == null) || (GeneratedCommands.Count == 0))
                return false;

            if (WorkingPath.Trim() == string.Empty)
                return false;

            return true;
        }

        public bool CanExecuteCommand()
        {
            if (!CanGenerateCommand())
                return false;

            if ((GeneratedCommands == null) || (GeneratedCommands.Count == 0))
                return false;

            if (!AllowExecute)
                return false;

            return true;
        }

        #endregion

        #region Methods

        public void RefreshCanexecute()
        {
            GenerateCommandCommand.RaiseCanExecuteChanged();
            ExecuteCommandCommand.RaiseCanExecuteChanged();
            SaveBATCommand.RaiseCanExecuteChanged();            
        }
       
        private void GenerateCommand()
        {
            GeneratedCommands = new List<string>();

            var destconnections = (from p in DestinationConnectionList
                                  where p.IsChecked == true
                                  select p.Connection).ToList();

            if ((destconnections == null) || (destconnections.Count == 0))
                return;

            if (SelectedConnection == null)
                return;

            if (SelectedObjectTypeList == null)
                return;

            if ((ObjectID == null) || (ObjectID.Trim() == string.Empty))
                return;

            GeneratedCommands = _CommandService.GenerateCommands(
                SelectedConnection,
                SelectedObjectTypeList,
                ObjectID,
                destconnections,
                BackupPath,
                WorkingPath,
                this.UseTimestamp,
                DestinationBackup,
                ExportFob,
                ExportTxt,
                PauseBeforeImport
                );

            
        }

        public void ExecuteCommand()
        {
            GenerateCommand();

            SaveScritp2File(true);
        }
       
        public void SaveBAT()
        {
            GenerateCommand();

            SaveScritp2File(false);
        }

        private void SaveScritp2File(bool execute)
        {
            if ((GeneratedCommands == null) || (GeneratedCommands.Count == 0))
                return;


            _Scriptservice.Save2File(
                WorkingPath,
                SelectedObjectTypeList.ObjectTypeName,
                ObjectID,
                GeneratedCommands,
                true,
                execute
                );
        }

        // TODO: Add methods that will be called by the view

        private void LoadObjects()
        {
            ObjectList = new List<NavisionObject>();

            ObjectID = string.Empty;

            if (SelectedConnection == null)
                return;

            if (SelectedObjectTypeList == null)
                return;

            try
            {
                ObjectList = _ObjectService.GetObjectList(
                    (ENUM_NavObjetType)SelectedObjectTypeList.ObjectType,
                    SelectedConnection).OrderBy(x=>x.ObjectId).ToList();
            }
            catch (Exception ex)
            {
                NotifyError("Error al cargar los objectos", ex);
            }

        }

        #endregion

        #region Delegates
        private DelegateCommand _generatecommandCommand;
        public DelegateCommand GenerateCommandCommand
        {
            get
            {
                if (_generatecommandCommand == null) _generatecommandCommand =
                    new DelegateCommand(GenerateCommand, CanGenerateCommand);
                return _generatecommandCommand;
            }
            private set { _generatecommandCommand = value; }
        }

        private DelegateCommand _executeCommandCommand;
        public DelegateCommand ExecuteCommandCommand
        {
            get
            {
                if (_executeCommandCommand == null) _executeCommandCommand =
                    new DelegateCommand(ExecuteCommand, CanExecuteCommand);
                return _executeCommandCommand;
            }
            private set { _executeCommandCommand = value; }
        }

        private DelegateCommand _saveBatCommand;
        public DelegateCommand SaveBATCommand
        {
            get
            {
                if (_saveBatCommand == null) _saveBatCommand =
                    new DelegateCommand(SaveBAT, CanSaveBAT);
                return _saveBatCommand;
            }
            private set { _saveBatCommand = value; }
        }
        #endregion

        #region Completion Callbacks
        // TODO: Optionally add callback methods for async calls to the service agent
        #endregion

        #region Helpers

        // Helper method to notify View of an error
        private void NotifyError(string message, Exception error)
        {
            ExportCommand = message;
            ExportCommand += Environment.NewLine;
            ExportCommand += error.Message;
            ExportCommand += error.InnerException!=null?error.InnerException.Message:"";

            // Notify view of an error
            Notify(ErrorNotice, new NotificationEventArgs<Exception>(message, error));
        }

        #endregion
    }
}