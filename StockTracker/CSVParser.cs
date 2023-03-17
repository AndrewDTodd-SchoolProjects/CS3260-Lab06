using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Globalization;

namespace StockTracker
{
    internal class CSVParser : IFormatInterface
    {
        public List<ReversalInfo>? ParseData(string filePath)
        {
            List<ReversalInfo>? info = new();

            if (filePath.Split('.')[^1].ToLower() != "csv")
                throw new ArgumentException($"Non csv file type passed in. Undupported file type with extension {filePath.Split(',')[^1]}.");

            try
            {
                using (StreamReader reader = new StreamReader(filePath))
                {
                    string? line1 = null;
                    string? line2 = null;

                    float currentOpen;
                    float currentClose;

                    float previouseClose;
                    float previouseHigh;
                    float previouseLow;

                    reader.ReadLine();

                    while ((line2 = reader.ReadLine()) != null)
                    {
                        if (line1 != null)
                        {
                            string[] line1Split = line1.Split(',');
                            string[] line2Split = line2.Split(',');

                            if (!float.TryParse(line1Split[1], out currentOpen))
                            {
                                currentOpen = (float)int.Parse(line1Split[1]);
                            }
                            if (!float.TryParse(line1Split[4], out currentClose))
                            {
                                currentClose = (float)int.Parse(line1Split[4]);
                            }

                            if (!float.TryParse(line2Split[4], out previouseClose))
                            {
                                previouseClose = (float)int.Parse(line2Split[4]);
                            }
                            if (!float.TryParse(line2Split[2], out previouseHigh))
                            {
                                previouseHigh = (float)int.Parse(line2Split[2]);
                            }
                            if (!float.TryParse(line2Split[3], out previouseLow))
                            {
                                previouseLow = (float)int.Parse(line2Split[3]);
                            }

                            if ((currentOpen < previouseClose) &&
                               (currentClose > previouseHigh))
                            {
                                ReversalInfo rev = new();

                                if (!DateTime.TryParse(line1Split[0], new CultureInfo("en-US"), DateTimeStyles.None, out rev.reversalDate))
                                    throw new ArgumentException($"Unable to parse reversal time stamp.({line1Split[0]})");

                                rev.reversalUp = true;

                                info.Add(rev);
                            }
                            else if ((currentOpen > previouseHigh) &&
                                (currentClose < previouseLow))
                            {
                                ReversalInfo rev = new();

                                if (!DateTime.TryParse(line1Split[0], new CultureInfo("en-US"), DateTimeStyles.None, out rev.reversalDate))
                                    throw new ArgumentException($"Unable to parse reversal time stamp.({line1Split[0]})");

                                rev.reversalUp = false;

                                info.Add(rev);
                            }
                        }

                        line1 = line2;
                    }
                }
            }
            catch (IOException ioe)
            {
                throw ioe;
            }
            catch (ArgumentException arge)
            {
                throw arge;
            }
            catch(FormatException fe)
            {
                throw fe;
            }

            if (info.Count == 0)
                info = null;

            return info;
        }
    }
}
