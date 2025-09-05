using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MigracionDeDatosDeSqlServerAMySql
{
    public static class NumeroALetrasExtensions
    {
        public static string ToLetrasCheque(this decimal amount, string moneda = "PESOS")
        {
            long parteEntera = (long)Math.Truncate(amount);
            int centavos = (int)Math.Round((amount - parteEntera) * 100, 0);
            string letras = ConvertirEnteroALetras(parteEntera);
            string decimales = $"{centavos:00}/100";

            return $"{moneda} {letras} {decimales} M.N.";
        }

        private static string ConvertirEnteroALetras(long number)
        {
            if (number == 0) return "CERO";
            if (number < 0) return "MENOS " + ConvertirEnteroALetras(Math.Abs(number));

            var unidades = new[]
            {
            "", "UN", "DOS", "TRES", "CUATRO", "CINCO",
            "SEIS", "SIETE", "OCHO", "NUEVE", "DIEZ",
            "ONCE", "DOCE", "TRECE", "CATORCE", "QUINCE"
        };
            var decenas = new[]
            {
            "", "", "VEINTE", "TREINTA", "CUARENTA",
            "CINCUENTA", "SESENTA", "SETENTA", "OCHENTA", "NOVENTA"
        };
            var centenas = new[]
            {
            "", "CIENTO", "DOSCIENTOS", "TRESCIENTOS", "CUATROCIENTOS",
            "QUINIENTOS", "SEISCIENTOS", "SETECIENTOS", "OCHOCIENTOS", "NOVECIENTOS"
        };

            string resultado = "";

            // Billones (10^12)
            if (number >= 1_000_000_000_000)
            {
                long billones = number / 1_000_000_000_000;
                resultado += billones == 1
                    ? "UN BILLON "
                    : ConvertirEnteroALetras(billones) + " BILLONES ";
                number %= 1_000_000_000_000;
            }

            // Millones (10^6)
            if (number >= 1_000_000)
            {
                long millones = number / 1_000_000;
                resultado += millones == 1
                    ? "UN MILLON "
                    : ConvertirEnteroALetras(millones) + " MILLONES ";
                number %= 1_000_000;
            }

            // Miles (10^3)
            if (number >= 1_000)
            {
                long miles = number / 1_000;
                resultado += miles == 1
                    ? "MIL "
                    : ConvertirEnteroALetras(miles) + " MIL ";
                number %= 1_000;
            }

            // Centenas
            if (number >= 100)
            {
                int c = (int)(number / 100);
                if (c == 1 && number % 100 == 0)
                    resultado += "CIEN ";
                else
                    resultado += centenas[c] + " ";
                number %= 100;
            }

            // Decenas y unidades
            if (number >= 20)
            {
                int d = (int)(number / 10);
                resultado += decenas[d];
                int u = (int)(number % 10);
                if (u > 0)
                    resultado += " Y " + unidades[u];
            }
            else if (number > 0)
            {
                resultado += unidades[number];
            }

            return resultado.Trim();
        }
    }
}
