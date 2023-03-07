using Caliburn.Micro;
using Dapper;
using ToolboxUI.EventModels;
using ToolboxUI.Library.Api;
using ToolboxUI.Library.Models;

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows;

namespace ToolboxUI.ViewModels
{
    public class DataArchitectureViewModel : Conductor<object>
    {
        private readonly IServerEndpoint _serverEndpoint;
        private readonly IDatabaseEndpoint _databaseEndpoint;
        private readonly ISchemaEndpoint _schemaEndpoint;
        private readonly ITableEndpoint _tableEndpoint;
        private readonly IFieldEndpoint _fieldEndpoint;
        private readonly IDatabaseObjectEndpoint _databaseObjectEndpoint;
        private readonly IDataTableLineageEndpoint _dataTableLineageEndpoint;
        private readonly IEventAggregator _eventAggregator;

        public DataArchitectureViewModel(IServerEndpoint serverEndpoint, IDatabaseEndpoint databaseEndpoint, ISchemaEndpoint schemaEndpoint, ITableEndpoint tableEndpoint, IFieldEndpoint fieldEndpoint, IDatabaseObjectEndpoint databaseObjectEndpoint, IDataTableLineageEndpoint dataTableLineageEndpoint, IEventAggregator eventAggregator)
        {
            _serverEndpoint = serverEndpoint;
            _databaseEndpoint = databaseEndpoint;
            _schemaEndpoint = schemaEndpoint;
            _tableEndpoint = tableEndpoint;
            _fieldEndpoint = fieldEndpoint;
            _databaseObjectEndpoint = databaseObjectEndpoint;
            _dataTableLineageEndpoint = dataTableLineageEndpoint;
            _eventAggregator = eventAggregator;
        }

        protected async override void OnViewLoaded(object view)
        {
            System.Windows.Input.Mouse.OverrideCursor = System.Windows.Input.Cursors.Wait;

            base.OnViewLoaded(view);
            await LoadServers();
            await LoadDatabases();
            await LoadSchemas();
            await LoadTables();
            await LoadColumns();
            await LoadSearchResults();
            await LoadDataTableLineages();

            System.Windows.Input.Mouse.OverrideCursor = System.Windows.Input.Cursors.Arrow;

            _eventAggregator.PublishOnUIThread(new MessageEvent($"Finished loading page assets."));
        }

        #region Properties

        #region Server Properties
        public async Task LoadServers()
        {
            Console.WriteLine("Entering DataArchitectureViewModel.LoadServers().");

            // TODO: Will need to change to GetAllDatabasesByServerID(SelectedServer.Id)
            if (Properties.Settings.Default.DataArchitecture_CanViewDevelopmentServers)
            {
                List<ServerModel> serverList = await _serverEndpoint.GetAllServers();
                Servers = new BindingList<ServerModel>(serverList);
            }
            else
            {
                List<ServerModel> serverList = await _serverEndpoint.GetAllNonDevServers();
                Servers = new BindingList<ServerModel>(serverList);
            }

            _eventAggregator.PublishOnUIThread(new MessageEvent($"Loaded { Servers.Count } Servers"));
        }

        private BindingList<ServerModel> _servers;
        public BindingList<ServerModel> Servers
        {
            get
            {
                return _servers;
            }
            set
            {
                _servers = value;
                NotifyOfPropertyChange(() => Servers);
            }
        }

        private ServerModel _selectedServer;
        public ServerModel SelectedServer
        {
            get
            {
                return _selectedServer;
            }
            set
            {
                _selectedServer = value;
                NotifyOfPropertyChange(() => SelectedServer);

                UMLSelectedObjectServer = SelectedServer != null ? SelectedServer.ServerName : string.Empty;
                NotifyOfPropertyChange(() => Databases);
            }
        }
        #endregion

        #region Database Properties
        public async Task LoadDatabases()
        {
            Console.WriteLine("Entering DataArchitectureViewModel.LoadDatabases().");
            // TODO: Will need to change to GetAllDatabasesByServerID(SelectedServer.Id)

            List<DatabaseModel> databaseList = await _databaseEndpoint.GetAllDatabases();
            Databases = new BindingList<DatabaseModel>(databaseList);
            _eventAggregator.PublishOnUIThread(new MessageEvent($"Loaded { Databases.Count } Databases"));
        }

        private BindingList<DatabaseModel> _databases = new BindingList<DatabaseModel>();
        public BindingList<DatabaseModel> Databases
        {
            get
            {
                int serverId = SelectedServer != null ? SelectedServer.Id : 0;
                List<DatabaseModel> filteredDatabases = _databases.Where(x => serverId == x.ServerId).ToList();
                return new BindingList<DatabaseModel>(filteredDatabases);
            }
            set
            {
                _databases = value;
                NotifyOfPropertyChange(() => Databases);
            }
        }

        private DatabaseModel _selectedDatabase = new DatabaseModel();
        public DatabaseModel SelectedDatabase
        {
            get
            {
                return _selectedDatabase;
            }
            set
            {
                _selectedDatabase = value;
                NotifyOfPropertyChange(() => SelectedDatabase);
                NotifyOfPropertyChange(() => Schemas);

                if (value != null)
                {
                    UMLSelectedObjectDatabase = SelectedDatabase.DatabaseName;
                }
                else
                {
                    UMLSelectedObjectDatabase = string.Empty;
                }
            }
        }
        #endregion

        #region Schema Properties
        public async Task LoadSchemas()
        {
            Console.WriteLine("Entering DataArchitectureViewModel.LoadSchemas().");

            List<SchemaModel> schemaList = await _schemaEndpoint.GetAllSchemas();
            Schemas = new BindingList<SchemaModel>(schemaList);
            _eventAggregator.PublishOnUIThread(new MessageEvent($"Loaded { Schemas.Count } Schemas"));
        }

        private BindingList<SchemaModel> _schemas = new BindingList<SchemaModel>();
        public BindingList<SchemaModel> Schemas
        {
            get
            {
                int databaseId = SelectedDatabase != null ? SelectedDatabase.Id : 0;
                List<SchemaModel> filteredSchemas = _schemas.Where(x => databaseId == x.DatabaseId).ToList();
                return new BindingList<SchemaModel>(filteredSchemas);
            }
            set
            {
                _schemas = value;
                NotifyOfPropertyChange(() => Schemas);
            }
        }

        private SchemaModel _selectedSchema = new SchemaModel();
        public SchemaModel SelectedSchema
        {
            get
            {
                return _selectedSchema;
            }
            set
            {
                _selectedSchema = value;
                NotifyOfPropertyChange(() => SelectedSchema);
                NotifyOfPropertyChange(() => Tables);

                if (value != null)
                {
                    UMLSelectedObjectSchema = SelectedSchema.SchemaName;
                }
                else
                {
                    UMLSelectedObjectSchema = string.Empty;
                }
            }
        }
        #endregion

        #region Table Properties
        public async Task LoadTables()
        {
            Console.WriteLine("Entering DataArchitectureViewModel.LoadTables().");

            List<TableModel> tableList = await _tableEndpoint.GetAllTables();
            Tables = new BindingList<TableModel>(tableList);
            _eventAggregator.PublishOnUIThread(new MessageEvent($"Loaded { Tables.Count } Tables"));
        }

        private BindingList<TableModel> _tables = new BindingList<TableModel>();
        public BindingList<TableModel> Tables
        {
            get
            {
                int schemaId = SelectedSchema != null ? SelectedSchema.Id : 0;
                List<TableModel> filteredTables = _tables.Where(x => schemaId == x.SchemaId).ToList();
                return new BindingList<TableModel>(filteredTables);
            }
            set
            {
                _tables = value;
                NotifyOfPropertyChange(() => Tables);
            }
        }

        private TableModel _selectedTable = new TableModel();
        public TableModel SelectedTable
        {
            get
            {
                Console.WriteLine("Entering ToolboxUI.ViewModels.DataArchitectureViewModel.SelectedTable: Get");

                return _selectedTable;
            }
            set
            {
                Console.WriteLine("Entering ToolboxUI.ViewModels.DataArchitectureViewModel.SelectedTable: Set");

                _selectedTable = value;
                NotifyOfPropertyChange(() => SelectedTable);
                NotifyOfPropertyChange(() => Columns);
                NotifyOfPropertyChange(() => CanUpdateSelectedTableMapping);

                if (value != null)
                {
                    UMLSelectedObjectTable = SelectedTable.TableName;

                    List<DataTableLineageModel> parents = _dataTableLineages.Where(x => x.ChildTableId == SelectedTable.Id).ToList();
                    DataTableLineageParents = new BindingList<DataTableLineageModel>(parents);

                    List<DataTableLineageModel> children = _dataTableLineages.Where(x => x.ParentTableId == SelectedTable.Id).ToList();
                    DataTableLineageChildren = new BindingList<DataTableLineageModel>(children);
                }
                else
                {
                    UMLSelectedObjectTable = string.Empty;

                    DataTableLineageParents = new BindingList<DataTableLineageModel>();
                    DataTableLineageChildren = new BindingList<DataTableLineageModel>();
                }

                NotifyOfPropertyChange(() => DataTableLineageParents);
                NotifyOfPropertyChange(() => ParentColumns);

                NotifyOfPropertyChange(() => DataTableLineageChildren);
                NotifyOfPropertyChange(() => ChildColumns);
            }
        }
        #endregion

        #region Column Properties
        public async Task LoadColumns()
        {
            Console.WriteLine("Entering DataArchitectureViewModel.LoadColumns().");

            List<FieldModel> columnList = await _fieldEndpoint.GetAllFields();
            Columns = new BindingList<FieldModel>(columnList);
            _eventAggregator.PublishOnUIThread(new MessageEvent($"Loaded { Columns.Count } Columns"));
        }

        private BindingList<FieldModel> _columns = new BindingList<FieldModel>();
        public BindingList<FieldModel> Columns
        {
            get
            {
                int tableId = SelectedTable != null ? SelectedTable.Id : 0;
                List<FieldModel> filteredColumns = _columns.Where(x => tableId == x.TableId).ToList();
                return new BindingList<FieldModel>(filteredColumns);
            }
            set
            {
                _columns = value;
                NotifyOfPropertyChange(() => Columns);
            }
        }

        private FieldModel _selectedColumn = new FieldModel();
        public FieldModel SelectedColumn
        {
            get
            {
                return _selectedColumn;
            }
            set
            {
                _selectedColumn = value;
                NotifyOfPropertyChange(() => SelectedColumn);
                NotifyOfPropertyChange(() => Purpose);
                NotifyOfPropertyChange(() => OrdinalNumber);
                NotifyOfPropertyChange(() => DefaultValue);
                NotifyOfPropertyChange(() => Nullable);
                NotifyOfPropertyChange(() => DataType);
                NotifyOfPropertyChange(() => CharacterLength);
                NotifyOfPropertyChange(() => NumericPrecision);
                NotifyOfPropertyChange(() => NumericScale);
                NotifyOfPropertyChange(() => DateTimePrecision);
                NotifyOfPropertyChange(() => CharacterSetName);
                NotifyOfPropertyChange(() => CollationName);
                NotifyOfPropertyChange(() => PrimaryKey);
                NotifyOfPropertyChange(() => Indexed);
                NotifyOfPropertyChange(() => MinValue);
                NotifyOfPropertyChange(() => MaxValue);
                NotifyOfPropertyChange(() => SampleValue1);
                NotifyOfPropertyChange(() => SampleValue2);
                NotifyOfPropertyChange(() => SampleValue3);
                NotifyOfPropertyChange(() => SampleValue4);
                NotifyOfPropertyChange(() => SampleValue5);
                NotifyOfPropertyChange(() => SampleValue6);
                NotifyOfPropertyChange(() => SampleValue7);
                NotifyOfPropertyChange(() => SampleValue8);
                NotifyOfPropertyChange(() => SampleValue9);
                NotifyOfPropertyChange(() => SampleValue10);
            }
        }

        #region Parent Columns
        public BindingList<FieldModel> ParentColumns
        {
            get
            {
                int parentTableId = DataTableLineageParents.Count > 0 ? DataTableLineageParents.Where(x => x.ChildTableId == SelectedTable.Id).FirstOrDefault().ParentTableId : 0;
                List<FieldModel> filteredColumns = _columns.Where(x => parentTableId == x.TableId).ToList();
                return new BindingList<FieldModel>(filteredColumns);
            }
        }
        #endregion

        #region Child Columns
        public BindingList<FieldModel> ChildColumns
        {
            get
            {
                int childTableId = DataTableLineageChildren.Count > 0 ? DataTableLineageChildren.Where(x => x.ParentTableId == SelectedTable.Id).FirstOrDefault().ChildTableId : 0;
                List<FieldModel> filteredColumns = _columns.Where(x => childTableId == x.TableId).ToList();
                return new BindingList<FieldModel>(filteredColumns);
            }
        }
        #endregion
        #endregion

