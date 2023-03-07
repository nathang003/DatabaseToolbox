

using System.Collections;

namespace ToolboxWebLibrary.Models;

public class IndexFieldModels : IEnumerable
{
    private IndexFieldModel[] _indexFields;
    public IndexFieldModels(IndexFieldModel[] indexFieldArray)
    {
        _indexFields = new IndexFieldModel[indexFieldArray.Length];

        for (int i = 0; i < indexFieldArray.Length; i++)
        {
            _indexFields[i] = indexFieldArray[i];
        }
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return (IEnumerator) GetEnumerator();
    }

    public IndexFieldEnum GetEnumerator()
    {
        return new IndexFieldEnum(_indexFields);
    }
}
