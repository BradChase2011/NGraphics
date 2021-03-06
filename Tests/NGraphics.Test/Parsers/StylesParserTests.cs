﻿using System.Collections.Generic;
using System.Xml.Linq;
using FluentAssertions;
using NGraphics.Custom.Codes;
using NGraphics.Custom.Models;
using NGraphics.Custom.Models.Brushes;
using NGraphics.Custom.Parsers;
using NUnit.Framework;

namespace NGraphics.Test.Parsers
{
    [TestFixture]
    public class StylesParserTests
    {
        private StylesParser _stylesParser;

        [SetUp]
        public void Setup()
        {
            _stylesParser = new StylesParser(new ValuesParser());
        }

        [Test]
        public void GetBrush_ShouldReturnNull_IfNoStyleAttributesExist()
        {
            var styleAttributes = new Dictionary<string, string>();

            var brush = _stylesParser.GetBrush(styleAttributes, new Dictionary<string, XElement>(), null);

            brush.Should().BeNull();
        }

        [Test]
        public void GetBrush_ShouldReturnDefaultBrush_IfFillRuleIsDefault()
        {
            var styleAttributes = new Dictionary<string, string> {{"fill-rule", "nonzero"}};

            var expectedBrush = new SolidBrush
            {
                FillMode = FillMode.NonZero
            };

            var actualBrush = _stylesParser.GetBrush(styleAttributes, new Dictionary<string, XElement>(), null);

            actualBrush.Should().NotBeNull();
            ((SolidBrush) actualBrush).ShouldBeEquivalentTo(expectedBrush);
        }

        [Test]
        public void GetBrush_ShouldReturnBrushWithOpacity()
        {
            var styleAttributes = new Dictionary<string, string> {{"fill-opacity", "0.2"}};
            var expectedBrush = new SolidBrush(new Color(0, 0, 0, 0.2));

            var actualBrush = _stylesParser.GetBrush(styleAttributes, new Dictionary<string, XElement>(), null);

            actualBrush.Should().NotBeNull();
            ((SolidBrush) actualBrush).ShouldBeEquivalentTo(expectedBrush);
        }

        [Test]
        public void GetBrush_ShouldReturnBrushWithFillModeEvenOdd()
        {
            var styleAttributes = new Dictionary<string, string> {{"fill-rule", "evenodd"}};
            var expectedBrush = new SolidBrush
            {
                FillMode = FillMode.EvenOdd
            };

            var actualBrush = _stylesParser.GetBrush(styleAttributes, new Dictionary<string, XElement>(), null);

            actualBrush.Should().NotBeNull();
            ((SolidBrush) actualBrush).ShouldBeEquivalentTo(expectedBrush);
        }

        [Test]
        public void GetBrush_ShouldReturnNull_IfFillAttributeExistsButValueIsNull()
        {
            var styleAttributes = new Dictionary<string, string> {{"fill", ""}};

            var actualBrush = _stylesParser.GetBrush(styleAttributes, new Dictionary<string, XElement>(), null);

            actualBrush.Should().BeNull();
        }

        [Test]
        public void GetBrush_ShouldReturnNull_IfFillValueIsNone()
        {
            var styleAttributes = new Dictionary<string, string> {{"fill", "none"}};

            var actualBrush = _stylesParser.GetBrush(styleAttributes, new Dictionary<string, XElement>(), null);

            actualBrush.Should().BeNull();
        }


        [Test]
        public void GetBrush_ShouldReturnBrushWithColor_IfFillHasValidColor_White()
        {
            var styleAttributes = new Dictionary<string, string> {{"fill", "#FFFFFF"}};
            var expectedBrush = new SolidBrush
            {
                Color = new Color(1, 1, 1)
            };

            var actualBrush = _stylesParser.GetBrush(styleAttributes, new Dictionary<string, XElement>(), null);

            actualBrush.Should().NotBeNull();
            ((SolidBrush) actualBrush).ShouldBeEquivalentTo(expectedBrush);
        }

        [Test]
        public void GetBrush_ShouldReturnBrushWithColor_IfFillHasValidColor_Black()
        {
            var styleAttributes = new Dictionary<string, string> {{"fill", "#000000"}};
            var expectedBrush = new SolidBrush
            {
                Color = new Color(0, 0, 0)
            };

            var actualBrush = _stylesParser.GetBrush(styleAttributes, new Dictionary<string, XElement>(), null);

            actualBrush.Should().NotBeNull();
            ((SolidBrush) actualBrush).ShouldBeEquivalentTo(expectedBrush);
        }

        [Test]
        public void ParseStyleValues_ShouldCreateADictionaryWithStyleValue()
        {
            const string styleString = "opacity:1;fill:#33ff00;fill-opacity:0;fill-rule:evenodd;stroke:#33ffff;stroke-width:3.08268809;stroke-linecap:round;stroke-linejoin:round;stroke-miterlimit:4;stroke-dasharray:none;stroke-opacity:1";

            var expectedStylesDictionary = new Dictionary<string, string>
            {
                {"opacity", "1"},
                {"fill", "#33ff00"},
                {"fill-opacity", "0"},
                {"fill-rule", "evenodd"},
                {"stroke", "#33ffff"},
                {"stroke-width", "3.08268809"},
                {"stroke-linecap", "round"},
                {"stroke-linejoin", "round"},
                {"stroke-miterlimit", "4"},
                {"stroke-dasharray", "none"},
                {"stroke-opacity", "1"},
            };


            var actualStylesDictionary = _stylesParser.ParseStyleValues(styleString);

            actualStylesDictionary.ShouldBeEquivalentTo(expectedStylesDictionary);
        }
    }
}
