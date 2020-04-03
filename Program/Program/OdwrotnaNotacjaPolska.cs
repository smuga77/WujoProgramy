using System;
using System.Collections;
using System.Text;
using System.Text.RegularExpressions;

namespace Mat
{
    public enum TypTokenu
    {
        Zaden,
        Numer,
        Stala,
        Plus,
        Minus,
        Mnozenie,
        Dzielenie,
        Potegowanie,
        UnarnyMinus,
        Sinus,
        Cosinus,
        Tangens,
        LewyNawias,
        PrawyNawias,
        Sinush,
        Cosinush,
        Tangensh,
        Wartoscbezwzgledna,
        Wykladnicza,
        Logarytm,
        Pierwiastek,
        Arcussinus,
        Arcuscosinus,
        Arcustangens,
    }

    public struct OdwrotnaNotacjaPolskaToken
    {
        public string WartoscTokenu;
        public TypTokenu TypWartosciTokenu;
    }

    public class OdwrotnaNotacjaPolska
    {
        private Queue wyjscie;
        private Stack operacje;

        private string eOrygnialneWyrazenie;
        public string OrygnialneWyrazenie
        {
            get { return eOrygnialneWyrazenie; }
        }

        private string eZamianaWyrazenia;
        public string ZamianaWyrazenia
        {
            get { return eZamianaWyrazenia; }
        }

        private string eWyrazeniePostfixowe;
        public string WyrazeniePostfixowe
        {
            get { return eWyrazeniePostfixowe; }
        }

        public OdwrotnaNotacjaPolska()
        {
            eOrygnialneWyrazenie = string.Empty;
            eZamianaWyrazenia = string.Empty;
            eWyrazeniePostfixowe = string.Empty;
        }