        #region Search Result Properties
        /// <summary>
        /// Load search results for the FilteredComboBox (aka Searchbox)
        /// </summary>
        /// <returns></returns>
        public async Task LoadSearchResults()
        {
            Console.WriteLine("Entering DataArchitectureViewModel.LoadSearchResults()");

            // Retrieve all possible database objects.
            if (Properties.Settings.Default.DataArchitecture_CanViewDevelopmentServers)
            {
                List<DatabaseObjectModel> searchResultList = await _databaseObjectEndpoint.GetAllDatabaseObjects();
                Console.WriteLine("DataArchitectureViewModel.LoadSearchResults: Retrieved all possible database objects.");

                // Populate SearchableDatabaseObjects Property
                SearchableDatabaseObjects = new BindingList<DatabaseObjectModel>(searchResultList);
            }
			else
            {
                List<DatabaseObjectModel> searchResultList = await _databaseObjectEndpoint.GetAllNonDevDatabaseObjects();
                Console.WriteLine("DataArchitectureViewModel.LoadSearchResults: Retrieved all possible database objects.");

                // Populate SearchableDatabaseObjects Property
                SearchableDatabaseObjects = new BindingList<DatabaseObjectModel>(searchResultList);
            }
            _eventAggregator.PublishOnUIThread(new MessageEvent($"Loaded search bar results."));
        }

        private BindingList<DatabaseObjectModel> _searchableDatabaseObjects = new BindingList<DatabaseObjectModel>();
        public BindingList<DatabaseObjectModel> SearchableDatabaseObjects
        {
            get
            {
                Console.WriteLine("DataArchitectureViewModel.SearchableDatabaseObjects: Get SearchableDatabaseObjects");

                string searchPhrase = SearchText != null ? SearchText : String.Empty;
                if (searchPhrase != String.Empty)
                {
                    if (SearchTextOption == "ExactMatch")
                    {
                        List<DatabaseObjectModel> exactMatchResults = _searchableDatabaseObjects.Where(x => x.ServerName.ToLower().Equals(SearchText.ToLower())
                                                                                   || x.DatabaseName.ToLower().Equals(SearchText.ToLower())
                                                                                   || x.SchemaName.ToLower().Equals(SearchText.ToLower())
                                                                                   || x.TableName.ToLower().Equals(SearchText.ToLower())
                                                                                   || x.FieldName.ToLower().Equals(SearchText.ToLower())
                                                                                   || (x.ServerName + "." + x.DatabaseName + "." +
                                                                                        x.SchemaName + "." + x.TableName + "." +
                                                                                        x.FieldName).ToLower().Equals(SearchText.ToLower())).Take(50).ToList();
                        return new BindingList<DatabaseObjectModel>(exactMatchResults);
                    }
                    else
                    {
                        List<DatabaseObjectModel> partialMatchResults = _searchableDatabaseObjects.Where(x => x.ServerName.ToLower().Contains(SearchText.ToLower())
                                                                                   || x.DatabaseName.ToLower().Contains(SearchText.ToLower())
                                                                                   || x.SchemaName.ToLower().Contains(SearchText.ToLower())
                                                                                   || x.TableName.ToLower().Contains(SearchText.ToLower())
                                                                                   || x.FieldName.ToLower().Contains(SearchText.ToLower())
                                                                                   || (x.ServerName + "." + x.DatabaseName + "." +
                                                                                        x.SchemaName + "." + x.TableName + "." +
                                                                                        x.FieldName).ToLower().Contains(SearchText.ToLower())).Take(50).ToList();
                        return new BindingList<DatabaseObjectModel>(partialMatchResults);
                    }
                }
                else
                {
                    return new BindingList<DatabaseObjectModel>(_searchableDatabaseObjects.Take(10).ToList());
                }
            }
            set
            {
                Console.WriteLine("DataArchitectureViewModel.SearchableDatabaseObjects: Set SearchableDatabaseObjects");

                _searchableDatabaseObjects = value;
                NotifyOfPropertyChange(() => SearchableDatabaseObjects);
            }
        }

        private string SearchTextOption = string.Empty;

        public async Task UpdateSearchResults(string radioButtonName)
        {
            Console.WriteLine("Entering DataArchitectureViewModel.UpdateSearchResults()");

            Console.WriteLine("DataArchitectureViewModel.UpdateSearchResults(): {0}", radioButtonName);

            if (radioButtonName == "SearchOptionExactMatch")
            {
                SearchTextOption = "ExactMatch";
            }
            else
            {
                SearchTextOption = "PartialMatch";
            }

            NotifyOfPropertyChange(() => SearchableDatabaseObjects);
        }

        public async Task SearchResultSelected(object sender, EventArgs e)
        {
            Console.WriteLine("Entering DataArchitectureViewModel.SearchResultSelected(object sender, EventArgs e).");

            DatabaseObjectModel databaseObject = new DatabaseObjectModel();
            databaseObject = (DatabaseObjectModel)sender;

            if (databaseObject != null)
            {
                SelectedServer = Servers.Where(x => databaseObject.ServerName == x.ServerName).FirstOrDefault();
                UMLSelectedObjectServer = SelectedServer.ServerName;

                SelectedDatabase = Databases.Where(x => databaseObject.DatabaseName == x.DatabaseName &&
                                                        SelectedServer.Id == x.ServerId).FirstOrDefault();

                SelectedSchema = Schemas.Where(x => databaseObject.SchemaName == x.SchemaName &&
                                                    SelectedDatabase.Id == x.DatabaseId &&
                                                    SelectedServer.Id == x.ServerId).FirstOrDefault();

                SelectedTable = Tables.Where(x => databaseObject.TableName == x.TableName &&
                                                    SelectedSchema.Id == x.SchemaId &&
                                                    SelectedDatabase.Id == x.DatabaseId &&
                                                    SelectedServer.Id == x.ServerId).FirstOrDefault();

                SelectedColumn = Columns.Where(x => databaseObject.FieldName == x.FullFieldName &&
                                                    SelectedTable.Id == x.TableId &&
                                                    SelectedSchema.Id == x.SchemaId &&
                                                    SelectedDatabase.Id == x.DatabaseId &&
                                                    SelectedServer.Id == x.ServerId).FirstOrDefault();

                SearchText = string.Empty;
            }
        }
        #endregion

        #region SearchBar_Properties
        private string _searchText = String.Empty;
        public string SearchText
        {
            get
            {
                return _searchText;
            }
            set
            {
                _searchText = value;
                NotifyOfPropertyChange(() => SearchText);
                NotifyOfPropertyChange(() => SearchableDatabaseObjects);
            }
        }

        private bool _searchResultGridIsVisible = false;
        public bool SearchResultGridIsVisible
        {
            get
            {
                return _searchResultGridIsVisible;
            }
            set
            {
                if (value != _searchResultGridIsVisible)
                {
                    _searchResultGridIsVisible = value;
                    NotifyOfPropertyChange(() => SearchResultGridIsVisible);
                }
            }
        }

        public async Task UpdateSearchResultVisibility()
        {
            Console.WriteLine("Entering DataArchitectureViewModel.UpdateSearchResults()");
            Console.WriteLine("{0}", SearchText);

            if (SearchText == String.Empty || SearchText.Length == 0)
            {
                SearchResultGridIsVisible = false;
                Console.WriteLine("Set 'SearchResultGridIsVisible = false'.");
            }
            else
            {
                SearchResultGridIsVisible = true;
                Console.WriteLine("Set 'SearchResultGridIsVisible = true'.");
            }
        }

        #endregion

        #region Database Property Fields
        public string Purpose
        {
            get
            {
                return _selectedColumn == null ? string.Empty : (_selectedColumn.Purpose == null ? string.Empty : _selectedColumn.Purpose);
            }
            set
            {
                _selectedColumn.Purpose = value;
                NotifyOfPropertyChange(() => Purpose);
            }
        }

        public string OrdinalNumber
        {
            get
            {
                return _selectedColumn == null ? string.Empty : _selectedColumn.OrdinalNumber.ToString();
            }
            set
            {
                bool wasParsed = int.TryParse(value, out int ordinalPositionValue);
                if (wasParsed)
                {
                    _selectedColumn.OrdinalNumber = ordinalPositionValue;
                    NotifyOfPropertyChange(() => OrdinalNumber);
                }
            }
        }

        public string DefaultValue
        {
            get
            {
                return _selectedColumn == null ? string.Empty : (_selectedColumn.DefaultValue == null ? string.Empty : _selectedColumn.DefaultValue);
            }
            set
            {
                _selectedColumn.DefaultValue = value;
                NotifyOfPropertyChange(() => DefaultValue);
            }
        }

        public string Nullable
        {
            get
            {
                return _selectedColumn == null ? string.Empty : _selectedColumn.IsNullable.ToString();
            }
            set
            {
                bool wasParsed = int.TryParse(value, out int nullableValue);
                if (wasParsed)
                {
                    _selectedColumn.IsNullable = nullableValue;
                    NotifyOfPropertyChange(() => Nullable);
                }
            }
        }

        public string DataType
        {
            get
            {
                return _selectedColumn == null ? string.Empty : (_selectedColumn.DataType == null ? string.Empty : _selectedColumn.DataType);
            }
            set
            {
                _selectedColumn.DataType = value;
                NotifyOfPropertyChange(() => DataType);
            }
        }

        public string CharacterLength
        {
            get
            {
                return _selectedColumn == null ? string.Empty : _selectedColumn.CharacterLength.ToString();
            }
            set
            {
                bool wasParsed = int.TryParse(value, out int charLengthValue);
                if (wasParsed)
                {
                    _selectedColumn.CharacterLength = charLengthValue;
                    NotifyOfPropertyChange(() => CharacterLength);
                }
            }
        }

        public string NumericPrecision
        {
            get
            {
                return _selectedColumn == null ? string.Empty : _selectedColumn.NumericPrecision.ToString();
            }
            set
            {
                bool wasParsed = int.TryParse(value, out int numericPrecisionValue);
                if (wasParsed)
                {
                    _selectedColumn.NumericPrecision = numericPrecisionValue;
                    NotifyOfPropertyChange(() => NumericPrecision);
                }
            }
        }

        public string NumericScale
        {
            get
            {
                return _selectedColumn == null ? string.Empty : _selectedColumn.NumericScale.ToString();
            }
            set
            {
                bool wasParsed = int.TryParse(value, out int numericScaleValue);
                if (wasParsed)
                {
                    _selectedColumn.NumericScale = numericScaleValue;
                    NotifyOfPropertyChange(() => NumericScale);
                }
            }
        }

        public string DateTimePrecision
        {
            get
            {
                return _selectedColumn == null ? string.Empty : _selectedColumn.DateTimePrecision.ToString();
            }
            set
            {
                bool wasParsed = int.TryParse(value, out int dateTimePrecisionValue);
                if (wasParsed)
                {
                    _selectedColumn.DateTimePrecision = dateTimePrecisionValue;
                    NotifyOfPropertyChange(() => DateTimePrecision);
                }
            }
        }

        public string CharacterSetName
        {
            get
            {
                return _selectedColumn == null ? string.Empty : (_selectedColumn.CharacterSetName == null ? string.Empty : _selectedColumn.CharacterSetName);
            }
            set
            {
                _selectedColumn.CharacterSetName = value;
                NotifyOfPropertyChange(() => CharacterSetName);
            }
        }

        public string CollationName
        {
            get
            {
                return _selectedColumn == null ? string.Empty : (_selectedColumn.CollationName == null ? string.Empty : _selectedColumn.CollationName);
            }
            set
            {
                _selectedColumn.CollationName = value;
                NotifyOfPropertyChange(() => CollationName);
            }
        }

        public string PrimaryKey
        {
            get
            {
                return _selectedColumn == null ? string.Empty : _selectedColumn.PrimaryKey.ToString();
            }
            set
            {
                bool wasParsed = int.TryParse(value, out int primaryKeyValue);
                if (wasParsed)
                {
                    _selectedColumn.PrimaryKey = primaryKeyValue;
                    NotifyOfPropertyChange(() => PrimaryKey);
                }
            }
        }

        public string Indexed
        {
            get
            {
                return _selectedColumn == null ? string.Empty : _selectedColumn.Indexed.ToString();
            }
            set
            {
                bool wasParsed = int.TryParse(value, out int indexedValue);
                if (wasParsed)
                {
                    _selectedColumn.Indexed = indexedValue;
                    NotifyOfPropertyChange(() => Indexed);
                }
            }
        }

