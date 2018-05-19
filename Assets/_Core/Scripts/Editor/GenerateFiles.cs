using UnityEngine;
using UnityEditor;
using System.IO;
using static System.Environment;

namespace Randolph.Core {
    public class GenerateFiles {

        /// <summary>Generates an enum file from given parameters.</summary>
        /// <param name="enumName">Name of the enum.</param>
        /// <param name="path">Path to the target folder from the project directory, e.g. "Assets/Scripts/Enums" </param>
        /// <param name="enumEntries">All of the enum fields.</param>
        /// <param name="namespace">Optionally, enclose the file in a namespace.</param>
        public static void GenerateEnum(string enumName, string path, string[] enumEntries, string @namespace = "") {
            if (path.EndsWith("/")) path = path.Remove(path.Length - 1); // Remove last '/'
            if (!AssetDatabase.IsValidFolder(path)) {
                Debug.LogError($"Trying to create a file in folder <b>{path}</b>, which doesn't exist.");
                return;
            }

            string filePath = $"{path}/{enumName}.cs";
            using (var writer = new StreamWriter(filePath)) {
                if (@namespace != string.Empty) writer.WriteLine($"namespace {@namespace} {{");
                writer.WriteLine($"\tpublic enum {enumName} {{{NewLine}{NewLine}\t\t// Autogenerated{NewLine}");
                foreach (string entry in enumEntries) {
                    writer.WriteLine($"\t\t{entry},");
                }
                writer.WriteLine($"{NewLine}\t}}");
                if (@namespace != string.Empty) writer.WriteLine("}");
            }

            AssetDatabase.Refresh();
        }

    }
}