        public void Parse(string Wyrazenie)
        {
            wyjscie = new Queue();
            operacje = new Stack();

            eOrygnialneWyrazenie = Wyrazenie;

            string eBufor = Wyrazenie.ToLower();
            eBufor = Regex.Replace(eBufor, @"(?<numer>\d+(\.\d+)?)", " ${numer} ");
            eBufor = Regex.Replace(eBufor, @"(?<operacje>[+\-*/^()])", " ${operacje} ");
            eBufor = Regex.Replace(eBufor, "(?<alfa>(pi|exp|e|asin|acos|atan|sinh|cosh|tanh|sin|cos|tan|abs|log|sqrt))", " ${alfa} ");
            eBufor = Regex.Replace(eBufor, @"\s+", " ").Trim();
            eBufor = Regex.Replace(eBufor, "-", "MINUS");
            // Step 2. Szukanie  pi lub e lub ogólnego numera \d+(\.\d+)?
            eBufor = Regex.Replace(eBufor, @"(?<numer>(pi|e|(\d+(\.\d+)?)))\s+MINUS", "${numer} -");
            // Step 3.Użycie tyldy ~ jako unarnego minusa operatora
            eBufor = Regex.Replace(eBufor, "MINUS", "~");

            eZamianaWyrazenia = eBufor;
            string[] eAnaliza = eBufor.Split(" ".ToCharArray());
            int i = 0;
            double wartosctokena;
            OdwrotnaNotacjaPolskaToken token, tokenoperacji;
            for (i = 0; i < eAnaliza.Length; ++i)
            {
                token = new OdwrotnaNotacjaPolskaToken();
                token.WartoscTokenu = eAnaliza[i];
                token.TypWartosciTokenu = TypTokenu.Zaden;

                try
                {
                    wartosctokena = double.Parse(eAnaliza[i], System.Globalization.CultureInfo.InvariantCulture);
                    token.TypWartosciTokenu = TypTokenu.Numer;
                    wyjscie.Enqueue(token);
                }
                catch
                {
                    switch (eAnaliza[i])
                    {
                        case "+":
                            token.TypWartosciTokenu = TypTokenu.Plus;
                            if (operacje.Count > 0)
                            {
                                tokenoperacji = (OdwrotnaNotacjaPolskaToken)operacje.Peek();
                             
                                while (OperatorToken(tokenoperacji.TypWartosciTokenu))
                                {
                                  
                                    wyjscie.Enqueue(operacje.Pop());
                                    if (operacje.Count > 0)
                                    {
                                        tokenoperacji = (OdwrotnaNotacjaPolskaToken)operacje.Peek();
                                    }
                                    else
                                    {
                                        break;
                                    }
                                }
                            }
                          
                            operacje.Push(token);
                            break;
                        case "-":
                            token.TypWartosciTokenu = TypTokenu.Minus;
                            if (operacje.Count > 0)
                            {
                                tokenoperacji = (OdwrotnaNotacjaPolskaToken)operacje.Peek();

                                while (OperatorToken(tokenoperacji.TypWartosciTokenu))
                                {

                                    wyjscie.Enqueue(operacje.Pop());
                                    if (operacje.Count > 0)
                                    {
                                        tokenoperacji = (OdwrotnaNotacjaPolskaToken)operacje.Peek();
                                    }
                                    else
                                    {
                                        break;
                                    }
                                }
                            }

                            operacje.Push(token);
                            break;
                        case "*":
                            token.TypWartosciTokenu = TypTokenu.Mnozenie;
                            if (operacje.Count > 0)
                            {
                                tokenoperacji = (OdwrotnaNotacjaPolskaToken)operacje.Peek();

                                while (OperatorToken(tokenoperacji.TypWartosciTokenu))
                                {
                                    if (tokenoperacji.TypWartosciTokenu == TypTokenu.Plus || tokenoperacji.TypWartosciTokenu == TypTokenu.Minus)
                                    {
                                        break;
                                    }
                                    else
                                    {
                                        wyjscie.Enqueue(operacje.Pop());
                                        if (operacje.Count > 0)
                                        {
                                            tokenoperacji = (OdwrotnaNotacjaPolskaToken)operacje.Peek();
                                        }
                                        else
                                        {
                                            break;
                                        }
                                    }
                                }
                            }

                            operacje.Push(token);
                            break;
                        case "/":
                            token.TypWartosciTokenu = TypTokenu.Dzielenie;
                            if (operacje.Count > 0)
                            {
                                tokenoperacji = (OdwrotnaNotacjaPolskaToken)operacje.Peek();

                                while (OperatorToken(tokenoperacji.TypWartosciTokenu))
                                {
                                    if (tokenoperacji.TypWartosciTokenu == TypTokenu.Plus || tokenoperacji.TypWartosciTokenu == TypTokenu.Minus)
                                    {
                                        break;
                                    }
                                    else
                                    {
                                        wyjscie.Enqueue(operacje.Pop());
                                        if (operacje.Count > 0)
                                        {
                                            tokenoperacji = (OdwrotnaNotacjaPolskaToken)operacje.Peek();
                                        }
                                        else
                                        {
                                            break;
                                        }
                                    }
                                }
                            }

                            operacje.Push(token);
                            break;
                        case "^":
                            token.TypWartosciTokenu = TypTokenu.Potegowanie;

                            operacje.Push(token);
                            break;
                        case "~":
                            token.TypWartosciTokenu = TypTokenu.UnarnyMinus;

                            operacje.Push(token);
                            break;
                        case "(":
                            token.TypWartosciTokenu = TypTokenu.LewyNawias;

                            operacje.Push(token);
                            break;
                        case ")":
                            token.TypWartosciTokenu = TypTokenu.PrawyNawias;
                            if (operacje.Count > 0)
                            {
                                tokenoperacji = (OdwrotnaNotacjaPolskaToken)operacje.Peek();
                               
                                while (tokenoperacji.TypWartosciTokenu != TypTokenu.LewyNawias)
                                {
                                    
                                    wyjscie.Enqueue(operacje.Pop());
                                    if (operacje.Count > 0)
                                    {
                                        tokenoperacji = (OdwrotnaNotacjaPolskaToken)operacje.Peek();
                                    }
                                    else
                                    {
                                       
                                        throw new Exception("Niezrównoważony nawias!");
                                    }

                                }
                               
                                operacje.Pop();
                            }

                            if (operacje.Count > 0)
                            {
                                tokenoperacji = (OdwrotnaNotacjaPolskaToken)operacje.Peek();

                                if (FunkcjaToken(tokenoperacji.TypWartosciTokenu))
                                {

                                    wyjscie.Enqueue(operacje.Pop());
                                }
                            }
                            break;
                        case "pi":
                            token.TypWartosciTokenu = TypTokenu.Stala;

                            wyjscie.Enqueue(token);
                            break;
                        case "e":
                            token.TypWartosciTokenu = TypTokenu.Stala;

                            wyjscie.Enqueue(token);
                            break;
                        case "sin":
                            token.TypWartosciTokenu = TypTokenu.Sinus;

                            operacje.Push(token);
                            break;
                        case "cos":
                            token.TypWartosciTokenu = TypTokenu.Cosinus;

                            operacje.Push(token);
                            break;
                        case "tan":
                            token.TypWartosciTokenu = TypTokenu.Tangens;

                            operacje.Push(token);
                            break;
                        case "sinh":
                            token.TypWartosciTokenu = TypTokenu.Sinush;

                            operacje.Push(token);
                            break;
                        case "cosh":
                            token.TypWartosciTokenu = TypTokenu.Cosinush;

                            operacje.Push(token);
                            break;
                        case "tanh":
                            token.TypWartosciTokenu = TypTokenu.Tangensh;

                            operacje.Push(token);
                            break;
                        case "abs":
                            token.TypWartosciTokenu = TypTokenu.Wartoscbezwzgledna;

                            operacje.Push(token);
                            break;
                        case "exp":
                            token.TypWartosciTokenu = TypTokenu.Wykladnicza;

                            operacje.Push(token);
                            break;
                        case "log":
                            token.TypWartosciTokenu = TypTokenu.Logarytm;

                            operacje.Push(token);
                            break;
                        case "sqrt":
                            token.TypWartosciTokenu = TypTokenu.Pierwiastek;

                            operacje.Push(token);
                            break;
                        case "asin":
                            token.TypWartosciTokenu = TypTokenu.Arcussinus;

                            operacje.Push(token);
                            break;
                        case "acos":
                            token.TypWartosciTokenu = TypTokenu.Arcuscosinus;

                            operacje.Push(token);
                            break;
                        case "atan":
                            token.TypWartosciTokenu = TypTokenu.Arcustangens;

                            operacje.Push(token);
                            break;
                    }
                }
            }



          
            while (operacje.Count != 0)
            {
                tokenoperacji = (OdwrotnaNotacjaPolskaToken)operacje.Pop();

                if (tokenoperacji.TypWartosciTokenu == TypTokenu.LewyNawias)
                {
                  
                    throw new Exception("Niezrównoważone nawiasy!");
                }
                else
                {

                    wyjscie.Enqueue(tokenoperacji);
                }
            }

            eWyrazeniePostfixowe = string.Empty;
            foreach (object obj in wyjscie)
            {
                tokenoperacji = (OdwrotnaNotacjaPolskaToken)obj;
                eWyrazeniePostfixowe += string.Format("{0} ", tokenoperacji.WartoscTokenu);
            }
        }