        public string MinValue
        {
            get
            {
                return _selectedColumn == null ? string.Empty : (_selectedColumn.MinValue == null ? string.Empty : _selectedColumn.MinValue);
            }
            set
            {
                _selectedColumn.MinValue = value;
                NotifyOfPropertyChange(() => MinValue);
            }
        }

        public string MaxValue
        {
            get
            {
                return _selectedColumn == null ? string.Empty : (_selectedColumn.MaxValue == null ? string.Empty : _selectedColumn.MaxValue);
            }
            set
            {
                _selectedColumn.MaxValue = value;
                NotifyOfPropertyChange(() => MaxValue);
            }
        }

        public string SampleValue1
        {
            get
            {
                return _selectedColumn == null ? string.Empty : (_selectedColumn.SampleValue1 == null ? string.Empty : _selectedColumn.SampleValue1);
            }
            set
            {
                _selectedColumn.SampleValue1 = value;
                NotifyOfPropertyChange(() => SampleValue1);
            }
        }

        public string SampleValue2
        {
            get
            {
                return _selectedColumn == null ? string.Empty : (_selectedColumn.SampleValue2 == null ? string.Empty : _selectedColumn.SampleValue2);
            }
            set
            {
                _selectedColumn.SampleValue2 = value;
                NotifyOfPropertyChange(() => SampleValue2);
            }
        }

        public string SampleValue3
        {
            get
            {
                return _selectedColumn == null ? string.Empty : (_selectedColumn.SampleValue3 == null ? string.Empty : _selectedColumn.SampleValue3);
            }
            set
            {
                _selectedColumn.SampleValue3 = value;
                NotifyOfPropertyChange(() => SampleValue3);
            }
        }

        public string SampleValue4
        {
            get
            {
                return _selectedColumn == null ? string.Empty : (_selectedColumn.SampleValue4 == null ? string.Empty : _selectedColumn.SampleValue4);
            }
            set
            {
                _selectedColumn.SampleValue4 = value;
                NotifyOfPropertyChange(() => SampleValue4);
            }
        }

        public string SampleValue5
        {
            get
            {
                return _selectedColumn == null ? string.Empty : (_selectedColumn.SampleValue5 == null ? string.Empty : _selectedColumn.SampleValue5);
            }
            set
            {
                _selectedColumn.SampleValue5 = value;
                NotifyOfPropertyChange(() => SampleValue5);
            }
        }

        public string SampleValue6
        {
            get
            {
                return _selectedColumn == null ? string.Empty : (_selectedColumn.SampleValue6 == null ? string.Empty : _selectedColumn.SampleValue6);
            }
            set
            {
                _selectedColumn.SampleValue6 = value;
                NotifyOfPropertyChange(() => SampleValue6);
            }
        }

        public string SampleValue7
        {
            get
            {
                return _selectedColumn == null ? string.Empty : (_selectedColumn.SampleValue7 == null ? string.Empty : _selectedColumn.SampleValue7);
            }
            set
            {
                _selectedColumn.SampleValue7 = value;
                NotifyOfPropertyChange(() => SampleValue7);
            }
        }

        public string SampleValue8
        {
            get
            {
                return _selectedColumn == null ? string.Empty : (_selectedColumn.SampleValue8 == null ? string.Empty : _selectedColumn.SampleValue8);
            }
            set
            {
                _selectedColumn.SampleValue8 = value;
                NotifyOfPropertyChange(() => SampleValue8);
            }
        }

        public string SampleValue9
        {
            get
            {
                return _selectedColumn == null ? string.Empty : (_selectedColumn.SampleValue9 == null ? string.Empty : _selectedColumn.SampleValue9);
            }
            set
            {
                _selectedColumn.SampleValue9 = value;
                NotifyOfPropertyChange(() => SampleValue9);
            }
        }

        public string SampleValue10
        {
            get
            {
                return _selectedColumn == null ? string.Empty : (_selectedColumn.SampleValue10 == null ? string.Empty : _selectedColumn.SampleValue10);
            }
            set
            {
                _selectedColumn.SampleValue10 = value;
                NotifyOfPropertyChange(() => SampleValue10);
            }
        }
        #endregion

        #region Data Table Lineage Objects
        public async Task LoadDataTableLineages()
        {
            Console.WriteLine("Entering DataArchitectureViewModel.LoadDataTableLineages().");

            List<DataTableLineageModel> dataTableLineages = await _dataTableLineageEndpoint.GetAllDataTableLineages();
            DataTableLineages = new BindingList<DataTableLineageModel>(dataTableLineages);
            _eventAggregator.PublishOnUIThread(new MessageEvent($"Loaded data table lineages."));
        }

        private BindingList<DataTableLineageModel> _dataTableLineages = new BindingList<DataTableLineageModel>();
        public BindingList<DataTableLineageModel> DataTableLineages
        {
            get
            {
                return _dataTableLineages;
            }
            set
            {
                _dataTableLineages = value;
                NotifyOfPropertyChange(() => DataTableLineages);
            }
        }

        private BindingList<DataTableLineageModel> _foundDataTableLineages = new BindingList<DataTableLineageModel>();
        public BindingList<DataTableLineageModel> FoundDataTableLineages
        {
            get
            {
                return _foundDataTableLineages;
            }
            set
            {
                _foundDataTableLineages = value;
                NotifyOfPropertyChange(() => FoundDataTableLineages);
            }
        }

        #region Data Table Lineage - Parents
        private BindingList<DataTableLineageModel> _dataTableLineageParents = new BindingList<DataTableLineageModel>();
        public BindingList<DataTableLineageModel> DataTableLineageParents
        {
            get
            {
                Console.WriteLine("Entering DataArchitectureViewModel.DataTableLineageParents: GET");

                return _dataTableLineageParents;
            }
            set
            {
                Console.WriteLine("Entering DataArchitectureViewModel.DataTableLineageParents: SET");

                _dataTableLineageParents = value;

                NotifyOfPropertyChange(() => DataTableLineageParents);

                if (_dataTableLineageParents.Count > 0)
                {
                    BindingList<DatabaseObjectModel> parents = new BindingList<DatabaseObjectModel>();
                    BindingList<DatabaseObjectModel> parentCollection = new BindingList<DatabaseObjectModel>();
                    foreach (DataTableLineageModel parent in _dataTableLineageParents)
                    {
                        List<DatabaseObjectModel> parentObjects = _searchableDatabaseObjects.Where(x => x.TableId == parent.ParentTableId).ToList();
                        parents = new BindingList<DatabaseObjectModel>(parentObjects);

                        foreach (DatabaseObjectModel parentObject in parents)
                        {
                            parentCollection.Add(parentObject);
                        }
                    }

                    UMLParentObjectServer = parentCollection.FirstOrDefault().ServerName;
                    UMLParentObjectDatabase = parentCollection.FirstOrDefault().DatabaseName;
                    UMLParentObjectSchema = parentCollection.FirstOrDefault().SchemaName;
                    UMLParentObjectTable = parentCollection.FirstOrDefault().TableName;
                }
                else
                {

                    UMLParentObjectServer = string.Empty;
                    UMLParentObjectDatabase = string.Empty;
                    UMLParentObjectSchema = string.Empty;
                    UMLParentObjectTable = string.Empty;
                }

                NotifyOfPropertyChange(() => UMLParentObjectServer);
                NotifyOfPropertyChange(() => UMLParentObjectDatabase);
                NotifyOfPropertyChange(() => UMLParentObjectSchema);
                NotifyOfPropertyChange(() => UMLParentObjectTable);
            }
        }
        #endregion

        #region Data Table Lineage - Children
        private BindingList<DataTableLineageModel> _dataTableLineageChildren = new BindingList<DataTableLineageModel>();
        public BindingList<DataTableLineageModel> DataTableLineageChildren
        {
            get
            {
                Console.WriteLine("Entering DataArchitectureViewModel.DataTableLineageChildren: GET");

                return _dataTableLineageChildren;
            }
            set
            {
                Console.WriteLine("Entering DataArchitectureViewModel.DataTableLineageChildren: SET");

                _dataTableLineageChildren = value;

                NotifyOfPropertyChange(() => DataTableLineageChildren);

                if (_dataTableLineageChildren.Count > 0)
                {
                    BindingList<DatabaseObjectModel> children = new BindingList<DatabaseObjectModel>();
                    foreach (DataTableLineageModel child in _dataTableLineageChildren)
                    {
                        List<DatabaseObjectModel> childObjects = _searchableDatabaseObjects.Where(x => x.TableId == child.ChildTableId).ToList();
                        children = new BindingList<DatabaseObjectModel>(childObjects);
                    }

                    if (children.Count > 0)
                    {
                        UMLChildObjectServer = children.FirstOrDefault().ServerName;
                        UMLChildObjectDatabase = children.FirstOrDefault().DatabaseName;
                        UMLChildObjectSchema = children.FirstOrDefault().SchemaName;
                        UMLChildObjectTable = children.FirstOrDefault().TableName;
                    }
                    else
                    {

                        UMLChildObjectServer = string.Empty;
                        UMLChildObjectDatabase = string.Empty;
                        UMLChildObjectSchema = string.Empty;
                        UMLChildObjectTable = string.Empty;
                    }
                }
                else
                {

                    UMLChildObjectServer = string.Empty;
                    UMLChildObjectDatabase = string.Empty;
                    UMLChildObjectSchema = string.Empty;
                    UMLChildObjectTable = string.Empty;
                }

                NotifyOfPropertyChange(() => UMLChildObjectServer);
                NotifyOfPropertyChange(() => UMLChildObjectDatabase);
                NotifyOfPropertyChange(() => UMLChildObjectSchema);
                NotifyOfPropertyChange(() => UMLChildObjectTable);
            }
        }
        #endregion
        #endregion

        #region UML Selected Object Properties
        private string _umlSelectedObjectServer = string.Empty;
        public string UMLSelectedObjectServer
        {
            get
            {
                Console.WriteLine("Entering DataArchitectureViewModel.UMLSelectedObjectServer: Get {0}", _umlSelectedObjectServer);

                return _umlSelectedObjectServer;
            }
            set
            {
                Console.WriteLine("Entering DataArchitectureViewModel.UMLSelectedObjectServer: Set {0}", value);

                _umlSelectedObjectServer = value;
                NotifyOfPropertyChange(() => UMLSelectedObjectServer);
            }
        }

        private string _umlSelectedObjectDatabase = string.Empty;
        public string UMLSelectedObjectDatabase
        {
            get
            {
                Console.WriteLine("Entering DataArchitectureViewModel.UMLSelectedObjectDatabase: Get {0}", _umlSelectedObjectDatabase);

                return _umlSelectedObjectDatabase;
            }
            set
            {
                Console.WriteLine("Entering DataArchitectureViewModel.UMLSelectedObjectDatabase: Set {0}", value);

                _umlSelectedObjectDatabase = value;
                NotifyOfPropertyChange(() => UMLSelectedObjectDatabase);

                _umlSelectedObjectName = _umlSelectedObjectDatabase;
                NotifyOfPropertyChange(() => UMLSelectedObjectName);
            }
        }

        private string _umlSelectedObjectSchema = string.Empty;
        public string UMLSelectedObjectSchema
        {
            get
            {
                Console.WriteLine("Entering DataArchitectureViewModel.UMLSelectedObjectSchema: Get {0}", _umlSelectedObjectSchema);

                return _umlSelectedObjectSchema;
            }
            set
            {
                Console.WriteLine("Entering DataArchitectureViewModel.UMLSelectedObjectSchema: Set {0}", value);

                _umlSelectedObjectSchema = value;
                NotifyOfPropertyChange(() => UMLSelectedObjectSchema);

                _umlSelectedObjectName = _umlSelectedObjectDatabase + "." + _umlSelectedObjectSchema;
                NotifyOfPropertyChange(() => UMLSelectedObjectName);
            }
        }

        private string _umlSelectedObjectTable = string.Empty;
        public string UMLSelectedObjectTable
        {
            get
            {
                Console.WriteLine("Entering DataArchitectureViewModel.UMLSelectedObjectTable: Get {0}", _umlSelectedObjectTable);

                return _umlSelectedObjectTable;
            }
            set
            {
                Console.WriteLine("Entering DataArchitectureViewModel.UMLSelectedObjectTable: Set {0}", value);

                _umlSelectedObjectTable = value;
                NotifyOfPropertyChange(() => UMLSelectedObjectTable);

                _umlSelectedObjectName = _umlSelectedObjectDatabase + "." + _umlSelectedObjectSchema + "." + _umlSelectedObjectTable;
                NotifyOfPropertyChange(() => UMLSelectedObjectName);
            }
        }

