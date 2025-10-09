using MedSolutions.App.Common.Models;
using MedSolutions.Domain.Models;
using MedSolutions.Shared.Extensions;
using Microsoft.EntityFrameworkCore;

//?filter[rank][in]=1,41&filter[birthdate][gte]=1950-01-01&filter[birthdate][lte]=2005-01-01

namespace MedSolutions.App.Filters;

public class PatientQueryFilter : QueryFilter<Patient>
{
    public override IQueryable<Patient> ApplyTo(IQueryable<Patient> query)
    {

        if (Filters is null)
        {
            return query;
        }

        foreach (var (property, dict) in Filters)
        {
            var propertyOperator = dict.FirstOrDefault().Key.Trim().ToLower();
            var propertyValue = dict.FirstOrDefault().Value.Trim();

            if (string.IsNullOrEmpty(property))
            {
                continue;
            }

            switch (property.ToLower())
            {
                // &filter[lastName][eq]=

                case "lastname":
                    query = query.Where(p => EF.Functions.Like(propertyValue.ToLatin().ToLower(), p.LastNameLatin + "%"));
                    break;

                // &filter[amka][eq]=

                case "amka":
                    query = query.Where(p =>
                        p.AMKA != null &&
                        EF.Functions.Like(propertyValue, p.AMKA + "%"));
                    break;

                // &filter[personalIdNumber][eq]=

                case "personalidnumber":
                    query = query.Where(p =>
                        p.PersonalIdNumber != null &&
                        EF.Functions.Like(propertyValue, p.PersonalIdNumber + "%"));
                    break;

                // &filter[phoneNumber][eq]=

                case "phonenumber":
                    query = query.Where(p =>
                        p.PhoneNumber != null &&
                        EF.Functions.Like(propertyValue, p.PhoneNumber + "%"));
                    break;

                // &filter[mobileNumber][eq]=

                case "mobilenumber":
                    query = query.Where(p =>
                        p.MobileNumber != null &&
                        EF.Functions.Like(propertyValue, p.MobileNumber + "%"));
                    break;

                // &filter[email][eq]=

                case "email":
                    query = query.Where(p =>
                        p.Email != null &&
                        EF.Functions.Like(propertyValue.ToLowerInvariant(), p.Email + "%"));
                    break;

                case "birthdate":

                    // &filter[birthdate][gte]=

                    if (propertyOperator == "gte" &&
                        DateOnly.TryParse(propertyValue, out var gteDate))
                    {
                        query = query.Where(p => p.BirthDate >= gteDate);
                    }

                    //&filter[birthdate][lte]=

                    if (propertyOperator == "lte" &&
                        DateOnly.TryParse(propertyValue, out var lteDate))
                    {
                        query = query.Where(p => p.BirthDate <= lteDate);
                    }
                    break;
                default:
                    break;
            }
        }

        return query;
    }
}
