#nullable disable warnings
using Melville.MVVM.Undo;
using Xunit;

namespace Melville.Mvvm.Test.Undo;

public sealed class UndoEngineTest
{
  [Fact]
  public void PushPopSingle()
  {
    var val = 1;
    var ue = new UndoEngine();
    Assert.False(ue.CanUndo);
    ue.PushAndDoAction(() => val = 2, () => val = 1);
    Assert.True(ue.CanUndo);
    Assert.Equal(2, val);
    ue.Redo();  // should do nothing but not crash
    Assert.True(ue.CanUndo);
    Assert.Equal(2, val);
    ue.Undo();
    Assert.False(ue.CanUndo);
    Assert.True(ue.CanRedo);
    Assert.Equal(1, val);
    ue.Undo(); // should do nothing but should not crash.
    Assert.False(ue.CanUndo);
    Assert.True(ue.CanRedo);
    Assert.Equal(1, val);
    ue.Redo();
    Assert.True(ue.CanUndo);
    Assert.False(ue.CanRedo);
    Assert.Equal(2, val);
  }

  [Fact]
  public void DoCompositeUndo()
  {
    var v1 = 0;
    var v2 = 0;
    var ue = new UndoEngine();
    ue.CompositeUndo(() =>
    {
      ue.PushAndDoAction(() => v1 = 1, () => v1 = 2);
      ue.PushAndDoAction(() => v2 = 10, () => v2 = 20);
    });

    Assert.Equal(1, v1);
    Assert.Equal(10, v2);

    ue.Undo();
    Assert.Equal(2, v1);
    Assert.Equal(20, v2);

    ue.Redo();
    Assert.Equal(1, v1);
    Assert.Equal(10, v2);
  }

  [Fact]
  public void PushClearsRedoStack()
  {
    var val = 1;
    var ue = new UndoEngine();
    ue.PushAndDoAction(() => val = 10, () => val = 1);
    ue.Undo();
    Assert.Equal(1, val);
    Assert.True(ue.CanRedo);
    ue.PushAndDoAction(() => val = 100, () => val = 1);
    Assert.False(ue.CanRedo);
  }
}