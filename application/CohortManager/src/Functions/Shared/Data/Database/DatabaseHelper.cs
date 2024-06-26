using System.Globalization;
using System.Text.RegularExpressions;
using Microsoft.Extensions.Logging;
namespace Data.Database;
public class DatabaseHelper : IDatabaseHelper
{

    private readonly ILogger<DatabaseHelper> _logger;
    public DatabaseHelper(ILogger<DatabaseHelper> logger)
    {
        _logger = logger;
    }

    public bool CheckIfDateNull(string property)
    {
        if (string.IsNullOrEmpty(property))
        {
            return true;
        }
        return !DateTime.TryParse(property, out _);
    }

    public bool CheckIfNumberNull(string property)
    {
        if (string.IsNullOrEmpty(property))
        {
            return true;
        }

        return !long.TryParse(property, out _);

    }

    public DateTime parseDates(string dateString)
    {
        DateTime tempDate = new DateTime();
        bool success = DateTime.TryParseExact(dateString, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out tempDate);

        if (!success)
        {
            _logger.LogError($"****Failed to parse date: {dateString}");
        }
        return tempDate;

    }

    public object ConvertNullToDbNull(string value)
    {
        return string.IsNullOrEmpty(value) ? (object)DBNull.Value : value;
    }

    public string ParseDateToString(string dateToParse)
    {
        return (DateTime.ParseExact(dateToParse, "dd/MM/yyyy", CultureInfo.InvariantCulture)).ToString();
    }
}
