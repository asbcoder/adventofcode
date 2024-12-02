using System.IO;


// read the levels from file
string filePath = args[0];

ReadLevels(filePath);

void ReadLevels(string filePath)
{   
    int currentLevel = 0;
    int previousLevel = 0;
    int allowedDiff = 2;
    bool isSafe = true;
    SortOrder? currentSortOrder = null;
    SortOrder? previousSortOrder = null;
 
    try
    {
        using (StreamReader reader = new StreamReader(filePath))
        {
            string line;
            while ((line = reader.ReadLine()) != null)
            {
                var levels = line.Split(" ");
                for(int i = 0; i < levels.Length; i++)
                {
                   
                    if (isSafe == true)
                    {
                        // first number on the line will always be safe
                        if (i == 0)
                        {
                            currentLevel = int.Parse(levels[i]);
                        }

                        // second number, no need to check for sort order is valid
                        else if (i == 1)
                        {
                            previousLevel = currentLevel;
                            currentLevel = int.Parse(levels[i]);
                            if (currentLevel < previousLevel)
                            {
                                currentSortOrder = SortOrder.Descending;
                            }
                            else if(currentLevel > previousLevel)
                            {
                                currentSortOrder = SortOrder.Ascending;
                            }
                            else
                            {
                                isSafe = false;
                            }
                            
                            // check within tolernance
                            if (isSafe == true)
                            {
                                
                                if (Math.Abs(currentLevel - previousLevel) > allowedDiff)
                                {
                                    isSafe = false;
                                }
                            }
                        }
                        else 
                        {
                            previousLevel = currentLevel;
                            currentLevel = int.Parse(levels[i]);
                            previousSortOrder = currentSortOrder;
                                                
                            if (currentLevel < previousLevel)
                            {
                                currentSortOrder = SortOrder.Descending;
                            }
                            else if(currentLevel > previousLevel)
                            {
                                currentSortOrder = SortOrder.Ascending;
                            }
                            else
                            {
                                isSafe = false;
                            }
                            if (currentSortOrder != previousSortOrder)
                            {
                                isSafe = false;
                            }
                           
                            // check within tolernance
                            if (isSafe == true)
                            {                                
                                if (Math.Abs(currentLevel - previousLevel) > allowedDiff)
                                {
                                    isSafe = false;                                    
                                }
                            }
                        }
                    }  
                                                                    
                }
                
                Console.WriteLine();
            }
        }        
    }
    catch (IOException e)
    {
        Console.WriteLine($"Error reading file {e.Message}");
    }
}

enum SortOrder 
{
    Ascending,
    Descending
}

