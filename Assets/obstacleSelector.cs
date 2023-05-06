using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using Random = System.Random;
using System.Linq;

public class ObstacleSelector : MonoBehaviour
{
    Dictionary<HashSet<int>, int> Distances = new Dictionary<HashSet<int>, int>();
    Layout[] layouts = new Layout[1023];
    public int seed = 123098123;
    Random rand;
    Layout currLayout;
    int offset;
    ObstacleGenerator gen;
    
    // Start is called before the first frame update
    void Start()
    {
        PopulateLayouts();
        rand = new Random(seed);
        currLayout = layouts[0];
        offset = 0;
        gen = GetComponent<ObstacleGenerator>();
        StartCoroutine(Event());
    }

    private IEnumerator Event()
    {
        yield return new WaitForSeconds(5f);
        currLayout = GetNextLayout(currLayout);
        offset += 100;
        gen.MakeObstacle(currLayout, offset);
        Debug.Log("Making Obstacle at " + offset.ToString());
        
    }

    void PopulateLayouts()
    {
        for (int i = 0; i < 1023; i++){
            layouts[i] = new Layout(i);
        }
    }
    private double GetDesiredDifficulty()
    {
        return 0.8;
    }

    private Dictionary<double, List<Layout>> getDifficultyDict(Layout CurrentLayout, double difficulty)
    {
        Dictionary<double, List<Layout>> outputDict = new Dictionary<double, List<Layout>>();
        for ( int i = 0; i < 1023; i++){
            int testDifficulty = Difficulty(CurrentLayout, layouts[i]);
            if (testDifficulty < difficulty)
            {
                if (outputDict.ContainsKey(testDifficulty))
                {
                    outputDict[testDifficulty].Add(layouts[i]);
                }
                else
                {
                    outputDict[testDifficulty] = new List<Layout> {layouts[i]};
                }
            }
        }
        return outputDict;
    }

    public Layout GetNextLayout(Layout Current)
    {
        double desiredDifficulty = GetDesiredDifficulty();
        Dictionary<double, List<Layout>> Possibles = getDifficultyDict(Current, desiredDifficulty);
        //Now roll a random number between 0 and desired difficulty, using a bell curve hopefully
        double randomValue = Sample(desiredDifficulty);
        double closestKey = Possibles.OrderBy(kvp => Math.Abs(kvp.Key - randomValue))
                       .Select(kvp => kvp.Key)
                       .FirstOrDefault();
        List<Layout> layoutChoices = Possibles[closestKey];
        Layout nextLayout = layoutChoices[rand.Next(layoutChoices.Count)];
        return nextLayout;
    }

    private double Sample(double desiredDifficulty)
    {
        double u1 = 1.0-rand.NextDouble(); //uniform(0,1] random doubles
        double u2 = 1.0-rand.NextDouble();
        double randStdNormal = Math.Sqrt(-2.0 * Math.Log(u1)) * Math.Sin(2.0 * Math.PI * u2); //random normal(0,1)
        double mean = desiredDifficulty / 2;
        double stdDev = desiredDifficulty * .68;
        double randNormal = mean + stdDev * randStdNormal;
        return randNormal;
    }

    private int Difficulty(Layout Current, Layout Next)
    {
        int d1 = distance(Current.TopLayer, Next.TopLayer);
        int d2 = 1 + distance(Current.TopLayer, Next.BotLayer);
        int d3 = 1 + distance(Current.BotLayer, Next.TopLayer);
        int d4 = distance(Current.BotLayer, Next.BotLayer);
        return (Math.Min(Math.Min(d1, d2), Math.Min(d3, d4)) * Next.NumObstacles) / 45;
    }

    private int distance(bool[] Layer1, bool[] Layer2)
    {
        int Layer1Int = Convert.ToInt32(string.Join("", Layer1.Select(b => b ? "1": "0")), 2);
        int Layer2Int = Convert.ToInt32(string.Join("", Layer2.Select(b => b ? "1": "0")), 2);
        HashSet<int> set = new HashSet<int> {Layer1Int, Layer2Int};
        if( Distances.ContainsKey(set) )
        {
            return Distances[set];
        }

        int MinDistance = 5;
        for (int i = 0; i < Layer1.Length; i++)
        {
            if( Layer1[i] == false){
                for (int j = 0; j < Layer2.Length; j++)
                {
                    if (Layer2[j] == false)
                    {
                        int distance = Math.Abs(i - j);
                        if ( distance < MinDistance)
                        {
                            MinDistance = distance;
                        }
                    }
                }
            }
            
        }
        Distances[set] = MinDistance;
        return MinDistance;
    }
}


public class Layout 
{
    public int NumObstacles;
    public bool[] TopLayer = new bool[5];
    public bool[] BotLayer = new bool[5];
    

    public Layout(int num)
    {
        string binary = Convert.ToString(num, 2).PadLeft(10, '0');
        // String is now a string of 8 base 2 digits
        for (int i = 0; i < 10; i++)
        {
            char c = binary[i];
            if (i < 5)
            {
                // we should put the character in the Top Layer
                TopLayer[i] = Convert.ToBoolean(Convert.ToInt32(c));
            }
            else
            {
                BotLayer[i-5] = Convert.ToBoolean(Convert.ToInt32(c));
            }
        }
        //Layers are now filled
        //To fill NumObstacles, count 1's in base 2 representation of number
        while(num != 0)
        {
            NumObstacles += num & 1;
            num >>= 1;
        }
    }
}



// Difficulty:
// Inputs:
// current speed
// Level

// Func:
// Some linear function with a sigmoid applied to output

// Output:
// float between 0 and 1
// -------------------------
// Get Possible Layouts
// Inputs:
// Current Layout

// Func:
// This is the hard one

// difficulty should be a function of the distance needed to travel to get to a hole
// as well as how difficult it is to get through a hole

// So maybe every layout has a base difficulty, based off of how many holes are in it, as well as how connected they are

// Track is 5 wide 2 tall

// Difficulty function:
// max(argmin(distance between holes))*(squares closed) / 45

// Scale linearly between 0 and 1
// so devide by 9 squares closed * distance of 5 so deviding by 45

// Output:
// a map of all layouts with their difficulties
// -------------------------
// Pick next layout

// Input:
// Map of layouts with difficulties
// Current Difficulty

// Func:
// Choose a number from a bellcurve between 0 and difficulty
// Return layout with closest difficulty to number

// Output:
// A new layout