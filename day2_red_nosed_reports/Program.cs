using System.IO;

// if (args.Count() == 0)
// {
//     Console.WriteLine("Report file path not provided");
//     return;
// }

// string filePath = args[0];
string filePath = "reports_test.txt";

if (File.Exists(filePath) == false)
{
    Console.WriteLine($"File {filePath} does not exist");
    return;
}

int totalSafeReports = 0;
List<string> reports = GetReports(filePath);

foreach (var report in reports)
{
    var reportInt = report.Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToList();
    if (IsSafe(reportInt))
    {
        totalSafeReports++;
    }
    else
    {
        for (int i = 0; i < reportInt.Count; i++)
        {
            var reportCopy = reportInt.ToList();
            reportCopy.RemoveAt(i);
            if (IsSafe(reportCopy))
            {
                totalSafeReports++;
                break;
            }
        }
    }

}



Console.WriteLine($"Total Safe Reports : {totalSafeReports}");

bool IsSafe(List<int> report)
{
    if (report.Count < 2)
    {
        return true;
    }

    var firstDiff = report[1] - report[0];

    if (firstDiff == 0 || Math.Abs(firstDiff) > 3)
    {
        return false;
    }

    var expectedSgn = firstDiff / Math.Abs(firstDiff);

    for (int i = 1; i < report.Count - 1; i++)
    {
        var diff = report[i + 1] - report[i];
        if (diff == 0 || Math.Abs(diff) > 3)
        {
            return false;
        }

        var sgn = diff / Math.Abs(diff);
        if (sgn != expectedSgn)
        {
            return false;
        }
    }

    return true;
}

static bool IsReportSafe(string report, bool isDampenerIsUse)
{
    bool isReportSafe = true;
    List<int> levels = GetLevels(report);
    int levelsRemoved = 0;
    for (int i = 0; i < levels.Count; i++)
    {
        bool isLevelSafe = true;
        if (isReportSafe == true)
        {
            if (i > 0)
            {
                isLevelSafe = IsLevelDifferenceSafe(levels[i], levels[i - 1]);
                if (isLevelSafe == true)
                {
                    if (i > 1)
                    {
                        SortOrder currentSortOrder = GetSortOrder(levels[i], levels[i - 1]);
                        SortOrder previousSortOrder = GetSortOrder(levels[i - 1], levels[i - 2]);
                        isLevelSafe = IsLevelSortOrderSafe(currentSortOrder, previousSortOrder);
                    }
                }
            }

            // can we remove the level?
            if (isLevelSafe == false && levelsRemoved < 1 && isDampenerIsUse == true)
            {
                levels.RemoveAt(i);
                i = -1;
                levelsRemoved++;
                isLevelSafe = true;
            }

            isReportSafe = isLevelSafe;
        }
    }
    return isReportSafe;
}

static SortOrder GetSortOrder(int currentLevel, int previousLevel)
{
    SortOrder output;
    if (currentLevel == previousLevel)
    {
        output = SortOrder.Equal;
    }
    else if (currentLevel > previousLevel)
    {
        output = SortOrder.Ascending;
    }
    else
    {
        output = SortOrder.Descending;
    }

    return output;
}

static bool IsLevelSortOrderSafe(SortOrder currentSortOrder, SortOrder previousSortOrder)
{
    bool output = false;

    if (currentSortOrder == previousSortOrder)
    {
        output = true;
    }

    return output;
}

static bool IsLevelDifferenceSafe(int currentLevel, int previousLevel)
{
    bool output = false;
    int levelDifference = Math.Abs(currentLevel - previousLevel);
    int maxAllowedDifference = 3;
    int minAllowedDifference = 1;

    if (levelDifference <= maxAllowedDifference && levelDifference >= minAllowedDifference)
    {
        output = true;
    }

    return output;
}

static List<int> GetLevels(string report)
{
    List<int> output = new List<int>();
    var levels = report.Split(" ");
    for (int i = 0; i < levels.Length; i++)
    {
        output.Add(int.Parse(levels[i]));
    }

    return output;
}

static List<string> GetReports(string filePath)
{
    List<string> output = new List<string>();
    try
    {
        using (StreamReader reader = new StreamReader(filePath))
        {
            string line;
            while ((line = reader.ReadLine()) != null)
            {
                output.Add(line);
            }
        }
    }

    catch (IOException e)
    {
        Console.WriteLine("Error reading file: " + e.Message);
    }

    return output;
}

enum SortOrder
{
    Ascending,
    Descending,
    Equal
}


//ReadLevels(filePath);




// void ReadLevels(string filePath)
// {
//     int currentLevel = 0;
//     int previousLevel = 0;
//     int allowedDiff = 3;
//     bool isLevelSafe = true;
//     bool isReportSafe = true;
//     bool dampenerInUse = false;
//     int levelsRemoved = 0;
//     int totalSafeLocations = 0;
//     SortOrder? currentSortOrder = null;
//     SortOrder? previousSortOrder = null;

