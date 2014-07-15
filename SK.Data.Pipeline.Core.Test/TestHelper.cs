using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SK.Data.Pipeline.Core.Test
{
    public class TestHelper
    {
        public static bool CompareTwoFile(string expectPath, string actualPath)
        {
            string expectContent = File.ReadAllText(expectPath);
            string actualContent = File.ReadAllText(actualPath);

            return expectContent.Equals(actualContent);
        }
    }
}
