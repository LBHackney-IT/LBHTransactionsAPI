using System;
using System.Threading.Tasks;
using Xunit;
using LBHTenancyAPI.Controllers;
using AgreementService;
using Microsoft.AspNetCore.Mvc;
using Moq;
using LBHTenancyAPI.UseCases.ArrearsActions;

namespace LBHTenancyAPITest.Test.Controllers
{
    public class ArrearsActionDairyControllerTests
    {
        public ArrearsActionDairyControllerTests()
        {

        }

        [Fact]
        public async Task WhenProvidedWithCorrectParamaters_ApiResponseWith200()
        {
            //Arrange
            var fakeUseCase = new Mock<ICreateArrearsActionDiaryUseCase>();
            var classUnderTest = new ArrearsActionDiaryController(fakeUseCase.Object);
            fakeUseCase.Setup(a => a.CreateActionDiaryRecordsAsync(It.IsAny<ArrearsActionCreateRequest>())).ReturnsAsync(new ArrearsActionResponse
            {
                Success = true
            });


            //Act
            ArrearsActionCreateRequest request = new ArrearsActionCreateRequest()
            {

            };

             var response = await classUnderTest.Post(request);

            //Assert
            Assert.IsType<OkObjectResult>(response);
        }

        [Fact]
        public async Task WhenProvidedWithCorrectParamaters_AndThereIsErrorFromWebService_ApiResponseWith500()
        {
            //Arrange
            var fakeUseCase = new Mock<ICreateArrearsActionDiaryUseCase>();
            var classUnderTest = new ArrearsActionDiaryController(fakeUseCase.Object);
            fakeUseCase.Setup(a => a.CreateActionDiaryRecordsAsync(It.IsAny<ArrearsActionCreateRequest>())).ReturnsAsync(new ArrearsActionResponse
            {
                Success = false
            });

            //Act
            ArrearsActionCreateRequest request = new ArrearsActionCreateRequest()
            {

            };

            var response = await classUnderTest.Post(request);

            //Assert
            Assert.IsType<ObjectResult>(response);
        }
    }
}
