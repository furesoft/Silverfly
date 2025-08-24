using System.Collections.Generic;
using System.Linq;
using Silverfly.Backend.Scoping.Items;
using Silverfly.Nodes;

namespace Silverfly.Backend.Scoping;

public class Scope
{
    private readonly List<ScopeItem> _items = [];

    public Scope(Scope? parent)
    {
        Parent = parent;

        if (parent == null)
        {
            Root = this;
        }
        else
        {
            Root = parent.Root;
        }
    }

    public Scope? Parent { get; set; }
    public Scope Root { get; set; }

    public bool Add(ScopeItem item)
    {
        if (_items.FirstOrDefault(_ => _.Name == item.Name) is FunctionScopeItem fsi
            && item is FunctionScopeItem isI)
        {
            fsi.Overloads.AddRange(isI.Overloads);
            return true;
        }

        if (Contains(item.Name))
        {
            return false;
        }

        _items.Add(item);
        return true;
    }

    public bool Contains(string name)
    {
        return _items.Any(_ => _.Name == name);
    }

    public Scope CreateChildScope()
    {
        return new Scope(this);
    }

    public IEnumerable<string> GetAllScopeNames()
    {
        var scope = this;
        while (scope != null)
        {
            foreach (var item in scope._items)
            {
                yield return item.Name;
            }

            scope = scope.Parent;
        }
    }

    public bool TryGet<T>(string name, out T? item)
        where T : ScopeItem
    {
        item = (T)_items.FirstOrDefault(i => i is T && i.Name == name);

        if (item == null && Parent != null)
        {
            if (!Parent.TryGet(name, out item))
            {
                item = null;
            }
        }

        return item != null;
    }

    public ScopeItem? GetFromNode(AstNode node)
    {
        if (node is NameNode id)
        {
            if (TryGet<ScopeItem>(id.Token.Text.ToString(), out var item))
            {
                return item!;
            }

            node.AddError(id.Token.Text + " not found");
        }

        return null;
    }
}
