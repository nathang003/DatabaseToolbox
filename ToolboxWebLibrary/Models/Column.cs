

namespace ToolboxWebLibrary.Models;

public class Column
{
    public event Action Changed;

    public string Name { get; set; }
    public ColumnType Type { get; set; }
    public bool Primary { get; set; }

    public void Refresh() => Changed?.Invoke();

    public enum ColumnType
    {
        bigint,
        binary,
        bit,
        @char,
        date,
        datetime,
        datetime2,
        datetimeoffset,
        @decimal,
        @float,
        geography,
        geometry,
        hierarchyid,
        image,
        @int,
        money,
        nchar,
        ntext,
        numeric,
        nvarchar,
        real,
        smalldatetime,
        smallint,
        smallmoney,
        sqlvariant,
        text,
        time,
        timestamp,
        tinyint,
        uniqueidentifier,
        varbinary,
        varchar,
        xml
    }
}
