namespace MedSolutions.App.Interfaces;

public interface IAppContextProvider
{
    Guid? CurrentMedicalProfileId { get; }
    bool IsVisibilityModeEnabled { get; }
}
