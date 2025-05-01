using AutoMapper;
using Moq;
using SiaAdmin.Application.Features.Queries.NotificationHistory.CheckNotificationCooldown;
using SiaAdmin.Application.Repositories.NotificationHistory;
using SiaAdmin.Domain.Entities.Procedure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiaAdmin.Application.Test.Notification
{
    public class CheckNotificationCooldownHandlerTests
    {
        private readonly Mock<INotificationHistoryReadRepository> _mockRepository;
        private readonly Mock<IMapper> _mockMapper;
        private readonly CheckNotificationCooldownHandler _handler;

        public CheckNotificationCooldownHandlerTests()
        { 
            _mockRepository = new Mock<INotificationHistoryReadRepository>();
            _mockMapper = new Mock<IMapper>();
             
            _handler = new CheckNotificationCooldownHandler(
                _mockRepository.Object,
                _mockMapper.Object
            );
        }

        [Fact]
        public async Task Handle_ShouldReturnMappedResponse()
        { 
            int surveyId = 123;
            var request = new CheckNotificationCooldownRequest { SurveyId = surveyId }; 
            var repositoryResult = new NotificationCooldownResult
            { 
                CanSendNotification = true,
                Message = "Daha önce bildirim gönderilmemiş, gönderebilirsiniz. ",
                LastNotificationTime = null
            }; 
            var expectedResponse = new CheckNotificationCooldownResponse
            {
                CanSendNotification = true,
                Message = "Daha önce bildirim gönderilmemiş, gönderebilirsiniz. ",
                LastNotificationTime = null
            };
             
            _mockRepository.Setup(repo =>
                repo.CheckNotificationCooldownAsync(surveyId,6))
                .ReturnsAsync(repositoryResult);
             
            _mockMapper.Setup(mapper =>
                mapper.Map<CheckNotificationCooldownResponse>(repositoryResult))
                .Returns(expectedResponse);
             
            var result = await _handler.Handle(request, CancellationToken.None);
             
            Assert.Equal(expectedResponse.LastNotificationTime, result.LastNotificationTime);
            Assert.Equal(expectedResponse.CanSendNotification, result.CanSendNotification);
            Assert.Equal(expectedResponse.Message,result.Message); 

            _mockRepository.Verify(repo =>
                repo.CheckNotificationCooldownAsync(surveyId,6), Times.Once);
             
            _mockMapper.Verify(mapper =>
                mapper.Map<CheckNotificationCooldownResponse>(repositoryResult), Times.Once);
        }

        [Fact]
        public async Task Handle_WithNullRepositoryResult_ShouldHandleGracefully()
        {
            // Arrange
            int surveyId = 5002;
            var request = new CheckNotificationCooldownRequest { SurveyId = surveyId };

            // Repository'den null dönmesi durumu
            _mockRepository.Setup(repo =>
                    repo.CheckNotificationCooldownAsync(surveyId,6))
                .ReturnsAsync((NotificationCooldownResult)null);

            // Null için mapping ayarı
            _mockMapper.Setup(mapper =>
                    mapper.Map<CheckNotificationCooldownResponse>(null))
                .Returns(new CheckNotificationCooldownResponse
                {
                    CanSendNotification = true,
                    Message = "Daha önce bildirim gönderilmemiş, gönderebilirsiniz."
                });

            // Act
            var result = await _handler.Handle(request, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.True(result.CanSendNotification);
            Assert.Equal(null, result.LastNotificationTime);
        }
    }
}
