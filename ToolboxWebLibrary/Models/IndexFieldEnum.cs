
using System.Collections;

namespace ToolboxWebLibrary.Models;

public class IndexFieldEnum : IEnumerator
{
    public IndexFieldModel[] _indexFields;
    int position = -1;

    public IndexFieldEnum(IndexFieldModel[] list)
    {
        _indexFields = list;
    }

    public bool MoveNext()
    {
        position++;
        return (position < _indexFields.Length);
    }

    public void Reset()
    {
        position = -1;
    }

    object IEnumerator.Current
    {
        get
        {
            return Current;
        }
    }

    public IndexFieldModel Current
    {
        get
        {
            try
            {
                return _indexFields[position];
            }
            catch (IndexOutOfRangeException)
            {
                throw new InvalidOperationException();
            }
        }
    }
}
