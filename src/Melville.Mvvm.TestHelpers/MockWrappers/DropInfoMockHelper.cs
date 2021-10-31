using System.Windows;
using Melville.MVVM.Wpf.MouseDragging.Drop;
using Moq;

namespace Melville.Mvvm.TestHelpers.MockWrappers;

public static class DropInfoMockHelper
{
    public static Mock<T> SetDropData<T, TItem>(this Mock<T> mock, TItem item)
        where T : class, IDropInfo
    {
        var dataObj = new Mock<IDataObject>();
        dataObj.Setup(i => i.GetData(typeof(TItem))).Returns(item);
        dataObj.Setup(i => i.GetDataPresent(typeof(T))).Returns(true);
        mock.SetupGet(i => i.Item).Returns(dataObj.Object);
        return mock;
    }
}