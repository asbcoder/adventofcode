namespace Day1_Historian_Hystria;


public static class HelperMethods
{
    public static (List<int>, List<int>) GetLocations(string filePath)
    {
        List<int> LocationsOne = new List<int>();
        List<int> LocationsTwo = new List<int>();
        try
        {
            using (StreamReader reader = new StreamReader(filePath))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    var locations = line.Split("   ");
                    LocationsOne.Add(int.Parse(locations[0]));
                    LocationsTwo.Add(int.Parse(locations[1]));
                }
            }
        }
        catch (IOException e)
        {
            Console.WriteLine("An error occurred: " + e.Message);
        }
       
        return (LocationsOne, LocationsTwo);
    }

    public static int CalculateTotalDistance(List<int> locationsOne, List<int> locationsTwo)
    {
        int output = 0;
        for (int i = 0; i < locationsOne.Count; i++)
        {
            var difference = Math.Abs(locationsOne[i] - locationsTwo[i]);
            output += difference;
        }

        return output;
    }

    public static int CalculateSimilarityScore(List<int> locationsOne, List<int> locationsTwo)
    {
        var output = 0;

        for (int i = 0; i < locationsOne.Count; i++)
        {
            // get the number
            // how many times does it appear in the second list
            var currentNumber = locationsOne[i];
            var matchCount = 0;
      
            foreach(var n in locationsTwo)
            {
                if (currentNumber == n)
                {
                    matchCount++;
                }
            }
            output += currentNumber * matchCount;
        }        

        return output;
    }
}

