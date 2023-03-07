using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToolboxUI.Library.Models
{

    public class DatabaseObjectModel
    {
        public int ServerId
        {
            get; set;
        }
        public string ServerName
        {
            get; set;
        }
        public int DatabaseId
        {
            get; set;
        }
        public string DatabaseName
        {
            get; set;
        }
        public int SchemaId
        {
            get; set;
        }
        public string SchemaName
        {
            get; set;
        }
        public int TableId
        {
            get; set;
        }
        public string TableName
        {
            get; set;
        }
        public int FieldId
        {
            get; set;
        }
        public string FieldName
        {
            get; set;
        }
    }
}