        public double Ocenianie()
        {
            Stack wynik = new Stack();
            double operacja1 = 0.0, operacja2 = 0.0;
            OdwrotnaNotacjaPolskaToken token = new OdwrotnaNotacjaPolskaToken();

            foreach (object obj in wyjscie)
            {
                
                token = (OdwrotnaNotacjaPolskaToken)obj;
                switch (token.TypWartosciTokenu)
                {
                    case TypTokenu.Numer:
                      
                        wynik.Push(double.Parse(token.WartoscTokenu, System.Globalization.CultureInfo.InvariantCulture));
                        break;
                    case TypTokenu.Stala:

                        wynik.Push(OcenianieStalej(token.WartoscTokenu));
                        break;
                    case TypTokenu.Plus:
                       
                        if (wynik.Count >= 2)
                        {
                            
                            operacja2 = (double)wynik.Pop();
                            operacja1 = (double)wynik.Pop();
                           
                            wynik.Push(operacja1 + operacja2);
                        }
                        else
                        {
                         
                            throw new Exception("Błąd analizy !");
                        }
                        break;
                    case TypTokenu.Minus:

                        if (wynik.Count >= 2)
                        {

                            operacja2 = (double)wynik.Pop();
                            operacja1 = (double)wynik.Pop();

                            wynik.Push(operacja1 - operacja2);
                        }
                        else
                        {

                            throw new Exception("Błąd analizy !");
                        }
                        break;
                    case TypTokenu.Mnozenie:

                        if (wynik.Count >= 2)
                        {

                            operacja2 = (double)wynik.Pop();
                            operacja1 = (double)wynik.Pop();

                            wynik.Push(operacja1 * operacja2);
                        }
                        else
                        {

                            throw new Exception("Błąd analizy !");
                        }
                        break;
                    case TypTokenu.Dzielenie:

                        if (wynik.Count >= 2)
                        {

                            operacja2 = (double)wynik.Pop();
                            operacja1 = (double)wynik.Pop();

                            wynik.Push(operacja1 / operacja2);
                        }
                        else
                        {

                            throw new Exception("Błąd analizy !");
                        }
                        break;
                    case TypTokenu.Potegowanie:

                        if (wynik.Count >= 2)
                        {

                            operacja2 = (double)wynik.Pop();
                            operacja1 = (double)wynik.Pop();


                            wynik.Push(Math.Pow(operacja1, operacja2));
                        }
                        else
                        {

                            throw new Exception("Błąd analizy !");
                        }
                        break;
                    case TypTokenu.UnarnyMinus:
                        
                        if (wynik.Count >= 1)
                        {

                            operacja1 = (double)wynik.Pop();

                            wynik.Push(-operacja1);
                        }
                        else
                        {

                            throw new Exception("Błąd analizy !");
                        }
                        break;
                    case TypTokenu.Sinus:

                        if (wynik.Count >= 1)
                        {

                            operacja1 = (double)wynik.Pop();

                            wynik.Push(Math.Sin(operacja1));
                        }
                        else
                        {

                            throw new Exception("Błąd analizy !");
                        }
                        break;
                    case TypTokenu.Cosinus:

                        if (wynik.Count >= 1)
                        {

                            operacja1 = (double)wynik.Pop();

                            wynik.Push(Math.Cos(operacja1));
                        }
                        else
                        {

                            throw new Exception("Błąd analizy !");
                        }
                        break;
                    case TypTokenu.Tangens:

                        if (wynik.Count >= 1)
                        {

                            operacja1 = (double)wynik.Pop();

                            wynik.Push(Math.Tan(operacja1));
                        }
                        else
                        {
                            throw new Exception("Błąd analizy !");
                        }
                        break;
                    case TypTokenu.Sinush:

                        if (wynik.Count >= 1)
                        {

                            operacja1 = (double)wynik.Pop();

                            wynik.Push(Math.Sinh(operacja1));
                        }
                        else
                        {
                            throw new Exception("Błąd analizy !");
                        }
                        break;
                    case TypTokenu.Cosinush:

                        if (wynik.Count >= 1)
                        {

                            operacja1 = (double)wynik.Pop();

                            wynik.Push(Math.Cosh(operacja1));
                        }
                        else
                        {
                            throw new Exception("Błąd analizy !");
                        }
                        break;
                    case TypTokenu.Tangensh:

                        if (wynik.Count >= 1)
                        {

                            operacja1 = (double)wynik.Pop();

                            wynik.Push(Math.Tanh(operacja1));
                        }
                        else
                        {
                            throw new Exception("Błąd analizy !");
                        }
                        break;
                    case TypTokenu.Wartoscbezwzgledna:

                        if (wynik.Count >= 1)
                        {

                            operacja1 = (double)wynik.Pop();

                            wynik.Push(Math.Abs(operacja1));
                        }
                        else
                        {
                            throw new Exception("Błąd analizy !");
                        }
                        break;
                    case TypTokenu.Wykladnicza:

                        if (wynik.Count >= 1)
                        {

                            operacja1 = (double)wynik.Pop();

                            wynik.Push(Math.Exp(operacja1));
                        }
                        else
                        {
                            throw new Exception("Błąd analizy !");
                        }
                        break;
                    case TypTokenu.Logarytm:

                        if (wynik.Count >= 1)
                        {

                            operacja1 = (double)wynik.Pop();

                            wynik.Push(Math.Log(operacja1));
                        }
                        else
                        {
                            throw new Exception("Błąd analizy !");
                        }
                        break;
                    case TypTokenu.Pierwiastek:

                        if (wynik.Count >= 1)
                        {

                            operacja1 = (double)wynik.Pop();

                            wynik.Push(Math.Sqrt(operacja1));
                        }
                        else
                        {
                            throw new Exception("Błąd analizy !");
                        }
                        break;
                    case TypTokenu.Arcussinus:

                        if (wynik.Count >= 1)
                        {

                            operacja1 = (double)wynik.Pop();

                            wynik.Push(Math.Asin(operacja1));
                        }
                        else
                        {
                            throw new Exception("Błąd analizy !");
                        }
                        break;
                    case TypTokenu.Arcuscosinus:

                        if (wynik.Count >= 1)
                        {

                            operacja1 = (double)wynik.Pop();

                            wynik.Push(Math.Acos(operacja1));
                        }
                        else
                        {
                            throw new Exception("Błąd analizy !");
                        }
                        break;
                    case TypTokenu.Arcustangens:

                        if (wynik.Count >= 1)
                        {

                            operacja1 = (double)wynik.Pop();

                            wynik.Push(Math.Atan(operacja1));
                        }
                        else
                        {
                            throw new Exception("Błąd analizy !");
                        }
                        break;
                }
            }

          
            if (wynik.Count == 1)
            {
                
                return (double)wynik.Pop();
            }
            else
            {

                throw new Exception("Błąd analizy !");
            }
        }

