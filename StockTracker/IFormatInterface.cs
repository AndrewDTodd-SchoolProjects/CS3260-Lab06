using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockTracker
{
    internal interface IFormatInterface
    {
        /// <summary>
        /// Purpose: The class that implements this interface will read in a file passed in and produce a list of 
        /// reversal points found in the stock tracking data inside the file
        /// </summary>
        /// <param name="filePath">string representing the file path of a file with the stock traking data</param>
        /// <returns>List of <cref>ReversalInfo</cref> or null if no reversal info is found</returns>
        /// <exeption>IOExeption: can be any exeption that occurs that causes reading of the file to fail</exeption>
        /// <exeption>ArgumentExeption: thrown if the file path entered is for the wrong file type</exeption>
        List<ReversalInfo>? ParseData(string filePath);
    }
}
