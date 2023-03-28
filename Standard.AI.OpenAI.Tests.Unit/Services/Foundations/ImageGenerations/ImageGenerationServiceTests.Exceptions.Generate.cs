﻿// ---------------------------------------------------------------------------------- 
// Copyright (c) The Standard Organization, a coalition of the Good-Hearted Engineers 
// ----------------------------------------------------------------------------------

using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using RESTFulSense.Exceptions;
using Standard.AI.OpenAI.Models.Services.Foundations.ExternalImageGenerations;
using Standard.AI.OpenAI.Models.Services.Foundations.ImageGenerations;
using Standard.AI.OpenAI.Models.Services.Foundations.ImageGenerations.Exceptions;
using Xunit;

namespace Standard.AI.OpenAI.Tests.Unit.Services.Foundations.ImageGenerations
{
    public partial class ImageGenerationServiceTests
    {
        [Fact]
        public async Task ShouldThrowDependencyExceptionOnGenerateIfUrlNotFoundAsync()
        {
            // given
            ImageGeneration someImageGeneration = CreateRandomImageGeneration();

            var httpResponseUrlNotFoundException =
                new HttpResponseUrlNotFoundException();

            var invalidConfigurationImageGenerationException =
                new InvalidConfigurationImageGenerationException(
                    httpResponseUrlNotFoundException);

            var expectedImageGenerationDependencyException =
                new ImageGenerationDependencyException(
                    invalidConfigurationImageGenerationException);

            this.openAIBrokerMock.Setup(broker =>
                broker.PostImageGenerationRequestAsync(
                    It.IsAny<ExternalImageGenerationRequest>()))
                        .ThrowsAsync(httpResponseUrlNotFoundException);

            // when
            ValueTask<ImageGeneration> generateImageTask =
                this.imageGenerationService.GenerateImageAsync(someImageGeneration);

            ImageGenerationDependencyException actualImageGenerationDependencyException =
                await Assert.ThrowsAsync<ImageGenerationDependencyException>(
                    generateImageTask.AsTask);

            // then
            actualImageGenerationDependencyException.Should().BeEquivalentTo(
                expectedImageGenerationDependencyException);

            this.openAIBrokerMock.Verify(broker =>
                broker.PostImageGenerationRequestAsync(
                    It.IsAny<ExternalImageGenerationRequest>()),
                        Times.Once);

            this.openAIBrokerMock.VerifyNoOtherCalls();
        }

        [Theory]
        [MemberData(nameof(UnAuthorizationExceptions))]
        public async Task ShouldThrowDependencyExceptionOnGenerateIfUnAuthorizedAsync(
            HttpResponseException unAuthorizedException)
        {
            // given
            ImageGeneration someImageGeneration = CreateRandomImageGeneration();

            var unauthorizedImageGenerationException =
                new UnauthorizedImageGenerationException(unAuthorizedException);

            var expectedImageGenerationDependencyException =
                new ImageGenerationDependencyException(unauthorizedImageGenerationException);

            this.openAIBrokerMock.Setup(broker =>
                broker.PostImageGenerationRequestAsync(
                    It.IsAny<ExternalImageGenerationRequest>()))
                        .ThrowsAsync(unAuthorizedException);

            // when
            ValueTask<ImageGeneration> generateImageTask =
                this.imageGenerationService.GenerateImageAsync(someImageGeneration);

            ImageGenerationDependencyException actualImageGenerationDependencyException =
                await Assert.ThrowsAsync<ImageGenerationDependencyException>(
                    generateImageTask.AsTask);

            // then
            actualImageGenerationDependencyException.Should().BeEquivalentTo(
                expectedImageGenerationDependencyException);

            this.openAIBrokerMock.Verify(broker =>
                broker.PostImageGenerationRequestAsync(
                    It.IsAny<ExternalImageGenerationRequest>()),
                        Times.Once);

            this.openAIBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowDependencyValidationExceptionOnGenerateIfImageGenerationNotFoundOccurredAsync()
        {
            // given
            ImageGeneration someImageGeneration = CreateRandomImageGeneration();

            var httpResponseNotFoundException =
                new HttpResponseNotFoundException();

            var notFoundImageGenerationException =
                new NotFoundImageGenerationException(
                    httpResponseNotFoundException);

            var expectedImageGenerationDependencyValidationException =
                new ImageGenerationDependencyValidationException(
                    notFoundImageGenerationException);

            this.openAIBrokerMock.Setup(broker =>
                broker.PostImageGenerationRequestAsync(
                    It.IsAny<ExternalImageGenerationRequest>()))
                        .ThrowsAsync(httpResponseNotFoundException);

            // when
            ValueTask<ImageGeneration> generateImageTask =
                this.imageGenerationService.GenerateImageAsync(someImageGeneration);

            ImageGenerationDependencyValidationException actualImageGenerationDependencyValidationException =
                await Assert.ThrowsAsync<ImageGenerationDependencyValidationException>(
                    generateImageTask.AsTask);

            // then
            actualImageGenerationDependencyValidationException.Should().BeEquivalentTo(
                expectedImageGenerationDependencyValidationException);

            this.openAIBrokerMock.Verify(broker =>
                broker.PostImageGenerationRequestAsync(
                    It.IsAny<ExternalImageGenerationRequest>()),
                        Times.Once);

            this.openAIBrokerMock.VerifyNoOtherCalls();
        }
    }
}