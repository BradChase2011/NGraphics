﻿using System.IO;
using NGraphics.Custom;
using NGraphics.Custom.Parsers;
using NUnit.Framework;

namespace NGraphics.Test.SvgReader
{
    public class SvgReaderTestBase : PlatformTest
    {
        Graphic Read(string path)
        {
            using (var s = OpenResource(path))
            {
              var valuesParser = new ValuesParser();

              var r = new Custom.Parsers.SvgReader(new StreamReader(s),new StylesParser(valuesParser),valuesParser );
                Assert.GreaterOrEqual(r.Graphic.Children.Count, 0);
                Assert.Greater(r.Graphic.Size.Width, 1);
                Assert.Greater(r.Graphic.Size.Height, 1);
                return r.Graphic;
            }
        }

        protected void ReadAndDraw(string path)
        {
            var g = Read(path);
            var c = Platform.CreateImageCanvas(g.Size);
            g.Draw(c);
          SaveImage(c.GetImage(), path.Replace(".svg",".png"));
        }
    }
}
