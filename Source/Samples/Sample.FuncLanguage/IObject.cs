namespace Sample.FuncLanguage;

public interface IObject
{
    void Set(Value key, Value value);
    Value Get(Value key);
}