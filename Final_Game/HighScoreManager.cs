using System;
using System.IO;
using System.Linq;

namespace Final_Game
{
    public class HighScoreManager
    {
        //initialize highScores array with initial data
        public string[] highScores = { "0", "0", "0", "0", "0" };

        // Get file path
        public static string projectRoot = Directory.GetParent(AppContext.BaseDirectory).Parent.Parent.Parent.FullName;       
        public string filepath = Path.Combine(projectRoot, "Content", "highscores.txt");

        //boolean for checking if highscore has been added
        public bool isHighScoreAdded = false;

        public HighScoreManager()
        {

            // Create the file with default scores if it doesn't exist
            if (!File.Exists(filepath))
            {
                File.WriteAllLines(filepath, highScores);
            }
        }

        /// <summary>
        /// Load high scores from the file
        /// </summary>
        /// <returns> an array with sorted 5 highscore string </returns>
        public string[] LoadHighScores()
        {
            if (File.Exists(filepath))
            {
                highScores = File.ReadAllLines(filepath)
                                 .Select(hs => int.TryParse(hs, out int score) ? score : 0) 
                                 .OrderByDescending(hs => hs) // Sort in descending order
                                 .Take(5) // Get top 5
                                 .Select(score => score.ToString()) // Convert back to strings
                                 .ToArray();
            }

            return highScores;
        }

        /// <summary>
        /// Save high scores to the file
        /// </summary>
        public void SaveHighScores()
        {
            // Write the top 5 scores to the file
            File.WriteAllLines(filepath, highScores);
        }

        /// <summary>
        /// Add a new score and make sure the list is sorted and trimmed to top 5
        /// </summary>
        /// <param name="newScore">new score set by player</param>
        public void AddHighScore(int newScore)
        {
            LoadHighScores(); // Load existing high scores

            // Add the new score, sort in descending order, and take top 5
            var updatedScores = highScores.Select(s => int.Parse(s)) // Convert to integers
                                          .Append(newScore) // Add new score
                                          .OrderByDescending(score => score) // Sort in descending order
                                          .Take(5) // Get top 5 scores
                                          .ToArray();

            // Convert back to strings
            highScores = updatedScores.Select(score => score.ToString()).ToArray();

            // Save updated high scores to the file
            SaveHighScores();

            isHighScoreAdded = true; // Flag that a high score was added
        }
    }
}
