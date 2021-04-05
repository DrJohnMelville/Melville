using System;
using System.Collections.Generic;
using Melville.IOC.IocContainers;
using Melville.IOC.IocContainers.ActivationStrategies.TypeActivation;
using Xunit;

namespace Melville.IOC.Test.TypeResolutionPolicy
{
    public class GenericResolutionPolicyTest
    {
         private readonly IocContainer sut = new IocContainer();

         [Fact]
         public void SourceNotGeneric()
         {
             Assert.Throws<InvalidOperationException>(()=>sut.BindGeneric(typeof(string), typeof(List<>)));
         }
         [Fact]
         public void DestinationNotGeneric()
         {
             Assert.Throws<InvalidOperationException>(()=>sut.BindGeneric(typeof(List<>), typeof(string)));
         }
         [Fact]
         public void DifferentNumberOfArguments()
         {
             Assert.Throws<InvalidOperationException>(()=>sut.BindGeneric(typeof(List<>), typeof(KeyValuePair<,>)));
         }
         
         public interface IGeneric1<T> { }
         public interface IGeneric2<T> { }
         public class Concrete<T>:IGeneric1<T>, IGeneric2<T> {} 
         public class Concrete2<T>:IGeneric1<T>, IGeneric2<T> {} 

         [Fact]
         public void SimpleArg()
         {
             sut.BindGeneric(typeof(IGeneric1<>), typeof(Concrete<>));
             Assert.True(sut.Get<IGeneric1<int>>() is Concrete<int>);
             Assert.True(sut.Get<IGeneric1<string>>() is Concrete<string>);
         }
         [Fact]
         public void ReRegistrationReplacesPrior()
         {
             sut.BindGeneric(typeof(IGeneric1<>), typeof(Concrete2<>));
             sut.BindGeneric(typeof(IGeneric1<>), typeof(Concrete<>));
             Assert.True(sut.Get<IGeneric1<int>>() is Concrete<int>);
             Assert.True(sut.Get<IGeneric1<string>>() is Concrete<string>);
         }
         [Fact]
         public void IfNeededDoesNotReplace()
         {
             sut.BindGeneric(typeof(IGeneric1<>), typeof(Concrete<>));
             sut.BindGenericIfNeeded(typeof(IGeneric1<>), typeof(Concrete2<>));
             Assert.True(sut.Get<IGeneric1<int>>() is Concrete<int>);
             Assert.True(sut.Get<IGeneric1<string>>() is Concrete<string>);
         }
         [Fact]
         public void IfNeededWorksOnItsOwn()
         {
             sut.BindGenericIfNeeded(typeof(IGeneric1<>), typeof(Concrete<>));
             sut.BindGenericIfNeeded(typeof(IGeneric1<>), typeof(Concrete2<>));
             Assert.True(sut.Get<IGeneric1<int>>() is Concrete<int>);
             Assert.True(sut.Get<IGeneric1<string>>() is Concrete<string>);
         }
         
         

         [Fact]
         public void CanDoSingleton()
         {
             sut.BindGeneric(typeof(IGeneric1<>), typeof(Concrete<>));
             sut.BindGeneric(typeof(IGeneric2<>), typeof(Concrete<>),
                 i=>i.AsSingleton());
             Assert.Equal(sut.Get<IGeneric2<DateTime>>(), sut.Get<IGeneric2<DateTime>>());
             Assert.NotEqual(sut.Get<IGeneric1<DateTime>>(), sut.Get<IGeneric1<DateTime>>());
         }

         public class GenericMultiConstructor<T> : IGeneric1<T>, IGeneric2<T>
         {
             public GenericMultiConstructor()
             {
             }
             public GenericMultiConstructor(string s1) // most numerous constructor is not constructable
             {
             }
         }
         [Fact]
         public void PickGenericConstructor()
         {
             sut.BindGeneric(typeof(IGeneric1<>), typeof(GenericMultiConstructor<>));
             sut.BindGeneric(typeof(IGeneric2<>), typeof(GenericMultiConstructor<>), i=>i.WithArgumentTypes());
             Assert.Throws<IocException>(() => sut.Get<IGeneric1<DateTime>>());
             Assert.NotNull(sut.Get<IGeneric2<DateTime>>());
         }
    }
}