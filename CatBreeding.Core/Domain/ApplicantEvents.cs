using System;

namespace CatBreeding.Core.Domain
{
    public interface IDomainEvent
    {
        Guid EventId { get; }
        DateTime OccurredOn { get; }
    }

    public record ApplicantDossierSubmitted : IDomainEvent
    {
        public Guid EventId { get; init; } = Guid.NewGuid();
        public DateTime OccurredOn { get; init; } = DateTime.UtcNow;

        public required string ApplicantName { get; init; }
        public required string ApplicantEmail { get; init; }
        public required string AcquisitionIntent { get; init; }
        public required string EnvTopography { get; init; }
        public required string EnvDemographics { get; init; }
        public bool Waitlist2026 { get; init; }
        public bool CalibrationConsent { get; init; }
        public string? PublicCredit { get; init; }
        public required string HumanElementDrive { get; init; }
        public required string HumanElementEmpathy { get; init; }
        public string? GeneralNotes { get; init; }
    }

    public record ApplicantDossierAppended : IDomainEvent
    {
        public Guid EventId { get; init; } = Guid.NewGuid();
        public DateTime OccurredOn { get; init; } = DateTime.UtcNow;

        public required string ApplicantEmail { get; init; }
        public required string Message { get; init; }
    }
}
