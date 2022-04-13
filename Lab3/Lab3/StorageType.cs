using System;
using System.Data.SqlTypes;
using System.Text;
using Microsoft.SqlServer.Server;


[Serializable]
[Microsoft.SqlServer.Server.SqlUserDefinedType(Format.Native, IsByteOrdered = true, ValidationMethodName = "ValidatePlace")]
public struct StorageType: INullable
{
    private bool is_Null;
    private int _rack;
    private int _shelf;

    public override string ToString()
    {
        if(this.IsNull)
        {
            return "NULL";
        }
        else
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(_rack);
            sb.Append(":");
            sb.Append(_shelf);
            return sb.ToString();
        }
    }
    
    public bool IsNull
    {
        get
        {
            // Введите здесь код
            return (is_Null);
        }
    }
    
    public static StorageType Null
    {
        get
        {
            StorageType h = new StorageType();
            h.is_Null = true;
            return h;
        }
    }

    [SqlMethod(OnNullCall = false)]
    public static StorageType Parse(SqlString s)
    {
        if (s.IsNull)
            return Null;
        StorageType u = new StorageType();
        string[] sr = s.Value.Split(",".ToCharArray());
        u._rack = Int32.Parse(sr[0]);
        u._shelf = Int32.Parse(sr[1]);
        return u;
    }
    
    public Int32 Rack
    {
        get => this._rack;
        set
        {
            Int32 temp = _rack;
            _rack = value;
            if(!ValidatePlace())
            {
                _rack = temp;
                throw new ArgumentException("Invalid rack");
            }
        }
    }

    public Int32 Shelf
    {
        get => this._shelf;
        set
        {
            Int32 temp = _shelf;
            _rack = value;
            if (!ValidatePlace())
            {
                _shelf = temp;
                throw new ArgumentException("Invalid shelf");
            }
        }
    }

    private bool ValidatePlace()
    {
        if((_rack >= 0) && (_shelf >= 0))
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}