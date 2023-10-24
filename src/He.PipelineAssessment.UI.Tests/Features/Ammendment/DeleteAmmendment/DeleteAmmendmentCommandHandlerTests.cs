﻿using AutoFixture.Xunit2;
using He.PipelineAssessment.Infrastructure.Repository;
using He.PipelineAssessment.Models;
using He.PipelineAssessment.Tests.Common;
using He.PipelineAssessment.UI.Authorization;
using He.PipelineAssessment.UI.Features.Ammendment.DeleteAmmendment;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace He.PipelineAssessment.UI.Tests.Features.Ammendment.DeleteAmmendment
{
    public class DeleteAmmendmentCommandHandlerTests
    {
        [Theory]
        [AutoMoqData]
        public async Task Handle_ShouldError_GivenAssessmentToolWorkflowInstanceCannotBeFound(
            DeleteAmmendmentCommand command,
            DeleteAmmendmentCommandHandler sut)
        {
            //Act
            var ex = await Assert.ThrowsAsync<ApplicationException>(() => sut.Handle(command, CancellationToken.None));

            //Assert
            Assert.Equal($"Unable to delete ammendment. WorkflowInstanceId: {command.WorkflowInstanceId}", ex.Message);
        }

        [Theory]
        [AutoMoqData]
        public async Task Handle_ShouldError_GivenUserIsNotPermitted(
            [Frozen] Mock<IAssessmentRepository> repository,
            AssessmentIntervention intervention,
            DeleteAmmendmentCommand command,
            DeleteAmmendmentCommandHandler sut)
        {
            //Arrange
            repository.Setup(x => x.GetAssessmentIntervention(command.AssessmentInterventionId))
                .ReturnsAsync(intervention);

            //Act
            var ex = await Assert.ThrowsAsync<UnauthorizedAccessException>(() => sut.Handle(command, CancellationToken.None));

            //Assert
            Assert.Equal($"You do not have permission to access this resource.", ex.Message);
        }

        [Theory]
        [AutoMoqData]
        public async Task Handle_ShouldDelete(
            [Frozen] Mock<IAssessmentRepository> repository,
            [Frozen] Mock<IRoleValidation> roleValidation,
            AssessmentIntervention intervention,
            DeleteAmmendmentCommand command,
            DeleteAmmendmentCommandHandler sut)
        {
            //Arrange
            repository.Setup(x => x.GetAssessmentIntervention(command.AssessmentInterventionId))
                .ReturnsAsync(intervention);

            roleValidation.Setup(x =>
                x.ValidateRole(intervention.AssessmentToolWorkflowInstance.AssessmentId, intervention.AssessmentToolWorkflowInstance.WorkflowDefinitionId))
                .ReturnsAsync(true);

            //Act
            var result = await sut.Handle(command, CancellationToken.None);

            //Assert
            repository.Verify(x => x.DeleteIntervention(intervention), Times.Once);

        }
    }
}
