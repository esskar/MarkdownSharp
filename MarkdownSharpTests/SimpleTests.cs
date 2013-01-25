using MarkdownSharp;
using NUnit.Framework;

namespace MarkdownSharpTests
{
    [TestFixture]
    public class SimpleTests : BaseTest
    {
        private readonly Markdown _target = new Markdown();

        [Test]
        public void Bold()
        {
            const string input = "This is **bold**. This is also __bold__.";
            const string expected = "<p>This is <strong>bold</strong>. This is also <strong>bold</strong>.</p>\n";

            var actual = _target.Transform(input);

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void Italic()
        {
            const string input = "This is *italic*. This is also _italic_.";
            const string expected = "<p>This is <em>italic</em>. This is also <em>italic</em>.</p>\n";

            var actual = _target.Transform(input);

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void Link()
        {
            const string input = "This is [a link][1].\n\n  [1]: http://www.example.com";
            const string expected = "<p>This is <a href=\"http://www.example.com\">a link</a>.</p>\n";

            var actual = _target.Transform(input);

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void LinkBracket()
        {
            const string input = "Have you visited <http://www.example.com> before?";
            const string expected = "<p>Have you visited <a href=\"http://www.example.com\">http://www.example.com</a> before?</p>\n";

            var actual = _target.Transform(input);

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void LinkBareWithoutAutoHyperLink()
        {
            const string input = "Have you visited http://www.example.com before?";
            const string expected = "<p>Have you visited http://www.example.com before?</p>\n";

            var actual = _target.Transform(input);

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void LinkBareWithAutoHyperLink()
        {
            var autoHyperLink = _target.AutoHyperlink;
            try
            {
                _target.AutoHyperlink = true;

                const string input = "Have you visited http://www.example.com before?";
                const string expected =
                    "<p>Have you visited <a href=\"http://www.example.com\">http://www.example.com</a> before?</p>\n";

                var actual = _target.Transform(input);

                Assert.AreEqual(expected, actual);
            }
            finally
            {
                _target.AutoHyperlink = autoHyperLink;
            }
        }

        [Test]
        public void LinkAlt()
        {
            const string input = "Have you visited [example](http://www.example.com) before?";
            const string expected = "<p>Have you visited <a href=\"http://www.example.com\">example</a> before?</p>\n";

            var actual = _target.Transform(input);

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void Image()
        {
            const string input = "An image goes here: ![alt text][1]\n\n  [1]: http://www.google.com/intl/en_ALL/images/logo.gif";
            const string expected = "<p>An image goes here: <img src=\"http://www.google.com/intl/en_ALL/images/logo.gif\" alt=\"alt text\" /></p>\n";

            var actual = _target.Transform(input);

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void ArticleEnabled()
        {
            var supportArticles = _target.SupportArticles;
            try
            {
                _target.SupportArticles = true;

                const string input = "Here is an article\n\n$ Hello sir!\n";
                const string expected = "<p>Here is an article</p>\n\n<article>\n<p>Hello sir!</p>\n</article>\n";

                var actual = _target.Transform(input);
                Assert.AreEqual(expected, actual);
                Assert.AreEqual(1, _target.ArticleCount, "ArticelCount mismatch");
            }
            finally
            {
                _target.SupportArticles = supportArticles;
            }            
        }

        [Test]
        public void ArticleDisabled()
        {
            var supportArticles = _target.SupportArticles;
            try
            {
                _target.SupportArticles = false;

                const string input = "Here is an article\n\n$ Hello sir!\n";
                const string expected = "<p>Here is an article</p>\n\n<p>$ Hello sir!</p>\n";

                var actual = _target.Transform(input);

                Assert.AreEqual(expected, actual);
                Assert.AreEqual(0, _target.ArticleCount, "ArticelCount mismatch");
            }
            finally
            {
                _target.SupportArticles = supportArticles;
            }
        }

        [Test]
        public void Blockquote()
        {
            const string input = "Here is a quote\n\n> Sample blockquote\n";
            const string expected = "<p>Here is a quote</p>\n\n<blockquote>\n  <p>Sample blockquote</p>\n</blockquote>\n";

            var actual = _target.Transform(input);

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void NumberList()
        {
            const string input = "A numbered list:\n\n1. a\n2. b\n3. c\n";
            const string expected = "<p>A numbered list:</p>\n\n<ol>\n<li>a</li>\n<li>b</li>\n<li>c</li>\n</ol>\n";

            var actual = _target.Transform(input);

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void BulletList()
        {
            const string input = "A bulleted list:\n\n- a\n- b\n- c\n";
            const string expected = "<p>A bulleted list:</p>\n\n<ul>\n<li>a</li>\n<li>b</li>\n<li>c</li>\n</ul>\n";

            var actual = _target.Transform(input);

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void Header1()
        {
            const string input = "#Header 1\nHeader 1\n========";
            const string expected = "<h1>Header 1</h1>\n\n<h1>Header 1</h1>\n";

            var actual = _target.Transform(input);

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void Header2()
        {
            const string input = "##Header 2\nHeader 2\n--------";
            const string expected = "<h2>Header 2</h2>\n\n<h2>Header 2</h2>\n";

            var actual = _target.Transform(input);

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void CodeBlock()
        {
            const string input = "code sample:\n\n    <head>\n    <title>page title</title>\n    </head>\n";
            const string expected = "<p>code sample:</p>\n\n<pre><code>&lt;head&gt;\n&lt;title&gt;page title&lt;/title&gt;\n&lt;/head&gt;\n</code></pre>\n";

            string actual = _target.Transform(input);

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void CodeSpan()
        {
            const string input = "HTML contains the `<blink>` tag";
            const string expected = "<p>HTML contains the <code>&lt;blink&gt;</code> tag</p>\n";

            var actual = _target.Transform(input);

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void HtmlPassthrough()
        {
            const string input = "<div>\nHello World!\n</div>\n";
            const string expected = "<div>\nHello World!\n</div>\n";

            var actual = _target.Transform(input);

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void Escaping()
        {
            const string input = @"\`foo\`";
            const string expected = "<p>`foo`</p>\n";

            var actual = _target.Transform(input);

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void HorizontalRule()
        {
            const string input = "* * *\n\n***\n\n*****\n\n- - -\n\n---------------------------------------\n\n";
            const string expected = "<hr />\n\n<hr />\n\n<hr />\n\n<hr />\n\n<hr />\n";

            var actual = _target.Transform(input);

            Assert.AreEqual(expected, actual);
        }
    }
}
