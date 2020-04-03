using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mat
{
    class Program
    {
        static void Main(string[] args)
        {
         

         double wynik = 0.0;
            

         Mat.OdwrotnaNotacjaPolska onp = new Mat.OdwrotnaNotacjaPolska();
            onp.Parse("3+4*2/(1-5)^2^3");
            wynik = onp.Ocenianie();


            Console.WriteLine("Przyklad 1.");

            Console.WriteLine("oryginał: {0}", onp.OrygnialneWyrazenie);
            Console.WriteLine("zamiana: {0}", onp.ZamianaWyrazenia);
            Console.WriteLine("postfix: {0}", onp.WyrazeniePostfixowe);
            Console.WriteLine("wynik: {0}", wynik);

            Console.WriteLine();
            onp.Parse("sin2");
            wynik = onp.Ocenianie();
            Console.WriteLine("Przyklad 2.");
            Console.WriteLine("oryginał: {0}", onp.OrygnialneWyrazenie);
            Console.WriteLine("zamiana: {0}", onp.ZamianaWyrazenia);
            Console.WriteLine("postfix: {0}", onp.WyrazeniePostfixowe);
            Console.WriteLine("wynik: {0}", wynik);

            Console.WriteLine();
            onp.Parse("abs(-10)");
            wynik = onp.Ocenianie();
            Console.WriteLine("Przyklad 3.");
            Console.WriteLine("oryginał: {0}", onp.OrygnialneWyrazenie);
            Console.WriteLine("zamiana: {0}", onp.ZamianaWyrazenia);
            Console.WriteLine("postfix: {0}", onp.WyrazeniePostfixowe);
            Console.WriteLine("wynik: {0}", wynik);
            Console.WriteLine("");


            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine("Koniec programu");
            Console.ReadLine();


        }

        static void Oblicz(string wyrazenie)
        {
            double wynik;
            Mat.OdwrotnaNotacjaPolska onp = new Mat.OdwrotnaNotacjaPolska();
            Console.WriteLine();
            onp.Parse(wyrazenie);
            wynik = onp.Ocenianie();
            Console.WriteLine("oryginał: {0}", onp.OrygnialneWyrazenie);
            Console.WriteLine("zamiana: {0}", onp.ZamianaWyrazenia);
            Console.WriteLine("postfix: {0}", onp.WyrazeniePostfixowe);
            Console.WriteLine("wynik: {0}", wynik);
            Console.WriteLine("");
        }
    }
}
