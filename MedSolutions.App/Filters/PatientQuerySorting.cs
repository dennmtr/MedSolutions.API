
using System.ComponentModel;
using MedSolutions.App.Common.Models;
using MedSolutions.App.Extensions;
using MedSolutions.Domain.Models;

namespace MedSolutions.App.Filters;

public class PatientQuerySorting() : QuerySorting<Patient>()
{
    protected override IOrderedQueryable<Patient> GetOrderedQuery(string key)
    {
        var isDescending = SortDirection == ListSortDirection.Descending;
        return key switch {
            "lastname" => Query.OrderByLastName(isDescending),
            "city" => Query.OrderByCity(isDescending),
            "birthdate" => Query.OrderByDateOfBirth(isDescending),
            "datemodified" => Query.OrderByDateModified(isDescending),
            _ => Query.OrderByAppointments(isDescending),
        };
    }
}
