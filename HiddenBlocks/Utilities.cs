using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace HiddenBlocks
{
    class Utilities
    {
        public static void Log(string msg)
        {
            Console.WriteLine($"[{Plugin.Instance.Name}] {msg}");
        }
    }
}
