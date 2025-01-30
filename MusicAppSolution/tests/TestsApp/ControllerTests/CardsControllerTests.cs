using Microsoft.AspNetCore.Mvc;
using Moq;
using MusicApp.Controllers;
using N_Tier.Application.Services;
using N_Tier.Core.DTOs.CardDtos;
using N_Tier.Core.Exceptions;

namespace N_Tier.Tests.ControllerTests;

public class CardsControllerTests
{
    private readonly Mock<ICardsService> _cardsServiceMock;
    private readonly CardsController _controller;

    public CardsControllerTests()
    {
        _cardsServiceMock = new Mock<ICardsService>();
        _controller = new CardsController(_cardsServiceMock.Object);
    }

    [Fact]
    public async Task GetAll_ShouldReturnOk_WithListOfCards()
    {
        // Arrange
        var fakeCards = new List<CardDto>
        {
            new CardDto { UserId = Guid.NewGuid(), CardNumber = 8600123412341234 },
            new CardDto { UserId = Guid.NewGuid(), CardNumber = 9860123412341234 }
        };

        _cardsServiceMock.Setup(s => s.GetAllAsync()).ReturnsAsync(fakeCards);

        // Act
        var result = await _controller.GetAll();

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var returnedCards = Assert.IsType<List<CardDto>>(okResult.Value);
        Assert.Equal(2, returnedCards.Count);
    }

    [Fact]
    public async Task GetById_ShouldReturnOk_WhenCardExists()
    {
        // Arrange
        var cardId = Guid.NewGuid();
        var fakeCard = new CardDto { UserId = cardId, CardNumber = 8600123412341234 };

        _cardsServiceMock
            .Setup(s => s.GetByIdAsync(It.Is<Guid>(id => id == cardId)))
            .ReturnsAsync(new List<CardDto> { fakeCard }); 

        // Act
        var result = await _controller.GetById(cardId);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result); 
        var returnedCard = Assert.IsType<CardDto>(okResult.Value);
        Assert.Equal(cardId, returnedCard.UserId);
    }

    [Fact]
    public async Task GetById_ShouldReturnNotFound_WhenCardDoesNotExist()
    {
        // Arrange
        var cardId = Guid.NewGuid();

        _cardsServiceMock.Setup(s => s.GetByIdAsync(cardId)).ThrowsAsync(new ResourceNotFoundException());

        // Act
        var result = await _controller.GetById(cardId);

        // Assert
        Assert.IsType<NotFoundResult>(result.Result);
    }

    [Fact]
    public async Task Create_ShouldReturnCreatedAtAction_WhenCardIsAdded()
    {
        // Arrange
        var cardDto = new CardDto { UserId = Guid.NewGuid(), CardNumber = 8600123412341234 };

        _cardsServiceMock.Setup(s => s.AddCardAsync(It.IsAny<CardDto>()))
                         .ReturnsAsync(cardDto);

        // Act
        var result = await _controller.Create(cardDto);

        // Assert
        var createdResult = Assert.IsType<CreatedAtActionResult>(result.Result);
        var returnedCard = Assert.IsType<CardDto>(createdResult.Value);
        Assert.Equal(cardDto.CardNumber, returnedCard.CardNumber);
    }

    [Fact]
    public async Task Create_ShouldReturnBadRequest_WhenExceptionOccurs()
    {
        // Arrange
        var cardDto = new CardDto { UserId = Guid.NewGuid(), CardNumber = 8600123412341234 };

        _cardsServiceMock.Setup(s => s.AddCardAsync(It.IsAny<CardDto>()))
                         .ThrowsAsync(new Exception("Some error"));

        // Act
        var result = await _controller.Create(cardDto);

        // Assert
        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result.Result);
        Assert.Equal("Some error", badRequestResult.Value);
    }



    [Fact]
    public async Task Delete_ShouldReturnNoContent_WhenCardDeleted()
    {
        // Arrange
        var cardId = Guid.NewGuid();

        _cardsServiceMock.Setup(s => s.DeleteCardAsync(cardId)).ReturnsAsync(true);

        // Act
        var result = await _controller.Delete(cardId);

        // Assert
        Assert.IsType<NoContentResult>(result);
    }

    [Fact]
    public async Task Delete_ShouldReturnNotFound_WhenCardDoesNotExist()
    {
        // Arrange
        var cardId = Guid.NewGuid();

        _cardsServiceMock.Setup(s => s.DeleteCardAsync(cardId)).ReturnsAsync(false);

        // Act
        var result = await _controller.Delete(cardId);

        // Assert
        Assert.IsType<NotFoundResult>(result);
    }
}