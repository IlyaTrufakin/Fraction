using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Runtime.Serialization.Formatters;
using System.Text;
using System.Threading.Tasks;

//Разработать класс Fraction, представляющий простую дробь, в классе предусмотреть два поля: числитель и знаменатель дроби.
//Выполнить перегрузку следующих операторов: +,-,*,/,==,!=,<,>, true и false.
//Арифметические действия и сравнение выполняется в соответствии с правилами работы с дробями.
//Оператор true возвращает true если дробь правильная (числитель меньше знаменателя), оператор false возвращает true если дробь неправильная (числитель больше знаменателя).
//Выполнить перегрузку операторов, необходимых для успешной компиляции следующего фрагмента кода:
//Fraction f = new Fraction(3, 4);
//int a = 10;
//Fraction f1 = f * a;
//Fraction f2 = a * f;
//double d = 1.5;
//Fraction f3 = f + d;

namespace Fraction
{

    internal class Fraction
    {
        private int numerator;
        private int denominator;
        public int Numerator
        {
            get { return numerator; }
            set { numerator = value; }
        }
        public int Denominator
        {
            get { return denominator; }
            set
            {
                if (value > 0)
                {
                    denominator = value;
                }
                else
                {
                    numerator = 0;
                    denominator = 1;
                    Console.WriteLine("Warning: значение знаменателя должно быть натуральным числом. Дробь была инициализирована значением по умолчанию: 0/1");
                }
            }
        }

        internal Fraction() : this(0) { }
       
        internal Fraction(int numerator = 0, int denominator = 1)
        {
            Numerator = numerator;
            Denominator = denominator;
            Reduce();
        }

        internal Fraction(in Fraction fraction)
        {
            Numerator = fraction.Numerator;
            Denominator = fraction.Denominator;
            Reduce();
        }

        internal Fraction(double d)
        {
            int divisor = 1;
            while (d % 1 != 0)
            {
                d *= 10;
                divisor *= 10;
            }
            int k = EvklGcd((int)d, divisor);
            Numerator = (int)(d / k);
            Denominator = divisor / k;
            Reduce();
        }

        private void Reduce()// сокращение дроби
        {
            int gcd = EvklGcd(Numerator, Denominator);
            if (gcd != 1)
            {
                Numerator /= gcd;
                Denominator /= gcd;
            }
        }

        private int EvklGcd(int a, int b) // вычисление наибольшего общего делителя
        {
            return (a == 0 ? b : EvklGcd(b % a, a));
        }


        private int Lcm(int a, int b) // вычисление наименьшего общего кратного
        {
            return Math.Abs(a * b) / EvklGcd(a, b);
        }

// ------------------------------------------------ перегрузки опреатора * -------------------------------------------------------
        public static Fraction operator *(in Fraction fraction1, in Fraction fraction2)
        {
            return new Fraction(fraction1.Numerator * fraction2.Numerator, fraction1.Denominator * fraction2.Denominator);
        }

        public static Fraction operator *(in Fraction fraction, int a)
        {
            return  fraction * new Fraction(a);
        }

        public static Fraction operator *(int a, in Fraction fraction)
        {
            return new Fraction(a) * fraction;
        }

// ------------------------------------------------ перегрузки опреатора + -------------------------------------------------------
        public static Fraction operator +(in Fraction fraction1, in Fraction fraction2)
        {
            Fraction tmp = new Fraction();
            if (fraction1.Denominator == fraction2.Denominator)
            {
                tmp.Numerator = fraction1.Numerator + fraction2.Numerator;
                tmp.Denominator = fraction1.Denominator;
            }
            else
            {
                tmp.Denominator = fraction1.Denominator * fraction2.Denominator;
                tmp.Numerator = fraction1.Numerator * fraction2.Denominator + fraction1.Denominator * fraction2.Numerator;
            }

            tmp.Reduce();
            return tmp;
        }

        public static Fraction operator +(in Fraction fraction, double d)
        {
             return (fraction + new Fraction(d));
        }
        public static Fraction operator +(in Fraction fraction, int i)
        {
            return (fraction + new Fraction(i));
        }

// ------------------------------------------------ перегрузки опреатора - -------------------------------------------------------
        public static Fraction operator -(in Fraction fraction1, in Fraction fraction2)
        {
            Fraction tmp = new Fraction();
            if (fraction1.Denominator == fraction2.Denominator)
            {
                tmp.Numerator = fraction1.Numerator - fraction2.Numerator;
                tmp.Denominator = fraction1.Denominator;
            }
            else
            {
                tmp.Denominator = fraction1.Denominator * fraction2.Denominator;
                tmp.Numerator = fraction1.Numerator * fraction2.Denominator - fraction1.Denominator * fraction2.Numerator;
            }

            tmp.Reduce();
            return tmp;
        }

