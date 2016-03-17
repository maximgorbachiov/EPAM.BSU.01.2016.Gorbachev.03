using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace PolynomLib
{
    public class Polynom
    {
        private int[] coefficients;
        private int[] degrees;

        private delegate int Calculate(int a);

        private static Calculate Sum = (a) => a;
        private static Calculate Deduct = (a) => a * (-1);

        public Polynom(int[] userCoefficients, int[] userDegrees)
        {
            List<int> tempPolynomialCoefficients = new List<int>();
            List<int> tempPolynomialDegrees = new List<int>();

            if ((userCoefficients == null) || (userDegrees == null))
            {
                throw new Exception("Received params are null");
            }

            for (int i = 0; i < userCoefficients.Length; i++)
            {
                if (userCoefficients[i] != 0)
                {
                    tempPolynomialCoefficients.Add(userCoefficients[i]);
                    tempPolynomialDegrees.Add(userDegrees[i]);
                }
            }

            coefficients = tempPolynomialCoefficients.ToArray();
            degrees = tempPolynomialDegrees.ToArray();
            Sort();
        }

        public double CalculatePolynomial(double x)
        {
            double result = 0.0;

            for (int i = 0; i < coefficients.Length; i++)
            {
                result += coefficients[i] * Math.Pow(x, degrees[i]);
            }

            return result;
        }

        //Operators
        #region
        public static Polynom operator +(Polynom polynomial1, Polynom polynomial2)
        {
            return CalculatePolynomials(polynomial1, polynomial2, Sum);
        }

        public static Polynom operator -(Polynom polynomial1, Polynom polynomial2)
        {
            return CalculatePolynomials(polynomial1, polynomial2, Deduct);
        }

        public static Polynom operator *(Polynom polynomial1, Polynom polynomial2)
        {
            int length1 = polynomial1.coefficients.Length, length2 = polynomial2.coefficients.Length;

            Polynom[] polynomials = (length1 < length2)
                ? CreateSummandsPolynomials(polynomial1, polynomial2)
                : CreateSummandsPolynomials(polynomial2, polynomial1);

            Polynom resPolynomial = polynomials[0];

            for (int i = 1; i < polynomials.Length; i++)
            {
                resPolynomial += polynomials[i];
            }

            return resPolynomial;
        }
        #endregion

        //Object's override methods
        #region
        public override string ToString()
        {
            int i;
            string polynomialView = "";

            for (i = 0; i < coefficients.Length - 1; i++)
            {
                polynomialView += (degrees[i] != 0) ? coefficients[i] + "*x^" + degrees[i] + " + " : coefficients[i] + " + ";
            }
            polynomialView += coefficients[i] + "*x^" + degrees[i];

            return polynomialView;
        }

        public override bool Equals(object obj)
        {
            Polynom polynomial = (Polynom)obj;

            if (coefficients.Length != polynomial.coefficients.Length)
            {
                return false;
            }

            for (int i = 0; i < coefficients.Length; i++)
            {
                if ((coefficients[i] != polynomial.coefficients[i]) || (degrees[i] != polynomial.degrees[i]))
                {
                    return false;
                }
            }

            return true;
        }

        public override int GetHashCode()
        {
            int result = 0;

            for (int i = 0; i < coefficients.Length; i++)
            {
                result += coefficients[i] + degrees[i];
            }

            return result;
        }

        #endregion

        private void Sort()
        {
            for (int i = 0; i < degrees.Length - 1; i++)
            {
                for (int j = i; j < degrees.Length; j++)
                {
                    if (degrees[i] > degrees[j])
                    {
                        Swap(ref degrees[i], ref degrees[j]);
                        Swap(ref coefficients[i], ref coefficients[j]);
                    }
                }
            }
        }

        private void Swap(ref int a, ref int b)
        {
            int temp = a;
            a = b;
            b = temp;
        }

        private static Polynom CalculatePolynomials(Polynom pol1, Polynom pol2, Calculate calculate)
        {
            Polynom tempPolynomial1 = new Polynom(pol1.coefficients, pol1.degrees);
            Polynom tempPolynomial2 = new Polynom(pol2.coefficients, pol2.degrees);
            List<int> tempCoefficients = new List<int>();
            List<int> tempDegrees = new List<int>();

            InitializeCoefficientsAndDegrees(pol1, ref tempPolynomial1);
            InitializeCoefficientsAndDegrees(pol2, ref tempPolynomial2);

            for (int i = 0; i < tempPolynomial1.coefficients.Length; i++)
            {
                bool isElementAdd = false;
                for (int j = 0; j < tempPolynomial2.coefficients.Length; j++)
                {
                    if (tempPolynomial1.degrees[i] == tempPolynomial2.degrees[j])
                    {
                        tempCoefficients.Add(tempPolynomial1.coefficients[i] + calculate(tempPolynomial2.coefficients[j]));
                        tempDegrees.Add(tempPolynomial1.degrees[i]);
                        tempPolynomial2.coefficients[j] = 0;
                        isElementAdd = true;
                        break;
                    }
                }
                if (!isElementAdd)
                {
                    tempCoefficients.Add(tempPolynomial1.coefficients[i]);
                    tempDegrees.Add(tempPolynomial1.degrees[i]);
                }
            }

            AddRemainingElements(tempPolynomial2, tempCoefficients, tempDegrees, calculate);

            return new Polynom(tempCoefficients.ToArray(), tempDegrees.ToArray());
        }

        private static void AddRemainingElements(Polynom polynomial, List<int> tempCoefficients, List<int> tempDegrees, Calculate calculate)
        {
            for (int i = 0; i < polynomial.coefficients.Length; i++)
            {
                if (polynomial.coefficients[i] != 0)
                {
                    tempCoefficients.Add(calculate(polynomial.coefficients[i]));
                    tempDegrees.Add(polynomial.degrees[i]);
                }
            }
        }

        private static void InitializeCoefficientsAndDegrees(Polynom polynomial, ref Polynom tempPolynomial)
        {
            polynomial.coefficients.CopyTo(tempPolynomial.coefficients, 0);
            polynomial.degrees.CopyTo(tempPolynomial.degrees, 0);
        }

        private static Polynom[] CreateSummandsPolynomials(Polynom polynomial1, Polynom polynomial2)
        {
            Polynom[] polynomials = new Polynom[polynomial1.coefficients.Length];

            for (int i = 0; i < polynomial1.coefficients.Length; i++)
            {
                int[] tempCoefficients = new int[polynomial2.coefficients.Length];
                int[] tempDegrees = new int[polynomial2.coefficients.Length];

                for (int j = 0; j < polynomial2.coefficients.Length; j++)
                {
                    tempCoefficients[j] = polynomial1.coefficients[i] * polynomial2.coefficients[j];
                    tempDegrees[j] = polynomial1.degrees[i] + polynomial2.degrees[j];
                }
                polynomials[i] = new Polynom(tempCoefficients, tempDegrees);
            }

            return polynomials;
        }
    }
}
