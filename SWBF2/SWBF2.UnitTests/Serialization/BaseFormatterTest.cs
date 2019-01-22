using Microsoft.VisualStudio.TestTools.UnitTesting;
using SWBF2.Serialization;
using System.Collections.Generic;
using System.IO;

namespace SWBF2.UnitTests.Serialization
{
    public abstract class BaseFormatterTest<T>
    {
        private string bf2ModToolsDirectory = @"C:\BF2_ModTools\";
        private ITypedFormatter<IList<T>> formatter;
        private string fileExtension;

        public BaseFormatterTest(ITypedFormatter<IList<T>> formatter, string fileExtension)
        {
            this.formatter = formatter;
            this.fileExtension = fileExtension;
        }

        protected void DeserializeSerializeTest(string worldName)
        {
            // Get all files matching the file extension in the specified world directory
            var files = new DirectoryInfo(string.Format(@"{0}\assets\worlds\{1}\", bf2ModToolsDirectory, worldName)).GetFiles("*." + fileExtension, SearchOption.AllDirectories);

            // Create directory to hold the NEW files
            var rootDir = string.Format(@"Out\{0}\", worldName);
            Directory.CreateDirectory(rootDir);

            foreach (var file in files)
            {
                var path = file.FullName;

                // Read the original file using the deserializer
                IList<T> original = null;
                using (var fs = new FileStream(path, FileMode.Open))
                {
                    original = formatter.Deserialize(fs);
                }

                // Take the original and serialize it back out (NOT in the BF2_ModTools dir)
                var newFileName = rootDir + file.Name + ".new";
                using (var fs = new FileStream(newFileName, FileMode.Create))
                {
                    formatter.Serialize(fs, original);
                }

                // Compare
                byte[] expected = File.ReadAllBytes(path);
                byte[] actual = File.ReadAllBytes(newFileName);

                var max = expected.Length > actual.Length ? actual.Length : expected.Length;

                for (int i = 0; i < max; i++)
                {
                    Assert.AreEqual(expected[i], actual[i]);
                }

                Assert.AreEqual(expected.Length, actual.Length);
            }
        }
    }
}