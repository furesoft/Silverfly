using System.Collections;

namespace Silverfly.Sample.Func.Values;

public abstract record Value
{
    public List<Annotation> Annotations = [];
    public Scope Members = new();

    protected virtual Value GetByIndex(int index)
    {
        return UnitValue.Shared;
    }

    protected virtual Value GetByRange(RangeValue range)
    {
        return UnitValue.Shared;
    }

    public Value Get(Value key)
    {
        if (key is NameValue name)
        {
            return Members.Get(name.Name);
        }

        if (key is StringValue s)
        {
            return Members.Get(s.Value);
        }

        if (key is NumberValue index)
        {
            return GetByIndex((int)index.Value);
        }

        if (key is RangeValue range)
        {
            return GetByRange(range);
        }

        return UnitValue.Shared;
    }

    public abstract bool IsTruthy();

    public virtual void Set(Value key, Value value)
    {
        throw new NotImplementedException();
    }

    public static implicit operator Value(string str)
    {
        return new StringValue(str);
    }

    public static implicit operator Value(char c)
    {
        return new StringValue(c.ToString());
    }

    public static implicit operator Value(int c)
    {
        return new NumberValue(c);
    }

    public static implicit operator Value(double c)
    {
        return new NumberValue(c);
    }

    public static implicit operator Value(bool c)
    {
        return new BoolValue(c);
    }

    public static implicit operator Value(List<Value> c)
    {
        return new ListValue(c);
    }

    public static implicit operator Value(List<object> c)
    {
        return new ListValue(c.Select(v => (Value)v).ToList());
    }

    public object Unmarshal()
    {
        return this switch
        {
            StringValue s => s.Value,
            NumberValue n => n.Value,
            BoolValue b => b.Value,
            _ => null
        };
    }

    public static Value MarshalPrimitive(object obj)
    {
        return obj switch
        {
            string str => new StringValue(str),
            char c => new StringValue(c.ToString()),
            int i => new NumberValue(i),
            double d => new NumberValue(d),
            bool b => new BoolValue(b),
            null => UnitValue.Shared,
            _ => null
        };
    }

    public static Value From(object o)
    {
        var marshalled = MarshalPrimitive(o);
        if (marshalled != null)
        {
            return marshalled;
        }

        if (o is Delegate d)
        {
            return MarshalDelegate(d);
        }

        if (o is IEnumerable e)
        {
            return MarshalList(e);
        }

        return MarshalObject(o);
    }

    private static Value MarshalObject(object o)
    {
        var scope = new Scope();
        var type = o.GetType();

        foreach (var prop in type.GetProperties())
        {
            var value = prop.GetValue(o);

            scope.Define(prop.Name.ToLower(), From(value));
        }

        return new ModuleValue(scope);
    }

    private static Value MarshalList(IEnumerable e)
    {
        var values = new List<Value>();
        foreach (var item in e)
        {
            values.Add(From(item));
        }

        return new ListValue(values);
    }

    private static Value MarshalDelegate(Delegate d)
    {
        return new LambdaValue(args =>
        {
            var parameters = d.Method.GetParameters();
            var convertedArgs = new object[parameters.Length];

            for (var i = 0; i < parameters.Length; i++)
            {
                if (parameters[i].ParameterType.IsAssignableFrom(typeof(Value)))
                {
                    convertedArgs[i] = args[i];
                    continue;
                }

                if (i < args.Length)
                {
                    convertedArgs[i] = args[i].Unmarshal() ?? args[i];
                }
                else if (parameters[i].HasDefaultValue)
                {
                    convertedArgs[i] = parameters[i].DefaultValue;
                }
                else
                {
                    throw new ArgumentException($"Missing argument for parameter '{parameters[i].Name}'");
                }
            }

            var result = d.DynamicInvoke(convertedArgs);

            return From(result);
        }, null);
    }
}
