using System;
using Xunit;
using Moq;

namespace Naive.Consul.Configuration.Tests
{
    public class ConsulKeyValueConfigurationProviderTests
    {
        [Fact]
        public void Should_throw_for_null_IConsulKeyValueClient()
        {
            //Given
            IConsulKeyValueClient client = null;

            // When
            var shouldThrow = new Action(() => new ConsulKeyValueConfigurationProvider(client));

            //Then
            Assert.Throws<ArgumentNullException>(shouldThrow);
        }

        [Fact]
        public void Should_call_GetKeysAsync_on_load()
        {
            //Given
            var consulClientMock = new Mock<IConsulKeyValueClient>();
            var consulKeyValueConfigurationProvider = new ConsulKeyValueConfigurationProvider(consulClientMock.Object);

            //When
            consulKeyValueConfigurationProvider.Load();

            //Then
            consulClientMock.Verify(mock => mock.GetAllAsync(), Times.Once());
        }

        [Fact]
        public void Should_call_GetKeyAsync_with_matching_Key()
        {
            //given
            var consulClientMock = new Mock<IConsulKeyValueClient>();
            var consulKeyValueConfigurationProvider = new ConsulKeyValueConfigurationProvider(consulClientMock.Object);
            consulClientMock.Setup(c => c.GetKeysAsync()).ReturnsAsync(new[] { "Test_Key_1", "Test_Key_2", "Test_Key_3" });

            //When
            consulKeyValueConfigurationProvider.Load();

            //Then
            consulClientMock.Verify(mock => mock.GetValueAsync("Test_Key_1"), Times.Once());
            consulClientMock.Verify(mock => mock.GetValueAsync("Test_Key_2"), Times.Once());
            consulClientMock.Verify(mock => mock.GetValueAsync("Test_Key_3"), Times.Once());
        }

        [Fact]
        public void Should_support_folder_KeyName()
        {
            //given
            var consulClientMock = new Mock<IConsulKeyValueClient>();
            var consulKeyValueConfigurationProvider = new ConsulKeyValueConfigurationProvider(consulClientMock.Object);
            consulClientMock.Setup(c => c.GetKeysAsync()).ReturnsAsync(new[] { "SomeFolder/Test_Key_1", "Test_Key_2", "Test_Key_3" });

            //When
            consulKeyValueConfigurationProvider.Load();

            //Then
            consulClientMock.Verify(mock => mock.GetValueAsync("SomeFolder/Test_Key_1"), Times.Once());
            consulClientMock.Verify(mock => mock.GetValueAsync("Test_Key_2"), Times.Once());
            consulClientMock.Verify(mock => mock.GetValueAsync("Test_Key_3"), Times.Once());
        }

        [Fact]
        public void Should_Contains_Return_value_once_loaded()
        {
            //given
            var returnedKey = "SomeFolder/Test_Key_1";
            var expectedValue = "Some_SubValue";
            var consulClientMock = new Mock<IConsulKeyValueClient>();
            var consulKeyValueConfigurationProvider = new ConsulKeyValueConfigurationProvider(consulClientMock.Object);
            consulClientMock.Setup(c => c.GetKeysAsync()).ReturnsAsync(new[] { returnedKey });
            consulClientMock.Setup(c => c.GetValueAsync(It.Is<string>(key => string.Compare(key, "SomeFolder/Test_Key_1") == 0)))
                            .ReturnsAsync(expectedValue);

            //When
            consulKeyValueConfigurationProvider.Load();

            //Then
            consulKeyValueConfigurationProvider.TryGet(returnedKey, out var value);
            Assert.Equal(expectedValue, value);
        }
    }
}
