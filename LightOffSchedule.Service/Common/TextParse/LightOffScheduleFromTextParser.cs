using System.Globalization;
using System.Text.RegularExpressions;
using LightOffSchedule.Common;
using LightOffSchedule.Repository.Models;

namespace LightOffSchedule.Service.Common.TextParse;

public partial class LightOffScheduleFromTextParser
{
    public ServiceResponse<IEnumerable<LightOffScheduleModel>> Parse(string text)
    {
        var scheduleRows = text.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries);

        if (scheduleRows is not { Length: > 0 })
            return ServiceResponse<IEnumerable<LightOffScheduleModel>>.Error(
                new Error("Текст порожній"));

        var result = new List<LightOffScheduleModel>();
        
        foreach (var row in scheduleRows)
        {
            var isSuccessGroupNumberParse = TryParseGroupNumber(row, out var groupNumber);
            if (!isSuccessGroupNumberParse)
                return ServiceResponse<IEnumerable<LightOffScheduleModel>>.Error(
                    new Error("Невірний формат номера групи"));
            
            var isSuccessIntervalsParse = TryParseLightOffIntervals(row, out var intervals);
            
            if (!isSuccessIntervalsParse)
                return ServiceResponse<IEnumerable<LightOffScheduleModel>>.Error(
                    new Error("Невірний формат інтервалів вимкнення"));
            
            result.Add(new LightOffScheduleModel()
            {
                GroupNumber = groupNumber,
                Intervals = intervals
            });
        }
        
        

        return ServiceResponse<IEnumerable<LightOffScheduleModel>>.Success(result);
    }
    
    private bool TryParseGroupNumber(string text, out int groupNumber)
    {
        groupNumber = -1;
        var groupNumberMatch = GroupNumberRegex().Match(text);

        if (!groupNumberMatch.Success)
            return false;
        
        var isSuccess = int.TryParse(groupNumberMatch.Value, CultureInfo.InvariantCulture, out groupNumber);
        return isSuccess;
    }
    
    private bool TryParseLightOffIntervals(string text, out IEnumerable<LightOffScheduleIntervalModel> intervals)
    {
        intervals = null!;
        var intervalMatches = IntervalRegex().Matches(text);

        if (intervalMatches.Count == 0)
            return false;

        var result = new List<LightOffScheduleIntervalModel>();
        foreach (Match intervalMatch in intervalMatches)
        {
            var interval = intervalMatch.Value.Split('-', StringSplitOptions.RemoveEmptyEntries);
            if (interval.Length != 2)
                return false;

            var isSuccessStartParse = TimeSpan.TryParse(interval[0], out var start);
            var isSuccessEndParse = TimeSpan.TryParse(interval[1], out var end);

            if (!isSuccessStartParse || !isSuccessEndParse)
                return false;

            result.Add(new LightOffScheduleIntervalModel
            {
                Start = start,
                End = end
            });
        }

        intervals = result;
        return true;
    }

    [GeneratedRegex(@"^(\d+)\.", RegexOptions.Compiled)]
    private static partial Regex GroupNumberRegex();
    
    [GeneratedRegex(@"\d{2}:\d{2}-\d{2}:\d{2}", RegexOptions.Compiled)]
    private static partial Regex IntervalRegex();
}