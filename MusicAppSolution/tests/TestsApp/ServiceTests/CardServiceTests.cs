using Moq;
using N_Tier.Application.Helper;
using N_Tier.Application.Services.Impl;
using N_Tier.Core.DTOs.CardDtos;
using N_Tier.Core.Entities;
using N_Tier.DataAccess.Repositories;
using System.Linq.Expressions;

namespace N_Tier.Tests.ServicesTests;

public class CardServiceTests
{
    private readonly Mock<ICardsRepository> _cardsRepositoryMock;
    private readonly Mock<ICardTypeRepository> _cardTypeRepositoryMock;
    private readonly CardService _cardService;

    public CardServiceTests()
    {
        _cardsRepositoryMock = new Mock<ICardsRepository>();
        _cardTypeRepositoryMock = new Mock<ICardTypeRepository>();

        _cardService = new CardService(_cardsRepositoryMock.Object, _cardTypeRepositoryMock.Object);
    }

    [Fact]
    public async Task AddCardAsync_Should_Add_Card_Successfully()
    {
        // Arrange
        var cardDto = new CardDto
        {
            UserId = Guid.NewGuid(),
            CardNumber = 8600123412341234,
            Expire_Date = DateTime.UtcNow.AddYears(2)
        };

        var cardEntity = new Cards
        {
            UserId = cardDto.UserId,
            CardNumber = cardDto.CardNumber,
            Expire_Date = cardDto.Expire_Date,
            CardTypeId = Guid.NewGuid(),
            CreatedOn = DateTime.UtcNow
        };

        _cardsRepositoryMock
            .Setup(repo => repo.GetFirstExistCardAsync(It.IsAny<Expression<Func<Cards, bool>>>()))
            .ReturnsAsync((Cards)null);

        _cardTypeRepositoryMock
            .Setup(repo => repo.GetAllAsync(It.IsAny<Expression<Func<CardType, bool>>>()))
            .ReturnsAsync(new List<CardType>
            {
                new CardType { Id = Guid.NewGuid(), Name = PlasticCardTypesConst.UZCARD }
            });

        _cardsRepositoryMock
            .Setup(repo => repo.AddAsync(It.IsAny<Cards>()))
            .ReturnsAsync(cardEntity);

        // Act
        var result = await _cardService.AddCardAsync(cardDto);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(cardDto.CardNumber, result.CardNumber);
        _cardsRepositoryMock.Verify(repo => repo.AddAsync(It.IsAny<Cards>()), Times.Once);
    }

    [Fact]
    public async Task AddCardAsync_Should_Throw_Exception_When_Card_Exists()
    {
        // Arrange
        var cardDto = new CardDto
        {
            UserId = Guid.NewGuid(),
            CardNumber = 8600123412341234,
            Expire_Date = DateTime.UtcNow.AddYears(2)
        };

        var existingCard = new Cards
        {
            UserId = cardDto.UserId,
            CardNumber = cardDto.CardNumber,
            Expire_Date = cardDto.Expire_Date
        };

        _cardsRepositoryMock
            .Setup(repo => repo.GetFirstExistCardAsync(It.IsAny<Expression<Func<Cards, bool>>>()))
            .ReturnsAsync(existingCard);

        // Act & Assert
        await Assert.ThrowsAsync<InvalidOperationException>(() => _cardService.AddCardAsync(cardDto));
    }

    [Fact]
    public async Task DeleteCardAsync_Should_Delete_Card_Successfully()
    {
        // Arrange
        var cardId = Guid.NewGuid();
        var cardEntity = new Cards { Id = cardId, CardNumber = 8600123412341234 };

        _cardsRepositoryMock
            .Setup(repo => repo.GetFirstAsync(It.IsAny<Expression<Func<Cards, bool>>>()))
            .ReturnsAsync(cardEntity);

        _cardsRepositoryMock
            .Setup(repo => repo.DeleteAsync(It.IsAny<Cards>()))
            .ReturnsAsync(cardEntity);

        // Act
        var result = await _cardService.DeleteCardAsync(cardId);

        // Assert
        Assert.True(result);
        _cardsRepositoryMock.Verify(repo => repo.DeleteAsync(It.IsAny<Cards>()), Times.Once);
    }

    [Fact]
    public async Task GetByIdAsync_Should_Return_Correct_Cards()
    {
        var userId = Guid.NewGuid();
        var cards = new List<Cards>
        {
            new Cards { UserId = userId, CardNumber = 8600123412341234, Expire_Date = DateTime.UtcNow.AddYears(2) },
            new Cards { UserId = userId, CardNumber = 9860123412341234, Expire_Date = DateTime.UtcNow.AddYears(3) }
        };

        _cardsRepositoryMock.Setup(repo => repo.GetAllAsync(It.IsAny<Expression<Func<Cards, bool>>>()))
            .ReturnsAsync(cards);

        var result = await _cardService.GetByIdAsync(userId);

        Assert.Equal(2, result.Count);
        Assert.Contains(result, c => c.CardNumber == 8600123412341234);
        Assert.Contains(result, c => c.CardNumber == 9860123412341234);
    }
}