using  System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using Melville.MVVM.AdvancedLists;
using Melville.MVVM.BusinessObjects;

namespace Melville.MVVM.Undo
{
  public interface IUndoEngine
  {
    void PushAndDoAction(Action doAction, Action undoAction);
    void CompositeUndo(Action act);
    void Undo();
    void Redo();
  }

  public static class UndoEngineOperations
  {
    public static void PushToggleAction(this IUndoEngine engine, Action doUndoAction)
    {
      engine.PushAndDoAction(doUndoAction, doUndoAction);
    }

    public static void PushAndDoAction(this IUndoEngine engine, Action doAction, Action undoAction, bool recordUndo)
    {
      if (recordUndo)
      {
        engine.PushAndDoAction(doAction, undoAction);
      }
      else
      {
        doAction();
      }
    }
  }

  public sealed class UndoEngine : NotifyBase, IUndoEngine
  {
    #region Push and Pop
    private readonly FixedStack<IUndoItem> undoItems = new FixedStack<IUndoItem>(200);
    private readonly FixedStack<IUndoItem> redoItems = new FixedStack<IUndoItem>(200);
    public bool CanUndo => undoItems.HasItems;
    public bool CanRedo => redoItems.HasItems;

    public UndoEngine()
    {
      DelegatePropertyChangeFrom(undoItems, "HasItems", "CanUndo");
      DelegatePropertyChangeFrom(redoItems, "HasItems", "CanRedo");
    }

    public Action<UndoEngine> DoActionWithDelayedPush(Action doAction, Action undoAction)
    {
      PushAndDoAction(doAction, undoAction);
      var undoItem = undoItems.Pop();
      return (UndoEngine i) => i.undoItems.Push(undoItem);
    }

    public void PushAndDoAction(Action doAction, Action undoAction)
    {
      if (NoUndoOrRedoPending())
      {
        redoItems.Clear();
        undoItems.Push(new UndoItem(doAction, undoAction));
      }

      doAction();
    }

    private bool NoUndoOrRedoPending() => insideUndoAction == 0;

    private int insideUndoAction = 0;

    private void DoStackAction(Action inner)
    {
      try
      {
        insideUndoAction++;
        inner();
      }
      finally
      {
        insideUndoAction--;
      }
    }

    public void Undo()
    {
      if (!CanUndo) return;
      var value = undoItems.Pop();
      redoItems.Push(value);
      DoStackAction(value.Undo);
    }

    public void Redo()
    {
      if (!CanRedo) return;
      var value = redoItems.Pop();
      undoItems.Push(value);
      DoStackAction(value.Redo);
    }
    #endregion

    #region UndoItem
    private interface IUndoItem
    {
      void Undo();
      void Redo();
    }

    private class UndoItem : IUndoItem
    {
      private readonly Action undoAction;
      private readonly Action redoAction;

      public void Undo()
      {
        undoAction();
      }

      public void Redo()
      {
        redoAction();
      }

      public UndoItem(Action redoAction, Action undoAction)
      {
        this.redoAction = redoAction;
        this.undoAction = undoAction;
      }
    }
    #endregion

    #region CompositeUndo
    public void CompositeUndo(Action act)
    {
      redoItems.Clear();
      var compositeItem = new CompositeToken();
      undoItems.Push(compositeItem);
      act();
      if (!undoItems.Contains(compositeItem)) return;
      var items = PopUntil(undoItems, compositeItem).ToList();
      undoItems.Pop(); // take the compositeItem Placeholder of the stack
      new CompositeUndoItem(items).PushOntoStack(undoItems);
    }

    private IEnumerable<IUndoItem> PopUntil(FixedStack<IUndoItem> stack, IUndoItem sentinel)
    {
      while (stack.HasItems && stack.Peek() != sentinel)
      {
        yield return stack.Pop();
      }
    }
    
     private sealed class CompositeToken:IUndoItem
     {
       // this is just a placeholder -- it should never actually be undone
       public void Undo() => throw new NotSupportedException();
       public void Redo() => throw new NotSupportedException();
     }

    private sealed class CompositeUndoItem : IUndoItem
    {
      private IList<IUndoItem> subItems;

      public CompositeUndoItem(IList<IUndoItem> items)
      {
        subItems = items;
      }

      public void Undo()
      {
        foreach (var undoItem in subItems)
        {
          undoItem.Undo();
        }
      }

      public void Redo()
      {
        foreach (var undoItem in subItems.Reverse())
        {
          undoItem.Redo();
        }
      }

      public void PushOntoStack(FixedStack<IUndoItem> fixedStack)
      {
        switch (subItems.Count)
        {
          case 0:
            return;
          case 1:
            fixedStack.Push(subItems[0]);
            return;
          default:
            fixedStack.Push(this);
            return;
        }
      }
    }
    #endregion
  }
}
