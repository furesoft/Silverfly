using System.Collections;

namespace Silverfly.Sample.Func.Values;

public abstract record Value()
{
    public Scope Members = new();
    public List<Annotation> Annotations = [];

    protected virtual Value GetByIndex(int index) => UnitValue.Shared;
    protected virtual Value GetByRange(RangeValue range) => UnitValue.Shared;

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

    public static implicit operator Value(string str) => new StringValue(str);
    public static implicit operator Value(char c) => new StringValue(c.ToString());
    public static implicit operator Value(int c) => new NumberValue(c);
    public static implicit operator Value(double c) => new NumberValue(c);
    public static implicit operator Value(bool c) => new BoolValue(c);
    public static implicit operator Value(List<Value> c) => new ListValue(c);
    public static implicit operator Value(List<object> c) => new ListValue(c.Select(v => (Value)v).ToList());

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

    public static Value Marshal(object obj)
    {
        return obj switch
        {
            string str => new StringValue(str),
            char c => new StringValue(c.ToString()),
            int i => new NumberValue(i),
            double d => new NumberValue(d),
            bool b => new BoolValue(b),
            List<Value> lv => new ListValue(lv),
            List<object> lo => new ListValue(lo.Select(v => Marshal(v)).ToList()),
            null => UnitValue.Shared,
            _ => null
        };
    }

    public static Value From(object o)
    {
        var marshalled = Marshal(o);
        if (marshalled != null)
        {
            return marshalled;
        }

        if (o is IEnumerable e)
        {
            var values = new List<Value>();
            foreach (var item in e)
            {
                values.Add(From(item));
            }

            return new ListValue(values);
        }

        var scope = new Scope();
        var type = o.GetType();
        foreach (var prop in type.GetProperties())
        {
            var value = prop.GetValue(o);

            scope.Define(prop.Name.ToLower(), From(value));
        }

        return new ModuleValue(scope);
    }
    }
