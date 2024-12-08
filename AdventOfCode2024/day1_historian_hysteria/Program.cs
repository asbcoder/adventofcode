

using Day1_Historian_Hystria;

// 1, 2, 3, 3, 3, 4

// 3, 3, 3, 4, 5, 9

// 1    3   = 2
// 2    3   = 1
// 3    3   = 0
// 3    4   = 1
// 3    5   = 2
// 4    9   = 5
// 11

var filePath = args[0];

List<int> LocationsOne;
List<int> LocationsTwo;

(LocationsOne, LocationsTwo) = HelperMethods.GetLocations(filePath);

LocationsOne = LocationsOne.Order().ToList();
LocationsTwo = LocationsTwo.Order().ToList();

int totalDistance = HelperMethods.CalculateTotalDistance(LocationsOne, LocationsTwo);
int similarityScore = HelperMethods.CalculateSimilarityScore(LocationsOne, LocationsTwo);

Console.WriteLine($"Total Distance : {totalDistance}");
Console.WriteLine($"Similarity Score : {similarityScore}");