        private ObservableCollection<FieldModel> _umlSelectedObjectFields = new ObservableCollection<FieldModel>();
        public ObservableCollection<FieldModel> UMLSelectedObjectFields
        {
            get
            {
                Console.WriteLine("Entering DataArchitectureViewModel.UMLSelectedObjectFields: Get {0}", _umlSelectedObjectFields);

                return _umlSelectedObjectFields;
            }
            set
            {
                Console.WriteLine("Entering DataArchitectureViewModel.UMLSelectedObjectFields: Set {0}", value);

                _umlSelectedObjectFields = value;
                NotifyOfPropertyChange(() => UMLSelectedObjectFields);
            }
        }

        private StackPanel _umlSelectedObject = new StackPanel();
        public StackPanel UMLSelectedObject
        {
            get
            {
                return _umlSelectedObject;
            }
            set
            {
                _umlSelectedObject = value;
                NotifyOfPropertyChange(() => UMLSelectedObject);
            }
        }

        private string _umlSelectedObjectName = string.Empty;
        public string UMLSelectedObjectName
        {
            get
            {
                Console.WriteLine("Entering DataArchitectureViewModel.UMLSelectedObjectName: Get {0}", _umlSelectedObjectName);

                return _umlSelectedObjectName;
            }
        }
        #endregion

        #region UML Parent Object Properties
        private string _umlParentObjectServer = string.Empty;
        public string UMLParentObjectServer
        {
            get
            {
                Console.WriteLine("Entering DataArchitectureViewModel.UMLParentObjectServer: Get {0}", _umlParentObjectServer);

                return _umlParentObjectServer;
            }
            set
            {
                Console.WriteLine("Entering DataArchitectureViewModel.UMLParentObjectServer: Set {0}", value);

                _umlParentObjectServer = value;
                NotifyOfPropertyChange(() => UMLParentObjectServer);
            }
        }

        private string _umlParentObjectDatabase = string.Empty;
        public string UMLParentObjectDatabase
        {
            get
            {
                Console.WriteLine("Entering DataArchitectureViewModel.UMLParentObjectDatabase: Get {0}", _umlParentObjectDatabase);

                return _umlParentObjectDatabase;
            }
            set
            {
                Console.WriteLine("Entering DataArchitectureViewModel.UMLParentObjectDatabase: Set {0}", value);

                _umlParentObjectDatabase = value;
                NotifyOfPropertyChange(() => UMLParentObjectDatabase);

                _umlParentObjectName = _umlParentObjectDatabase;
                NotifyOfPropertyChange(() => UMLParentObjectName);
            }
        }

        private string _umlParentObjectSchema = string.Empty;
        public string UMLParentObjectSchema
        {
            get
            {
                Console.WriteLine("Entering DataArchitectureViewModel.UMLParentObjectSchema: Get {0}", _umlParentObjectSchema);

                return _umlParentObjectSchema;
            }
            set
            {
                Console.WriteLine("Entering DataArchitectureViewModel.UMLParentObjectSchema: Set {0}", value);

                _umlParentObjectSchema = value;
                NotifyOfPropertyChange(() => UMLParentObjectSchema);

                _umlParentObjectName = _umlParentObjectDatabase + "." + _umlParentObjectSchema;
                NotifyOfPropertyChange(() => UMLParentObjectName);
            }
        }

        private string _umlParentObjectTable = string.Empty;
        public string UMLParentObjectTable
        {
            get
            {
                Console.WriteLine("Entering DataArchitectureViewModel.UMLParentObjectTable: Get {0}", _umlParentObjectTable);

                return _umlParentObjectTable;
            }
            set
            {
                Console.WriteLine("Entering DataArchitectureViewModel.UMLParentObjectTable: Set {0}", value);

                _umlParentObjectTable = value;
                NotifyOfPropertyChange(() => UMLParentObjectTable);

                _umlParentObjectName = _umlParentObjectDatabase + "." + _umlParentObjectSchema + "." + _umlParentObjectTable;
                NotifyOfPropertyChange(() => UMLParentObjectName);
            }
        }

        private ObservableCollection<FieldModel> _umlParentObjectFields = new ObservableCollection<FieldModel>();
        public ObservableCollection<FieldModel> UMLParentObjectFields
        {
            get
            {
                Console.WriteLine("Entering DataArchitectureViewModel.UMLParentObjectFields: Get {0}", _umlParentObjectFields);

                return _umlSelectedObjectFields;
            }
            set
            {
                Console.WriteLine("Entering DataArchitectureViewModel.UMLParentObjectFields: Set {0}", value);

                _umlParentObjectFields = value;
                NotifyOfPropertyChange(() => UMLParentObjectFields);
            }
        }

        private StackPanel _umlParentObject = new StackPanel();
        public StackPanel UMLParentObject
        {
            get
            {
                return _umlParentObject;
            }
            set
            {
                _umlParentObject = value;
                NotifyOfPropertyChange(() => UMLParentObject);
            }
        }

        private string _umlParentObjectName = string.Empty;
        public string UMLParentObjectName
        {
            get
            {
                Console.WriteLine("Entering DataArchitectureViewModel.UMLParentObjectName: Get {0}", _umlParentObjectName);

                return _umlParentObjectName;
            }
        }
        #endregion

        #region UML Child Object Properties
        private string _umlChildObjectServer = string.Empty;
        public string UMLChildObjectServer
        {
            get
            {
                Console.WriteLine("Entering DataArchitectureViewModel.UMLChildObjectServer: Get {0}", _umlChildObjectServer);

                return _umlChildObjectServer;
            }
            set
            {
                Console.WriteLine("Entering DataArchitectureViewModel.UMLChildObjectServer: Set {0}", value);

                _umlChildObjectServer = value;
                NotifyOfPropertyChange(() => UMLChildObjectServer);
            }
        }

        private string _umlChildObjectDatabase = string.Empty;
        public string UMLChildObjectDatabase
        {
            get
            {
                Console.WriteLine("Entering DataArchitectureViewModel.UMLChildObjectDatabase: Get {0}", _umlChildObjectDatabase);

                return _umlChildObjectDatabase;
            }
            set
            {
                Console.WriteLine("Entering DataArchitectureViewModel.UMLChildObjectDatabase: Set {0}", value);

                _umlChildObjectDatabase = value;
                NotifyOfPropertyChange(() => UMLChildObjectDatabase);

                _umlChildObjectName = _umlChildObjectDatabase;
                NotifyOfPropertyChange(() => UMLChildObjectName);
            }
        }

        private string _umlChildObjectSchema = string.Empty;
        public string UMLChildObjectSchema
        {
            get
            {
                Console.WriteLine("Entering DataArchitectureViewModel.UMLChildObjectSchema: Get {0}", _umlChildObjectSchema);

                return _umlChildObjectSchema;
            }
            set
            {
                Console.WriteLine("Entering DataArchitectureViewModel.UMLChildObjectSchema: Set {0}", value);

                _umlChildObjectSchema = value;
                NotifyOfPropertyChange(() => UMLChildObjectSchema);

                _umlChildObjectName = _umlChildObjectDatabase + "." + _umlChildObjectSchema;
                NotifyOfPropertyChange(() => UMLChildObjectName);
            }
        }

        private string _umlChildObjectTable = string.Empty;
        public string UMLChildObjectTable
        {
            get
            {
                Console.WriteLine("Entering DataArchitectureViewModel.UMLChildObjectTable: Get {0}", _umlChildObjectTable);

                return _umlChildObjectTable;
            }
            set
            {
                Console.WriteLine("Entering DataArchitectureViewModel.UMLChildObjectTable: Set {0}", value);

                _umlChildObjectTable = value;
                NotifyOfPropertyChange(() => UMLChildObjectTable);

                _umlChildObjectName = _umlChildObjectDatabase + "." + _umlChildObjectSchema + "." + _umlChildObjectTable;
                NotifyOfPropertyChange(() => UMLChildObjectName);
            }
        }

        private ObservableCollection<FieldModel> _umlChildObjectFields = new ObservableCollection<FieldModel>();
        public ObservableCollection<FieldModel> UMLChildObjectFields
        {
            get
            {
                Console.WriteLine("Entering DataArchitectureViewModel.UMLChildObjectFields: Get {0}", _umlChildObjectFields);

                return _umlSelectedObjectFields;
            }
            set
            {
                Console.WriteLine("Entering DataArchitectureViewModel.UMLChildObjectFields: Set {0}", value);

                _umlChildObjectFields = value;
                NotifyOfPropertyChange(() => UMLChildObjectFields);
            }
        }

        private StackPanel _umlChildObject = new StackPanel();
        public StackPanel UMLChildObject
        {
            get
            {
                return _umlChildObject;
            }
            set
            {
                _umlChildObject = value;
                NotifyOfPropertyChange(() => UMLChildObject);
            }
        }

        private string _umlChildObjectName = string.Empty;
        public string UMLChildObjectName
        {
            get
            {
                Console.WriteLine("Entering DataArchitectureViewModel.UMLChildObjectName: Get {0}", _umlChildObjectName);

                return _umlChildObjectName;
            }
        }
        #endregion

        #endregion

        #region Draw UML Visual
        public async Task DrawUMLMap()
        {
            Console.WriteLine("Entering DataArchitectureViewModel.DrawUMLMap()");

            // Draw Server Container
            UMLSelectedObject.Children.Clear();

            Border ServerBorder = new Border();
            Grid ServerGrid = new Grid();
            ServerGrid.ColumnDefinitions.Add(new ColumnDefinition() { Name = "SelectedUMLColumn1", Width = new GridLength(1, GridUnitType.Auto) });
            ServerGrid.RowDefinitions.Add(new RowDefinition() { Name = "SelectedUMLRow1", Height = new GridLength(1, GridUnitType.Auto) });
            ServerGrid.RowDefinitions.Add(new RowDefinition() { Name = "SelectedUMLRow2", Height = new GridLength(1, GridUnitType.Auto) });
            ServerGrid.RowDefinitions.Add(new RowDefinition() { Name = "SelectedUMLRow3", Height = new GridLength(1, GridUnitType.Auto) });
            Console.WriteLine("DataArchitectureViewModel.DrawUMLMap: Defined Grid.");

            Style borderStyle = (Style)App.Current.Resources["ToolTipBorder"];
            ServerBorder.Style = borderStyle;
            ServerBorder.Child = ServerGrid;
            Console.WriteLine("DataArchitectureViewModel.DrawUMLMap: Defined Border.");

            UMLSelectedObject.Children.Add(ServerBorder);
            Console.WriteLine("DataArchitectureViewModel.DrawUMLMap: Added border to UMLSelectedObject.");

            TextBlock UMLSelectedServerNameTextBlock = new TextBlock();
            TextBlock UMLSelectedObjectNameTextBlock = new TextBlock();

            if (SelectedServer != null && SelectedServer.ServerName != null)
            {
                Grid.SetColumn(UMLSelectedServerNameTextBlock, 0);
                Grid.SetRow(UMLSelectedServerNameTextBlock, 0);
                ServerGrid.Children.Add(UMLSelectedServerNameTextBlock);
                UMLSelectedServerNameTextBlock.Text = SelectedServer.ServerName;
                Console.WriteLine("DataArchitectureViewModel.DrawUMLMap: Added Server Details.");
            }

            if (SelectedDatabase != null && SelectedDatabase.DatabaseName != null)
            {
                Grid.SetColumn(UMLSelectedObjectNameTextBlock, 0);
                Grid.SetRow(UMLSelectedObjectNameTextBlock, 1);
                ServerGrid.Children.Add(UMLSelectedObjectNameTextBlock);
                UMLSelectedObjectNameTextBlock.Text = SelectedDatabase.DatabaseName;
                Console.WriteLine("DataArchitectureViewModel.DrawUMLMap: Added Database Details.");
            }

            if (SelectedSchema != null && SelectedSchema.SchemaName != null)
            {
                UMLSelectedObjectNameTextBlock.Text += "." + SelectedSchema.SchemaName;
                Console.WriteLine("DataArchitectureViewModel.DrawUMLMap: Added Schema Details.");
            }

            if (SelectedTable != null && SelectedTable.TableName != null)
            {
                UMLSelectedObjectNameTextBlock.Text += "." + SelectedTable.TableName;
                Console.WriteLine("DataArchitectureViewModel.DrawUMLMap: Added Table Details.");
            }
        }

        #endregion

        #region Server Mapping

        public bool CanUpdateSelectedServerMapping
        {
            get
            {
                return Properties.Settings.Default.DataArchitecture_CanRunDatabaseScrape;
            }
        }

