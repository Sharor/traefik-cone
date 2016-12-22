using System;
using Xunit;
using TraefikCone; 

namespace Tests
{
    public class Tests
    {
        [Fact]
        public void IndentCheck() 
        {
			IngressGenerator access = new IngressGenerator();
			string testIndent = access.FixIndention(0, "hello!");
			string testIndentOne = access.FixIndention(1, "hello!");
			string testIndentTwo = access.FixIndention(2, "hello!");

			Assert.Equal("hello!", testIndent);
			Assert.Equal("  hello!", testIndentOne);
			Assert.Equal("    hello!", testIndentTwo);
		}
    }
}