        public static Fraction operator -(in Fraction fraction, double d)
        {
            return (fraction - new Fraction(d));
        }
        public static Fraction operator -(double d, in Fraction fraction)
        {
            return (new Fraction(d)- fraction);
        }
        public static Fraction operator -(int i, in Fraction fraction)
        {
            return (new Fraction(i) - fraction);
        }
        public static Fraction operator -(in Fraction fraction, int i)
        {
            return (fraction - new Fraction(i));
        }


// ------------------------------------------------ перегрузки опреатора / -------------------------------------------------------
        public static Fraction operator /(in Fraction fraction1, in Fraction fraction2)
        {
            return new Fraction(fraction1.Numerator * fraction2.Denominator, fraction1.Denominator * fraction2.Numerator);
        }

        public static Fraction operator /(in Fraction fraction, int a)
        {
            return fraction / new Fraction(a);
        }

        public static Fraction operator /(int a, in Fraction fraction)
        {
            return new Fraction(a) / fraction;
        }


// ------------------------------------------------ перегрузки опреатора == и != -------------------------------------------------------
        public static bool operator ==(in Fraction fraction1, in Fraction fraction2)
        {
            if (ReferenceEquals(fraction1, fraction2))
                return true;

            if (ReferenceEquals(fraction1, null) || ReferenceEquals(fraction2, null))
                return false;

            return fraction1.Numerator == fraction2.Numerator && fraction1.Denominator == fraction2.Denominator;
        }

        public static bool operator !=(in Fraction fraction1, in Fraction fraction2)
        {
            return !(fraction1 == fraction2);
        }


        // ------------------------------------------------ перегрузки опреатора > и < с учетом знака -------------------------------------------------------
        public static bool operator <(Fraction fraction1, Fraction fraction2)
        {
                return fraction1.Numerator * fraction2.Denominator < fraction2.Numerator * fraction1.Denominator;
        }


        public static bool operator >(Fraction fraction1, Fraction fraction2)
        {
                return fraction1.Numerator * fraction2.Denominator > fraction2.Numerator * fraction1.Denominator;
        }

// ------------------------------------------------ перегрузки опреатора true & false с учетом знака ------------------------------------------------------
        public static bool operator true(Fraction fraction)
        {
            return fraction.Numerator < 0 == fraction.Denominator < 0 && Math.Abs(fraction.Numerator) < Math.Abs(fraction.Denominator);
        }

        public static bool operator false(Fraction fraction)
        {
            return fraction.Numerator < 0 == fraction.Denominator < 0 && Math.Abs(fraction.Numerator) > Math.Abs(fraction.Denominator);
        }


// ------------------------------------------------ перегрузки методов класса object ------------------------------------------------------
        public override bool Equals(object obj)
        {
            if (obj is Fraction otherFraction)
            {
                return Numerator == otherFraction.Numerator && Denominator == otherFraction.Denominator;
            }

            return false;
        }


        public override int GetHashCode()
        {
            unchecked
            {
                int hash = 1;
                hash = hash * 23 + Numerator.GetHashCode();
                hash = hash * 23 + Denominator.GetHashCode();
                return hash;
            }
        }


        public override string ToString()
        {
            return $"{Numerator} / {Denominator}";
        }
    }



    internal class Program
    {
        static void Main(string[] args)
        {
            Fraction f = new Fraction(3, 2);
            int a = 10;
            Fraction f1 = f * a;
            Console.WriteLine($"{f} * {a} = {f1}");

            Fraction f2 = a * f;
            Console.WriteLine($"{a} * {f} = {f2}");

            double d = 1.5;
            Fraction f3 = f + d;
            Console.WriteLine($"{f} * {d} = {f3}");


            Console.WriteLine(new Fraction(6, 4) == new Fraction(3,2));
            Console.WriteLine(new Fraction(6, 4) < new Fraction(4, 2));
            Console.WriteLine(new Fraction(6, 5) - new Fraction(3, 3) > new Fraction(4, 2));
            if (new Fraction(3, 4))
            Console.WriteLine("дробь правильная");
            else Console.WriteLine("дробь НЕ правильная");
        }
    }
}