        public async Task UpdateSelectedServerMapping(int nestedLevels = 0)
        {

            Console.WriteLine("Entering DataArchitectureViewModel.UpdateSelectedServerMapping().");

            _eventAggregator.PublishOnUIThread(new MessageEvent($"Searching server: { SelectedServer.ServerName }..."));
            await FindDatabases(SelectedServer);
            await LoadDatabases();

            List<DatabaseModel> serverDatabases = _databases.Where(x => SelectedServer.Id == x.ServerId).ToList();
            Console.WriteLine($"UpdateDatabaseMapping: serverDatabases Count: { serverDatabases.Count }");
            _eventAggregator.PublishOnUIThread(new MessageEvent($"Found { serverDatabases.Count } database(s) in server { SelectedServer.ServerName }."));

            if (nestedLevels > 0)
            {

                foreach (DatabaseModel serverDatabase in serverDatabases)
                {

                    Console.WriteLine($"Searching database: { SelectedServer.ServerName }.{ serverDatabase.DatabaseName }...");
                    await FindSchemas(SelectedServer, serverDatabase);
                    await LoadSchemas();

                    List<SchemaModel> databaseSchemas = _schemas.Where(x => serverDatabase.Id == x.DatabaseId).ToList();
                    Console.WriteLine($"UpdateDatabaseMapping: databaseSchemas Count: { databaseSchemas.Count }");

                    if (nestedLevels > 1)
                    {

                        foreach (SchemaModel databaseSchema in databaseSchemas)
                        {

                            Console.WriteLine($"Searching schema: { SelectedServer.ServerName }.{ serverDatabase.DatabaseName }.{ databaseSchema.SchemaName }...");
                            await FindTables(SelectedServer, serverDatabase, databaseSchema);
                            await LoadTables();

                            List<TableModel> schemaTables = _tables.Where(x => databaseSchema.Id == x.SchemaId).ToList();
                            Console.WriteLine($"UpdateDatabaseMapping: schemaTables Count: { schemaTables.Count }");

                            if (nestedLevels > 2)
                            {

                                foreach (TableModel schemaTable in schemaTables)
                                {

                                    Console.WriteLine($"Searching table: { SelectedServer.ServerName }.{ serverDatabase.DatabaseName }.{ databaseSchema.SchemaName }.{ schemaTable.TableName }...");
                                    await FindFields(SelectedServer, serverDatabase, databaseSchema, schemaTable);
                                    await LoadColumns();

                                    List<FieldModel> tableColumns = _columns.Where(x => schemaTable.Id == x.TableId).ToList();
                                    Console.WriteLine($"UpdateDatabaseMapping: tableColumns Count: { tableColumns.Count }");

                                }
                            }
                        }
                    }
                }
            }
        }

        #endregion

        #region Database Mapping

        public bool CanUpdateDatabaseMapping
        {
            get
            {
                return Properties.Settings.Default.DataArchitecture_CanRunDatabaseScrape;
            }
        }


        public async Task UpdateDatabaseMapping()
        {
            Console.WriteLine("Entering DataArchitectureViewModel.UpdateDatabaseMapping().");

            foreach (ServerModel server in Servers)
            {
                _eventAggregator.PublishOnUIThread(new MessageEvent($"Searching server: { server.ServerName }..."));
                await FindDatabases(server);
                await LoadDatabases();

                List<DatabaseModel> serverDatabases = _databases.Where(x => server.Id == x.ServerId).ToList();
                Console.WriteLine($"UpdateDatabaseMapping: serverDatabases Count: { serverDatabases.Count }");
                _eventAggregator.PublishOnUIThread(new MessageEvent($"Found { serverDatabases.Count } database(s) in server { server.ServerName }."));

                foreach (DatabaseModel serverDatabase in serverDatabases)
                {
                    _eventAggregator.PublishOnUIThread(new MessageEvent($"Searching database: { server.ServerName }.{ serverDatabase.DatabaseName }..."));
                    await FindSchemas(server, serverDatabase);
                    await LoadSchemas();

                    List<SchemaModel> databaseSchemas = _schemas.Where(x => serverDatabase.Id == x.DatabaseId).ToList();
                    Console.WriteLine($"UpdateDatabaseMapping: databaseSchemas Count: { databaseSchemas.Count }");
                    _eventAggregator.PublishOnUIThread(new MessageEvent($"Found { databaseSchemas.Count } schema(s) in database { server.ServerName }.{ serverDatabase.DatabaseName }."));

                    foreach (SchemaModel databaseSchema in databaseSchemas)
                    {
                        _eventAggregator.PublishOnUIThread(new MessageEvent($"Searching schema: { server.ServerName }.{ serverDatabase.DatabaseName }.{ databaseSchema.SchemaName }..."));
                        await FindTables(server, serverDatabase, databaseSchema);
                        await LoadTables();

                        List<TableModel> schemaTables = _tables.Where(x => databaseSchema.Id == x.SchemaId).ToList();
                        Console.WriteLine($"UpdateDatabaseMapping: schemaTables Count: { schemaTables.Count }");
                        _eventAggregator.PublishOnUIThread(new MessageEvent($"Found { schemaTables.Count } table(s) in schema { server.ServerName }.{ serverDatabase.DatabaseName }.{ databaseSchema.SchemaName }."));

                        foreach (TableModel schemaTable in schemaTables)
                        {
                            _eventAggregator.PublishOnUIThread(new MessageEvent($"Searching table: { server.ServerName }.{ serverDatabase.DatabaseName }.{ databaseSchema.SchemaName }.{ schemaTable.TableName }..."));
                            await FindFields(server, serverDatabase, databaseSchema, schemaTable);
                            await LoadColumns();

                            List<FieldModel> tableColumns = _columns.Where(x => schemaTable.Id == x.TableId).ToList();
                            Console.WriteLine($"UpdateDatabaseMapping: tableColumns Count: { tableColumns.Count }");
                            _eventAggregator.PublishOnUIThread(new MessageEvent($"Found { tableColumns.Count } field(s) in schema { server.ServerName }.{ serverDatabase.DatabaseName }.{ databaseSchema.SchemaName }.{ schemaTable.TableName }."));
                        }
                    }
                }
            }
        }

        public bool CanUpdateSelectedDatabaseMapping
        {
            get
            {
                return Properties.Settings.Default.DataArchitecture_CanRunSchemaScrape;
            }
        }


        public async Task UpdateSelectedDatabaseMapping(int nestedLevels = 0)
        {
            Console.WriteLine("Entering DataArchitectureViewModel.UpdateSelectedDatabaseMapping().");

            _eventAggregator.PublishOnUIThread($"Searching database: { SelectedDatabase.DatabaseName }...");
            await FindSchemas(SelectedServer, SelectedDatabase);
            await LoadSchemas();

            List<SchemaModel> databaseSchemas = _schemas.Where(x => SelectedDatabase.Id == x.DatabaseId).ToList();
            Console.WriteLine($"UpdateDatabaseMapping: databaseSchemas Count: { databaseSchemas.Count }");

            if (nestedLevels > 0)
            {
                foreach (SchemaModel databaseSchema in databaseSchemas)
                {
                    Console.WriteLine($"Searching schema: { SelectedServer.ServerName }.{ SelectedDatabase.DatabaseName }.{ databaseSchema.SchemaName }...");
                    await FindTables(SelectedServer, SelectedDatabase, databaseSchema);
                    await LoadTables();

                    List<TableModel> schemaTables = _tables.Where(x => databaseSchema.Id == x.SchemaId).ToList();
                    Console.WriteLine($"UpdateDatabaseMapping: schemaTables Count: { schemaTables.Count }");

                    if (nestedLevels > 1)
                    {
                        foreach (TableModel schemaTable in schemaTables)
                        {
                            Console.WriteLine($"Searching table: { SelectedServer.ServerName }.{ SelectedDatabase.DatabaseName }.{ databaseSchema.SchemaName }.{ schemaTable.TableName }...");
                            await FindFields(SelectedServer, SelectedDatabase, databaseSchema, schemaTable, nestedLevels > 2 ? 1 : 0);
                            await LoadColumns();

                            List<FieldModel> tableColumns = _columns.Where(x => schemaTable.Id == x.TableId).ToList();
                            Console.WriteLine($"UpdateDatabaseMapping: tableColumns Count: { tableColumns.Count }");
                        }
                    }
                }
            }
        }

        public Task FindDatabases(ServerModel server)
        {
            Console.WriteLine("Entering DataArchitectureViewModel.FindDatabases().");

            //var foundDatabases = await _databaseEndpoint.FindAllDatabases(server.ServerName);
            List<DatabaseModel> foundDatabases = FindAllDatabases(server);
            SaveAllDatabases(foundDatabases);
            return Task.CompletedTask;
        }

