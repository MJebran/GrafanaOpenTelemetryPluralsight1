using FluentAssertions;
using System.Net.Http.Json;
using TicketClassLib.Data;

namespace TestsRUS;

public class EventTests : IClassFixture<TicketsApiFactory>
{
    private HttpClient _httpClient;
    public EventTests(TicketsApiFactory factory)
    {
        _httpClient = factory.CreateDefaultClient();
    }

    [Fact]
    public async Task GetAllEventsTest()
    {
        //Arrange

        //Act
        var response = await _httpClient.GetFromJsonAsync<List<Event>>("Event/getAll");

        //Assert
        response.Count().Should().Be(1); //2
    }
}