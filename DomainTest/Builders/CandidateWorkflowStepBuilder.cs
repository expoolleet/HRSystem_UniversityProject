using AutoFixture.Kernel;
using Domain.Candidates;
using Domain.Companies;

namespace DomainTest.Builders
{
    public class CandidateWorkflowStepBuilder : ISpecimenBuilder
    {
        private User _user;
        public CandidateWorkflowStepBuilder(User user)
        {
            ArgumentNullException.ThrowIfNull(user);
            _user = user;
        }
        
        private int _stepCounter = 1;
        
        public object Create(object request, ISpecimenContext context)
        {
            if (!typeof(CandidateWorkflowStep).Equals(request))
            {
                return new NoSpecimen();
            }

            return CandidateWorkflowStep.Create(
                _user?.Id,
                _user?.RoleId,
                _stepCounter++
                );
        }
    }
}