        private List<DatabaseModel> FindAllDatabases(ServerModel server)
        {
            Console.WriteLine("Entered FindAllDatabases");
            List<DatabaseModel> databases = new List<DatabaseModel>();
            string connectionString = server.ServerName == "mncportalprod-sql.database.windows.net" ? GetConnectionString(server.ServerName, "master") : GetConnectionString(server.ServerName);
            string sql = server.ServerName == "mncportalprod-sql.database.windows.net" ? "Select distinct [name] database_name from sys.databases where [name] not in ('master','tempdb','msdb','SSISDB','model','TruncateTest1')" : "Select distinct [name] database_name from master.sys.databases where [name] not in ('master','tempdb','msdb','SSISDB','model','TruncateTest1')";

            using (var connection = new System.Data.SqlClient.SqlConnection(connectionString))
            {
                try
                {
                    var output = connection.Query<string>(sql, new { }).ToList();
                    foreach (string databaseName in output)
                    {
                        databases.Add(new DatabaseModel
                        {
                            ServerId = server.Id,
                            DatabaseName = databaseName,
                            Purpose = "",
                            CreatedBy = "3c8d4fa8-9587-43b8-8de5-d5e28f80dba5",
                            CreatedDate = DateTime.UtcNow,
                            UpdatedBy = "3c8d4fa8-9587-43b8-8de5-d5e28f80dba5",
                            UpdatedDate = DateTime.UtcNow
                        });
                    }
                }
                catch (SqlException e)
                {
                    Console.WriteLine(e.Message);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }

            return databases;
        }

        private void SaveAllDatabases(List<DatabaseModel> foundDatabases)
        {
            Console.WriteLine("Entered SaveAllDatabases");
            using (var connection = new System.Data.SqlClient.SqlConnection(GetConnectionString("mncportalprod-sql.database.windows.net")))
            {
                string sql = @"
                    if ((select count(*) from dbo.[Databases] where ServerId = @ServerId and DatabaseName = @DatabaseName) = 0 )
                    begin
                        INSERT INTO dbo.[Databases](ServerId,
                        DatabaseName,
                        Purpose,
                        CreatedDate,
                        CreatedBy,
                        UpdatedDate,
                        UpdatedBy)
                        values
                        (@ServerId,
                        @DatabaseName,
                        @Purpose,
                        @CreatedDate,
                        @CreatedBy,
                        @UpdatedDate,
                        @UpdatedBy);
                    end; ";

                foreach (DatabaseModel database in foundDatabases)
                {
                    try
                    {
                        using (SqlCommand command = new SqlCommand(sql, connection))
                        {
                            command.Parameters.AddWithValue("@ServerId", database.ServerId);
                            command.Parameters.AddWithValue("@DatabaseName", database.DatabaseName);
                            command.Parameters.AddWithValue("@Purpose", database.Purpose);
                            command.Parameters.AddWithValue("@CreatedDate", database.CreatedDate);
                            command.Parameters.AddWithValue("@CreatedBy", database.CreatedBy);
                            command.Parameters.AddWithValue("@UpdatedDate", database.UpdatedDate);
                            command.Parameters.AddWithValue("@UpdatedBy", database.UpdatedBy);

                            connection.Open();
                            var affectedRows = command.ExecuteNonQuery();
                            connection.Close();
                        }
                    }
                    catch (Exception e)
                    {
                        Console.Write(e.Message);
                    }
                }
            }
        }

        #endregion

		#region Schema Mapping

		public bool CanUpdateSelectedSchemaMapping
        {
            get
            {
                return Properties.Settings.Default.DataArchitecture_CanRunTableScrape;
            }
        }

        public async Task UpdateSelectedSchemaMapping(int nestedLevels = 0)
        {
            Console.WriteLine("Entering DataArchitectureViewModel.UpdateSelectedSchemaMapping().");

            _eventAggregator.PublishOnUIThread($"Searching schema: { SelectedServer.ServerName }.{ SelectedDatabase.DatabaseName }.{ SelectedSchema.SchemaName }...");
            await FindTables(SelectedServer, SelectedDatabase, SelectedSchema);
            await LoadTables();

            List<TableModel> schemaTables = _tables.Where(x => SelectedSchema.Id == x.SchemaId).ToList();
            Console.WriteLine($"UpdateDatabaseMapping: schemaTables Count: { schemaTables.Count }");

            if (nestedLevels > 0)
            {
                foreach (TableModel schemaTable in schemaTables)
                {
                    await UpdateTableMapping(SelectedServer, SelectedDatabase, SelectedSchema, schemaTable, nestedLevels > 1 ? 1 : 0);
                    //Console.WriteLine($"Searching table: { SelectedServer.ServerName }.{ SelectedDatabase.DatabaseName }.{ SelectedSchema.SchemaName }.{ schemaTable.TableName }...");
                    //await FindFields(SelectedServer, SelectedDatabase, SelectedSchema, schemaTable);
                    //await LoadColumns();

                    //List<FieldModel> tableColumns = _columns.Where(x => schemaTable.Id == x.TableId).ToList();
                    //Console.WriteLine($"UpdateDatabaseMapping: tableColumns Count: { tableColumns.Count }");
                }
            }
        }

        public async Task FindSchemas(ServerModel server, DatabaseModel database)
        {
            Console.WriteLine("Entering DataArchitectureViewModel.FindSchemas().");

            List<SchemaModel> foundSchemas = FindAllSchemas(server, database);
            SaveAllSchemas(foundSchemas);
        }

        private List<SchemaModel> FindAllSchemas(ServerModel server, DatabaseModel database)
        {
            Console.WriteLine("Entered FindAllSchemas");
            List<SchemaModel> schemas = new List<SchemaModel>();
            string connectionString = string.Empty;

            if (server.ServerName == "mncportalprod-sql.database.windows.net" ||
                server.ServerName == "secondary-mncportalprod.database.windows.net")
            {
                connectionString = GetConnectionString(server.ServerName, database.DatabaseName);
            }
            else
            {
                connectionString = GetConnectionString(server.ServerName);
            }

            string sql = $@"
                USE { database.DatabaseName };
                SELECT SCHEMA_NAME AS schemaName
                FROM INFORMATION_SCHEMA.SCHEMATA
                WHERE SCHEMA_NAME NOT LIKE 'db[_]%'
                    AND SCHEMA_NAME NOT LIKE 'CORP[\]%'
                    AND SCHEMA_NAME NOT IN 
                        (
                            'INFORMATION_SCHEMA'
                            ,'sys'
                            ,'guest'
                        );";

            using (var connection = new System.Data.SqlClient.SqlConnection(connectionString))
            {
                try
                {
                    var output = connection.Query<string>(sql, new { }).ToList();
                    foreach (string schemaName in output)
                    {
                        schemas.Add(new SchemaModel
                        {
                            ServerId = server.Id,
                            DatabaseId = database.Id,
                            SchemaName = schemaName,
                            Purpose = "",
                            CreatedBy = "3c8d4fa8-9587-43b8-8de5-d5e28f80dba5",
                            CreatedDate = DateTime.UtcNow,
                            UpdatedBy = "3c8d4fa8-9587-43b8-8de5-d5e28f80dba5",
                            UpdatedDate = DateTime.UtcNow
                        });
                    }
                }
                catch (Exception e)
                {
                    Console.Write(e.Message);
                }
            }

            return schemas;
        }

        private void SaveAllSchemas(List<SchemaModel> foundSchemas)
        {
            Console.WriteLine("Entered SaveAllSchemas");
            using (var connection = new System.Data.SqlClient.SqlConnection(GetConnectionString("mncportalprod-sql.database.windows.net")))
            {
                string sql = @"
                    if ((select count(*) FROM dbo.[Schemas] WHERE ServerId = @ServerId AND DatabaseId = @DatabaseId AND SchemaName = @SchemaName) = 0 )
                    begin
                        INSERT INTO dbo.[Schemas](ServerId,
                        DatabaseId,
                        SchemaName,
                        Purpose,
                        CreatedDate,
                        CreatedBy,
                        UpdatedDate,
                        UpdatedBy)
                        values
                        (@ServerId,
                        @DatabaseId,
                        @SchemaName,
                        @Purpose,
                        @CreatedDate,
                        @CreatedBy,
                        @UpdatedDate,
                        @UpdatedBy);
                    end; ";

                foreach (SchemaModel schema in foundSchemas)
                {
                    try
                    {
                        using (SqlCommand command = new SqlCommand(sql, connection))
                        {
                            command.Parameters.AddWithValue("@ServerId", schema.ServerId);
                            command.Parameters.AddWithValue("@DatabaseId", schema.DatabaseId);
                            command.Parameters.AddWithValue("@SchemaName", schema.SchemaName);
                            command.Parameters.AddWithValue("@Purpose", schema.Purpose);
                            command.Parameters.AddWithValue("@CreatedDate", schema.CreatedDate);
                            command.Parameters.AddWithValue("@CreatedBy", schema.CreatedBy);
                            command.Parameters.AddWithValue("@UpdatedDate", schema.UpdatedDate);
                            command.Parameters.AddWithValue("@UpdatedBy", schema.UpdatedBy);

                            connection.Open();
                            var affectedRows = command.ExecuteNonQuery();
                            connection.Close();
                        }
                    }
                    catch (Exception e)
                    {
                        Console.Write(e.Message);
                    }
                }
            }
        }

        #endregion

        #region Table Mapping

        public bool CanUpdateSelectedTableMapping
        {
            get
            {
                if ( SelectedTable != null )
                {
                    if ( SelectedTable.Id > 0
                        && Properties.Settings.Default.DataArchitecture_CanRunFieldScrape )
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            }
        }

        public async Task UpdateSelectedTableMapping(int nestedLevels = 0)
        {
            Console.WriteLine("Entering DataArchitectureViewModel.UpdateSelectedTableMapping().");

            _eventAggregator.PublishOnUIThread(new MessageEvent($"Searching table: { SelectedServer.ServerName }.{ SelectedDatabase.DatabaseName }.{ SelectedSchema.SchemaName }.{ SelectedTable.TableName }..."));
            await FindFields(SelectedServer, SelectedDatabase, SelectedSchema, SelectedTable, nestedLevels);
            await LoadColumns();

            List<FieldModel> tableColumns = _columns.Where(x => SelectedTable.Id == x.TableId).ToList();
            Console.WriteLine($"UpdateDatabaseMapping: tableColumns Count: { tableColumns.Count }");
        }

        public bool CanUpdateTableMapping
        {
            get
            {
                return Properties.Settings.Default.DataArchitecture_CanRunFieldScrape;
            }
        }

        public async Task UpdateTableMapping(ServerModel server, DatabaseModel database, SchemaModel schema, TableModel table, int nestedLevels = 0)
        {
            Console.WriteLine("Entering DataArchitectureViewModel.UpdateTableMapping().");

            _eventAggregator.PublishOnUIThread(new MessageEvent($"Searching table: { server.ServerName }.{ database.DatabaseName }.{ schema.SchemaName }.{ table.TableName }..."));
            await FindFields(server, database, schema, table, nestedLevels);
            await LoadColumns();

            List<FieldModel> tableColumns = _columns.Where(x => table.Id == x.TableId).ToList();
            Console.WriteLine($"UpdateDatabaseMapping: tableColumns Count: { tableColumns.Count }");
        }

        public async Task FindTables(ServerModel server, DatabaseModel database, SchemaModel schema)
        {
            Console.WriteLine("Entering DataArchitectureViewModel.FindTables().");

            List<TableModel> foundTables = FindAllTables(server, database, schema);
            SaveAllTables(foundTables);
        }

        private List<TableModel> FindAllTables(ServerModel server, DatabaseModel database, SchemaModel schema)
        {
            Console.WriteLine("Entering DataArchitectureViewModel.FindAllTables().");

            List<TableModel> tables = new List<TableModel>();
            string connectionString = string.Empty;

            if (server.ServerName == "mncportalprod-sql.database.windows.net" ||
                 server.ServerName == "secondary-mncportalprod.database.windows.net")
            {
                connectionString = GetConnectionString(server.ServerName, database.DatabaseName);
            }
            else
            {
                connectionString = GetConnectionString(server.ServerName);
            }

            string sql = $@"
                USE { database.DatabaseName };
                SELECT DISTINCT TABLE_NAME AS tableName
                FROM INFORMATION_SCHEMA.COLUMNS
                WHERE TABLE_CATALOG = '{ database.DatabaseName }'
                    AND TABLE_SCHEMA = '{ schema.SchemaName }'";

            using (var connection = new System.Data.SqlClient.SqlConnection(connectionString))
            {
                try
                {
                    var output = connection.Query<string>(sql, new { }).ToList();
                    foreach (string tableName in output)
                    {
                        tables.Add(new TableModel
                        {
                            ServerId = server.Id,
                            DatabaseId = database.Id,
                            SchemaId = schema.Id,
                            TableName = tableName,
                            Purpose = "",
                            CreatedBy = "3c8d4fa8-9587-43b8-8de5-d5e28f80dba5",
                            CreatedDate = DateTime.UtcNow,
                            UpdatedBy = "3c8d4fa8-9587-43b8-8de5-d5e28f80dba5",
                            UpdatedDate = DateTime.UtcNow
                        });
                    }
                }
                catch (Exception e)
                {
                    Console.Write(e.Message);
                }
            }

            return tables;
        }

        private void SaveAllTables(List<TableModel> foundTables)
        {
            Console.WriteLine("Entered SaveAllTables");
            using (var connection = new System.Data.SqlClient.SqlConnection(GetConnectionString("mncportalprod-sql.database.windows.net")))
            {
                string sql = @"
                    if (
                        (
                            select count(*) 
                            FROM dbo.[Tables] 
                            WHERE ServerId = @ServerId 
                                AND DatabaseId = @DatabaseId 
                                AND SchemaId = @SchemaId
                                AND TableName = @TableName
                        ) = 0 
                    )
                    begin
                        INSERT INTO dbo.[Tables](ServerId,
                        DatabaseId,
                        SchemaId,
                        TableName,
                        Purpose,
                        CreatedDate,
                        CreatedBy,
                        UpdatedDate,
                        UpdatedBy)
                        values
                        (@ServerId,
                        @DatabaseId,
                        @SchemaId,
                        @TableName,
                        @Purpose,
                        @CreatedDate,
                        @CreatedBy,
                        @UpdatedDate,
                        @UpdatedBy);
                    end; ";

                foreach (TableModel table in foundTables)
                {
                    try
                    {
                        using (SqlCommand command = new SqlCommand(sql, connection))
                        {
                            command.Parameters.AddWithValue("@ServerId", table.ServerId);
                            command.Parameters.AddWithValue("@DatabaseId", table.DatabaseId);
                            command.Parameters.AddWithValue("@SchemaId", table.SchemaId);
                            command.Parameters.AddWithValue("@TableName", table.TableName);
                            command.Parameters.AddWithValue("@Purpose", table.Purpose);
                            command.Parameters.AddWithValue("@CreatedDate", table.CreatedDate);
                            command.Parameters.AddWithValue("@CreatedBy", table.CreatedBy);
                            command.Parameters.AddWithValue("@UpdatedDate", table.UpdatedDate);
                            command.Parameters.AddWithValue("@UpdatedBy", table.UpdatedBy);

                            connection.Open();
                            var affectedRows = command.ExecuteNonQuery();
                            connection.Close();
                        }
                    }
                    catch (Exception e)
                    {
                        Console.Write(e.Message);
                    }
                }
            }
        }

		#endregion

		#region Field Mapping

		public async Task FindFields(ServerModel server, DatabaseModel database, SchemaModel schema, TableModel table, int nestedLevel = 0)
        {
            Console.WriteLine("Entering DataArchitectureViewModel.FindFields().");

            List<FieldModel> foundFields = FindAllFields(server, database, schema, table, nestedLevel);
            SaveAllFields(foundFields);
            _eventAggregator.PublishOnUIThread(new MessageEvent($"Updated table mapping for { server.ServerName }.{ database.DatabaseName }.{ schema.SchemaName }.{ table.TableName }."));
        }

        private List<FieldModel> FindAllFields(ServerModel server, DatabaseModel database, SchemaModel schema, TableModel table, int nestedLevel = 0)
        {
            Console.WriteLine("Entered FindAllFields");
            List<FieldModel> fields = new List<FieldModel>();

            string connectionString = string.Empty;

            if (server.ServerName == "mncportalprod-sql.database.windows.net" ||
                 server.ServerName == "secondary-mncportalprod.database.windows.net")
            {
                connectionString = GetConnectionString(server.ServerName, database.DatabaseName);
            }
            else
            {
                connectionString = GetConnectionString(server.ServerName);
            }

            string sql = $@"
                USE { database.DatabaseName };
                SELECT COLUMN_NAME AS columnName,
                    ORDINAL_POSITION AS ordinalNumber,
                    COLUMN_DEFAULT AS defaultValue,
                    IS_NULLABLE AS nullable,
                    DATA_TYPE AS dataType,
                    CHARACTER_MAXIMUM_LENGTH AS characterLength,
                    NUMERIC_PRECISION AS numericPrecision,
                    NUMERIC_SCALE AS numericScale,
                    DATETIME_PRECISION AS datetimePrecision,
                    CHARACTER_SET_NAME AS characterSetName,
                    COLLATION_NAME AS collationName
                FROM INFORMATION_SCHEMA.COLUMNS
                WHERE TABLE_CATALOG = '{ database.DatabaseName }'
                    AND TABLE_SCHEMA = '{ schema.SchemaName }'
                    AND TABLE_NAME = '{ table.TableName}';";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        if (connection.State == ConnectionState.Closed)
                            connection.Open();

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                FieldModel valueField = new FieldModel();

                                if (nestedLevel > 0)
                                {
                                    valueField = GetFieldValues(server, database, schema, table, reader["columnName"].ToString());
                                }
                                fields.Add(new FieldModel
                                {
                                    ServerId = server.Id,
                                    DatabaseId = database.Id,
                                    SchemaId = schema.Id,
                                    TableId = table.Id,
                                    FullFieldName = reader["columnName"].ToString(),
                                    Purpose = "",
                                    OrdinalNumber = reader["ordinalNumber"].ToString().Length > 0 ? int.Parse(reader["ordinalNumber"].ToString()) : 0,
                                    DefaultValue = reader["defaultValue"].ToString().Length < 1 ? "" : reader["defaultValue"].ToString(),
                                    IsNullable = reader["nullable"].ToString() == "NO" ? 0 : 1,
                                    DataType = reader["dataType"].ToString(),
                                    CharacterLength = reader["characterLength"].ToString().Length > 0 ? int.Parse(reader["characterLength"].ToString()) : 0,
                                    NumericPrecision = reader["numericPrecision"].ToString().Length > 0 ? int.Parse(reader["numericPrecision"].ToString()) : 0,
                                    NumericScale = reader["numericScale"].ToString().Length > 0 ? int.Parse(reader["numericScale"].ToString()) : 0,
                                    DateTimePrecision = reader["datetimePrecision"].ToString().Length > 0 ? int.Parse(reader["datetimePrecision"].ToString()) : 0,
                                    CharacterSetName = reader["characterSetName"].ToString().Length < 1 ? "" : reader["characterSetName"].ToString(),
                                    CollationName = reader["collationName"].ToString().Length < 1 ? "" : reader["collationName"].ToString(),
                                    PrimaryKey = 0,
                                    Indexed = 0,
                                    MinValue = valueField.MinValue,
                                    MaxValue = valueField.MaxValue,
                                    SampleValue1 = valueField.SampleValue1,
                                    SampleValue2 = valueField.SampleValue2,
                                    SampleValue3 = valueField.SampleValue3,
                                    SampleValue4 = valueField.SampleValue4,
                                    SampleValue5 = valueField.SampleValue5,
                                    SampleValue6 = valueField.SampleValue6,
                                    SampleValue7 = valueField.SampleValue7,
                                    SampleValue8 = valueField.SampleValue8,
                                    SampleValue9 = valueField.SampleValue9,
                                    SampleValue10 = valueField.SampleValue10,
                                    CreatedBy = "3c8d4fa8-9587-43b8-8de5-d5e28f80dba5",
                                    CreatedDate = DateTime.UtcNow,
                                    UpdatedBy = "3c8d4fa8-9587-43b8-8de5-d5e28f80dba5",
                                    UpdatedDate = DateTime.UtcNow
                                });
                            }
                        }
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }

