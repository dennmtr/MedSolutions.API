using MedSolutions.App.Common.Models;
using MedSolutions.Domain.Entities;
using MedSolutions.Shared.Extensions;
using Microsoft.EntityFrameworkCore;

namespace MedSolutions.App.Filters;

public class PatientQueryGlobalSearch : QueryGlobalSearch<Patient>
{
    public override IQueryable<Patient> ApplyTo(IQueryable<Patient> query)
    {
        if (string.IsNullOrWhiteSpace(Query))
        {
            return query;
        }

        var normalized = Query.ToLatin().ToLowerInvariant();

        return query.Where(p =>
            (p.AMKA != null && EF.Functions.Like(normalized, p.AMKA + "%")) ||
            (p.PersonalIdNumber != null && EF.Functions.Like(normalized, p.PersonalIdNumber + "%")) ||
            (p.PhoneNumber != null && EF.Functions.Like(normalized, p.PhoneNumber + "%")) ||
            (p.MobileNumber != null && EF.Functions.Like(normalized, p.MobileNumber + "%")) ||
            (p.Email != null && EF.Functions.Like(normalized, p.Email.ToLower() + "%")) ||
            (p.LastNameLatin != null && EF.Functions.Like(normalized, p.LastNameLatin + "%"))
        );
    }

}
