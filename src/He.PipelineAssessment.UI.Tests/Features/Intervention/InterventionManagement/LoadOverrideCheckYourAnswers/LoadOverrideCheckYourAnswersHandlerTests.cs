﻿using AutoFixture.Xunit2;
using He.PipelineAssessment.Infrastructure.Repository.StoredProcedure;
using He.PipelineAssessment.Infrastructure.Repository;
using He.PipelineAssessment.Models.ViewModels;
using He.PipelineAssessment.Tests.Common;
using He.PipelineAssessment.UI.Features.Assessment.AssessmentSummary;
using Moq;
using Xunit;
using He.PipelineAssessment.UI.Features.Intervention.InterventionManagement.CreateOverride;
using He.PipelineAssessment.Models;
using He.PipelineAssessment.UI.Features.Intervention.InterventionManagement.EditOverride;
using He.PipelineAssessment.UI.Features.Intervention.InterventionManagement;
using AutoMapper;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
using He.PipelineAssessment.UI.Features.Intervention.InterventionManagement.LoadOverrideCheckYourAnswers;
using He.PipelineAssessment.UI.Features.Intervention.InterventionManagement.SubmitOverride;
using Newtonsoft.Json;
using He.PipelineAssessment.UI.Common.Exceptions;
using Microsoft.EntityFrameworkCore;
using System;

namespace He.PipelineAssessment.UI.Tests.Features.Intervention.InterventionManagement.LoadOverrideCheckYourAnswers
{
    public class LoadOverrideCheckYourAnswersRequestHandlerTests
    {
        [Theory]
        [AutoMoqData]
        public async Task Handle_ThrowsException_GivenNullResponseFromRepo(
                     [Frozen] Mock<IAssessmentRepository> assessmentRepository,
                     LoadOverrideCheckYourAnswersRequest request,
                     AssessmentIntervention? intervention,
                     LoadOverrideCheckYourAnswersRequestHandler sut
                 )
        {
            //Arrange
            intervention = null;

            assessmentRepository.Setup(x => x.GetAssessmentIntervention(request.InterventionId)).ReturnsAsync(intervention);

            //Act
            var ex = await Assert.ThrowsAsync<NotFoundException>(() => sut.Handle(request, CancellationToken.None));

            //Assert
            Assert.Equal(($"Assessment Intervention with Id {request.InterventionId} not found"), ex.Message);
        }

        [Theory]
        [AutoMoqData]
        public async Task Handle_DoesNotHandleError_GivenRepoThrowsException(
                     [Frozen] Mock<IAssessmentRepository> assessmentRepository,
                     LoadOverrideCheckYourAnswersRequest request,
                     DbUpdateException exception,
                     LoadOverrideCheckYourAnswersRequestHandler sut
                 )
        {
            //Arrange
            assessmentRepository.Setup(x => x.GetAssessmentIntervention(request.InterventionId)).ThrowsAsync(exception);

            //Act
            var ex = await Assert.ThrowsAsync<DbUpdateException>(() => sut.Handle(request, CancellationToken.None));

            //Assert
            Assert.Equal(exception.Message, ex.Message);
        }

        [Theory]
        [AutoMoqData]
        public async Task Handle_DoesNotHandleError_GivenMapperThrowsError(
          [Frozen] Mock<IAssessmentRepository> assessmentRepository,
          [Frozen] Mock<IAssessmentInterventionMapper> mapper,
          AssessmentIntervention intervention,
          LoadOverrideCheckYourAnswersRequest request,
          ArgumentException exception,
          LoadOverrideCheckYourAnswersRequestHandler sut
)
        {
            //Arrange
            var emptyDto = new AssessmentInterventionDto();

            assessmentRepository.Setup(x => x.GetAssessmentIntervention(request.InterventionId)).ReturnsAsync(intervention);
            mapper.Setup(x => x.AssessmentInterventionCommandFromAssessmentIntervention(intervention)).Throws(exception);

            //Act
            ArgumentException ex = await Assert.ThrowsAsync<ArgumentException>(() => sut.Handle(request, CancellationToken.None));

            //Assert
            Assert.Equal(exception.Message, ex.Message);
        }

        [Theory]
        [AutoMoqData]
        public async Task Handle_ThrowsArgumentException_GivenMapperReturnsNull(
              [Frozen] Mock<IAssessmentRepository> assessmentRepository,
              [Frozen] Mock<IAssessmentInterventionMapper> mapper,
              AssessmentInterventionCommand command,
              AssessmentIntervention intervention,
              LoadOverrideCheckYourAnswersRequest request,
              LoadOverrideCheckYourAnswersRequestHandler sut
)
        {
            var serializedCommand = JsonConvert.SerializeObject(command);
            var submitOverrideCommand = JsonConvert.DeserializeObject<SubmitOverrideCommand>(serializedCommand);
            assessmentRepository.Setup(x => x.GetAssessmentIntervention(request.InterventionId)).ReturnsAsync(intervention);
            mapper.Setup(x => x.AssessmentInterventionCommandFromAssessmentIntervention(intervention)).Returns((AssessmentInterventionCommand)null!);

            //Act
            ArgumentException ex = await Assert.ThrowsAsync<ArgumentException>(() => sut.Handle(request, CancellationToken.None));

            //Assert
            Assert.Equal(($"Unable to map AssessmentInterventionCommand from intervention: {JsonConvert.SerializeObject(intervention)} from mapper"), ex.Message);
        }

        [Theory]
        [AutoMoqData]
        public async Task Handle_ReturnsValidResult_GivenNoErrors(
          [Frozen] Mock<IAssessmentRepository> assessmentRepository,
          [Frozen] Mock<IAssessmentInterventionMapper> mapper,
          AssessmentInterventionCommand command,
          AssessmentIntervention intervention,
          LoadOverrideCheckYourAnswersRequest request,
          LoadOverrideCheckYourAnswersRequestHandler sut
    )
        {
            var serializedCommand = JsonConvert.SerializeObject(command);
            var submitOverrideCommand = JsonConvert.DeserializeObject<SubmitOverrideCommand>(serializedCommand);
            assessmentRepository.Setup(x => x.GetAssessmentIntervention(request.InterventionId)).ReturnsAsync(intervention);
            mapper.Setup(x => x.AssessmentInterventionCommandFromAssessmentIntervention(intervention)).Returns(command);

            //Act
            var result = await sut.Handle(request, CancellationToken.None);

            //Assert
            Assert.Equal(submitOverrideCommand!.ToString(), result!.ToString());
        }
    }
}
