using MedSolutions.Domain.Models;
using MedSolutions.Shared.Extensions;
using Microsoft.EntityFrameworkCore;

namespace MedSolutions.App.Extensions;
public static class PatientQueryExtensions
{
    public static IQueryable<Patient> WithAMKA(this IQueryable<Patient> query, string value) =>
        query.Where(p => EF.Functions.Like(value, p.AMKA + "%"));
    public static IQueryable<Patient> WithFamilyName(this IQueryable<Patient> query, string value) =>
        query.Where(p => EF.Functions.Like(value.ToLatin().ToLower(), p.LastNameLatin + "%"));
    public static IQueryable<Patient> WithPersonalIdNumber(this IQueryable<Patient> query, string value) =>
        query.Where(p => EF.Functions.Like(value, p.PersonalIdNumber + "%"));
    public static IQueryable<Patient> WithPhoneNumber(this IQueryable<Patient> query, string value) =>
        query.Where(p => EF.Functions.Like(value, p.PhoneNumber + "%") || EF.Functions.Like(value, p.MobileNumber + "%"));
    public static IQueryable<Patient> WithEmail(this IQueryable<Patient> query, string value) =>
        query.Where(p => p.Email != null && EF.Functions.Like(value.ToLowerInvariant(), p.Email + "%"));
    public static IQueryable<Patient> WithDateOfBirthGreaterThanOrEqual(this IQueryable<Patient> query, DateOnly date) =>
    query.Where(p => p.BirthDate != null && p.BirthDate.Value >= date);
    public static IQueryable<Patient> WithDateOfBirthLessThanOrEqual(this IQueryable<Patient> query, DateOnly date) =>
    query.Where(p => p.BirthDate != null && p.BirthDate.Value <= date);

}
