using Xunit;
using TraefikCone;
using System.Collections.Generic;

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

		[Fact]
		public void IngressCreation()
		{
			IngressGenerator access = new IngressGenerator();
			List<string> ingress = access.CreateIngress("test");

			Assert.Equal(ingress[0], "apiVersion: extensions/v1beta1");
			Assert.Equal(ingress[1], "kind: Ingress");
			Assert.Equal(ingress[2], "metadata:");
			Assert.Equal(ingress[3], "  name: test-ing");
			Assert.Equal(ingress[4], "spec:");
			Assert.Equal(ingress[5], "  rules:");
			Assert.Equal(ingress[6], "  - host: test.local");
			Assert.Equal(ingress[7], "    http:");
			Assert.Equal(ingress[8], "      paths:");
			Assert.Equal(ingress[9], "      - path: /");
			Assert.Equal(ingress[10], "        backend:");
			Assert.Equal(ingress[11], "          serviceName: test-svc");
			Assert.Equal(ingress[12], "          servicePort: 80");
		}
    }
}












