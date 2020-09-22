using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using UniRx;
using Debug = UnityEngine.Debug;

public class DirtyMarkNode
{
    public string id;
    public IReadOnlyReactiveProperty<bool> Dirty => _dirty;
    public event Action ChildNodeUpdated;
    public bool IsLeafNode => !_children.Any();
    public IEnumerable<DirtyMarkNode> Children => _children;
    
    private ReactiveProperty<bool> _dirty = new ReactiveProperty<bool>();
    private List<DirtyMarkNode> _children = new List<DirtyMarkNode>();
        
    private IReadOnlyReactiveProperty<bool> _childrenDirtyCombined;
    private IDisposable _markObserver;

    public DirtyMarkNode(string id)
    {
        this.id = id;
    }

    public DirtyMarkNode SearchChild(string childId)
    {
        return SearchChild(childId.Split('.').ToList());
    }

    private DirtyMarkNode SearchChild(List<string> splittedId)
    {
        if (splittedId.Count == 0) return this;
        
        var target = _children.FirstOrDefault(node => node.id.Equals(splittedId[0]));
        if (target == null)
        {
            target = new DirtyMarkNode(splittedId[0]);
            AddChild(target);
        }
        
        if (splittedId.Count == 1) return target;
        else return target.SearchChild(new List<string>(splittedId.Skip(1)));
    }

    private void AddChild(DirtyMarkNode child)
    {
        _children.Add(child);

        child.Dirty
            .Skip(1)
            .Subscribe(_ => ChildNodeUpdated?.Invoke());
        
        if (_childrenDirtyCombined == null)
        {
            _childrenDirtyCombined = child._dirty;
        }
        else
        {
            _childrenDirtyCombined =
                Observable.CombineLatest(_childrenDirtyCombined, child._dirty, (a, b) => a || b)
                    .ToReadOnlyReactiveProperty(_childrenDirtyCombined.Value || child._dirty.Value);
        }
        
        _markObserver?.Dispose();
        _markObserver = _childrenDirtyCombined.Subscribe(c => _dirty.Value = c);
    }

    public void MarkDirty()
    {
        if (_children.Count == 0)
        {
            _dirty.Value = true;   
        }
        else
        {
            throw new TreeNodeMarkException("Dirty Mark Node :: Only leaf node can be marked dirty actively!");
        }
    }

    public void Clean()
    {
        if (_children.Count == 0)
        {
            _dirty.Value = false;
        }
        else
        {
            foreach (var child in _children)
            {
                child.Clean();
            }
        }
    }
    
    public class TreeNodeMarkException: Exception
    {
        public TreeNodeMarkException(string msg) : base(msg){}
    }
}