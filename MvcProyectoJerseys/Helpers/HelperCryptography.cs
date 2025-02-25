using System.Security.Cryptography;
using System.Text;

namespace MvcProyectoJerseys.Helpers
{
    public class HelperCryptography
    {
        public static string GenerateSalt()
        {
            Random random = new Random();
            string salt = "";
            //EL NUMERO DE VUELTAS DEBE COINCIDIR CON 
            //EL VALOR DEL CAMPO NVARCHAR
            for (int i = 1; i <= 50; i++)
            {
                int aleat = random.Next(1, 255);
                char letra = Convert.ToChar(aleat);
                salt += letra;
            }
            return salt;
        }

        //NECESITAMOS SABER SI EL PASSWORD QUE HEMOS ALMACENADO
        //EN BBDD ES IGUAL AL PASSWORD QUE NOS HABRAN DADO EN LA APP
        //ESTE ES UN METODO PARA COMPARAR DOS ARRAYS DE BYTES
        public static bool CompararArrays(byte[] a, byte[] b)
        {
            bool iguales = true;
            //COMPARAMOS EL TAMAÑO
            if (a.Length != b.Length)
            {
                iguales = false;
            }
            else
            {
                //RECORREMOS EL ARRAY a
                for (int i = 0; i < a.Length; i++)
                {
                    //COMPARAMOS BYTE A BYTE
                    if (a[i].Equals(b[i]) == false)
                    {
                        iguales = false;
                        break;
                    }
                }
            }
            return iguales;
        }

        //TENDREMOS UN METODO PARA CIFRAR EL PASSWORD
        //VAMOS A RECIBIR EL PASSWORD A CIFRAR (string) 
        //Y TAMBIEN VAMOS A RECIBIR EL SALT (string)
        //DEVOLVEREMOS UN ARRAY CON EL RESULTADO
        public static byte[] EncryptPassword(string password, string salt)
        {
            string contenido = password + salt;
            SHA512 managed = SHA512.Create();
            //CONVERTIMOS EL CONTENIDO A byte[]
            byte[] salida = Encoding.UTF8.GetBytes(contenido);
            //CREAMOS EL BUCLE DE CIFRADO CON ITERACIONES
            for (int i = 1; i <= 15; i++)
            {
                salida = managed.ComputeHash(salida);
            }
            managed.Clear();
            return salida;
        }

    }
}
