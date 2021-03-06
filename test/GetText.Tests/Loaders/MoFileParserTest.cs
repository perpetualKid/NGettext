﻿using System.Collections.Generic;
using System.Text;
using System.IO;
using Xunit;
using GetText.Loaders;

namespace GetText.Tests.Loaders
{
    public class MoFileParserTest
    {
        private string localesDir;

        public MoFileParserTest()
        {
            this.localesDir = Path.Combine(Directory.GetCurrentDirectory(), Path.Combine("TestResources", "locales"));
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
        }

        [Fact]
        public void TestParsing()
        {
            using (var stream = File.OpenRead(Path.Combine(localesDir, Path.Combine("ru_RU", "Test.mo"))))
            {
                var parser = new MoFileParser();
                var parsedFile = parser.Parse(stream);
                TestLoadedTranslation(parsedFile.Translations);
            }
        }

        [Fact]
        public void TestBigEndianParsing()
        {
            using (var stream = File.OpenRead(Path.Combine(localesDir, Path.Combine("ru_RU", "Test_BigEndian.mo"))))
            {
                var parser = new MoFileParser();
                var parsedFile = parser.Parse(stream);
                TestLoadedTranslation(parsedFile.Translations);
            }
        }

        [Fact]
        public void TestAutoEncoding()
        {
            using (var stream = File.OpenRead(Path.Combine(localesDir, Path.Combine("ru_RU", "Test_KOI8-R.mo"))))
            {
                var parser = new MoFileParser();
                var parsedFile = parser.Parse(stream);
                TestLoadedTranslation(parsedFile.Translations);
            }
        }

        [Fact]
        public void TestManualEncoding()
        {
            using (var stream = File.OpenRead(Path.Combine(localesDir, Path.Combine("ru_RU", "Test_KOI8-R.mo"))))
            {
                var parser = new MoFileParser(Encoding.GetEncoding("KOI8-R"), false);
                var parsedFile = parser.Parse(stream);
                TestLoadedTranslation(parsedFile.Translations);
            }
        }

        private static void TestLoadedTranslation(IDictionary<string, string[]> dict)
        {
            Assert.Equal(new[] { "тест" }, dict["test"]);
            Assert.Equal(new[] { "тест2" }, dict["test2"]);
            Assert.Equal(new[] { "{0} минута", "{0} минуты", "{0} минут" }, dict["{0} minute"]);
            Assert.Equal(new[] { "тест3контекст1" }, dict["context1" + Catalog.CONTEXTGLUE + "test3"]);
            Assert.Equal(new[] { "тест3контекст2" }, dict["context2" + Catalog.CONTEXTGLUE + "test3"]);
        }
    }
}