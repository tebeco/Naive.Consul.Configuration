using System;
using Xunit;
using Moq;
using System.Text;

namespace Microsoft.Extensions.Configuration.ConsulKeyValue.Test
{
    public class ConsulKeyValueConfigurationProviderTests
    {
        [Fact]
        public void Should_throw_for_null_IConsulKeyValueClient()
        {
            //given
            IConsulKeyValueClient client = null;

            // When
            var shouldThrow = new Action(() => new ConsulKeyValueConfigurationProvider(client));

            Assert.Throws<ArgumentNullException>(shouldThrow);
        }

        [Fact]
        public void Should_call_GetKeyValuesAsync_on_load()
        {
            //given
            var consulClientMock = new Mock<IConsulKeyValueClient>();
            var consulKeyValueConfigurationProvider = new ConsulKeyValueConfigurationProvider(consulClientMock.Object);

            //When
            consulKeyValueConfigurationProvider.Load();

            //Then
            consulClientMock.Verify(mock => mock.GetAllAsync(), Times.Once());
        }

        [Fact]
        public void Should_Contains_Return_value_once_loaded()
        {
            //given
            ConsulKeyValuePair[] expectedKeyValues = new[]
            {
                new ConsulKeyValuePair()
                {
                    LockIndex= 0,
                    Key= "Key_0",
                    Flags= 0,
                    Value= "VmFsdWVfMA==",
                    CreateIndex= 27,
                    ModifyIndex= 27
                },
                new ConsulKeyValuePair()
                {
                    LockIndex= 0,
                    Key= "Key_1",
                    Flags= 0,
                    Value= "VmFsdWVfMQ==",
                    CreateIndex= 19,
                    ModifyIndex= 19
                }
            };

            var expectValues = new []
            {
                Encoding.UTF8.GetString(Convert.FromBase64String(expectedKeyValues[0].Value)),
                Encoding.UTF8.GetString(Convert.FromBase64String(expectedKeyValues[1].Value)),
            };
            
            var consulClientMock = new Mock<IConsulKeyValueClient>();
            var consulKeyValueConfigurationProvider = new ConsulKeyValueConfigurationProvider(consulClientMock.Object);
            consulClientMock.Setup(c => c.GetAllAsync()).ReturnsAsync(expectedKeyValues);

            //When
            consulKeyValueConfigurationProvider.Load();

            //Then
            consulKeyValueConfigurationProvider.TryGet(expectedKeyValues[0].Key, out var valueOne);
            Assert.Equal(expectValues[0], valueOne);

            consulKeyValueConfigurationProvider.TryGet(expectedKeyValues[1].Key, out var valueTwo);
            Assert.Equal(expectValues[1], valueTwo);
        }
    }
}
