using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MPSpell;
using System.Collections.Generic;
using MPSpell.Dictionaries.Affixes;
using MPSpell.Extensions;
using MPSpell.Check;
using MPSpell.Dictionaries;
using MPSpell.Tools.ErrorModel;

namespace MPSpellTests
{
    [TestClass]
    public class MatrixGeneratorTest
    {

        Dictionary<string, List<string>> testData;
        char[] alphabetWithSpace;
        char[] alphabet;

        public MatrixGeneratorTest()
        {
            DictionaryManager manager = new DictionaryManager(@"C:\dev\git\Pspell\SpellCheckerConsole\bin\Debug\dictionaries");
            Dictionary enUs = manager.GetDictionary("en_US");
            this.alphabetWithSpace = enUs.GetAlphabetForErrorModel(true).ToCharArray();
            this.alphabet = enUs.GetAlphabetForErrorModel().ToCharArray();
            Array.Sort<char>(this.alphabetWithSpace);
            Array.Sort<char>(this.alphabet);

            ErrorListParser parser = new ErrorListParser("test_errors.txt");
            testData = parser.Parse();
        }

        [TestMethod]
        public void ParseTest()
        {
            Assert.AreEqual(6, testData.Count);
            Assert.AreEqual(1, testData["cress"].Count);

            char[] expectedAlphabet = " abcdefghijklmnopqrstuvwxyz'".ToCharArray();
            Array.Sort<char>(expectedAlphabet);
            CollectionAssert.AreEqual(expectedAlphabet, this.alphabetWithSpace);
        }

        [TestMethod]
        public void InsertionsMatrixTest()
        {
            InsertionsMatrixGenerator generator = new InsertionsMatrixGenerator(alphabetWithSpace);
            var matrix = generator.GenerateMatrix(this.testData);

            Assert.AreEqual(1, matrix[' ']['a']);
            Assert.AreEqual(1, matrix['e']['s']);
            Assert.AreEqual(1, matrix['s']['s']);
            
            // other random field
            Assert.AreEqual(0, matrix['c']['d']);
        }

        [TestMethod]
        public void DeletionsMatrixTest()
        {
            DeletionsMatrixGenerator generator = new DeletionsMatrixGenerator(alphabetWithSpace);
            var matrix = generator.GenerateMatrix(this.testData);

            Assert.AreEqual(1, matrix['c']['t']);

            // other random field
            Assert.AreEqual(0, matrix['d']['t']);
        }

        [TestMethod]
        public void TranspositionsMatrixTest()
        {
            TranspositionsMatrixGenerator generator = new TranspositionsMatrixGenerator(alphabet);
            var matrix = generator.GenerateMatrix(this.testData);

            Assert.AreEqual(1, matrix['c']['a']);
            Assert.IsFalse(matrix.ContainsKey(' '));
        }

        [TestMethod]
        public void SubstitutionsMatrixTest()
        {
            SubstitutionsMatrixGenerator generator = new SubstitutionsMatrixGenerator(alphabet);
            var matrix = generator.GenerateMatrix(this.testData);

            Assert.AreEqual(1, matrix['r']['c']);
            Assert.AreEqual(1, matrix['e']['o']);
            Assert.IsFalse(matrix.ContainsKey(' '));
        }

    }
}
