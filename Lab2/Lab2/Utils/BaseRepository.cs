using Microsoft.Data.SqlClient;
using System.Collections.ObjectModel;

namespace Lab2.Utils
{
    internal abstract class BaseRepository<BaseEntity>
    {
        abstract protected string InsertProcedure { get; }
        abstract protected string UpdateProcedure { get; }
        abstract protected string RemoveProcedure { get; }
        abstract protected string GetProcedure { get; }

        private SqlConnection _connection;
        protected SqlCommand _insertCommand;
        protected SqlCommand _updateCommand;
        protected SqlCommand _removeCommand;
        protected SqlCommand _getCommand;

        private ObservableCollection<BaseEntity> _items;

        public ObservableCollection<BaseEntity> Items
        {
            get { return _items; }
        }

        protected BaseRepository(SqlConnection connection)
        {
            _items = new ObservableCollection<BaseEntity>();
            _connection = connection;

            _insertCommand = new SqlCommand(InsertProcedure, connection);
            _insertCommand.CommandType = System.Data.CommandType.StoredProcedure;
            _updateCommand = new SqlCommand(UpdateProcedure, connection);
            _updateCommand.CommandType = System.Data.CommandType.StoredProcedure;
            _removeCommand = new SqlCommand(RemoveProcedure, connection);
            _removeCommand.CommandType = System.Data.CommandType.StoredProcedure;
            _getCommand = new SqlCommand(GetProcedure, connection);
            _getCommand.CommandType = System.Data.CommandType.StoredProcedure;
        }
    }
}
