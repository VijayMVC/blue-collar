﻿//-----------------------------------------------------------------------
// <copyright file="UtilityTests.cs" company="Tasty Codes">
//     Copyright (c) 2011 Chad Burggraf.
// </copyright>
//-----------------------------------------------------------------------

namespace BlueCollar.Test
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Xml;
    using BlueCollar;
    using BlueCollar.Dashboard;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    /// <summary>
    /// Utility tests.
    /// </summary>
    [TestClass]
    public sealed class UtilityTests
    {
        private TestContext testContext;

        /// <summary>
        /// Gets or sets the current test context.
        /// </summary>
        public TestContext TestContext
        {
            get { return this.testContext; }
            set { this.testContext = value; }
        }

        /// <summary>
        /// Dictionary sequence equal tests.
        /// </summary>
        [TestMethod]
        public void UtilityDictionarySequenceEqual()
        {
            Assert.IsTrue(Extensions.DictionarySequenceEqual<string, string>(null, null));
            Assert.IsTrue(Extensions.DictionarySequenceEqual(null, new Dictionary<string, string>()));
            Assert.IsTrue(Extensions.DictionarySequenceEqual(new Dictionary<string, string>(), null));
            Assert.IsTrue(Extensions.DictionarySequenceEqual(new Dictionary<string, string>(), new Dictionary<string, string>()));

            Dictionary<string, string> left = new Dictionary<string, string>();
            Dictionary<string, string> right = new Dictionary<string, string>();

            left["A"] = "1";
            Assert.IsFalse(Extensions.DictionarySequenceEqual(left, right));

            right["A"] = "1";
            Assert.IsTrue(Extensions.DictionarySequenceEqual(left, right));

            left["A"] = "2";
            Assert.IsFalse(Extensions.DictionarySequenceEqual(left, right));

            left["A"] = "1";
            right["B"] = "2";
            Assert.IsFalse(Extensions.DictionarySequenceEqual(left, right));

            left["B"] = "3";
            Assert.IsFalse(Extensions.DictionarySequenceEqual(left, right));

            left["B"] = "2";
            Assert.IsTrue(Extensions.DictionarySequenceEqual(left, right));
        }

        /// <summary>
        /// Index to XML tests.
        /// </summary>
        [TestMethod]
        public void UtilityIndexToXml()
        {
            Index index = new Index();
            var xml = index.ToXml() as XmlDocument;
            Assert.IsNotNull(xml);

            var xmls = xml.InnerXml;
            Assert.IsTrue(xmls.StartsWith(@"<?xml version=""1.0"" encoding=""utf-16""?><Index", StringComparison.Ordinal));
            Assert.IsTrue(xmls.Contains("<Version>" + GetType().Assembly.GetName().Version.ToString(2) + "</Version>"));
        }

        /// <summary>
        /// Index transform tests.
        /// </summary>
        [TestMethod]
        public void UtilityIndexTransform()
        {
            Index index = new Index();
            string html = index.Transform();
            Assert.IsFalse(string.IsNullOrEmpty(html));
        }

        /// <summary>
        /// Invoke with timeout tests.
        /// </summary>
        [TestMethod]
        public void UtilityInvokeWithTimeout()
        {
            try
            {
                new Action(
                    () =>
                    {
                        Thread.Sleep(700);
                    }).InvokeWithTimeout(1000);
            }
            catch (TimeoutException)
            {
                Assert.Fail();
            }

            try
            {
                new Action(
                    () =>
                    {
                        Thread.Sleep(1000);
                        Assert.Fail();
                    }).InvokeWithTimeout(700);

                Assert.Fail();
            }
            catch (TimeoutException)
            {
            }
        }

        /// <summary>
        /// Queue name filters parse tests.
        /// </summary>
        [TestMethod]
        public void UtilityQueueNameFiltersParse()
        {
            QueueNameFilters filters = QueueNameFilters.Parse("not:scheduled");
            Assert.IsFalse(filters.IsEmpty);
            Assert.IsFalse(filters.IncludesAllQueues);
            Assert.AreEqual(0, filters.Include.Count);
            Assert.AreEqual(1, filters.Exclude.Count);

            filters = QueueNameFilters.Parse("*\nnot:scheduled");
            Assert.IsFalse(filters.IsEmpty);
            Assert.AreEqual(0, filters.Include.Count);
            Assert.AreEqual(1, filters.Exclude.Count);
            
            filters = QueueNameFilters.Parse("one\ntwo\nthree\nnot:four\nnot:five\nnot:six\nseven");
            Assert.IsFalse(filters.IsEmpty);
            Assert.IsFalse(filters.IncludesAllQueues);
            Assert.IsTrue(new string[] { "one", "seven", "three", "two" }.SequenceEqual(filters.Include));
            Assert.IsTrue(new string[] { "five", "four", "six" }.SequenceEqual(filters.Exclude));

            filters = QueueNameFilters.Any();
            Assert.IsFalse(filters.IsEmpty);
            Assert.IsTrue(filters.IncludesAllQueues);
            Assert.AreEqual(1, filters.Include.Count);
            Assert.AreEqual(0, filters.Exclude.Count);
        }

        /// <summary>
        /// Randomize tests.
        /// </summary>
        [TestMethod]
        public void UtilityRandomize()
        {
            List<long> values = new List<long>();

            for (int i = 0; i < 25; i++)
            {
                long value = Extensions.Randomize(5000);
                values.Add(value);
                this.TestContext.WriteLine("{0}", value);
            }

            Assert.AreNotEqual(values.Count, values.Where(v => v == 5000).Count());
            Assert.IsTrue(values.Min() >= 4500);
            Assert.IsTrue(values.Max() <= 5500);
        }

        /// <summary>
        /// Static file tests.
        /// </summary>
        [TestMethod]
        public void UtilityStaticFile()
        {
            StaticFile file = StaticFile.Create("~/collar.ashx", "js/collar.js");
            Assert.IsNotNull(file);
            Assert.IsFalse(string.IsNullOrEmpty(file.Hash));
            Assert.IsTrue(file.Url.StartsWith("/collar.ashx", StringComparison.OrdinalIgnoreCase));
            Assert.AreEqual("text/javascript", file.ContentType);

            file = StaticFile.Create("~/collar.ashx", "img/logo-header.png");
            Assert.IsNotNull(file);
            Assert.AreEqual("image/png", file.ContentType);

            file = StaticFile.Create("~/collar.ashx", "index.xslt");
            Assert.IsNotNull(file);
            Assert.AreEqual("text/html", file.ContentType);
        }
    }
}
