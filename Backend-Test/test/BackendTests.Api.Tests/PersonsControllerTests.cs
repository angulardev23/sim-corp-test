using System.Net;
using System.Net.Http.Json;
using System.Threading.Tasks;
using BackendTest.Api.Contracts;
using BackendTest.Application.Models;
using Xunit;

namespace BackendTests.Api.Tests;

public sealed class PersonsControllerTests
{
    [Fact]
    public async Task GetAll_ReturnsSeededPeople()
    {
        using var factory = ApiTestHost.CreateFactory();
        using var client = factory.CreateTestClient();

        var people = await client.GetFromJsonAsync<PersonData[]>("/persons/getAll");

        Assert.NotNull(people);
        Assert.Equal(10, people.Length);
        Assert.Contains(people, person => person.Id == 1 && person.Firstname == "John");
    }

    [Fact]
    public async Task GetById_ReturnsRequestedPerson()
    {
        using var factory = ApiTestHost.CreateFactory();
        using var client = factory.CreateTestClient();

        var person = await client.GetFromJsonAsync<PersonData>("/persons/get/1");

        Assert.NotNull(person);
        Assert.Equal(new PersonData(1, "John", "Doe", 1980), person);
    }

    [Fact]
    public async Task Add_CreatesPersonAndReturnsLocation()
    {
        using var factory = ApiTestHost.CreateFactory();
        using var client = factory.CreateTestClient();
        var request = new PersonContract
        {
            Id = 101,
            Firstname = "Grace",
            Lastname = "Hopper",
            YearOfBirth = 1906
        };

        var response = await client.PostAsJsonAsync("/persons/add", request);
        var created = await response.Content.ReadFromJsonAsync<PersonData>();

        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        Assert.Equal("https://localhost/persons/get/101", response.Headers.Location?.ToString());
        Assert.Equal(new PersonData(101, "Grace", "Hopper", 1906), created);
    }

    [Fact]
    public async Task Update_ReplacesPerson()
    {
        using var factory = ApiTestHost.CreateFactory();
        using var client = factory.CreateTestClient();
        var request = new PersonContract
        {
            Id = 1,
            Firstname = "Jonathan",
            Lastname = "Doe",
            YearOfBirth = 1981
        };

        var response = await client.PostAsJsonAsync("/persons/update/1", request);
        var updated = await response.Content.ReadFromJsonAsync<PersonData>();

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.Equal(new PersonData(1, "Jonathan", "Doe", 1981), updated);
    }

    [Fact]
    public async Task Delete_RemovesPerson()
    {
        using var factory = ApiTestHost.CreateFactory();
        using var client = factory.CreateTestClient();
        await client.PostAsJsonAsync("/persons/add", new PersonContract
        {
            Id = 101,
            Firstname = "Delete",
            Lastname = "Me",
            YearOfBirth = 1990
        });

        var response = await client.DeleteAsync("/persons/delete/101");
        var getResponse = await client.GetAsync("/persons/get/101");

        Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
        Assert.Equal(HttpStatusCode.NotFound, getResponse.StatusCode);
    }
}
