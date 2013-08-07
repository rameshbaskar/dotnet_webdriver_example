using System.Collections;
using NUnit.Framework;
using NUnit.Framework.Constraints;

namespace Common.Utils
{
    public class Verify
    {
        public static void AreEqual(object expected, object actual, string message, bool @continue = true)
        {
            try
            {
                Assert.AreEqual(expected, actual, message);
            }
            catch (AssertionException ae)
            {
                TestFailure.HandleAssertionException(ae, @continue);
            }
        }

        public static void That(object actual, IResolveConstraint expected, string message, bool @continue = true)
        {
            try
            {
                Assert.That(actual, expected, message);
            }
            catch (AssertionException ae)
            {
                TestFailure.HandleAssertionException(ae, @continue);
            }
        }

        public static void That(IEnumerable expectedCollection, IEnumerable actualCollection, string message, bool @continue = true)
        {
            try
            {
                CollectionAssert.AreEquivalent(expectedCollection, actualCollection);
            }
            catch (AssertionException ae)
            {
                TestFailure.HandleAssertionException(ae, @continue);
            }
        }

        public static void IsTrue(bool condition, string message, bool @continue = true)
        {
            try
            {
                Assert.IsTrue(condition, message);
            }
            catch (AssertionException ae)
            {
                TestFailure.HandleAssertionException(ae, @continue);
            }
        }

        public static void Fail(string message)
        {
            Assert.Fail(message);
        }
    }
}