        private bool OperatorToken(TypTokenu t)
        {
            bool wynik = false;
            switch (t)
            {
                case TypTokenu.Plus:
                case TypTokenu.Minus:
                case TypTokenu.Mnozenie:
                case TypTokenu.Dzielenie:
                case TypTokenu.Potegowanie:
                case TypTokenu.UnarnyMinus:


                    wynik = true;
                    break;
                default:
                    wynik = false;
                    break;
            }
            return wynik;
        }

        private bool FunkcjaToken(TypTokenu t)
        {
            bool wynik = false;
            switch (t)
            {
                case TypTokenu.Sinus:
                case TypTokenu.Cosinus:
                case TypTokenu.Tangens:
                case TypTokenu.Sinush:
                case TypTokenu.Cosinush:
                case TypTokenu.Tangensh:
                case TypTokenu.Wartoscbezwzgledna:
                case TypTokenu.Wykladnicza:
                case TypTokenu.Logarytm:
                case TypTokenu.Pierwiastek:
                case TypTokenu.Arcussinus:
                case TypTokenu.Arcuscosinus:
                case TypTokenu.Arcustangens:

                    wynik = true;
                    break;
                default:
                    wynik = false;
                    break;
            }
            return wynik;
        }

        private double OcenianieStalej(string WartoscTokenu)
        {
            double wynik = 0.0;
            switch (WartoscTokenu)
            {
                case "pi":
                    wynik = Math.PI;
                    break;
                case "e":
                    wynik = Math.E;
                    break;

            }
            return wynik;
        }
    }
}