            return fields;
        }

        private FieldModel GetFieldValues(ServerModel server, DatabaseModel database, SchemaModel schema, TableModel table, string fieldName)
        {
            Console.WriteLine("Entered GetFieldValues");
            FieldModel values = new FieldModel();

            string connectionString = string.Empty;

            if (server.ServerName == "mncportalprod-sql.database.windows.net" ||
                 server.ServerName == "secondary-mncportalprod.database.windows.net")
            {
                connectionString = GetConnectionString(server.ServerName, database.DatabaseName);
            }
            else
            {
                connectionString = GetConnectionString(server.ServerName);
            }

            string sql = $@"
                USE { database.DatabaseName };
                declare 
	                @pkey bit
	                , @indexed bit  
                    , @bittype int
	                , @min varchar(max)
	                , @max varchar(max)
	                , @sampleValue1 varchar(max)
	                , @sampleValue2 varchar(max)
	                , @sampleValue3 varchar(max)
	                , @sampleValue4 varchar(max)
	                , @sampleValue5 varchar(max)
	                , @sampleValue6 varchar(max)
	                , @sampleValue7 varchar(max)
	                , @sampleValue8 varchar(max)
	                , @sampleValue9 varchar(max)
	                , @sampleValue10 varchar(max)

                set @pkey = (
	                select case when count(col.[name]) > 0 
			                then 1 
			                else 0 end
	                from sys.tables tab
		                inner join sys.indexes pk
			                on tab.[name] = '{ table.TableName }'
			                and pk.is_primary_key = 1
			                and tab.object_id = pk.object_id
		                inner join sys.index_columns ic
			                on ic.object_id = pk.object_id
			                and ic.index_id = pk.index_id
		                inner join sys.columns col
			                on col.[name] = '{ fieldName }'
			                and pk.object_id = col.object_id
			                and col.column_id = ic.column_id
	                )
                set @indexed  = (
	                select case when count(col.[name]) > 0 
			                then 1 
			                else 0 end
	                from sys.tables tab
		                inner join sys.indexes pk
			                on tab.object_id = pk.object_id
			                and tab.[name] = '{ table.TableName }'
		                inner join sys.index_columns ic
			                on ic.object_id = pk.object_id
			                and ic.index_id = pk.index_id
		                inner join sys.columns col
			                on pk.object_id = col.object_id
			                and col.column_id = ic.column_id
			                and col.[name] = '{ fieldName }'
	                )
                set @bittype = (
                    SELECT Count(DATA_TYPE)
                    FROM INFORMATION_SCHEMA.COLUMNS
                    WHERE TABLE_CATALOG = '{database.DatabaseName}'
                        AND TABLE_SCHEMA = '{schema.SchemaName}'
                        AND TABLE_NAME = '{table.TableName}'
                        AND COLUMN_NAME = '{fieldName}'
                        AND DATA_TYPE = 'bit'
                        )
                set @min = (
	                SELECT MIN(CASE WHEN @bittype = 1 THEN cast(0 as varchar(max)) else cast([{ fieldName }] as varchar(max)) END)
	                from [{ database.DatabaseName }].[{ schema.SchemaName }].[{ table.TableName }]
                    WHERE [{fieldName}] IS NOT NULL
	                )
                set @max = (
	                SELECT MAX(CASE WHEN @bittype = 1 THEN cast(1 as varchar(max)) else cast([{ fieldName }] as varchar(max)) END) 
	                from [{ database.DatabaseName }].[{ schema.SchemaName }].[{ table.TableName }]
                    WHERE [{fieldName}] IS NOT NULL
	                )
                set @sampleValue1 = (
	                select distinct top 1 [{ fieldName }]
	                from [{ database.DatabaseName }].[{ schema.SchemaName }].[{ table.TableName }] a 
                    WHERE [{fieldName}] IS NOT NULL
	                )
                set @sampleValue2 = (
	                select distinct top 1 [{ fieldName }]
	                from [{ database.DatabaseName }].[{ schema.SchemaName }].[{ table.TableName }] a 
	                where [{fieldName}] IS NOT NULL AND [{ fieldName }] <> @sampleValue1
	                )
                set @sampleValue3 = (
	                select distinct top 1 [{ fieldName }]
	                from [{ database.DatabaseName }].[{ schema.SchemaName }].[{ table.TableName }] a 
	                where [{fieldName}] IS NOT NULL AND [{ fieldName }] not in (@sampleValue1, @sampleValue2)
	                )
                set @sampleValue4 = (
	                select distinct top 1 [{ fieldName }]
	                from [{ database.DatabaseName }].[{ schema.SchemaName }].[{ table.TableName }] a 
	                where [{fieldName}] IS NOT NULL AND [{ fieldName }] not in (@sampleValue1, @sampleValue2, @sampleValue3)
	                )
                set @sampleValue5 = (
	                select distinct top 1 [{ fieldName }]
	                from [{ database.DatabaseName }].[{ schema.SchemaName }].[{ table.TableName }] a 
	                where [{fieldName}] IS NOT NULL AND [{ fieldName }] not in (@sampleValue1, @sampleValue2, @sampleValue3, @sampleValue4)
	                )
                set @sampleValue6 = (
	                select distinct top 1 [{ fieldName }]
	                from [{ database.DatabaseName }].[{ schema.SchemaName }].[{ table.TableName }] a 
	                where [{fieldName}] IS NOT NULL AND [{ fieldName }] not in (@sampleValue1, @sampleValue2, @sampleValue3, @sampleValue4, @sampleValue5)
	                )
                set @sampleValue7 = (
	                select distinct top 1 [{ fieldName }]
	                from [{ database.DatabaseName }].[{ schema.SchemaName }].[{ table.TableName }] a 
	                where [{fieldName}] IS NOT NULL AND [{ fieldName }] not in (@sampleValue1, @sampleValue2, @sampleValue3, @sampleValue4, @sampleValue5, @sampleValue6)
	                )
                set @sampleValue8 = (
	                select distinct top 1 [{ fieldName }]
	                from [{ database.DatabaseName }].[{ schema.SchemaName }].[{ table.TableName }] a 
	                where [{fieldName}] IS NOT NULL AND [{ fieldName }] not in (@sampleValue1, @sampleValue2, @sampleValue3, @sampleValue4, @sampleValue5, @sampleValue6, @sampleValue7)
	                )
                set @sampleValue9 = (
	                select distinct top 1 [{ fieldName }]
	                from [{ database.DatabaseName }].[{ schema.SchemaName }].[{ table.TableName }] a 
	                where [{fieldName}] IS NOT NULL AND [{ fieldName }] not in (@sampleValue1, @sampleValue2, @sampleValue3, @sampleValue4, @sampleValue5, @sampleValue6, @sampleValue7, @sampleValue8)
	                )
                set @sampleValue10 = (
	                select distinct top 1 [{ fieldName }]
	                from [{ database.DatabaseName }].[{ schema.SchemaName }].[{ table.TableName }] a 
	                where [{fieldName}] IS NOT NULL AND [{ fieldName }] not in (@sampleValue1, @sampleValue2, @sampleValue3, @sampleValue4, @sampleValue5, @sampleValue6, @sampleValue7, @sampleValue8, @sampleValue9)
	                )

                select @pkey PrimaryKey
	                , @indexed Indexed
	                , @min MinValue
	                , @max MaxValue
	                , @sampleValue1 SampleValue1
	                , @sampleValue2 SampleValue2
	                , @sampleValue3 SampleValue3
	                , @sampleValue4 SampleValue4
	                , @sampleValue5 SampleValue5
	                , @sampleValue6 SampleValue6
	                , @sampleValue7 SampleValue7
	                , @sampleValue8 SampleValue8
	                , @sampleValue9 SampleValue9
	                , @sampleValue10 SampleValue10";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.CommandTimeout = 0;
                        if (connection.State == ConnectionState.Closed)
                            connection.Open();

                        using (SqlDataAdapter adapter = new SqlDataAdapter())
                        {
                            adapter.SelectCommand = command;
                            DataSet dataSet = new DataSet();
                            adapter.Fill(dataSet);
                            DataTable dataTable = dataSet.Tables[0];
                            DataRow dataRow = dataTable.Rows[0];

                            values.ServerId = server.Id;
                            values.DatabaseId = database.Id;
                            values.SchemaId = schema.Id;
                            values.TableId = table.Id;
                            values.FullFieldName = fieldName;
                            values.PrimaryKey = dataRow[0].ToString() == "true" ? 1 : 0;
                            values.Indexed = dataRow[1].ToString() == "true" ? 1 : 0;
                            values.MinValue = dataRow[2].ToString();
                            values.MaxValue = dataRow[3].ToString();
                            values.SampleValue1 = dataRow[4].ToString();
                            values.SampleValue2 = dataRow[5].ToString();
                            values.SampleValue3 = dataRow[6].ToString();
                            values.SampleValue4 = dataRow[7].ToString();
                            values.SampleValue5 = dataRow[8].ToString();
                            values.SampleValue6 = dataRow[9].ToString();
                            values.SampleValue7 = dataRow[10].ToString();
                            values.SampleValue8 = dataRow[11].ToString();
                            values.SampleValue9 = dataRow[12].ToString();
                            values.SampleValue10 = dataRow[13].ToString();
                        }
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }

