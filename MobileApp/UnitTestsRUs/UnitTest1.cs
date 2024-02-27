using FluentAssertions;
using MobileApp.Components.Pages;
using NSubstitute;
using System.Runtime.CompilerServices;
using TicketClassLib.Services;

namespace UnitTestsRUs
{
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public async Task ChangingDatabaseTests()
        {
            //Arrange
            var prefMock = Substitute.For<IPreferenceSaver>();
            prefMock.ApiAddress.Returns("TestApi");
            prefMock.DeleteTables().Returns(Task.FromResult);
            Settings settings = new Settings(prefMock);
            settings.apiAddress = "NotTestApi";
            //Act
            await settings.SaveChanges();

            //Assert
            await prefMock.Received().DeleteTables();
        }

        [Test]
        public async Task NotChangingDatabaseTests()
        {
            //Arrange
            var prefMock = Substitute.For<IPreferenceSaver>();
            prefMock.ApiAddress.Returns("");
            prefMock.DeleteTables().Returns(Task.FromResult);
            Settings settings = new Settings(prefMock);

            //Act
            await settings.SaveChanges();

            //Assert
            await prefMock.DidNotReceive().DeleteTables();
        }
    }
}