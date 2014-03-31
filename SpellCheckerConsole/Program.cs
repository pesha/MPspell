using MPSpell;
using MPSpell.Dictionary;
using MPSpell.Dictionary.Parsers;
using MPSpell.Check;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpellCheckerConsole
{
    class Program
    {
        static void Main(string[] args)
        {

            DictionaryBuilder builder = new DictionaryBuilder(
                new DefaultDictionaryFileParser(),
                new DefaultAffixFileParser()
            );

            //var dictionary = builder.BuildDictionary("dictionaries/en_US/en_US.dic", "dictionaries/en_US/en_US.aff");
            var dictionary = builder.BuildDictionary("dictionaries/cs_CZ/cs_CZ.dic", "dictionaries/cs_CZ/cs_CZ.aff");


            FileChecker checker = new FileChecker(dictionary);
            var output = checker.CheckFile("test.txt");


        }



    }
}