//     try
//     {
//         using (StreamReader reader = new StreamReader(filePath))
//         {
//             string line;
//             while ((line = reader.ReadLine()) != null)
//             {
//                 isReportSafe = true;
//                 var levels = line.Split(" ").ToList();
//                 for (int i = 0; i < levels.Count; i++)
//                 {
//                     if (dampenerInUse == true && levelsRemoved <= 1)
//                     {
//                         isReportSafe = true;
//                     }
//                     else if (dampenerInUse == true && levelsRemoved > 1)
//                     {
//                         isReportSafe = false;
//                     }
//                     else if (dampenerInUse == false && isLevelSafe == false)
//                     {
//                         isReportSafe = false;
//                     }

//                     if (isReportSafe == true)
//                     {
//                         // first number on the line will always be safe and no need to look forward or back
//                         if (i == 0)
//                         {
//                             currentLevel = int.Parse(levels[i]);
//                             isLevelSafe = true;
//                         }

//                         // second number, no need to check for sort order is valid
//                         // only need to check forward
//                         else if (i == 1)
//                         {
//                             previousLevel = currentLevel;
//                             currentLevel = int.Parse(levels[i]);
//                             if (currentLevel < previousLevel)
//                             {
//                                 currentSortOrder = SortOrder.Descending;
//                                 isLevelSafe = true;
//                             }
//                             else if (currentLevel > previousLevel)
//                             {
//                                 currentSortOrder = SortOrder.Ascending;
//                                 isLevelSafe = true;
//                             }
//                             else // must be equal
//                             {
//                                 currentSortOrder = SortOrder.Equal;
//                                 isLevelSafe = false;
//                                 if (dampenerInUse == true)
//                                 {
//                                     levels.RemoveAt(i); // remove the element from the list
//                                     i = -1; // reset the counter back to zero so begin loop again                                
//                                     levelsRemoved++;
//                                 }
//                             }

//                             // check within tolernance
//                             if (isLevelSafe == true)
//                             {
//                                 if (Math.Abs(currentLevel - previousLevel) > allowedDiff)
//                                 {
//                                     isLevelSafe = false;
//                                     if (dampenerInUse == true)
//                                     {
//                                         levels.RemoveAt(i); // remove the element from the list
//                                         i = -1; // reset the counter back to zero so begin loop again                                
//                                         levelsRemoved++;
//                                     }
//                                 }
//                                 else
//                                 {
//                                     isLevelSafe = true;
//                                 }
//                             }
//                         }
//                         else
//                         {
//                             previousLevel = currentLevel;
//                             currentLevel = int.Parse(levels[i]);
//                             previousSortOrder = currentSortOrder;

//                             if (currentLevel < previousLevel)
//                             {
//                                 currentSortOrder = SortOrder.Descending;
//                                 isLevelSafe = true;
//                             }
//                             else if (currentLevel > previousLevel)
//                             {
//                                 currentSortOrder = SortOrder.Ascending;
//                                 isLevelSafe = true;
//                             }
//                             else
//                             {
//                                 currentSortOrder = SortOrder.Equal;
//                                 isLevelSafe = false;
//                                 // if (dampenerInUse == true)
//                                 // {
//                                 //     levels.RemoveAt(i); // remove the element from the list
//                                 //     i = -1; // reset the counter back to zero so begin loop again                                
//                                 //     levelsRemoved++;
//                                 // }
//                             }
//                             if (currentSortOrder != previousSortOrder)
//                             {
//                                 isLevelSafe = false;
//                                 if (dampenerInUse == true)
//                                 {
//                                     levels.RemoveAt(i); // remove  the element from the list
//                                     i = -1; // reset the counter back to zero so begin loop again                                
//                                     levelsRemoved++;
//                                 }
//                             }
//                             else
//                             {
//                                 isLevelSafe = true;
//                             }

//                             // check within tolernance
//                             if (isLevelSafe == true)
//                             {
//                                 if (Math.Abs(currentLevel - previousLevel) > allowedDiff)
//                                 {
//                                     isLevelSafe = false;
//                                     if (dampenerInUse == true)
//                                     {
//                                         levels.RemoveAt(i); // remove the element from the list
//                                         i = -1; // reset the counter back to zero so begin loop again                                
//                                         levelsRemoved++;
//                                     }
//                                 }
//                                 else
//                                 {
//                                     isLevelSafe = true;
//                                 }
//                             }
//                         }
//                     }

//                 }
//                 Console.WriteLine($"{line} : Is Safe = {isReportSafe}");
//                 Console.WriteLine();
//                 if (isReportSafe)
//                 {
//                     totalSafeLocations++;
//                 }
//                 Console.WriteLine(totalSafeLocations);
//                 // reset the variables
//                 isLevelSafe = true;
//                 isReportSafe = true;
//                 levelsRemoved = 0;

//             }
//         }
//         Console.WriteLine($"Total Safe Locations: {totalSafeLocations}");
//     }
//     catch (IOException e)
//     {
//         Console.WriteLine($"Error reading file {e.Message}");
//     }
// }


