using System;
using System.Collections.Generic;
using Xunit;

namespace BlazorServerApp.xUnitTests
{
    public class IntStack : Stack<int>//, IDisposable
    {
        //public void Dispose() => Dispose();
    }

    public interface IStackFixture<T> /*: IDisposable */where T : IntStack
    {
        public int Count();
        public void Push(int i);
        public void Pop();
    }

    public class StackFixture<T> : IStackFixture<T> where T : IntStack, new()
    {
        public T Stack { get; set; }

        public StackFixture()
        {
            Stack = new T();
        }
        public int Count() => Stack.Count;

        public void Push(int i) => Stack.Push(i);
        public void Pop() => Stack.Pop();

        //public void Dispose()
        //{
        //    Stack.Dispose();
        //}
    }

    public class IntStackInstantiationTest : IClassFixture<StackFixture<IntStack>>
    {
        StackFixture<IntStack> _fixture;
        public IntStackInstantiationTest(StackFixture<IntStack> fixture)
        {
            _fixture = fixture;
        }

        [Fact]
        public void AfterPushOneItem_CountShouldReturnOne() {
            _fixture.Push(1);
            Assert.Equal(1,_fixture.Count());
        }
    }

    [CollectionDefinition("Shared stack instance collection")]
    public class SharedIntStackInstanceCollection : ICollectionFixture<StackFixture<IntStack>>
    {
        StackFixture<IntStack> _fixture;

        public SharedIntStackInstanceCollection(StackFixture<IntStack> fixture)
        {
            this._fixture = fixture;
            _fixture.Pop();
        }
    }

    [Collection("Shared stack instance collection")]
    public class SharedIntStackInstanceTest1
    {
        StackFixture<IntStack> _fixture;

        public SharedIntStackInstanceTest1(StackFixture<IntStack> fixture) {
            this._fixture = fixture;
        }

        [Fact]
        public void WithConstructor_CountShouldReturnTwo() {
            Assert.Equal(0, _fixture.Count());
        }
    }

    [Collection("Shared stack instance collection")]
    public class SharedIntStackInstanceTest2
    {
        StackFixture<IntStack> _fixture;
        public SharedIntStackInstanceTest2(StackFixture<IntStack> fixture) {
            this._fixture = fixture;
        }

        [Fact]
        public void WithConstructor_CountShouldReturnTwo() {
            Assert.Equal(0, _fixture.Count());
        }
    }
}
