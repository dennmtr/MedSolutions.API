using MedSolutions.App.Common.Models;
using MedSolutions.App.Extensions;
using MedSolutions.Domain.Entities;

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

            if (string.IsNullOrEmpty(property))
            {
                continue;
            }

            var propertyOperator = dict.FirstOrDefault().Key.Trim().ToLower();
            var propertyValue = dict.FirstOrDefault().Value.Trim();

            switch (property.ToLower())
            {
                // &filter[lastName][eq]=

                case "lastname":
                    query = query.WithFamilyName(propertyValue);
                    break;

                // &filter[amka][eq]=

                case "amka":
                    query = query.WithAMKA(propertyValue);
                    break;

                // &filter[personalIdNumber][eq]=

                case "personalidnumber":
                    query = query.WithPersonalIdNumber(propertyValue);
                    break;

                // &filter[phoneNumber|mobileNumber][eq]=

                case "mobilenumber":
                case "phonenumber":
                    query = query.WithPhoneNumber(propertyValue);
                    break;

                // &filter[email][eq]=

                case "email":
                    query = query.WithEmail(propertyValue);
                    break;

                // &filter[birthdate][gte|lte] =

                case "birthdate":

                    if (DateOnly.TryParse(propertyValue, out var targetDate))
                    {
                        query = propertyOperator == "gte" ? query.WithDateOfBirthGreaterThanOrEqual(targetDate) : query.WithDateOfBirthLessThanOrEqual(targetDate);
                    }
                    break;

                default:
                    break;
            }
        }
        return query;
    }
}
