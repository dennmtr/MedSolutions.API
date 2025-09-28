namespace MedSolutions.Domain.Enums;

public enum State : byte
{
    Open = 1,
    OnHold = 2,
    Resolved = 3,
    Duplicate = 4,
    Invalid = 5,
    WontFix = 6,
    Closed = 7
}
