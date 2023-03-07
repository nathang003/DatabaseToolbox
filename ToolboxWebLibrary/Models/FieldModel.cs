using System;
using System.ComponentModel.DataAnnotations;

namespace ToolboxWebLibrary.Models;

public class FieldModel
{
    public int FieldId
    {
        get; set;
    }
    public int ServerId
    {
        get; set;
    }
    public int DatabaseId
    {
        get; set;
    }
    public int SchemaId
    {
        get; set;
    }
    public int TableId
    {
        get; set;
    }

    [Required()]
    public string FieldName
    {
        get; set;
    }
    public string Purpose
    {
        get; set;
    }
    public int OrdinalNumber
    {
        get; set;
    }
    public string DefaultValue
    {
        get; set;
    }
    public int IsNullable
    {
        get; set;
    }
    public string DataType
    {
        get; set;
    }
    public int CharacterLength
    {
        get; set;
    }
    public int NumericPrecision
    {
        get; set;
    }
    public int NumericScale
    {
        get; set;
    }
    public int DateTimePrecision
    {
        get; set;
    }
    public string CharacterSetName
    {
        get; set;
    }
    public string CollationName
    {
        get; set;
    }
    public int PrimaryKey
    {
        get; set;
    }
    public int Indexed
    {
        get; set;
    }
    public string MinValue
    {
        get; set;
    }
    public string MaxValue
    {
        get; set;
    }
    public string SampleValue1
    {
        get; set;
    }
    public string SampleValue2
    {
        get; set;
    }
    public string SampleValue3
    {
        get; set;
    }
    public string SampleValue4
    {
        get; set;
    }
    public string SampleValue5
    {
        get; set;
    }
    public string SampleValue6
    {
        get; set;
    }
    public string SampleValue7
    {
        get; set;
    }
    public string SampleValue8
    {
        get; set;
    }
    public string SampleValue9
    {
        get; set;
    }
    public string SampleValue10
    {
        get; set;
    }
    public float NullPercentage
    {
        get; set;
    }
    public DateTime RemovalDate { get; set; }
    public DateTime CreatedDate
    {
        get; set;
    }
    public string CreatedBy
    {
        get; set;
    }
    public DateTime UpdatedDate
    {
        get; set;
    }
    public string UpdatedBy
    {
        get; set;
    }
}
