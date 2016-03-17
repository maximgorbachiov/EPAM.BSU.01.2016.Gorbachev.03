using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using PolynomLib;   

namespace Polynomial.NUnit.Test
{
    [TestFixture]
    public class PolynomialTest
    {
        [TestCase(new int[] { 2, 4, 6 }, new int[] { 0, 1, 2 }, Result = "2 + 4*x^1 + 6*x^2")]
        [TestCase(new int[] { 2, 4, 6 }, new int[] { 0, 10, 2 }, Result = "2 + 6*x^2 + 4*x^10")]
        [TestCase(new int[] { 2, 4, 6, 8 }, new int[] { 0, 1, 2, 5 }, Result = "2 + 4*x^1 + 6*x^2 + 8*x^5")]
        [TestCase(new int[] { 2, -4, 6 }, new int[] { 0, 1, 6 }, Result = "2 + -4*x^1 + 6*x^6")]
        [TestCase(new int[] { -2, 4, -6, 10, 23 }, new int[] { 8, 1, 2, 4, 3 }, Result = "4*x^1 + -6*x^2 + 23*x^3 + 10*x^4 + -2*x^8")]
        public string TestCreationOfPolynomial(int[] coefficients, int[] degrees)
        {
            Polynom polynom = new Polynom(coefficients, degrees);

            return polynom.ToString();
        }

        [TestCase(new int[] { 2, 4, 6 }, new int[] { 0, 1, 2 }, new int[] { 2, 4, 6 }, new int[] { 0, 10, 2 }, Result = "4 + 4*x^1 + 12*x^2 + 4*x^10")]
        [TestCase(new int[] { 2, -4, 6 }, new int[] { 0, 1, 6 }, new int[] { 2, 4, 6 }, new int[] { 0, 10, 2 }, Result = "4 + -4*x^1 + 6*x^2 + 6*x^6 + 4*x^10")]
        public string TestSumOperator(int[] coeff1, int[] degrees1, int[] coeff2, int[] degrees2)
        {
            Polynom pol1 = new Polynom(coeff1, degrees1);
            Polynom pol2 = new Polynom(coeff2, degrees2);
            Polynom pol3 = pol1 + pol2;

            return pol3.ToString();
        }

        [TestCase(new int[] { 2, 4, 6 }, new int[] { 0, 1, 2 }, new int[] { 2, 4, 6 }, new int[] { 0, 10, 2 }, Result = "4*x^1 + -4*x^10")]
        [TestCase(new int[] { 2, -4, 6 }, new int[] { 0, 1, 6 }, new int[] { 2, 4, 6 }, new int[] { 0, 10, 2 }, Result = "-4*x^1 + -6*x^2 + 6*x^6 + -4*x^10")]
        public string TestMinusOperator(int[] coeff1, int[] degrees1, int[] coeff2, int[] degrees2)
        {
            Polynom pol1 = new Polynom(coeff1, degrees1);
            Polynom pol2 = new Polynom(coeff2, degrees2);
            Polynom pol3 = pol1 - pol2;

            return pol3.ToString();
        }

        [TestCase(new int[] { 2, 4, 6 }, new int[] { 0, 1, 2 }, new int[] { 2, 4, 6 }, new int[] { 0, 10, 2 }, Result = "4 + 8*x^1 + 24*x^2 + 24*x^3 + 36*x^4 + 8*x^10 + 16*x^11 + 24*x^12")]
        [TestCase(new int[] { 2, -4, 6 }, new int[] { 0, 1, 6 }, new int[] { 2, 4, 6 }, new int[] { 0, 10, 2 }, Result = "4 + -8*x^1 + 12*x^2 + -24*x^3 + 12*x^6 + 36*x^8 + 8*x^10 + -16*x^11 + 24*x^16")]
        public string TestMultiplyOperator(int[] coeff1, int[] degrees1, int[] coeff2, int[] degrees2)
        {
            Polynom pol1 = new Polynom(coeff1, degrees1);
            Polynom pol2 = new Polynom(coeff2, degrees2);
            Polynom pol3 = pol1 * pol2;

            return pol3.ToString();
        }
    }
}