            return values;
        }

        private void SaveAllFields(List<FieldModel> foundFields)
        {
            Console.WriteLine("Entered ToolboxUI.ViewModels.DataArchitectureViewModel.SaveAllFields");

            using (var connection = new System.Data.SqlClient.SqlConnection(GetConnectionString("mncportalprod-sql.database.windows.net")))
            {
                string sql = @"
                    if (
                        (
                            select count(*) 
                            FROM dbo.[Fields] 
                            WHERE ServerId = @ServerId 
                                AND DatabaseId = @DatabaseId 
                                AND SchemaId = @SchemaId
                                AND TableId = @TableId
                                AND FieldName = @FieldName
                        ) = 0 
                    )
                    begin
                        INSERT INTO dbo.[Fields](ServerId,
                        DatabaseId,
                        SchemaId,
                        TableId,
                        FieldName,
                        Purpose,
                        OrdinalNumber,
                        DefaultValue,
                        Nullable,
                        DataType,
                        CharacterLength,
                        NumericPrecision,
                        NumericScale,
                        DatetimePrecision,
                        CharacterSetName,
                        [CollationName],
                        PrimaryKey,
                        Indexed,
                        MinValue,
                        MaxValue,
                        SampleValue1,
                        SampleValue2,
                        SampleValue3,
                        SampleValue4,
                        SampleValue5,
                        SampleValue6,
                        SampleValue7,
                        SampleValue8,
                        SampleValue9,
                        SampleValue10,
                        CreatedDate,
                        CreatedBy,
                        UpdatedDate,
                        UpdatedBy)
                        values
                        (@ServerId,
                        @DatabaseId,
                        @SchemaId,
                        @TableId,
                        @FieldName,
                        @Purpose,
                        @OrdinalNumber,
                        @DefaultValue,
                        @Nullable,
                        @DataType,
                        @CharacterLength,
                        @NumericPrecision,
                        @NumericScale,
                        @DatetimePrecision,
                        @CharacterSetName,
                        @CollationName,
                        @PrimaryKey,
                        @Indexed,
                        @MinValue,
                        @MaxValue,
                        @SampleValue1,
                        @SampleValue2,
                        @SampleValue3,
                        @SampleValue4,
                        @SampleValue5,
                        @SampleValue6,
                        @SampleValue7,
                        @SampleValue8,
                        @SampleValue9,
                        @SampleValue10,
                        @CreatedDate,
                        @CreatedBy,
                        @UpdatedDate,
                        @UpdatedBy);
                    end
                    else
                    begin
                        UPDATE dbo.[Fields]
                            SET Purpose = @Purpose,
                            OrdinalNumber = @OrdinalNumber,
                            DefaultValue = @DefaultValue,
                            Nullable = @Nullable,
                            DataType = @DataType,
                            CharacterLength = @CharacterLength,
                            NumericPrecision = @NumericPrecision,
                            NumericScale = @NumericScale,
                            DatetimePrecision = @DatetimePrecision,
                            CharacterSetName = @CharacterSetName,
                            [CollationName] = @CollationName,
                            PrimaryKey = @PrimaryKey,
                            Indexed = @Indexed,
                            MinValue = @MinValue,
                            MaxValue = @MaxValue,
                            SampleValue1 = @SampleValue1,
                            SampleValue2 = @SampleValue2,
                            SampleValue3 = @SampleValue3,
                            SampleValue4 = @SampleValue4,
                            SampleValue5 = @SampleValue5,
                            SampleValue6 = @SampleValue6,
                            SampleValue7 = @SampleValue7,
                            SampleValue8 = @SampleValue8,
                            SampleValue9 = @SampleValue9,
                            SampleValue10 = @SampleValue10,
                            UpdatedDate = @UpdatedDate,
                            UpdatedBy = @UpdatedBy
                        WHERE ServerId = @ServerId 
                            AND DatabaseId = @DatabaseId 
                            AND SchemaId = @SchemaId
                            AND TableId = @TableId
                            AND FieldName = @FieldName
                    end; ";

                foreach (FieldModel field in foundFields)
                {
                    try
                    {
                        using (SqlCommand command = new SqlCommand(sql, connection))
                        {
                            command.Parameters.AddWithValue("@ServerId", field.ServerId);
                            command.Parameters.AddWithValue("@DatabaseId", field.DatabaseId);
                            command.Parameters.AddWithValue("@SchemaId", field.SchemaId);
                            command.Parameters.AddWithValue("@TableId", field.TableId);
                            command.Parameters.AddWithValue("@FieldName", field.FullFieldName);
                            command.Parameters.AddWithValue("@Purpose", field.Purpose);
                            command.Parameters.AddWithValue("@OrdinalNumber", field.OrdinalNumber);
                            command.Parameters.AddWithValue("@DefaultValue", field.DefaultValue);
                            command.Parameters.AddWithValue("@Nullable", field.IsNullable);
                            command.Parameters.AddWithValue("@DataType", field.DataType);
                            command.Parameters.AddWithValue("@CharacterLength", field.CharacterLength);
                            command.Parameters.AddWithValue("@NumericPrecision", field.NumericPrecision);
                            command.Parameters.AddWithValue("@NumericScale", field.NumericScale);
                            command.Parameters.AddWithValue("@DatetimePrecision", field.DateTimePrecision);
                            command.Parameters.AddWithValue("@CharacterSetName", field.CharacterSetName);
                            command.Parameters.AddWithValue("@CollationName", field.CollationName);
                            command.Parameters.AddWithValue("@PrimaryKey", field.PrimaryKey);
                            command.Parameters.AddWithValue("@Indexed", field.Indexed);
                            command.Parameters.AddWithValue("@MinValue", field.MinValue != null ? (field.DataType == "bit" ? "0" : field.MinValue) : "");
                            command.Parameters.AddWithValue("@MaxValue", field.MaxValue != null ? (field.DataType == "bit" ? "1" : field.MaxValue) : "");
                            command.Parameters.AddWithValue("@SampleValue1", field.SampleValue1 != null ? (field.DataType == "bit" ? "0" : field.SampleValue1) : "");
                            command.Parameters.AddWithValue("@SampleValue2", field.SampleValue2 != null ? (field.DataType == "bit" ? "1" : field.SampleValue2) : "");
                            command.Parameters.AddWithValue("@SampleValue3", field.SampleValue3 != null ? field.SampleValue3 : "");
                            command.Parameters.AddWithValue("@SampleValue4", field.SampleValue4 != null ? field.SampleValue4 : "");
                            command.Parameters.AddWithValue("@SampleValue5", field.SampleValue5 != null ? field.SampleValue5 : "");
                            command.Parameters.AddWithValue("@SampleValue6", field.SampleValue6 != null ? field.SampleValue6 : "");
                            command.Parameters.AddWithValue("@SampleValue7", field.SampleValue7 != null ? field.SampleValue7 : "");
                            command.Parameters.AddWithValue("@SampleValue8", field.SampleValue8 != null ? field.SampleValue8 : "");
                            command.Parameters.AddWithValue("@SampleValue9", field.SampleValue9 != null ? field.SampleValue9 : "");
                            command.Parameters.AddWithValue("@SampleValue10", field.SampleValue10 != null ? field.SampleValue10 : "");
                            command.Parameters.AddWithValue("@CreatedDate", field.CreatedDate);
                            command.Parameters.AddWithValue("@CreatedBy", field.CreatedBy);
                            command.Parameters.AddWithValue("@UpdatedDate", field.UpdatedDate);
                            command.Parameters.AddWithValue("@UpdatedBy", field.UpdatedBy);

                            connection.Open();
                            var affectedRows = command.ExecuteNonQuery();
                            connection.Close();
                        }
                    }
                    catch (Exception e)
                    {
                        Console.Write(e.Message);
                        if (connection.State == ConnectionState.Open)
                            connection.Close();
                    }
                }
            }
        }

        #endregion

        #region Data Lineage Mapping

        public bool CanFindDataTableLineage
        {
            get
            {
                return Properties.Settings.Default.DataArchitecure_CanFindDataTableLineage;
            }
        }

        public async Task FindDataTableLineage()
        {
            Console.WriteLine("Entering DataArchitectureViewModel.FindDataTableLineage().");

            await ScanETLSubPackage();
            await SaveAllDataTableLineage();
        }

        private async Task ScanETLSubPackage()
        {
            Console.WriteLine("Entering DataArchitectureViewModel.ScanETLSubPackage().");

            var dataTableLineage = await _dataTableLineageEndpoint.FindDataTableLineages();
            await _eventAggregator.PublishOnUIThreadAsync(new MessageEvent($"Found { dataTableLineage.Count } records of Data Table Lineage."));

            FoundDataTableLineages = new BindingList<DataTableLineageModel>(dataTableLineage);
		}

        private async Task SaveAllDataTableLineage()
        {
            Console.WriteLine("Entering DataArchitectureViewModel.SaveAllDataTableLineage().");
            DateTime defaultDateTime = new DateTime(2999,12,31,23,59,59,999);

            foreach (DataTableLineageModel tableLineage in FoundDataTableLineages)
            {

                if ( tableLineage.LineageStartDate.Year >= 2017 )
				{
                    if ( tableLineage.LineageEndDate < tableLineage.LineageStartDate )
					{
                        tableLineage.LineageEndDate = defaultDateTime;
                        Console.WriteLine(tableLineage.ToString());
					}

                    await _dataTableLineageEndpoint.InsertDataTableLineage(tableLineage);
				}
            }

            await LoadDataTableLineages();
		}

		#endregion

		#region Connection Strings

		private string GetConnectionString(int serverId)
        {
            string connectionString;

            switch (serverId)
            {
                case 2:
                    connectionString = @"Data Source=AZE1SSQLME02; Integrated Security=true; Trusted_Connection=Yes";
                    break;
                case 3:
					connectionString = @"Server=mncportalprod-sql.database.windows.net; Authentication=Active Directory Password; User Id=svcASQLapi@mynexuscare.onmicrosoft.com; Password=myNEXUS2021";
					break;
                case 4:
                    connectionString = @"Data Source=AZE1DSQLME01.dev.corp.mynexuscare.com\DEV; Integrated Security=true; Trusted_Connection=Yes";
                    break;
                case 5:
					connectionString = @"Server=secondary-mncportalprod.database.windows.net; Authentication=Active Directory Password; User Id=svcASQLapi@mynexuscare.onmicrosoft.com; Password=myNEXUS2021";
					break;
                case 1:
                default:
                    connectionString = @"Data Source=AZE1PSQLME01; Integrated Security=true; Trusted_Connection=Yes";
                    break;
            }

            return connectionString;
        }

        private string GetConnectionString(string serverName, string databaseName = "ToolboxApp")
        {
            Console.WriteLine("Entered GetConnectionString");
            string connectionString;

            switch (serverName)
            {
                case "mncportalprod-sql.database.windows.net":
                case "secondary-mncportalprod.database.windows.net":
					connectionString = $@"Server={ serverName }; Authentication=Active Directory Password; Database={ databaseName }; UID=svcASQLapi@mynexuscare.onmicrosoft.com; PWD=myNEXUS2021";
					break;
                case "AZE1PSQLME01":
                case "AZE1SSQLME02":
                case @"AZE1DSQLME01.dev.corp.mynexuscare.com\DEV":
                default:
                    connectionString = $@"Data Source={ serverName }; Integrated Security=true; Trusted_Connection=Yes";
                    break;
            }

            return connectionString;
        }

		#endregion

		#region Navigation

        public async Task QuitToolbox()
        {
            Console.WriteLine("Entering DataArchitectureViewModel.QuitToolbox()");

            _eventAggregator.PublishOnUIThread(new QuitEvent());
        }


        public bool CanNavigateToSettingsView
        {
            get
            {
                return Properties.Settings.Default.Settings_CanView;
            }
        }

        public async Task NavigateToSettingsView()
        {
            Console.WriteLine("Entering DataArchitectureViewModel.NavigateToSettings()");

            _eventAggregator.PublishOnUIThread(new NavigateToViewEvent("Settings"));
        }


        public bool CanNavigateToTermsAndDefinitionsView
        {
            get
            {
                return Properties.Settings.Default.TermsAndDefinitions_CanView;
            }
        }

        public async Task NavigateToTermsAndDefinitionsView()
        {
            Console.WriteLine("Entering DataArchitectureViewModel.NavigateToTermsAndDefinitions()");

            _eventAggregator.PublishOnUIThread(new NavigateToViewEvent("Terms and Definitions"));
        }

		#endregion
	}
}
