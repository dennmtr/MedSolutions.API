
using System.ComponentModel;
using MedSolutions.App.Common.Models;
using MedSolutions.Domain.Models;

namespace MedSolutions.App.Filters;

public class PatientQuerySorting() : QuerySorting<Patient>()
{
    protected override void Sort(string key)
    {
        switch (key.ToLower())
        {
            case "lastname":
                OrderBy(p => p.LastName)
                    .ThenBy(p => p.FirstName)
                    ;
                break;

            default:
                OrderBy(p => p.Appointments.Max(a => a.Date), ListSortDirection.Descending)
                    ;
                break;
        }
    }
}
