using System.Diagnostics;

namespace Silverfly;

/// <summary>This class produces global symbols.</summary>
/// <remarks>
///     Call GSymbol.Get() to create a Symbol from a string, or GSymbol.GetIfExists()
///     to find a Symbol that has already been created.
/// </remarks>
public class GSymbol
{
    static GSymbol()
    {
        Pool = new(0, 0);
        Empty = Pool.Get("");

        Debug.Assert(Empty.Id == 0 && Empty.Name == "");
        Debug.Assert(Empty.Pool == Pool);
    }

    #region Public static members

    public static Symbol Get(string name) { return Pool.Get(name); }
    public static Symbol GetIfExists(string name) { return Pool.GetIfExists(name); }
    public static Symbol GetById(int id) { return Pool.GetById(id); }

    public static readonly Symbol Empty;
    public static readonly SymbolPool Pool;

    #endregion
}
