using AutoMapper;

namespace PIMTool.Api.Converters;

public sealed class DateTimeToDateOnlyConverter : IValueConverter<DateTime, DateOnly>, IValueConverter<DateTime?, DateOnly?>
{
    public DateOnly Convert(DateTime sourceMember, ResolutionContext context)
    {
        return DateOnly.FromDateTime(sourceMember);
    }

    public DateOnly? Convert(DateTime? sourceMember, ResolutionContext context)
    {
        if (sourceMember == null)
        {
            return null;
        }

        return DateOnly.FromDateTime((DateTime)sourceMember);
    }
}
